using System;
using QRCodeDecoder = Ophelia.Tools.QRCode.Codec.QRCodeDecoder;
using AlignmentPatternNotFoundException = Ophelia.Tools.QRCode.Exceptions.AlignmentPatternNotFoundException;
using FinderPatternNotFoundException = Ophelia.Tools.QRCode.Exceptions.FinderPatternNotFoundException;
using SymbolNotFoundException = Ophelia.Tools.QRCode.Exceptions.SymbolNotFoundException;
using InvalidVersionException = Ophelia.Tools.QRCode.Exceptions.InvalidVersionException;
using VersionInformationException = Ophelia.Tools.QRCode.Exceptions.VersionInformationException;
using SystemUtils = Ophelia.Tools.QRCode.Codec.Util.SystemUtils;
using Ophelia.Tools.QRCode.Codec.Data;
using Ophelia.Tools.QRCode.Geom;
using Ophelia.Tools.QRCode.Codec.Reader.Pattern;
using Ophelia.Tools.QRCode.Codec.Util;

namespace Ophelia.Tools.QRCode.Codec.Reader
{
    public class QRCodeImageReader
    {
        internal IDebugCanvas canvas;
        //boolean[][] image;
        //DP = 
        //23 ...side pixels of image will be limited maximum 255 (8 bits)
        //22 .. side pixels of image will be limited maximum 511 (9 bits)
        //21 .. side pixels of image will be limited maximum 1023 (10 bits)

        public static int DECIMAL_POINT = 21;
        public const bool POINT_DARK = true;
        public const bool POINT_LIGHT = false;
        internal SamplingGrid samplingGrid;
        internal bool[][] bitmap;


        public QRCodeImageReader()
        {
            this.canvas = QRCodeDecoder.Canvas;
        }

        internal virtual bool[][] ApplyMedianFilter(bool[][] image, int threshold)
        {
            bool[][] filteredMatrix = new bool[image.Length][];
            for (int i = 0; i < image.Length; i++)
            {
                filteredMatrix[i] = new bool[image[0].Length];
            }
            //filtering noise in image with median filter
            int numPointDark;
            for (int y = 1; y < image[0].Length - 1; y++)
            {
                for (int x = 1; x < image.Length - 1; x++)
                {
                    //if (image[x][y] == true) {
                    numPointDark = 0;
                    for (int fy = -1; fy < 2; fy++)
                    {
                        for (int fx = -1; fx < 2; fx++)
                        {
                            if (image[x + fx][y + fy] == true)
                            {
                                numPointDark++;
                            }
                        }
                    }
                    if (numPointDark > threshold)
                        filteredMatrix[x][y] = POINT_DARK;
                }
            }

            return filteredMatrix;
        }
        internal virtual bool[][] ApplyCrossMaskingMedianFilter(bool[][] image, int threshold)
        {
            bool[][] filteredMatrix = new bool[image.Length][];
            for (int i = 0; i < image.Length; i++)
            {
                filteredMatrix[i] = new bool[image[0].Length];
            }
            //filtering noise in image with median filter
            int numPointDark;
            for (int y = 2; y < image[0].Length - 2; y++)
            {
                for (int x = 2; x < image.Length - 2; x++)
                {
                    //if (image[x][y] == true) {
                    numPointDark = 0;
                    for (int f = -2; f < 3; f++)
                    {
                        if (image[x + f][y] == true)
                            numPointDark++;

                        if (image[x][y + f] == true)
                            numPointDark++;
                    }

                    if (numPointDark > threshold)
                        filteredMatrix[x][y] = POINT_DARK;
                }
            }

            return filteredMatrix;
        }
        internal virtual bool[][] FilterImage(int[][] image)
        {
            ImageToGrayScale(image);
            bool[][] bitmap = GrayScaleToBitmap(image);
            return bitmap;
        }

        internal virtual void ImageToGrayScale(int[][] image)
        {
            for (int y = 0; y < image[0].Length; y++)
            {
                for (int x = 0; x < image.Length; x++)
                {
                    int r = image[x][y] >> 16 & 0xFF;
                    int g = image[x][y] >> 8 & 0xFF;
                    int b = image[x][y] & 0xFF;
                    int m = (r * 30 + g * 59 + b * 11) / 100;
                    image[x][y] = m;
                }
            }
        }

        internal virtual bool[][] GrayScaleToBitmap(int[][] grayScale)
        {
            int[][] middle = GetMiddleBrightnessPerArea(grayScale);
            int sqrtNumArea = middle.Length;
            int areaWidth = grayScale.Length / sqrtNumArea;
            int areaHeight = grayScale[0].Length / sqrtNumArea;
            bool[][] bitmap = new bool[grayScale.Length][];
            for (int i = 0; i < grayScale.Length; i++)
            {
                bitmap[i] = new bool[grayScale[0].Length];
            }

            for (int ay = 0; ay < sqrtNumArea; ay++)
            {
                for (int ax = 0; ax < sqrtNumArea; ax++)
                {
                    for (int dy = 0; dy < areaHeight; dy++)
                    {
                        for (int dx = 0; dx < areaWidth; dx++)
                        {
                            bitmap[areaWidth * ax + dx][areaHeight * ay + dy] = (grayScale[areaWidth * ax + dx][areaHeight * ay + dy] < middle[ax][ay]) ? true : false;
                        }
                    }
                }
            }
            return bitmap;
        }

        internal virtual int[][] GetMiddleBrightnessPerArea(int[][] image)
        {
            int numSqrtArea = 4;
            //obtain middle brightness((min + max) / 2) per area
            int areaWidth = image.Length / numSqrtArea;
            int areaHeight = image[0].Length / numSqrtArea;
            int[][][] minmax = new int[numSqrtArea][][];
            for (int i = 0; i < numSqrtArea; i++)
            {
                minmax[i] = new int[numSqrtArea][];
                for (int i2 = 0; i2 < numSqrtArea; i2++)
                {
                    minmax[i][i2] = new int[2];
                }
            }
            for (int ay = 0; ay < numSqrtArea; ay++)
            {
                for (int ax = 0; ax < numSqrtArea; ax++)
                {
                    minmax[ax][ay][0] = 0xFF;
                    for (int dy = 0; dy < areaHeight; dy++)
                    {
                        for (int dx = 0; dx < areaWidth; dx++)
                        {
                            int target = image[areaWidth * ax + dx][areaHeight * ay + dy];
                            if (target < minmax[ax][ay][0])
                                minmax[ax][ay][0] = target;
                            if (target > minmax[ax][ay][1])
                                minmax[ax][ay][1] = target;
                        }
                    }
                    //minmax[ax][ay][0] = (minmax[ax][ay][0] + minmax[ax][ay][1]) / 2;
                }
            }
            int[][] middle = new int[numSqrtArea][];
            for (int i3 = 0; i3 < numSqrtArea; i3++)
            {
                middle[i3] = new int[numSqrtArea];
            }
            for (int ay = 0; ay < numSqrtArea; ay++)
            {
                for (int ax = 0; ax < numSqrtArea; ax++)
                {
                    middle[ax][ay] = (minmax[ax][ay][0] + minmax[ax][ay][1]) / 2;
                    //Console.out.print(middle[ax][ay] + ",");
                }
                //Console.out.println("");
            }
            //Console.out.println("");

            return middle;
        }

        public virtual QRCodeSymbol GetQRCodeSymbol(int[][] image)
        {
            int longSide = (image.Length < image[0].Length) ? image[0].Length : image.Length;
            QRCodeImageReader.DECIMAL_POINT = 23 - QRCodeUtility.Sqrt(longSide / 256);
            bitmap = FilterImage(image);
            canvas.Println("Drawing matrix.");
            canvas.DrawMatrix(bitmap);

            canvas.Println("Scanning Finder Pattern.");
            FinderPattern finderPattern = null;
            try
            {
                finderPattern = FinderPattern.FindFinderPattern(bitmap);
            }
            catch (FinderPatternNotFoundException)
            {
                canvas.Println("Not found, now retrying...");
                bitmap = ApplyCrossMaskingMedianFilter(bitmap, 5);
                canvas.DrawMatrix(bitmap);
                for (int i = 0; i < 1000000000; i++)
                    ;
                try
                {
                    finderPattern = FinderPattern.FindFinderPattern(bitmap);
                }
                catch (FinderPatternNotFoundException e2)
                {
                    throw new SymbolNotFoundException(e2.Message);
                }
                catch (VersionInformationException e2)
                {
                    throw new SymbolNotFoundException(e2.Message);
                }
            }
            catch (VersionInformationException e)
            {
                throw new SymbolNotFoundException(e.Message);
            }

            String finderPatternCoordinates = finderPattern.GetCenter(FinderPattern.UL).ToString() + finderPattern.GetCenter(FinderPattern.UR).ToString() + finderPattern.GetCenter(FinderPattern.DL).ToString();
            canvas.Println(finderPatternCoordinates);
            int[] sincos = finderPattern.GetAngle();

            int version = finderPattern.Version;
            if (version < 1 || version > 40)
                throw new InvalidVersionException("Invalid version: " + version);

            AlignmentPattern alignmentPattern = null;
            try
            {
                alignmentPattern = AlignmentPattern.FindAlignmentPattern(bitmap, finderPattern);
            }
            catch (AlignmentPatternNotFoundException e)
            {
                throw new SymbolNotFoundException(e.Message);
            }

            int matrixLength = alignmentPattern.GetCenter().Length;
            for (int y = 0; y < matrixLength; y++)
            {
                String alignmentPatternCoordinates = String.Empty;
                for (int x = 0; x < matrixLength; x++)
                {
                    alignmentPatternCoordinates += alignmentPattern.GetCenter()[x][y].ToString();
                }
                canvas.Println(alignmentPatternCoordinates);
            }

            //[TODO] need all-purpose method
            //samplingGrid = getSamplingGrid2_6(finderPattern, alignmentPattern);
            samplingGrid = GetSamplingGrid(finderPattern, alignmentPattern);
            bool[][] qRCodeMatrix = null;
            try
            {
                qRCodeMatrix = GetQRCodeMatrix(bitmap, samplingGrid);
            }
            catch (IndexOutOfRangeException)
            {
                throw new SymbolNotFoundException("Sampling grid exceeded image boundary");
            }
            return new QRCodeSymbol(qRCodeMatrix);
        }

        public virtual QRCodeSymbol GetQRCodeSymbolWithAdjustedGrid(Point adjust)
        {
            if (bitmap == null || samplingGrid == null)
            {
                throw new System.SystemException("This method must be called after QRCodeImageReader.getQRCodeSymbol() called");
            }
            samplingGrid.Adjust(adjust);
            canvas.Println("Sampling grid adjusted d(" + adjust.X + "," + adjust.Y + ")");

            bool[][] qRCodeMatrix = null;
            try
            {
                qRCodeMatrix = GetQRCodeMatrix(bitmap, samplingGrid);
            }
            catch (IndexOutOfRangeException)
            {
                throw new SymbolNotFoundException("Sampling grid exceeded image boundary");
            }
            return new QRCodeSymbol(qRCodeMatrix);
        }

        // For only version 1 which has no Alignement Patterns
        /*	SamplingGrid getSamplingGrid1(FinderPattern finderPattern) {
        int sqrtNumArea = 1;
        int sqrtNumModules = finderPattern.getSqrtNumModules(); //get nummber of modules at side
        int sqrtNumAreaModules = sqrtNumModules / sqrtNumArea;
        Point[] centers = finderPattern.getCenter();
        int logicalDistance = 14;
        SamplingGrid samplingGrid = new SamplingGrid(sqrtNumArea);
        Line baseLineX, baseLineY, gridLineX, gridLineY;
		
		
        ModulePitch modulePitch = new ModulePitch(); //store (up,left) order
        modulePitch.top = getAreaModulePitch(centers[0], centers[1], logicalDistance);
        modulePitch.left = getAreaModulePitch(centers[0], centers[2], logicalDistance);
		
        //X軸に垂直の基線(一般に縦)
        baseLineX = new Line(
        finderPattern.getCenter(FinderPattern.UL), 
        finderPattern.getCenter(FinderPattern.DL));
		
        Axis axis = new Axis(finderPattern.getAngle(), modulePitch.top);
        axis.setOrigin(baseLineX.getP1());
        baseLineX.setP1(axis.translate(-3, -3));
		
        axis.setModulePitch(modulePitch.left);
        axis.setOrigin(baseLineX.getP2());
        baseLineX.setP2(axis.translate(-3, 3));
		
        //Y軸に垂直の基線(一般に横)
        baseLineY =
        new Line(finderPattern.getCenter(FinderPattern.UL),
        finderPattern.getCenter(FinderPattern.UR));
		
        axis.setModulePitch(modulePitch.left);
        axis.setOrigin(baseLineY.getP1());
        baseLineY.setP1(axis.translate(-3, -3));
		
		
        axis.setModulePitch(modulePitch.left);
        axis.setOrigin(baseLineY.getP2());
        baseLineY.setP2(axis.translate(3, -3));
		
        //baseLineX.translate(1,1);
        //baseLineY.translate(1,1);
		
        samplingGrid.initGrid(0, 0, sqrtNumAreaModules, sqrtNumAreaModules);
		
        for (int i = 0; i < sqrtNumAreaModules; i++) {
		
        gridLineX = new Line(baseLineX.getP1(), baseLineX.getP2());
		
        axis.setOrigin(gridLineX.getP1());
        axis.setModulePitch(modulePitch.top);
        gridLineX.setP1(axis.translate(i,0));
		
        axis.setOrigin(gridLineX.getP2());
        axis.setModulePitch(modulePitch.top);
        gridLineX.setP2(axis.translate(i,0));
		
		
        gridLineY = new Line(baseLineY.getP1(), baseLineY.getP2());
        axis.setOrigin(gridLineY.getP1());
        axis.setModulePitch(modulePitch.left);
        gridLineY.setP1(axis.translate(0,i));
		
        axis.setOrigin(gridLineY.getP2());
        axis.setModulePitch(modulePitch.left);
        gridLineY.setP2(axis.translate(0,i));
		
		
        samplingGrid.setXLine(0,0,i,gridLineX);
        samplingGrid.setYLine(0,0,i,gridLineY);
        }
        for (int ay = 0; ay < samplingGrid.getHeight(); ay++) {
        for (int ax = 0; ax < samplingGrid.getWidth();ax++) {
        canvas.drawLines(samplingGrid.getXLines(ax,ay), Color.BLUE);
        canvas.drawLines(samplingGrid.getYLines(ax,ay), Color.BLUE);
        }
        }
        return samplingGrid;
        }*/

        //sampllingGrid[areaX][areaY][direction(x=0,y=1)][EachLines]	
        /*	SamplingGrid getSamplingGrid2_6(FinderPattern finderPattern, AlignmentPattern alignmentPattern) {
		
        Point centers[][] = alignmentPattern.getCenter();
        centers[0][0] = finderPattern.getCenter(FinderPattern.UL);
        centers[1][0] = finderPattern.getCenter(FinderPattern.UR);
        centers[0][1] = finderPattern.getCenter(FinderPattern.DL);
        int sqrtNumModules = finderPattern.getSqrtNumModules(); //一辺当たりのモジュール数を得る
		
        SamplingGrid samplingGrid = new SamplingGrid(1);
        Line baseLineX, baseLineY, gridLineX, gridLineY;
		
        int logicalDistance = alignmentPattern.getLogicalDistance();
        Axis axis = new Axis(finderPattern.getAngle(), finderPattern.getModuleSize());
		
        ModulePitch modulePitch = new ModulePitch(); //top left bottom rightの順に格納
		
        modulePitch.top = getAreaModulePitch(centers[0][0], centers[1][0], logicalDistance + 6);
        modulePitch.left = getAreaModulePitch(centers[0][0], centers[0][1], logicalDistance + 6);
        axis.setModulePitch(modulePitch.top);
        axis.setOrigin(centers[0][1]);
        modulePitch.bottom = getAreaModulePitch(axis.translate(0, -3), centers[1][1], logicalDistance + 3);
        axis.setModulePitch(modulePitch.left);
        axis.setOrigin(centers[1][0]);
        modulePitch.right = getAreaModulePitch(axis.translate(-3, 0), centers[1][1], logicalDistance + 3);
		
        //X軸に垂直の基線(一般に縦)
        baseLineX = new Line();
        baseLineY = new Line();
		
        axis.setOrigin(centers[0][0]);
        modulePitch.top = getAreaModulePitch(centers[0][0], centers[1][0], logicalDistance + 6);
        modulePitch.left = getAreaModulePitch(centers[0][0], centers[0][1], logicalDistance + 6);
        axis.setModulePitch(modulePitch.top);
        axis.setOrigin(centers[0][1]);
        modulePitch.bottom = getAreaModulePitch(axis.translate(0,-3), centers[1][1], logicalDistance + 3);
        axis.setModulePitch(modulePitch.left);
        axis.setOrigin(centers[1][0]);
        modulePitch.right = getAreaModulePitch(axis.translate(-3,0), centers[1][1], logicalDistance + 3);
		
		
        axis.setOrigin(centers[0][0]);
        axis.setModulePitch(modulePitch.top);
        baseLineX.setP1(axis.translate(-3,-3));
		
        axis.setModulePitch(modulePitch.left);
        baseLineY.setP1(axis.translate(-3,-3));
		
        axis.setOrigin(centers[0][1]);
        axis.setModulePitch(modulePitch.bottom);
        baseLineX.setP2(axis.translate(-3,3));
		
        axis.setOrigin(centers[1][0]);
        axis.setModulePitch(modulePitch.right);
        baseLineY.setP2(axis.translate(3,-3));
		
		
        baseLineX.translate(1,1);
        baseLineY.translate(1,1);
		
        samplingGrid.initGrid(0, 0, sqrtNumModules, sqrtNumModules);
		
        for (int i = 0; i < sqrtNumModules; i++) {
        gridLineX = new Line(baseLineX.getP1(), baseLineX.getP2());
		
        axis.setOrigin(gridLineX.getP1());
        axis.setModulePitch(modulePitch.top);
        gridLineX.setP1(axis.translate(i,0));
		
        axis.setOrigin(gridLineX.getP2());
        axis.setModulePitch(modulePitch.bottom);
        gridLineX.setP2(axis.translate(i,0));
		
		
        gridLineY = new Line(baseLineY.getP1(), baseLineY.getP2());
		
        axis.setOrigin(gridLineY.getP1());
        axis.setModulePitch(modulePitch.left);
        gridLineY.setP1(axis.translate(0,i));
		
        axis.setOrigin(gridLineY.getP2());
        axis.setModulePitch(modulePitch.right);
        gridLineY.setP2(axis.translate(0,i));
		
		
        samplingGrid.setXLine(0,0,i,gridLineX);
        samplingGrid.setYLine(0,0,i,gridLineY);
		
        }
		
        for (int ay = 0; ay < samplingGrid.getHeight(); ay++) {
        for (int ax = 0; ax < samplingGrid.getWidth();ax++) {
        canvas.drawLines(samplingGrid.getXLines(ax,ay), java.awt.Color.BLUE);
        canvas.drawLines(samplingGrid.getYLines(ax,ay), java.awt.Color.BLUE);
        }
        }
        return samplingGrid;
        }
		
		
		
        //for version 7-13
        SamplingGrid getSamplingGrid7_13(FinderPattern finderPattern, AlignmentPattern alignmentPattern) {
		
        Point centers[][] = alignmentPattern.getCenter();
        centers[0][0] = finderPattern.getCenter(FinderPattern.UL);
        centers[2][0] = finderPattern.getCenter(FinderPattern.UR);
        centers[0][2] = finderPattern.getCenter(FinderPattern.DL);
        int sqrtNumModules = finderPattern.getSqrtNumModules(); //一辺当たりのモジュール数を得る
        int sqrtNumArea = 2;
        int sqrtNumAreaModules = sqrtNumModules / sqrtNumArea;
        sqrtNumAreaModules++;
        SamplingGrid samplingGrid = new SamplingGrid(sqrtNumArea);
        Line baseLineX, baseLineY, gridLineX, gridLineY;
		
        int logicalDistance = alignmentPattern.getLogicalDistance();
        Axis axis = new Axis(finderPattern.getAngle(), finderPattern.getModuleSize());
        ModulePitch modulePitch;
        for (int ay = 0; ay < sqrtNumArea; ay++) {
        for (int ax = 0; ax < sqrtNumArea; ax++) {
        modulePitch = new ModulePitch(); //top left bottom rightの順に格納
        baseLineX = new Line();
        baseLineY = new Line();
        axis.setModulePitch(finderPattern.getModuleSize());
        if (ax == 0 && ay == 0) {
        axis.setOrigin(centers[0][0]);
        modulePitch.top = getAreaModulePitch(axis.translate(0,3), centers[1][0], logicalDistance + 3);
        modulePitch.left = getAreaModulePitch(axis.translate(3,0), centers[0][1], logicalDistance + 3);
        axis.setModulePitch(modulePitch.top);
        modulePitch.bottom = getAreaModulePitch(centers[0][1], centers[1][1], logicalDistance);
        axis.setModulePitch(modulePitch.left);
        modulePitch.right = getAreaModulePitch(centers[1][0], centers[1][1], logicalDistance);
		
        axis.setModulePitch(modulePitch.top);
        baseLineX.setP1(axis.translate(-3,-3));
		
        axis.setModulePitch(modulePitch.left);
        baseLineY.setP1(axis.translate(-3,-3));
		
        axis.setOrigin(centers[0][1]);
        axis.setModulePitch(modulePitch.bottom);
        baseLineX.setP2(axis.translate(-6,0));
		
        axis.setOrigin(centers[1][0]);
        axis.setModulePitch(modulePitch.right);
        baseLineY.setP2(axis.translate(0,-6));
        }
        else if (ax == 1 && ay == 0) {
        axis.setOrigin(centers[1][0]);
        modulePitch.top = getAreaModulePitch(axis.translate(0,-3), centers[2][0], logicalDistance + 3);
        modulePitch.left = getAreaModulePitch(centers[1][0], centers[1][1], logicalDistance);
        axis.setModulePitch(modulePitch.top);
        modulePitch.bottom = getAreaModulePitch(centers[1][1], centers[2][1], logicalDistance);
        axis.setModulePitch(modulePitch.left);
        axis.setOrigin(centers[2][0]);
        modulePitch.right = getAreaModulePitch(axis.translate(-3,0), centers[2][1], logicalDistance + 3);
		
        axis.setOrigin(centers[1][0]);
        axis.setModulePitch(modulePitch.left);
        baseLineX.setP1(axis.translate(0,-6));
		
        baseLineY.setP1(axis.translate(0,-6));
		
        baseLineX.setP2(centers[1][1]);
		
        axis.setOrigin(centers[2][0]);
        axis.setModulePitch(modulePitch.right);
        baseLineY.setP2(axis.translate(3,-3));
        }
        else if (ax == 0 && ay == 1) {
        modulePitch.top = getAreaModulePitch(centers[0][1], centers[1][1], logicalDistance);
        axis.setOrigin(centers[0][2]);
        modulePitch.left = getAreaModulePitch(centers[0][1], axis.translate(3,0), logicalDistance + 3);
        axis.setModulePitch(modulePitch.top);
        modulePitch.bottom = getAreaModulePitch(axis.translate(0,-3), centers[1][2], logicalDistance + 3);
        axis.setModulePitch(modulePitch.bottom);
        modulePitch.right = getAreaModulePitch(centers[1][1], centers[1][2], logicalDistance);
		
        axis.setOrigin(centers[0][1]);
        axis.setModulePitch(modulePitch.top);
        baseLineX.setP1(axis.translate(-6,0));
		
        baseLineY.setP1(axis.translate(-6,0));
		
        axis.setOrigin(centers[0][2]);
        axis.setModulePitch(modulePitch.bottom);
        baseLineX.setP2(axis.translate(-3, 3));
		
        baseLineY.setP2(centers[1][1]);					
        }
        else if (ax == 1 && ay == 1) {
        modulePitch.top = getAreaModulePitch(centers[1][1], centers[2][1], logicalDistance);
        modulePitch.left = getAreaModulePitch(centers[1][1], centers[1][2], logicalDistance);
        modulePitch.bottom = getAreaModulePitch(centers[1][2], centers[2][2], logicalDistance);
        modulePitch.right = getAreaModulePitch(centers[2][1], centers[2][2], logicalDistance);
		
        baseLineX.setP1(centers[1][1]);
        baseLineY.setP1(centers[1][1]);
		
        axis.setOrigin(centers[1][2]);
        axis.setModulePitch(modulePitch.left);
        baseLineX.setP2(axis.translate(0,6));
		
        axis.setOrigin(centers[2][1]);
        axis.setModulePitch(modulePitch.top);
        baseLineY.setP2(axis.translate(6,0));
        }
		
        samplingGrid.initGrid(ax,ay, sqrtNumAreaModules, sqrtNumAreaModules);
		
        for (int i = 0; i < sqrtNumAreaModules; i++) {
        gridLineX = new Line(baseLineX.getP1(), baseLineX.getP2());
		
        axis.setOrigin(gridLineX.getP1());
        axis.setModulePitch(modulePitch.top);
        gridLineX.setP1(axis.translate(i,0));
		
        axis.setOrigin(gridLineX.getP2());
        axis.setModulePitch(modulePitch.bottom);
        gridLineX.setP2(axis.translate(i,0));
		
		
        gridLineY = new Line(baseLineY.getP1(), baseLineY.getP2());
		
        axis.setOrigin(gridLineY.getP1());
        axis.setModulePitch(modulePitch.left);
        gridLineY.setP1(axis.translate(0,i));
		
        axis.setOrigin(gridLineY.getP2());
        axis.setModulePitch(modulePitch.right);
        gridLineY.setP2(axis.translate(0,i));
		
        samplingGrid.setXLine(ax,ay,i,gridLineX);
        samplingGrid.setYLine(ax,ay,i,gridLineY);
		
        }
        }
        }
		
        for (int ay = 0; ay < samplingGrid.getHeight(); ay++) {
        for (int ax = 0; ax < samplingGrid.getWidth();ax++) {
        canvas.drawLines(samplingGrid.getXLines(ax,ay), java.awt.Color.BLUE);
        canvas.drawLines(samplingGrid.getYLines(ax,ay), java.awt.Color.BLUE);
        }
        }
		
        return samplingGrid;
        }*/
        /// <summary> Generic Sampling grid method</summary>
        internal virtual SamplingGrid GetSamplingGrid(FinderPattern finderPattern, AlignmentPattern alignmentPattern)
        {

            Point[][] centers = alignmentPattern.GetCenter();

            int version = finderPattern.Version;
            int sqrtCenters = (version / 7) + 2;

            centers[0][0] = finderPattern.GetCenter(FinderPattern.UL);
            centers[sqrtCenters - 1][0] = finderPattern.GetCenter(FinderPattern.UR);
            centers[0][sqrtCenters - 1] = finderPattern.GetCenter(FinderPattern.DL);
            //int sqrtNumModules = finderPattern.getSqrtNumModules(); /// The number of modules per one side is obtained
            int sqrtNumArea = sqrtCenters - 1;

            //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
            SamplingGrid samplingGrid = new SamplingGrid(sqrtNumArea);

            Line baseLineX, baseLineY, gridLineX, gridLineY;

            ///???
            //Point[] targetCenters;

            //int logicalDistance = alignmentPattern.getLogicalDistance();
            Axis axis = new Axis(finderPattern.GetAngle(), finderPattern.GetModuleSize());
            ModulePitch modulePitch;

            // for each area :
            for (int ay = 0; ay < sqrtNumArea; ay++)
            {
                for (int ax = 0; ax < sqrtNumArea; ax++)
                {
                    modulePitch = new ModulePitch(this); /// Housing to order
                    baseLineX = new Line();
                    baseLineY = new Line();
                    axis.ModulePitch = finderPattern.GetModuleSize();

                    Point[][] logicalCenters = AlignmentPattern.GetLogicalCenter(finderPattern);

                    Point upperLeftPoint = centers[ax][ay];
                    Point upperRightPoint = centers[ax + 1][ay];
                    Point lowerLeftPoint = centers[ax][ay + 1];
                    Point lowerRightPoint = centers[ax + 1][ay + 1];

                    Point logicalUpperLeftPoint = logicalCenters[ax][ay];
                    Point logicalUpperRightPoint = logicalCenters[ax + 1][ay];
                    Point logicalLowerLeftPoint = logicalCenters[ax][ay + 1];
                    Point logicalLowerRightPoint = logicalCenters[ax + 1][ay + 1];

                    if (ax == 0 && ay == 0)
                    // left upper corner
                    {
                        if (sqrtNumArea == 1)
                        {
                            upperLeftPoint = axis.Translate(upperLeftPoint, -3, -3);
                            upperRightPoint = axis.Translate(upperRightPoint, 3, -3);
                            lowerLeftPoint = axis.Translate(lowerLeftPoint, -3, 3);
                            lowerRightPoint = axis.Translate(lowerRightPoint, 6, 6);

                            logicalUpperLeftPoint.Translate(-6, -6);
                            logicalUpperRightPoint.Translate(3, -3);
                            logicalLowerLeftPoint.Translate(-3, 3);
                            logicalLowerRightPoint.Translate(6, 6);
                        }
                        else
                        {
                            upperLeftPoint = axis.Translate(upperLeftPoint, -3, -3);
                            upperRightPoint = axis.Translate(upperRightPoint, 0, -6);
                            lowerLeftPoint = axis.Translate(lowerLeftPoint, -6, 0);

                            logicalUpperLeftPoint.Translate(-6, -6);
                            logicalUpperRightPoint.Translate(0, -6);
                            logicalLowerLeftPoint.Translate(-6, 0);
                        }
                    }
                    else if (ax == 0 && ay == sqrtNumArea - 1)
                    // left bottom corner
                    {
                        upperLeftPoint = axis.Translate(upperLeftPoint, -6, 0);
                        lowerLeftPoint = axis.Translate(lowerLeftPoint, -3, 3);
                        lowerRightPoint = axis.Translate(lowerRightPoint, 0, 6);


                        logicalUpperLeftPoint.Translate(-6, 0);
                        logicalLowerLeftPoint.Translate(-6, 6);
                        logicalLowerRightPoint.Translate(0, 6);
                    }
                    else if (ax == sqrtNumArea - 1 && ay == 0)
                    // right upper corner
                    {
                        upperLeftPoint = axis.Translate(upperLeftPoint, 0, -6);
                        upperRightPoint = axis.Translate(upperRightPoint, 3, -3);
                        lowerRightPoint = axis.Translate(lowerRightPoint, 6, 0);

                        logicalUpperLeftPoint.Translate(0, -6);
                        logicalUpperRightPoint.Translate(6, -6);
                        logicalLowerRightPoint.Translate(6, 0);
                    }
                    else if (ax == sqrtNumArea - 1 && ay == sqrtNumArea - 1)
                    // right bottom corner
                    {
                        lowerLeftPoint = axis.Translate(lowerLeftPoint, 0, 6);
                        upperRightPoint = axis.Translate(upperRightPoint, 6, 0);
                        lowerRightPoint = axis.Translate(lowerRightPoint, 6, 6);

                        logicalLowerLeftPoint.Translate(0, 6);
                        logicalUpperRightPoint.Translate(6, 0);
                        logicalLowerRightPoint.Translate(6, 6);
                    }
                    else if (ax == 0)
                    // left side
                    {
                        upperLeftPoint = axis.Translate(upperLeftPoint, -6, 0);
                        lowerLeftPoint = axis.Translate(lowerLeftPoint, -6, 0);

                        logicalUpperLeftPoint.Translate(-6, 0);
                        logicalLowerLeftPoint.Translate(-6, 0);
                    }
                    else if (ax == sqrtNumArea - 1)
                    // right
                    {
                        upperRightPoint = axis.Translate(upperRightPoint, 6, 0);
                        lowerRightPoint = axis.Translate(lowerRightPoint, 6, 0);

                        logicalUpperRightPoint.Translate(6, 0);
                        logicalLowerRightPoint.Translate(6, 0);
                    }
                    else if (ay == 0)
                    // top
                    {
                        upperLeftPoint = axis.Translate(upperLeftPoint, 0, -6);
                        upperRightPoint = axis.Translate(upperRightPoint, 0, -6);

                        logicalUpperLeftPoint.Translate(0, -6);
                        logicalUpperRightPoint.Translate(0, -6);
                    }
                    else if (ay == sqrtNumArea - 1)
                    // bottom
                    {
                        lowerLeftPoint = axis.Translate(lowerLeftPoint, 0, 6);
                        lowerRightPoint = axis.Translate(lowerRightPoint, 0, 6);

                        logicalLowerLeftPoint.Translate(0, 6);
                        logicalLowerRightPoint.Translate(0, 6);
                    }

                    if (ax == 0)
                    {
                        logicalUpperRightPoint.Translate(1, 0);
                        logicalLowerRightPoint.Translate(1, 0);
                    }
                    else
                    {
                        logicalUpperLeftPoint.Translate(-1, 0);
                        logicalLowerLeftPoint.Translate(-1, 0);
                    }

                    if (ay == 0)
                    {
                        logicalLowerLeftPoint.Translate(0, 1);
                        logicalLowerRightPoint.Translate(0, 1);
                    }
                    else
                    {
                        logicalUpperLeftPoint.Translate(0, -1);
                        logicalUpperRightPoint.Translate(0, -1);
                    }

                    int logicalWidth = logicalUpperRightPoint.X - logicalUpperLeftPoint.X;
                    int logicalHeight = logicalLowerLeftPoint.Y - logicalUpperLeftPoint.Y;

                    if (version < 7)
                    {
                        logicalWidth += 3;
                        logicalHeight += 3;
                    }
                    modulePitch.Top = GetAreaModulePitch(upperLeftPoint, upperRightPoint, logicalWidth - 1);
                    modulePitch.Left = GetAreaModulePitch(upperLeftPoint, lowerLeftPoint, logicalHeight - 1);
                    modulePitch.Bottom = GetAreaModulePitch(lowerLeftPoint, lowerRightPoint, logicalWidth - 1);
                    modulePitch.Right = GetAreaModulePitch(upperRightPoint, lowerRightPoint, logicalHeight - 1);

                    baseLineX.SetFirstPoint(upperLeftPoint);
                    baseLineY.SetFirstPoint(upperLeftPoint);
                    baseLineX.SetSecondPoint(lowerLeftPoint);
                    baseLineY.SetSecondPoint(upperRightPoint);

                    samplingGrid.InitGrid(ax, ay, logicalWidth, logicalHeight);

                    for (int i = 0; i < logicalWidth; i++)
                    {
                        gridLineX = new Line(baseLineX.GetFirstPoint(), baseLineX.GetSecondPoint());

                        axis.Origin = gridLineX.GetFirstPoint();
                        axis.ModulePitch = modulePitch.Top;
                        gridLineX.SetFirstPoint(axis.Translate(i, 0));

                        axis.Origin = gridLineX.GetSecondPoint();
                        axis.ModulePitch = modulePitch.Bottom;
                        gridLineX.SetSecondPoint(axis.Translate(i, 0));

                        samplingGrid.SetXLine(ax, ay, i, gridLineX);
                    }

                    for (int i = 0; i < logicalHeight; i++)
                    {

                        gridLineY = new Line(baseLineY.GetFirstPoint(), baseLineY.GetSecondPoint());

                        axis.Origin = gridLineY.GetFirstPoint();
                        axis.ModulePitch = modulePitch.Left;
                        gridLineY.SetFirstPoint(axis.Translate(0, i));

                        axis.Origin = gridLineY.GetSecondPoint();
                        axis.ModulePitch = modulePitch.Right;
                        gridLineY.SetSecondPoint(axis.Translate(0, i));

                        samplingGrid.SetYLine(ax, ay, i, gridLineY);
                    }
                }
            }

            return samplingGrid;
        }



        //get module pitch in single area
        internal virtual int GetAreaModulePitch(Point start, Point end, int logicalDistance)
        {
            Line tempLine;
            tempLine = new Line(start, end);
            int realDistance = tempLine.Length;
            int modulePitch = (realDistance << DECIMAL_POINT) / logicalDistance;
            return modulePitch;
        }


        //gridLines[areaX][areaY][direction(x=0,y=1)][EachLines]	
        internal virtual bool[][] GetQRCodeMatrix(bool[][] image, SamplingGrid gridLines)
        {
            //int gridSize = gridLines.getWidth() * gridLines.getWidth(0,0);
            int gridSize = gridLines.TotalWidth;

            // now this is done within the SamplingGrid class...
            //		if (gridLines.getWidth() >= 2)
            //			gridSize-=1;

            Point bottomRightPoint = null;
            bool[][] sampledMatrix = new bool[gridSize][];
            for (int i = 0; i < gridSize; i++)
            {
                sampledMatrix[i] = new bool[gridSize];
            }
            for (int ay = 0; ay < gridLines.GetHeight(); ay++)
            {
                for (int ax = 0; ax < gridLines.GetWidth(); ax++)
                {
                    System.Collections.ArrayList sampledPoints = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)); //only for visualiz;
                    for (int y = 0; y < gridLines.GetHeight(ax, ay); y++)
                    {
                        for (int x = 0; x < gridLines.GetWidth(ax, ay); x++)
                        {
                            int x1 = gridLines.GetXLine(ax, ay, x).GetFirstPoint().X;
                            int y1 = gridLines.GetXLine(ax, ay, x).GetFirstPoint().Y;
                            int x2 = gridLines.GetXLine(ax, ay, x).GetSecondPoint().X;
                            int y2 = gridLines.GetXLine(ax, ay, x).GetSecondPoint().Y;
                            int x3 = gridLines.GetYLine(ax, ay, y).GetFirstPoint().X;
                            int y3 = gridLines.GetYLine(ax, ay, y).GetFirstPoint().Y;
                            int x4 = gridLines.GetYLine(ax, ay, y).GetSecondPoint().X;
                            int y4 = gridLines.GetYLine(ax, ay, y).GetSecondPoint().Y;

                            int e = (y2 - y1) * (x3 - x4) - (y4 - y3) * (x1 - x2);
                            int f = (x1 * y2 - x2 * y1) * (x3 - x4) - (x3 * y4 - x4 * y3) * (x1 - x2);
                            int g = (x3 * y4 - x4 * y3) * (y2 - y1) - (x1 * y2 - x2 * y1) * (y4 - y3);
                            sampledMatrix[gridLines.GetX(ax, x)][gridLines.GetY(ay, y)] = image[f / e][g / e];
                            if ((ay == gridLines.GetHeight() - 1 && ax == gridLines.GetWidth() - 1) && y == gridLines.GetHeight(ax, ay) - 1 && x == gridLines.GetWidth(ax, ay) - 1)
                                bottomRightPoint = new Point(f / e, g / e);
                            //calling canvas.drawPoint in loop can be very slow.
                            // use canvas.drawPoints if you need
                            //canvas.drawPoint(new Point(f / e,g / e), Color.RED);
                        }
                    }
                }
            }
            if (bottomRightPoint.X > image.Length - 1 || bottomRightPoint.Y > image[0].Length - 1)
                throw new System.IndexOutOfRangeException("Sampling grid pointed out of image");
            canvas.DrawPoint(bottomRightPoint, Ophelia.Tools.QRCode.Codec.Util.ColorCode.BLUE);

            return sampledMatrix;
        }
    }
}
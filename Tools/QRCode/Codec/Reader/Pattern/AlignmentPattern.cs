using System;
using QRCodeDecoder = Ophelia.Tools.QRCode.Codec.QRCodeDecoder;
using AlignmentPatternNotFoundException = Ophelia.Tools.QRCode.Exceptions.AlignmentPatternNotFoundException;
using InvalidVersionException = Ophelia.Tools.QRCode.Exceptions.InvalidVersionException;
using Ophelia.Tools.QRCode.Codec.Reader;
using Ophelia.Tools.QRCode.Geom;
using Ophelia.Tools.QRCode.Codec.Util;

namespace Ophelia.Tools.QRCode.Codec.Reader.Pattern
{
	public class AlignmentPattern
	{
        internal const int RIGHT = 1;
        internal const int BOTTOM = 2;
        internal const int LEFT = 3;
        internal const int TOP = 4;

        internal static IDebugCanvas canvas;
        internal Point[][] center;
        internal int patternDistance;

        virtual public int LogicalDistance
		{
			get
			{
				return this.patternDistance;
			}
		}
		
		internal AlignmentPattern(Point[][] center, int patternDistance)
		{
			this.center = center;
			this.patternDistance = patternDistance;
		}
		
		public static AlignmentPattern FindAlignmentPattern(bool[][] image, FinderPattern finderPattern)
		{			
			Point[][] logicalCenters = GetLogicalCenter(finderPattern);
			int logicalDistance = logicalCenters[1][0].X - logicalCenters[0][0].X;
			Point[][] centers = null;
			centers = GetCenter(image, finderPattern, logicalCenters);
			return new AlignmentPattern(centers, logicalDistance);
		}
		
		public virtual Point[][] GetCenter()
		{
			return this.center;
		}
		
		public virtual void  SetCenter(Point[][] center)
		{
			this.center = center;
		}
		
		internal static Point[][] GetCenter(bool[][] image, FinderPattern finderPattern, Point[][] logicalCenters)
		{
			int moduleSize = finderPattern.GetModuleSize();
			
			Axis axis = new Axis(finderPattern.GetAngle(), moduleSize);
			int sqrtCenters = logicalCenters.Length;
			Point[][] centers = new Point[sqrtCenters][];
			for (int i = 0; i < sqrtCenters; i++)
				centers[i] = new Point[sqrtCenters];
			
			axis.Origin = finderPattern.GetCenter(FinderPattern.UL);
			centers[0][0] = axis.Translate(3, 3);
			canvas.DrawCross(centers[0][0], Ophelia.Tools.QRCode.Codec.Util.ColorCode.BLUE);
			
			axis.Origin = finderPattern.GetCenter(FinderPattern.UR);
			centers[sqrtCenters - 1][0] = axis.Translate(- 3, 3);
			canvas.DrawCross(centers[sqrtCenters - 1][0], Ophelia.Tools.QRCode.Codec.Util.ColorCode.BLUE);
			
			axis.Origin = finderPattern.GetCenter(FinderPattern.DL);
			centers[0][sqrtCenters - 1] = axis.Translate(3, - 3);
			canvas.DrawCross(centers[0][sqrtCenters - 1], Ophelia.Tools.QRCode.Codec.Util.ColorCode.BLUE);
			
			Point tmpPoint = centers[0][0];
			
			for (int y = 0; y < sqrtCenters; y++)
			{
				for (int x = 0; x < sqrtCenters; x++)
				{
					if ((x == 0 && y == 0) || (x == 0 && y == sqrtCenters - 1) || (x == sqrtCenters - 1 && y == 0))
						continue;
					Point target = null;
					if (y == 0)
					{
						if (x > 0 && x < sqrtCenters - 1)
						{
							target = axis.Translate(centers[x - 1][y], logicalCenters[x][y].X - logicalCenters[x - 1][y].X, 0);
						}
						centers[x][y] = new Point(target.X, target.Y);
						canvas.DrawCross(centers[x][y], Ophelia.Tools.QRCode.Codec.Util.ColorCode.RED);
					}
					else if (x == 0)
					{
						if (y > 0 && y < sqrtCenters - 1)
						{
							target = axis.Translate(centers[x][y - 1], 0, logicalCenters[x][y].Y - logicalCenters[x][y - 1].Y);
						}
						centers[x][y] = new Point(target.X, target.Y);
						canvas.DrawCross(centers[x][y], Ophelia.Tools.QRCode.Codec.Util.ColorCode.RED);
					}
					else
					{
						Point t1 = axis.Translate(centers[x - 1][y], logicalCenters[x][y].X - logicalCenters[x - 1][y].X, 0);
						Point t2 = axis.Translate(centers[x][y - 1], 0, logicalCenters[x][y].Y - logicalCenters[x][y - 1].Y);
						centers[x][y] = new Point((t1.X + t2.X) / 2, (t1.Y + t2.Y) / 2 + 1);
					}
					if (finderPattern.Version > 1)
					{
						Point precisionCenter = GetPrecisionCenter(image, centers[x][y]);
						
						if (centers[x][y].DistanceOf(precisionCenter) < 6)
						{
							canvas.DrawCross(centers[x][y], Ophelia.Tools.QRCode.Codec.Util.ColorCode.RED);
							int dx = precisionCenter.X - centers[x][y].X;
							int dy = precisionCenter.Y - centers[x][y].Y;
							canvas.Println("Adjust AP(" + x + "," + y + ") to d(" + dx + "," + dy + ")");
							
							centers[x][y] = precisionCenter;
						}
					}
					canvas.DrawCross(centers[x][y], Ophelia.Tools.QRCode.Codec.Util.ColorCode.BLUE);
					canvas.DrawLine(new Line(tmpPoint, centers[x][y]), Ophelia.Tools.QRCode.Codec.Util.ColorCode.LIGHTBLUE);
					tmpPoint = centers[x][y];					
				}
			}
			return centers;
		}
				
		internal static Point GetPrecisionCenter(bool[][] image, Point targetPoint)
		{
			// find nearest dark point and update it as new rough center point 
			// when original rough center points light point 
			int targetX = targetPoint.X, targetY = targetPoint.Y;
			if ((targetX < 0 || targetY < 0) || (targetX > image.Length - 1 || targetY > image[0].Length - 1))
				throw new AlignmentPatternNotFoundException("Alignment Pattern finder exceeded out of image");
			
			if (image[targetPoint.X][targetPoint.Y] == QRCodeImageReader.POINT_LIGHT)
			{
				int scope = 0;
				bool found = false;
				while (!found)
				{
					scope++;
					for (int dy = scope; dy > - scope; dy--)
					{
						for (int dx = scope; dx > - scope; dx--)
						{
							int x = targetPoint.X + dx;
							int y = targetPoint.Y + dy;
							if ((x < 0 || y < 0) || (x > image.Length - 1 || y > image[0].Length - 1))
								throw new AlignmentPatternNotFoundException("Alignment Pattern finder exceeded out of image");
							if (image[x][y] == QRCodeImageReader.POINT_DARK)
							{
								targetPoint = new Point(targetPoint.X + dx, targetPoint.Y + dy);
								found = true;
							}
						}
					}
				}
			}
			int x2, lx, rx, y2, uy, dy2;
			x2 = lx = rx = targetPoint.X;
			y2 = uy = dy2 = targetPoint.Y;
			
			while (lx >= 1 && !TargetPointOnTheCorner(image, lx, y2, lx - 1, y2))
				lx--;
			while (rx < image.Length - 1 && !TargetPointOnTheCorner(image, rx, y2, rx + 1, y2))
				rx++;
			while (uy >= 1 && !TargetPointOnTheCorner(image, x2, uy, x2, uy - 1))
				uy--;
			while (dy2 < image[0].Length - 1 && !TargetPointOnTheCorner(image, x2, dy2, x2, dy2 + 1))
				dy2++;
			
			return new Point((lx + rx + 1) / 2, (uy + dy2 + 1) / 2);
		}
		
		internal static bool TargetPointOnTheCorner(bool[][] image, int x, int y, int nx, int ny)
		{
			if (x < 0 || y < 0 || nx < 0 || ny < 0 || x > image.Length || y > image[0].Length || nx > image.Length || ny > image[0].Length)
				throw new AlignmentPatternNotFoundException("Alignment Pattern Finder exceeded image edge");
            // Console.out.println("Overflow: x="+x+", y="+y+" nx="+nx+" ny="+ny+" x.max="+image.length+", y.max="+image[0].length);
			else
				return (image[x][y] == QRCodeImageReader.POINT_LIGHT && image[nx][ny] == QRCodeImageReader.POINT_DARK);
		}
		
		/// <summary>
        /// Get logical center coordinates of each alignment patterns
		/// </summary>
		public static Point[][] GetLogicalCenter(FinderPattern finderPattern)
		{
			int version = finderPattern.Version;
			Point[][] logicalCenters = new Point[1][];
			for (int i = 0; i < 1; i++)
			{
				logicalCenters[i] = new Point[1];
			}
			int[] logicalSeeds = new int[1];
			logicalSeeds = LogicalSeed.GetSeed(version);
			logicalCenters = new Point[logicalSeeds.Length][];
			for (int i2 = 0; i2 < logicalSeeds.Length; i2++)
			{
				logicalCenters[i2] = new Point[logicalSeeds.Length];
			}
			
			//create real relative coordinates
			for (int col = 0; col < logicalCenters.Length; col++)
			{
				for (int row = 0; row < logicalCenters.Length; row++)
				{
					logicalCenters[row][col] = new Point(logicalSeeds[row], logicalSeeds[col]);
				}
			}
			return logicalCenters;
		}
		static AlignmentPattern()
		{
			canvas = QRCodeDecoder.Canvas;
		}
	}
}
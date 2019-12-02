using System;
using QRCodeUtility = Ophelia.Tools.QRCode.Codec.Util.QRCodeUtility;

namespace Ophelia.Tools.QRCode.Geom
{
    public class Line
    {
        internal int x1, y1, x2, y2;

        virtual public bool Horizontal
        {
           get { return this.y1 == this.y2; }
        }
        virtual public bool Vertical
        {
            get { return this.x1 == this.x2; }
        }
        virtual public Point Center
        {
            get
            {
                int x = (this.x1 + this.x2) / 2;
                int y = (this.y1 + this.y2) / 2;
                return new Point(x, y);
            }

        }
        virtual public int Length
        {
            get
            {
                int x = System.Math.Abs(this.x2 - this.x1);
                int y = System.Math.Abs(this.y2 - this.y1);
                int r = QRCodeUtility.Sqrt(x * x + y * y);
                return r;
            }
        }

        public Line()
        {
            this.x1 = this.y1 = this.x2 = this.y2 = 0;
        }

        public Line(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
        public Line(Point p1, Point p2)
        {
            this.x1 = p1.X;
            this.y1 = p1.Y;
            this.x2 = p2.X;
            this.y2 = p2.Y;
        }
        public virtual Point GetFirstPoint()
        {
            return new Point(this.x1, this.y1);
        }

        public virtual Point GetSecondPoint()
        {
            return new Point(this.x2, this.y2);
        }

        public virtual void SetLine(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
        public virtual void SetFirstPoint(Point p1)
        {
            this.x1 = p1.X;
            this.y1 = p1.Y;
        }
        public virtual void SetFirstPoint(int x1, int y1)
        {
            this.x1 = x1;
            this.y1 = y1;
        }
        public virtual void SetSecondPoint(Point p2)
        {
            this.x2 = p2.X;
            this.y2 = p2.Y;
        }
        public virtual void SetSecondPoint(int x2, int y2)
        {
            this.x2 = x2;
            this.y2 = y2;
        }

        public virtual void Translate(int dx, int dy)
        {
            this.x1 += dx;
            this.y1 += dy;
            this.x2 += dx;
            this.y2 += dy;
        }

        public static bool IsNeighbor(Line firstLine, Line secondLine)
        {
            return ((System.Math.Abs(firstLine.GetFirstPoint().X - secondLine.GetFirstPoint().X) < 2 && System.Math.Abs(firstLine.GetFirstPoint().Y - secondLine.GetFirstPoint().Y) < 2) && (System.Math.Abs(firstLine.GetSecondPoint().X - secondLine.GetSecondPoint().X) < 2 && System.Math.Abs(firstLine.GetSecondPoint().Y - secondLine.GetSecondPoint().Y) < 2));
        }

        public static bool IsCross(Line firstLine, Line secondLine)
        {
            if (firstLine.Horizontal && secondLine.Vertical)
            {
                if (firstLine.GetFirstPoint().Y > secondLine.GetFirstPoint().Y && firstLine.GetFirstPoint().Y < secondLine.GetSecondPoint().Y && secondLine.GetFirstPoint().X > firstLine.GetFirstPoint().X && secondLine.GetFirstPoint().X < firstLine.GetSecondPoint().X)
                    return true;
            }
            else if (firstLine.Vertical && secondLine.Horizontal)
            {
                if (firstLine.GetFirstPoint().X > secondLine.GetFirstPoint().X && firstLine.GetFirstPoint().X < secondLine.GetSecondPoint().X && secondLine.GetFirstPoint().Y > firstLine.GetFirstPoint().Y && secondLine.GetFirstPoint().Y < firstLine.GetSecondPoint().Y)
                    return true;
            }

            return false;
        }
        public static Line GetLongestLine(Line[] lines)
        {
            Line longestLine = new Line();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > longestLine.Length)
                {
                    longestLine = lines[i];
                }
            }
            return longestLine;
        }
        public override String ToString()
        {
            return "(" + System.Convert.ToString(this.x1) + "," + System.Convert.ToString(this.y1) + ")-(" + System.Convert.ToString(this.x2) + "," + System.Convert.ToString(this.y2) + ")";
        }
    }
}
using System;
using QRCodeUtility = Ophelia.Tools.QRCode.Codec.Util.QRCodeUtility;

namespace Ophelia.Tools.QRCode.Geom
{
    public class Point
    {
        public const int RIGHT = 1;
        public const int BOTTOM = 2;
        public const int LEFT = 4;
        public const int TOP = 8;

        internal int x;
        internal int y;

        virtual public int X
        {
            get { return x; }
            set { this.x = value; }
        }
        virtual public int Y
        {
            get { return y; }
            set { this.y = value; }
        }

        public Point()
        {
            this.x = 0;
            this.y = 0;
        }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void Translate(int dx, int dy)
        {
            this.x += dx;
            this.y += dy;
        }

        public virtual void SetRenamed(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override String ToString()
        {
            return "(" + System.Convert.ToString(x) + "," + System.Convert.ToString(y) + ")";
        }

        public static Point GetCenter(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public bool Equals(Point compare)
        {
            if (x == compare.x && y == compare.y)
                return true;
            else
                return false;
        }

        public virtual int DistanceOf(Point other)
        {
            int x2 = other.X;
            int y2 = other.Y;
            return QRCodeUtility.Sqrt((x - x2) * (x - x2) + (y - y2) * (y - y2));
        }
    }
}
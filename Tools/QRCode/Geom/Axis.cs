using System;
using QRCodeImageReader = Ophelia.Tools.QRCode.Codec.Reader.QRCodeImageReader;
namespace Ophelia.Tools.QRCode.Geom
{
    public class Axis
    {
        internal int sin, cos;
        internal int modulePitch;
        internal Point origin;

        virtual public Point Origin
        {
            set
            {
                this.origin = value;
            }

        }
        virtual public int ModulePitch
        {
            set
            {
                this.modulePitch = value;
            }

        }

        public Axis(int[] angle, int modulePitch)
        {
            this.sin = angle[0];
            this.cos = angle[1];
            this.modulePitch = modulePitch;
            this.origin = new Point();
        }

        public virtual Point Translate(Point offset)
        {
            int moveX = offset.X;
            int moveY = offset.Y;
            return this.Translate(moveX, moveY);
        }

        public virtual Point Translate(Point origin, Point offset)
        {
            Origin = origin;
            int moveX = offset.X;
            int moveY = offset.Y;
            return this.Translate(moveX, moveY);
        }

        public virtual Point Translate(Point origin, int moveX, int moveY)
        {
            Origin = origin;
            return this.Translate(moveX, moveY);
        }

        public virtual Point Translate(Point origin, int modulePitch, int moveX, int moveY)
        {
            Origin = origin;
            this.modulePitch = modulePitch;
            return this.Translate(moveX, moveY);
        }

        public virtual Point Translate(int moveX, int moveY)
        {
            long dp = QRCodeImageReader.DECIMAL_POINT;
            Point point = new Point();
            int dx = (moveX == 0) ? 0 : (modulePitch * moveX) >> (int)dp;
            int dy = (moveY == 0) ? 0 : (modulePitch * moveY) >> (int)dp;
            point.Translate((dx * cos - dy * sin) >> (int)dp, (dx * sin + dy * cos) >> (int)dp);
            point.Translate(origin.X, origin.Y);
            return point;
        }
    }
}
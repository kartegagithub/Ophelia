using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Tools.QRCode.Geom
{
    internal class AreaGrid
    {
        private SamplingGrid enclosingInstance;
        private Line[] xLine;
        private Line[] yLine;

        private void InitBlock(SamplingGrid enclosingInstance)
        {
            this.enclosingInstance = enclosingInstance;
        }

        virtual public int Width
        {
            get
            {
                return (xLine.Length);
            }

        }
        virtual public int Height
        {
            get
            {
                return (yLine.Length);
            }

        }
        virtual public Line[] XLines
        {
            get
            {
                return xLine;
            }

        }
        virtual public Line[] YLines
        {
            get
            {
                return yLine;
            }

        }
        public SamplingGrid EnclosingInstance
        {
            get
            {
                return enclosingInstance;
            }

        }

        public AreaGrid(SamplingGrid enclosingInstance, int width, int height)
        {
            InitBlock(enclosingInstance);
            xLine = new Line[width];
            yLine = new Line[height];
        }

        public virtual Line GetXLine(int x)
        {
            return xLine[x];
        }

        public virtual Line GetYLine(int y)
        {
            return yLine[y];
        }

        public virtual void SetXLine(int x, Line line)
        {
            xLine[x] = line;
        }

        public virtual void SetYLine(int y, Line line)
        {
            yLine[y] = line;
        }
    }
}

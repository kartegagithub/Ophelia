using System;
namespace Ophelia.Tools.QRCode.Geom
{
    public class SamplingGrid
    {
        virtual public int TotalWidth
        {
            get
            {
                int total = 0;
                for (int i = 0; i < grid.Length; i++)
                {
                    total += grid[i][0].Width;
                    if (i > 0)
                        total -= 1;
                }
                return total;
            }

        }
        virtual public int TotalHeight
        {
            get
            {
                int total = 0;
                for (int i = 0; i < grid[0].Length; i++)
                {
                    total += grid[0][i].Height;
                    if (i > 0)
                        total -= 1;
                }
                return total;
            }

        }

        private AreaGrid[][] grid;

        public SamplingGrid(int sqrtNumArea)
        {
            grid = new AreaGrid[sqrtNumArea][];
            for (int i = 0; i < sqrtNumArea; i++)
            {
                grid[i] = new AreaGrid[sqrtNumArea];
            }
        }

        public virtual void InitGrid(int ax, int ay, int width, int height)
        {
            grid[ax][ay] = new AreaGrid(this, width, height);
        }

        public virtual void SetXLine(int ax, int ay, int x, Line line)
        {
            grid[ax][ay].SetXLine(x, line);
        }

        public virtual void SetYLine(int ax, int ay, int y, Line line)
        {
            grid[ax][ay].SetYLine(y, line);
        }

        public virtual Line GetXLine(int ax, int ay, int x)
        {
            return (grid[ax][ay].GetXLine(x));
        }

        public virtual Line GetYLine(int ax, int ay, int y)
        {
            return (grid[ax][ay].GetYLine(y));
        }

        public virtual Line[] GetXLines(int ax, int ay)
        {
            return (grid[ax][ay].XLines);
        }

        public virtual Line[] GetYLines(int ax, int ay)
        {
            return (grid[ax][ay].YLines);
        }

        public virtual int GetWidth()
        {
            return (grid[0].Length);
        }

        public virtual int GetHeight()
        {
            return (grid.Length);
        }

        public virtual int GetWidth(int ax, int ay)
        {
            return (grid[ax][ay].Width);
        }

        public virtual int GetHeight(int ax, int ay)
        {
            return (grid[ax][ay].Height);
        }

        public virtual int GetX(int ax, int x)
        {
            int total = x;
            for (int i = 0; i < ax; i++)
            {
                total += grid[i][0].Width - 1;
            }
            return total;
        }

        public virtual int GetY(int ay, int y)
        {
            int total = y;
            for (int i = 0; i < ay; i++)
            {
                total += grid[0][i].Height - 1;
            }
            return total;
        }

        public virtual void Adjust(Point adjust)
        {
            int dx = adjust.X, dy = adjust.Y;
            for (int ay = 0; ay < grid[0].Length; ay++)
            {
                for (int ax = 0; ax < grid.Length; ax++)
                {
                    for (int i = 0; i < grid[ax][ay].XLines.Length; i++)
                        grid[ax][ay].XLines[i].Translate(dx, dy);
                    for (int j = 0; j < grid[ax][ay].YLines.Length; j++)
                        grid[ax][ay].YLines[j].Translate(dx, dy);
                }
            }
        }
    }
}
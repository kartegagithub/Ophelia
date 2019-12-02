using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ophelia.Tools.QRCode.Codec.Data
{
    public class QRCodeImage : IQRCodeImage
    {
        private Bitmap Image;

        public QRCodeImage(Bitmap image)
        {
            this.Image = image;
        }
        virtual public int Width
        {
            get
            {
                return this.Image.Width;
            }
        }
        virtual public int Height
        {
            get
            {
                return this.Image.Height;
            }
        }
        public virtual int GetPixel(int x, int y)
        {
            return this.Image.GetPixel(x, y).ToArgb();
        }
    }
}

using System;

namespace Ophelia.Tools.QRCode.Codec.Data
{
    public interface IQRCodeImage
    {
        int Width { get; }
        int Height { get; }
        int GetPixel(int x, int y);
    }
}
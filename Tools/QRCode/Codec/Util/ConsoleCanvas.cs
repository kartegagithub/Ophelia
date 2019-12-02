using System;
using Line = Ophelia.Tools.QRCode.Geom.Line;
using Point = Ophelia.Tools.QRCode.Geom.Point;

namespace Ophelia.Tools.QRCode.Codec.Util
{
    public class ConsoleCanvas : IDebugCanvas
    {

        public void Println(String text)
        {
            Console.WriteLine(text);
        }

        public void DrawPoint(Point point, int color)
        {
        }

        public void DrawCross(Point point, int color)
        {

        }

        public void DrawPoints(Point[] points, int color)
        {
        }

        public void DrawLine(Line line, int color)
        {
        }

        public void DrawLines(Line[] lines, int color)
        {
        }

        public void DrawPolygon(Point[] points, int color)
        {
        }

        public void DrawMatrix(bool[][] matrix)
        {
        }

    }
}

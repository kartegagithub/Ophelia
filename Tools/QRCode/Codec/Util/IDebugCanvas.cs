using System;
using Line = Ophelia.Tools.QRCode.Geom.Line;
using Point = Ophelia.Tools.QRCode.Geom.Point;

namespace Ophelia.Tools.QRCode.Codec.Util
{
	public interface IDebugCanvas
	{
		void  Println(String text);
		void  DrawPoint(Point point, int color);
		void  DrawCross(Point point, int color);
		void  DrawPoints(Point[] points, int color);
		void  DrawLine(Line line, int color);
		void  DrawLines(Line[] lines, int color);
		void  DrawPolygon(Point[] points, int color);
		void  DrawMatrix(bool[][] matrix);
	}
}
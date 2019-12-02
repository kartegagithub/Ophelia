using System;
using Line = Ophelia.Tools.QRCode.Geom.Line;
using Point = Ophelia.Tools.QRCode.Geom.Point;

namespace Ophelia.Tools.QRCode.Codec.Util
{
	public class DebugCanvasAdapter : IDebugCanvas
	{
		public virtual void  Println(String stringRenamed)
		{
		}
		
		public virtual void  DrawPoint(Point point, int color)
		{
		}
		
		public virtual void  DrawCross(Point point, int color)
		{
		}
		
		public virtual void  DrawPoints(Point[] points, int color)
		{
		}
		
		public virtual void  DrawLine(Line line, int color)
		{
		}
		
		public virtual void  DrawLines(Line[] lines, int color)
		{
		}
		
		public virtual void  DrawPolygon(Point[] points, int color)
		{
		}
		
		public virtual void  DrawMatrix(bool[][] matrix)
		{
		}
		
	}
}
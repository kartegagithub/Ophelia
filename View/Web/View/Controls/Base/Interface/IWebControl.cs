using System.Web;
namespace Ophelia.Web.View.Controls
{
	public interface IWebControl
	{
		UI.IPage Page { get; }
		IContainer Container { get; set; }
		IComplexWebControl ParentControl { get; set; }
		HttpRequest Request { get; }
		Controls.ServerSide.ScriptManager.Script Script { get; }
		StyleSheet StyleSheet { get; }
		Style Style { get; }
		string ID { get; set; }
		
		string OnClickEvent { get; set; }
		string OnDoubleClickEvent { get; set; }
		string OnKeyDownEvent { get; set; }
		string OnMouseMoveEvent { get; set; }
		string OnMouseOverEvent { get; set; }
		string OnMouseOutEvent { get; set; }
		string OnMouseUpEvent { get; set; }
		string OnKeyUpEvent { get; set; }
		string OnKeyPressEvent { get; set; }
		string Draw();
	}
}


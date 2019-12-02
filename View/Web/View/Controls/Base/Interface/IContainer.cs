namespace Ophelia.Web.View.Controls
{
	public interface IContainer : IComplexWebControl
	{
		WebControlCollection Controls { get; }
		Controls.ServerSide.ScriptManager.ScriptCollection ScriptCollection { get; }
		Content Content { get; }
	}
}

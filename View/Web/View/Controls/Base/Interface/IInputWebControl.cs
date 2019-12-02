namespace Ophelia.Web.View.Controls
{
	public interface IInputDataControl : IDataControl
	{
		string OnChangeEvent { get; set; }
		string OnBlurEvent { get; set; }
		string OnFocusEvent { get; set; }
		string Name { get; set; }
		bool Disable { get; set; }
		int TabIndex { get; set; }
	}
}

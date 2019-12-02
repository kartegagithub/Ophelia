namespace Ophelia.Web.View.Controls
{
	public interface IDataControl : IWebControl
	{
		string Value { get; set; }
		bool ReadOnly { get; set; }
		Ophelia.Web.View.Controls.Validator.ValidatorCollection Validators { get; }
	}
}

namespace Ophelia.Web.View.Controls
{
	public abstract class AjaxControl : Container, IAjaxControl
	{
		protected abstract void OnLoad();
		public override void OnBeforeDraw(Content Content)
		{
			this.OnLoad();
			base.OnBeforeDraw(Content);
		}
		protected internal virtual bool Authorize()
		{
			return true;
		}
		public QueryString QueryString {
			get { return new QueryString(this.Request); }
		}
		public object AddAjax(string FunctionName, string NeededElementIDs)
		{
			this.AddAjaxEvent(FunctionName, "", FunctionName, NeededElementIDs, true);
			return FunctionName + "();";
		}
		public object AddAjax(string FunctionName, string NeededElementIDs, string FunctionParameters)
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.AjaxFunction ajaxFunction = this.AddAjaxEvent(FunctionName, "", FunctionName, NeededElementIDs, true);
			if (ajaxFunction != null) {
				string[] Params = FunctionParameters.Split(",");
				string Param = "";
				for (int i = 0; i <= Params.Count - 1; i++) {
					Param = Params[i].Trim();
					ajaxFunction.Parameters.Add("o" + Param);
					ajaxFunction.AjaxRequestParameter.Add(Param, "' + o" + Param + " + '");
				}
				return FunctionName + "();";
			}
			return FunctionName + "();";
		}

		public virtual void CustomizeAjaxFunctionProperties(ServerSide.ScriptManager.AjaxFunction AjaxFunction)
		{
		}
		private ServerSide.ScriptManager.AjaxFunction AddAjaxEvent(string EventName, string FunctionParametersInText, string CallBackFunction, string ParameterMemberNames, bool NeedApplication = false)
		{
			if (this.Script.Function(EventName) == null) {
				ServerSide.ScriptManager.AjaxFunction Ajax = this.Script.AddAjaxEvent(EventName, this.GetType().FullName, this.ID, FunctionParametersInText, CallBackFunction, ParameterMemberNames, NeedApplication);
				this.CustomizeAjaxFunctionProperties(Ajax);
				this.Script.oFunctionsTable(EventName) = Ajax;
				return Ajax;
			}
			return null;
		}
		private ServerSide.ScriptManager.AjaxFunction AddAjaxEvent(string EventName, string CallBackFunction, string ParameterMemberNames, bool NeedApplication = false)
		{
			return this.AddAjaxEvent(EventName, "", CallBackFunction, ParameterMemberNames, NeedApplication);
		}
	}
}

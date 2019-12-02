using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.ServerSide.ScriptManager
{
	public class Function
	{
		private Script oScript;
		private string sName = "";
		private ArrayList oParameters = new ArrayList();
		private Content oContent;
		private bool bIsDrawned;
		private bool IsDrawned {
			get { return this.bIsDrawned; }
		}
		public Script Script {
			get { return this.oScript; }
		}
		protected Content Content {
			get {
				if (this.oContent == null) {
					this.oContent = new Content();
				}
				return this.oContent;
			}
		}
		public string Name {
			get { return this.sName; }
		}
		public ArrayList Parameters {
			get { return this.oParameters; }
		}
		public string ParametersInText {
			get {
				string ReturnString = "";
				for (int i = 0; i <= this.Parameters.Count - 1; i++) {
					if (!string.IsNullOrEmpty(ReturnString))
						ReturnString += ",";
					ReturnString += this.Parameters[i];
				}
				return ReturnString;
			}
		}
		public void AddAjaxValidation(AjaxFunction AjaxFunction, string AjaxFunctionParameterInText, string AjaxFunctionProcessEnd, string CheckValue = "true")
		{
			if (!AjaxFunction.OnAfterRequestFinished.Contains(AjaxFunctionProcessEnd)) {
				AjaxFunction.OnAfterRequestFinished += AjaxFunction.Name + "_validvalue = true;" + AjaxFunctionProcessEnd;
				this.Script.AppendLine("var " + AjaxFunction.Name + "_validvalue = false;");
			}
			this.AppendLine("if (" + AjaxFunction.Name + "_validvalue == false)");
			this.AppendLine("{");
			this.AppendLine(AjaxFunction.Name + "(" + AjaxFunctionParameterInText + ")");
			this.AppendLine("return false;");
			this.AppendLine("}");
		}
		public string AddValidationRequirement(string ElementID, string PropertyName, string CheckElementID, string CheckElementPropertyName, string Message, string OnBeforeReturnResult = "", FilterComparison FilterComparison = FilterComparison.Equal, bool AppendLine = true)
		{
			return this.AddValidationRequirement(ElementID, this.GetElement(CheckElementID, false, false, false) + "." + CheckElementPropertyName, Message, PropertyName, FilterComparison, OnBeforeReturnResult, AppendLine);
		}
		public string AddValidationRequirement(string ElementID, string Value, string Message, string CheckProperty, bool AppendLine = true)
		{
			return this.AddValidationRequirement(ElementID, Value, Message, CheckProperty, FilterComparison.Equal, "", AppendLine);
		}
		public string AddValidationRequirement(string ElementID, string Value, string Message, bool AppendLine = true)
		{
			return this.AddValidationRequirement(ElementID, Value, Message, "value", FilterComparison.Equal, "", AppendLine);
		}
		public string AddValidationRequirement(string ElementID, string Value, string Message, string CheckProperty, FilterComparison FilterComparison, string OnBeforeReturnResult = "", bool AppendLine = true)
		{
			string FilterComparisonInText = "";
			switch (FilterComparison) {
				case Function.FilterComparison.Bigger:
					FilterComparisonInText = " > ";
					break;
				case Function.FilterComparison.BiggerOrEqual:
					FilterComparisonInText = " >= ";
					break;
				case Function.FilterComparison.Equal:
					FilterComparisonInText = " == ";
					break;
				case Function.FilterComparison.NotEqual:
					FilterComparisonInText = " != ";
					break;
				case Function.FilterComparison.Smaller:
					FilterComparisonInText = " < ";
					break;
				case Function.FilterComparison.SmallerEqual:
					FilterComparisonInText = " <= ";
					break;
			}
			if (string.IsNullOrEmpty(Value))
				Value = "''";
			return this.AddCondition(this.GetElement(ElementID, false, false, false) + "." + CheckProperty + FilterComparisonInText + Value, this.AddMessage(Message, false) + this.FocusElement(ElementID, false, false) + OnBeforeReturnResult + " return false;", false, AppendLine);
		}
		public string AddCondition(string Condition, string TruePart, bool IsDependent, bool AppendLine = true)
		{
			if (IsDependent) {
				string ReturnString = "else ";
				if (!string.IsNullOrEmpty(Condition)) {
					ReturnString += "if (" + Condition + ")";
				}
				ReturnString += "{" + (this.Script.DebugMode ? Constants.vbCrLf : "");
				ReturnString += TruePart + (this.Script.DebugMode ? Constants.vbCrLf : "");
				ReturnString += "}";
				if (AppendLine) {
					this.AppendLine(ReturnString);
				}
				return ReturnString;
			} else {
				return this.AddCondition(Condition, TruePart, , AppendLine);
			}
		}
		public string AddCondition(string Condition, string TruePart, string FalsePart = "", bool AppendLine = true)
		{
			string ReturnString = "if (" + Condition + "){" + (this.Script.DebugMode ? Constants.vbCrLf : "");
			ReturnString += TruePart + (this.Script.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "}" + (this.Script.DebugMode ? Constants.vbCrLf : "");
			if (!string.IsNullOrEmpty(FalsePart)) {
				ReturnString += "else {" + (this.Script.DebugMode ? Constants.vbCrLf : "");
				ReturnString += FalsePart + (this.Script.DebugMode ? Constants.vbCrLf : "");
				ReturnString += "}";
			}
			if (AppendLine) {
				this.AppendLine(ReturnString);
			}
			return ReturnString;
		}
		public string AddVariable(string Name, string DefaultValue = "", bool Appendline = true)
		{
			string ReturnString = View.Web.Controls.ServerSide.ScriptManager.Utility.AddVariable(Name, DefaultValue);
			if (Appendline) {
				this.AppendLine(ReturnString);
			}
			return ReturnString;
		}
		public string GetElement(string ID, bool IsVariable, bool UseEndBracket = false, bool AppendLine = true)
		{
			string ReturnString = View.Web.Controls.ServerSide.ScriptManager.Utility.GetElement(ID, IsVariable, UseEndBracket);
			if (AppendLine) {
				this.AppendLine(ReturnString);
			}
			return ReturnString;
		}
		public string GetElementValue(string ID, bool IsVariable, bool UseEndBracket = true, bool AppendLine = true)
		{
			string ReturnString = View.Web.Controls.ServerSide.ScriptManager.Utility.GetElementValue(ID, IsVariable, UseEndBracket);
			if (AppendLine) {
				this.AppendLine(ReturnString);
			}
			return ReturnString;
		}
		public string AddMessage(string Message, bool AppendLine = true)
		{
			string ReturnString = View.Web.Controls.ServerSide.ScriptManager.Utility.AddMessage(Message);
			if (AppendLine) {
				this.AppendLine(ReturnString);
			}
			return ReturnString;
		}
		public string FocusElement(string ID, bool IsVariable = true, bool AppendLine = true)
		{
			string ReturnString = View.Web.Controls.ServerSide.ScriptManager.Utility.FocusElement(ID, IsVariable);
			return ReturnString;
		}
		public void AppendLine(string Value)
		{
			if (this.Script.DebugMode) {
				this.Content.Add(Value + Constants.vbCrLf);
			} else {
				this.Content.Add(Value);
			}
		}
		internal void ReturnSuccessMessage()
		{
			this.AppendLine("if (Message == '') {");
			this.AppendLine("Control.style.backgroundColor ='#FFFFFF' ;");
			this.AppendLine("}");
			this.AppendLine("return true;");
		}
		internal void ReturnErrorMessage()
		{
			this.AppendLine("Control.focus();");
			this.AppendLine("if (Message != '') {");
			this.AppendLine("alert(Message);");
			this.AppendLine("}");
			//Me.AppendLine("else {;")
			//Me.AppendLine("var MessageLabel = document.createElement(""div"");")
			//Me.AppendLine("MessageLabel.id=""MessageLabel"";")
			//Me.AppendLine("MessageLabel.innerHTML='deneme';")
			//Me.AppendLine("MessageLabel.style.backgroundColor='#DCDCDC';")
			//Me.AppendLine("MessageLabel.style.color='#AE2039';")
			//Me.AppendLine("MessageLabel.style.height=""18px"";")
			//Me.AppendLine("MessageLabel.style.width=""120px"";")
			//Me.AppendLine("MessageLabel.style.opacity=""0.8"";")
			//Me.AppendLine("MessageLabel.style.paddingTop=""4px"";")
			//Me.AppendLine("MessageLabel.style.marginTop=""6px"";")
			//Me.AppendLine("MessageLabel.style.marginLeft=""-10px"";")
			//Me.AppendLine("MessageLabel.style.position=""absolute"";")
			//Me.AppendLine("Control.parentNode.appendChild(MessageLabel);")
			//Me.AppendLine("}")
			this.AppendLine("return false;");
		}
		internal void ReturnErrorTooltip()
		{
			this.AppendLine("Control.setAttribute('bt-xtitle', Message);");
			this.AppendLine("$('#'+Control.id).bt({trigger: ['focus', 'blur'],positions: ['top'],fill: 'black',strokeWidth: 0,cssStyles: {color: 'white', fontWeight: 'bold'}});");
			this.AppendLine("setTimeout(function () {Control.focus();}, 100);");
			this.AppendLine("return false;");
		}
		public void AddConfirmMessage(string Message, string UncomfirmContent = "")
		{
			this.AppendLine("if (!confirm(\"" + Message + "\")) {");
			this.AppendLine(UncomfirmContent);
			this.AppendLine("return false;");
			this.AppendLine("}");
		}
		public void CreateOption(string Control, string Text, string Value)
		{
			this.AppendLine("var Option = document.createElement(\"OPTION\");");
			this.AppendLine("Option.text = " + Text + ";");
			this.AppendLine("Option.value = " + Value + ";");
			this.AppendLine("document.getElementById('" + Control + "').options.add(Option);");
		}
		public virtual string Draw()
		{
			if (!this.bIsDrawned) {
				this.bIsDrawned = true;
				string ReturnString = Constants.vbCrLf + "function " + this.Name + "(" + this.ParametersInText + "){" + (this.Script.DebugMode ? Constants.vbCrLf : "") + this.Content.Value + "}";
				return ReturnString;
			}
			return "";
		}
		public Function(string Name, Script Script)
		{
			this.oScript = Script;
			this.sName = Name;
		}
		public enum FilterComparison
		{
			Equal = 1,
			Bigger = 2,
			BiggerOrEqual = 3,
			Smaller = 4,
			SmallerEqual = 5,
			NotEqual = 6
		}
	}
}

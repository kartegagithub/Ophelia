using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.UI;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager.AjaxFunction;

namespace Ophelia.Web.View.Controls.ServerSide.ScriptManager
{
	public class Script
	{
		private ScriptCollection oScriptCollection;
		private string sName;
		private Content oContentBuilder;
		private string sPath = "";
		//Private nHttpObjectCount As Integer = 0
		public bool DebugMode = true;
		private ArrayList oFunctions;
		public Hashtable oFunctionsTable;
		private string sTruePart = "";
		private Hashtable oGlobalVariables;
		private bool bEnabled = true;
		private bool bAsync = false;
		private bool bIsDefered = false;
		private string sOnLoadFunction = "";
		private AjaxQueueManagementType eAjaxQueueType = AjaxQueueManagementType.UseMultipleRequest;
		public ScriptCollection Manager {
			get { return this.oScriptCollection; }
		}
		public string Name {
			get { return this.sName; }
			set { this.sName = value; }
		}
		public bool IsDefered {
			get { return this.bIsDefered; }
			set { this.bIsDefered = value; }
		}
		public bool Async {
			get { return this.bAsync; }
			set { this.bAsync = value; }
		}
		public string OnLoadFunction {
			get { return this.sOnLoadFunction; }
			set { this.sOnLoadFunction = value; }
		}
		public bool Enabled {
			get { return this.bEnabled; }
			set { this.bEnabled = value; }
		}
		public AjaxQueueManagementType AjaxQueueType {
			get { return this.eAjaxQueueType; }
			set { this.eAjaxQueueType = value; }
		}
		private Content ContentBuilder {
			get {
				if (this.oContentBuilder == null) {
					this.oContentBuilder = new Content();
				}
				return this.oContentBuilder;
			}
		}
		public string Path {
			get { return this.sPath; }
			set { this.sPath = value; }
		}
		public Function Function {
			get {
				if (this.oFunctionsTable == null)
					this.oFunctionsTable = new Hashtable();
				return this.oFunctionsTable[FunctionName];
			}
		}
		public ArrayList Functions {
			get {
				if (this.oFunctions == null) {
					this.oFunctions = new ArrayList();
				}
				return this.oFunctions;
			}
		}
		public bool HasFunction(string Name)
		{
			if (this.Functions != null) {
				foreach (Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function Function in this.Functions) {
					if (Function.Name == Name) {
						return true;
					}
				}
			}
			return false;
		}
		internal int HttpObjectCount {
			get {
				if (Ophelia.Web.View.UI.Current != null && Ophelia.Web.View.UI.Current.Session != null) {
					if (Ophelia.Web.View.UI.Current.Session("HttpObjectCount") != null && int.TryParse(Ophelia.Web.View.UI.Current.Session("HttpObjectCount"), out Ophelia.Web.View.UI.Current.Session("HttpObjectCount"))) {
						return Ophelia.Web.View.UI.Current.Session("HttpObjectCount");
					} else {
						Ophelia.Web.View.UI.Current.Session("HttpObjectCount") = 0;
						return 0;
					}
				}
			}
		}
		private string CacheName {
			get { return "Script_" + this.Name; }
		}
		private bool HasCachedContent {
			get { return (Content != null); }
		}
		public string Content {
			get { return CacheManager.GetCachedObject(this.CacheName); }
		}
		public string AddVariable(string Name, string DefaultValue = "", bool Appendline = true)
		{
			string ReturnString = View.Web.Controls.ServerSide.ScriptManager.Utility.AddVariable(Name, DefaultValue);
			if (Appendline) {
				this.AppendLine(ReturnString);
				return "";
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
		public string GetElementValue(string ID, bool IsVariable = true, bool UseEndBracket = true, bool AppendLine = true)
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
		internal string AddHttpObject(bool Appendline = true)
		{
			//Me.nHttpObjectCount += 1
			if (Ophelia.Web.View.UI.Current.Session != null) {
				Ophelia.Web.View.UI.Current.Session("HttpObjectCount") = this.HttpObjectCount + 1;
			}
			return this.AddVariable("http_" + this.HttpObjectCount, "createobject()", Appendline);
		}
		internal string GetHttpObject(int Index)
		{
			if (Index < 0 || Index > this.HttpObjectCount) {
				return "http_" + this.HttpObjectCount;
			}
			return "http_" + Index;
		}
		public Hashtable GlobalVariables {
			get {
				if (this.oGlobalVariables == null)
					oGlobalVariables = new Hashtable();
				return this.oGlobalVariables;
			}
		}
		public void AddGlobalVariable(string Name, string DefaultValue = "")
		{
			this.GlobalVariables[Name] = DefaultValue;
		}
		public ScriptManager.Function AddFunction(string Name, string Content = "", string ParametersInText = "", string ErrorControl = "")
		{
			if (this.Function[Name] == null) {
				ScriptManager.Function Function = new ScriptManager.Function(Name, this);
				Function.AppendLine(Content);
				string[] Parameters = ParametersInText.Split(",");
				for (int i = 0; i <= Parameters.Length - 1; i++) {
					Function.Parameters.Add(Parameters[i]);
				}
				this.Functions.Add(Function);
				this.oFunctionsTable.Add(Name, Function);
				return Function;
			}
			return this.Function[Name];
		}
		public ServerSide.ScriptManager.AjaxFunction AddAjaxEvent(string EventName, string AjaxControlType, string FunctionParametersInText, string CallBackFunction, string ParameterMemberNames, bool NeedApplication = false, bool AddParameters = false)
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.AjaxFunction AjaxFunction = this.AddAjaxFunction(EventName, "", "", FunctionParametersInText, false, NeedApplication);
			string[] Parameters = ParameterMemberNames.Split(",");
			for (int i = 0; i <= Parameters.Count - 1; i++) {
				if (!string.IsNullOrEmpty(Parameters[i])) {
					AjaxFunction.AjaxRequestParameter.Add(Parameters[i], "'+ DecideElementValueScript('" + Parameters[i] + "') +'");
				}
			}

			if (AddParameters) {
				string[] ParametersInText = FunctionParametersInText.Split(",");
				for (int i = 0; i <= ParametersInText.Count - 1; i++) {
					string ParameterInText = ParametersInText[i];
					if (!string.IsNullOrEmpty(ParameterInText) && ("," + ParameterMemberNames + ",").IndexOf("," + ParameterInText + "i") == -1) {
						AjaxFunction.AjaxRequestParameter.Add(ParametersInText[i], "'+" + ParameterInText + "+'");
					}
				}
			}

			AjaxFunction.AjaxRequestParameter.Add("AjaxControlType", AjaxControlType);
			AjaxFunction.AjaxRequestParameter.Add("CallBackFunctionName", CallBackFunction);

			AjaxFunction.AppendLine("CallbackAjaxFunction( " + AjaxFunction.HttpObjectResult + ");");
			this.AddAjaxHtmlParserFunction();
			this.AddDecideElementValueScript();
			return AjaxFunction;
		}
		public ServerSide.ScriptManager.AjaxFunction AddAjaxEvent(string EventName, string AjaxControlType, string AjaxControlID, string FunctionParametersInText, string CallBackFunction, string ParameterMemberNames, bool NeedApplication = false, bool AddParameters = false)
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.AjaxFunction AjaxFunction = this.AddAjaxEvent(EventName, AjaxControlType, FunctionParametersInText, CallBackFunction, ParameterMemberNames, NeedApplication, AddParameters);
			AjaxFunction.AjaxRequestParameter.Add("AjaxControlID", AjaxControlID);
			return AjaxFunction;
		}
		public ServerSide.ScriptManager.AjaxFunction AddAjaxEvent(string EventName, string AjaxControlType, string CallBackFunction, string ParameterMemberNames, bool NeedApplication = false)
		{
			return this.AddAjaxEvent(EventName, AjaxControlType, "", "", CallBackFunction, ParameterMemberNames, NeedApplication);
		}
		public ServerSide.ScriptManager.AjaxFunction AddAjaxEvent(string EventName, string AjaxControlType, string CallBackFunction, string FunctionParametersInText, bool NeedApplication, bool AddParameters, bool ShowOverlay = false)
		{
			ServerSide.ScriptManager.AjaxFunction AjaxFunction = this.AddAjaxEvent(EventName, AjaxControlType, "", FunctionParametersInText, CallBackFunction, string.Empty, NeedApplication, AddParameters);
			AjaxFunction.ShowOverlay = ShowOverlay;
			return AjaxFunction;
		}
		public ServerSide.ScriptManager.AjaxFunction AddAjaxFunction(string Name, string ReadyFunctionContent, string Page, string ParametersInText, bool PageIsSecure, bool NeedApplication)
		{
			ScriptManager.AjaxFunction AjaxFunction = this.AddAjaxFunction(Name, ReadyFunctionContent, ParametersInText, NeedApplication);
			AjaxFunction.PageIsSecure = PageIsSecure;
			AjaxFunction.PageName = Page;
			return AjaxFunction;
		}
		public ServerSide.ScriptManager.AjaxFunction AddAjaxFunction(string Name, string ReadyFunctionContent, string Page, string ParametersInText, bool NeedApplication = true)
		{
			ScriptManager.AjaxFunction AjaxFunction = this.AddAjaxFunction(Name, ReadyFunctionContent, ParametersInText, NeedApplication);
			AjaxFunction.PageName = Page;
			return AjaxFunction;
		}
		public Function AddDecideElementValueScript()
		{
			if (this.Function["DecideElementValueScript"] == null) {
				Content script = new Content();
				script.Add("if(elementID==undefined || elementID==''){return '';}");
				script.Add("var Element=document.getElementById(elementID);if(Element==undefined){return '';}");
				script.Add("if (document.getElementById(elementID).type == 'checkbox') { " + Constants.vbCrLf);
				script.Add("    return document.getElementById(elementID).checked; } " + Constants.vbCrLf);
				script.Add("else if (document.getElementById(elementID).tagName == 'DIV'){  " + Constants.vbCrLf);
				script.Add("    return document.getElementById(elementID).innerHTML.split('&').join('%26'); } " + Constants.vbCrLf);
				script.Add("else if (document.getElementById(elementID).type == 'radio') { " + Constants.vbCrLf);
				script.Add("    var elements = document.getElementsByName(elementID);");
				script.Add("    for (var i= 0;i<elements.length;i++){");
				script.Add("        if (elements[i].checked) return elements[i].value;");
				script.Add("    }");
				script.Add("    return '';  " + Constants.vbCrLf);
				script.Add("}");
				script.Add("else { " + Constants.vbCrLf);
				script.Add("    return document.getElementById(elementID).value.split('&').join('%26'); }; " + Constants.vbCrLf);

				this.AddFunction("DecideElementValueScript", script.Value, "elementID");
			}
			return this.Function["DecideElementValueScript"];
		}
		public Function AddBuildScript()
		{
			if (this.Function["BuildScript"] == null) {
				Content ScriptCreater = new Content();
				ScriptCreater.Add("var headID = document.getElementsByTagName('head')[0];" + Constants.vbCrLf);
				ScriptCreater.Add("var i;" + Constants.vbCrLf);
				ScriptCreater.Add("var node;" + Constants.vbCrLf);
				ScriptCreater.Add("var CanBeAdded = true;" + Constants.vbCrLf);
				ScriptCreater.Add("for (var i=0;i<=headID.childNodes.length-1;i++)" + Constants.vbCrLf);
				ScriptCreater.Add("{" + Constants.vbCrLf);
				ScriptCreater.Add("node = headID.childNodes[i];" + Constants.vbCrLf);
				ScriptCreater.Add("if (node.localName == 'script')" + Constants.vbCrLf);
				ScriptCreater.Add("{" + Constants.vbCrLf);
				ScriptCreater.Add("if (SourceName.indexOf('http') == -1 &&  node.src.indexOf(SourceName)>-1)" + Constants.vbCrLf);
				ScriptCreater.Add("{" + Constants.vbCrLf);
				ScriptCreater.Add("CanBeAdded = false;" + Constants.vbCrLf);
				ScriptCreater.Add("}" + Constants.vbCrLf);
				ScriptCreater.Add("}" + Constants.vbCrLf);
				ScriptCreater.Add("}" + Constants.vbCrLf);
				ScriptCreater.Add("if (CanBeAdded)" + Constants.vbCrLf);
				ScriptCreater.Add("{" + Constants.vbCrLf);
				ScriptCreater.Add("var newScript = document.createElement('script');" + Constants.vbCrLf);
				ScriptCreater.Add("newScript.type = 'text/javascript';" + Constants.vbCrLf);
				ScriptCreater.Add("newScript.src = SourceName;" + Constants.vbCrLf);
				ScriptCreater.Add("headID.appendChild(newScript);" + Constants.vbCrLf);
				ScriptCreater.Add("}");
				this.AddFunction("BuildScript", ScriptCreater.Value, "SourceName");
			}
			return this.Function["BuildScript"];
		}
		public void AddGetElementsByClassNameFunction()
		{
			this.AppendLine("if (document.getElementsByClassName == undefined && document.querySelectorAll != undefined)");
			this.AppendLine("{");
			this.AppendLine("     document.getElementsByClassName = function (className) {");
			this.AppendLine("                                                             return document.querySelectorAll('.' + className);");
			this.AppendLine("                                                             };");
			this.AppendLine("}");
			this.AppendLine("else if (document.getElementsByClassName == undefined && document.querySelectorAll == undefined)");
			this.AppendLine("{");
			this.AppendLine("     document.getElementsByClassName = function (className)");
			this.AppendLine("     {");
			this.AppendLine("         var allElements;");
			this.AppendLine("         if (document.all)");
			this.AppendLine("         {");
			this.AppendLine("             allElements = document.all;");
			this.AppendLine("         } else {");
			this.AppendLine("             allElements = document.getElementsByTagName('*');");
			this.AppendLine("         }");
			this.AppendLine("         var foundElements = []; ");
			this.AppendLine("         for (var i = 0; i < allElements.length; i++) {");
			this.AppendLine("             if (allElements[i].className == className)");
			this.AppendLine("             {");
			this.AppendLine("                 foundElements[foundElements.length] = allElements[i];");
			this.AppendLine("             }");
			this.AppendLine("         }");
			this.AppendLine("         return foundElements;");
			this.AppendLine("     };");
			this.AppendLine("}");
		}
		public Function AddAjaxHtmlParserFunction()
		{
			//to do: Birden fazla çizim olayı tekrar kontrol edilmelidir. 

			if (this.Function["CallbackAjaxFunction"] == null && (string.IsNullOrEmpty(PageContext.Current.QueryString("EventName")) || PageContext.Current.QueryString("EventName") == "true")) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function EvalScript = this.AddFunction("CallbackAjaxFunction", "", "output");
				EvalScript.AppendLine("var headobjects = new Array();");
				EvalScript.AppendLine("var ajaxTemplatePanel = document.createElement('div');");
				EvalScript.AppendLine("ajaxTemplatePanel.innerHTML = output;");
				EvalScript.AppendLine("var ajaxchild = null;");
				EvalScript.AppendLine("var child = null;");
				EvalScript.AppendLine("for (var i = ajaxTemplatePanel.childNodes.length-1; i >= 0 ; i--) {");
				EvalScript.AppendLine("     ajaxchild = ajaxTemplatePanel.childNodes[i];");
				EvalScript.AppendLine("if (ajaxchild.tagName == undefined) continue;");
				EvalScript.AppendLine("if (ajaxchild.tagName.toLowerCase() == 'script' || ajaxchild.tagName.toLowerCase() == 'style' || ajaxchild.tagName.toLowerCase() == 'link') {");
				EvalScript.AppendLine("    var headID = document.getElementsByTagName('head')[0];");
				EvalScript.AppendLine("    if (ajaxchild.tagName.toLowerCase().indexOf('script') > -1)");
				EvalScript.AppendLine("    {");
				EvalScript.AppendLine("         if (ajaxchild.src == undefined || ajaxchild.src == '')");
				EvalScript.AppendLine("         {");
				EvalScript.AppendLine("             headobjects.push(ajaxchild);");
				EvalScript.AppendLine("         }");
				EvalScript.AppendLine("         else");
				EvalScript.AppendLine("         {");
				EvalScript.AppendLine("             headobjects.unshift(ajaxchild)");
				EvalScript.AppendLine("         }");
				EvalScript.AppendLine("     }");
				EvalScript.AppendLine("     else{");
				EvalScript.AppendLine("         headID.appendChild(ajaxchild);");
				EvalScript.AppendLine("     }");
				EvalScript.AppendLine("continue;");
				EvalScript.AppendLine("}");
				EvalScript.AppendLine("     child = document.getElementById(ajaxchild.id.toString());");
				EvalScript.AppendLine("     if (child == null || child == undefined) {");
				EvalScript.AppendLine("         ChangeChildElementsIDForAjax(ajaxchild);");
				EvalScript.AppendLine("         document.body.appendChild(ajaxchild);");
				EvalScript.AppendLine("     }");
				EvalScript.AppendLine("     else {");
				EvalScript.AppendLine("         ChangeChildElementsIDForAjax(ajaxchild);");
				EvalScript.AppendLine("         swap(child, ajaxchild);");
				EvalScript.AppendLine("     }");
				EvalScript.AppendLine("}");
				EvalScript.AppendLine("for(var k=0;k<headobjects.length;k++){");
				EvalScript.AppendLine("   if (headobjects[k].src == undefined || headobjects[k].src == '')");
				EvalScript.AppendLine("   {");
				EvalScript.AppendLine("         var headelement = document.createElement(headobjects[k].tagName);");
				EvalScript.AppendLine("         headelement.text = headobjects[k].innerHTML; ");
				EvalScript.AppendLine("         headID.appendChild(headelement); ");
				EvalScript.AppendLine("   }");
				EvalScript.AppendLine("   else");
				EvalScript.AppendLine("   {");
				EvalScript.AppendLine("         var headelement = document.createElement(headobjects[k].tagName);");
				EvalScript.AppendLine("         headelement.src = headobjects[k].src; ");
				EvalScript.AppendLine("         headID.appendChild(headelement); ");
				EvalScript.AppendLine("   }");
				EvalScript.AppendLine("}");

				this.AddNodeReplaceFunction();
				this.ChangeChildElementsIDForAjax();
				return EvalScript;
			}
			return this.Function["CallbackAjaxFunction"];
		}
		private Function ChangeChildElementsIDForAjax()
		{
			if (this.Function["ChangeChildElementsIDForAjax"] == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function Function = this.AddFunction("ChangeChildElementsIDForAjax", "", "element");
				Function.AppendLine("for (var k=0;k<element.childNodes.length;k++)");
				Function.AppendLine("{");
				Function.AppendLine("   ChangeChildElementsIDForAjax(element.childNodes[k]);");
				Function.AppendLine("}");
				return Function;
			}
			return this.Function["ChangeChildElementsIDForAjax"];
		}
		public Function AddNodeReplaceFunction()
		{
			if (this.Function["swap"] == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function SwapFunction = this.AddFunction("swap", "", "oldNode, newNode");
				SwapFunction.AppendLine("var parentNode = oldNode.parentNode;");
				SwapFunction.AppendLine("parentNode.insertBefore(newNode, oldNode);");
				SwapFunction.AppendLine("parentNode.removeChild(oldNode);");
				return SwapFunction;
			}
			return this.Function["swap"];
		}
		public ServerSide.ScriptManager.AjaxFunction AddAjaxFunction(string Name, string ReadyFunctionContent, string ParametersInText = "", bool NeedApplication = true)
		{
			if (this.Function[Name] == null) {
				ScriptManager.AjaxFunction AjaxFunction = new ScriptManager.AjaxFunction(Name, this);
				if (!string.IsNullOrEmpty(ParametersInText)) {
					string[] Parameters = ParametersInText.Split(",");
					for (int i = 0; i <= Parameters.Length - 1; i++) {
						AjaxFunction.Parameters.Add(Parameters[i]);
					}
				}
				AjaxFunction.NeedApplication = NeedApplication;
				AjaxFunction.AppendLine(ReadyFunctionContent);
				this.Functions.Add(AjaxFunction);
				return AjaxFunction;
			}
			return this.Function[Name];
		}
		public Function AddTrimFunction()
		{
			Function Function = this.AddFunction("Trim", "", "sText");
			Function.AddVariable("sTrimmedText", "''");
			Function.AddVariable("Char", "''");
			Function.AppendLine("for (var i = 0; i < sText.length; i++) {" + Constants.vbCrLf);
			Function.AppendLine("Char = sText.charAt(i);" + Constants.vbCrLf);
			Function.AppendLine("if (Char != ' ') {" + Constants.vbCrLf);
			Function.AppendLine(" sTrimmedText += Char" + Constants.vbCrLf);
			Function.AppendLine("}" + Constants.vbCrLf);
			Function.AppendLine("}" + Constants.vbCrLf);
			Function.AppendLine("return sTrimmedText" + Constants.vbCrLf);
			return Function;
		}
		public Function AddRoundNumberFunction()
		{
			Function Function = this.AddFunction("RoundNumber", "", "Number, DecimalDigits");
			Function.AppendLine("var DecimalNumber = Number;");
			Function.AppendLine("var ResultNumber = Math.round(DecimalNumber*Math.pow(10,DecimalDigits))/Math.pow(10,DecimalDigits);");
			Function.AppendLine("var ResultString = new String(ResultNumber);");
			Function.AppendLine("return parseFloat(ResultString);");
			return Function;
		}
		public Function AddConfirmFunction(string Name, string Message)
		{
			if (this.Function[Name] == null) {
				Function Function = this.AddFunction(Name);
				Function.AddConfirmMessage(Message);
				return Function;
			}
			return this.Function[Name];
		}
		public string AllowNumeric(bool AllowDecimal, string ControlID = "")
		{
			if (this.Function["AllowNumeric"] == null) {
				this.AddAllowNumericFunction(AllowDecimal);
			}
			return "AllowNumeric(event,'" + ControlID + "')";
		}
		public string IsNumeric(string ControlID, int ValidationType, string Message, bool AllowInfitiyCharacter, bool ShowToolTip = false)
		{
			if (this.Function["CheckNumeric"] == null) {
				this.AddNumericCheckFunction(ShowToolTip);
			}
			return "CheckNumeric(" + ControlID + "," + ValidationType + ",'" + Message + "', " + (AllowInfitiyCharacter ? 1 : 0) + ")";
		}
		public string IsOnNumericRange(string ControlID, int ValidationType, string Message, int MinValue, int MaxValue, bool AllowInfitiyCharacter, bool ShowToolTip = false)
		{
			if (this.Function["CheckNumericRange"] == null) {
				this.AddNumericalRangeFunction(ShowToolTip);
			}
			return "CheckNumericRange(" + ControlID + "," + ValidationType + ",'" + Message + "'," + MaxValue + " , " + MinValue + "," + (AllowInfitiyCharacter ? 1 : 0) + " )";
		}
		public string IsDate(string ControlID, string Message, bool ShowToolTip = false)
		{
			if (this.Function["CheckDate"] == null) {
				this.AddDateValidatorFunction(ShowToolTip);
			}
			return "CheckDate(" + ControlID + ",'" + Message + "')";
		}
		public string IsText(string ControlID, int ValidationType, string Message, int MinLength, bool ShowToolTip = false)
		{
			if (this.Function["CheckText"] == null) {
				this.AddCheckTextFunction(ShowToolTip);
			}
			return "CheckText(" + ControlID + "," + ValidationType + ",'" + Message + "'," + MinLength + ")";
		}
		public string IsEmail(string ControlID, int ValidationType, string Message, bool ShowToolTip = false)
		{
			if (this.Function["CheckEmail"] == null) {
				this.AddCheckEmailFunction(ShowToolTip);
			}
			return "CheckEmail(" + ControlID + "," + ValidationType + ",'" + Message + "')";
		}
		public string IsEmailText(string ControlID, int ValidationType, string Message, string ErrorControlID, bool ShowToolTip = false)
		{
			if (this.Function["CheckEmailText"] == null) {
				this.AddCheckEmailTextFunction(ShowToolTip);
			}
			return "CheckEmailText(" + ControlID + "," + ValidationType + ",'" + Message + "'," + ErrorControlID + ")";
		}
		public string IsAcceptedFile(string ControlID, int ValidationType, string Message, string FileExtension, bool ShowToolTip = false)
		{
			if (this.Function["CheckFile"] == null) {
				this.AddCheckFileFunction(ShowToolTip);
			}
			return "CheckFile(" + ControlID + "," + ValidationType + ",'" + Message + "','" + FileExtension + "')";
		}
		public string CheckMaxLength(string ControlID, string Message, bool ShowToolTip = false)
		{
			if (this.Function["CheckMaxLength"] == null) {
				this.AddCheckMaxLengthFunction(ShowToolTip);
			}
			return "CheckMaxLength(" + ControlID + ",'" + Message + "')";
		}
		private void AddToolTipScriptFiles()
		{
			this.Manager.AddGeneralJQueryLibrary();
			this.Manager.AddToolTipJQueryLibrary();
		}
		public Function AddCheckMaxLengthFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckMaxLength", "", "Control, Message, Event");
			Function.AppendLine("if (Event==undefined || !(Event.keyCode ==8 || Event.keyCode ==46 || (Event.keyCode >=37 && Event.keyCode <=40))) {");
			Function.AppendLine("var MaxLength = Control.getAttribute(\"maxLength\");");
			Function.AppendLine("if (Control.value.length >= MaxLength) {");
			Function.AppendLine("if (Event==undefined) { ");
			Function.AppendLine("if (Message='') {");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message);");
			}
			Function.AppendLine("}");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("}");
			Function.AppendLine("return true;");
			return Function;
		}
		public Function AddCheckFileFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckFile", "", "Control, ValidationType, Message, FileExtensions");
			Function.AppendLine("var Extension = Control.value.substring(Control.value.lastIndexOf('.')).toLowerCase();");
			Function.AppendLine("if(FileExtensions.toLowerCase().indexOf(FileExtensions.toLowerCase()) = -1){");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message);");
			}
			Function.AppendLine("var elementfocus = \"document.getElementById('\" + Control.id + \"').focus();\"");
			Function.AppendLine("setTimeout(elementfocus,1);");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("else {;");
			Function.AppendLine("return true;");
			Function.AppendLine("}");
			return Function;
		}
		public Function AddCheckTextFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckText", "", "Control, ValidationType, Message, RequiredCharCount");
			Function.AppendLine("if(!(Control.style.display==\"none\")){");
			Function.AppendLine("if(Control.value.replace(/^\\s+|\\s+$/, '') == \"\"){");
			Function.AppendLine("if(ValidationType==1){");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.ReturnErrorMessage();
			}
			Function.AppendLine("}");
			Function.AppendLine("}");
			Function.AppendLine("else if(RequiredCharCount!=undefined){");
			Function.AppendLine("if(Control.value.length<RequiredCharCount){");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.ReturnErrorMessage();
			}
			Function.AppendLine("}");
			Function.AppendLine("}");
			Function.AppendLine("}");
			Function.ReturnSuccessMessage();
			return Function;
		}
		public Function AddCheckEmailFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckEmail", "", "Control, ValidationType, Message");
			Function.AppendLine("var chkemail=/^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\\.([a-zA-Z])+([a-zA-Z])+/;");
			Function.AppendLine("if(!chkemail.test(Control.value)){");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message);");
			}
			//setTimeout("document.getElementById(fldID).focus();",1);
			Function.AppendLine("var elementfocus = \"document.getElementById('\" + Control.id + \"').focus();\"");
			Function.AppendLine("setTimeout(elementfocus,1);");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("else {;");
			Function.AppendLine("return true;");
			Function.AppendLine("}");
			return Function;
		}
		public Function AddCheckEmailTextFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckEmailText", "", "Control, ValidationType, Message, ErrorControl");
			Function.AppendLine("var chkemailtext=/^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\\.([a-zA-Z])+([a-zA-Z])+/;");
			Function.AppendLine("if(!chkemailtext.test(Control.value)){");
			Function.AppendLine("ErrorControl.innerHTML = Message");
			Function.AppendLine("var elementfocus = \"document.getElementById('\" + Control.id + \"').focus();\"");
			Function.AppendLine("setTimeout(elementfocus,1);");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("else {;");
			Function.AppendLine("ErrorControl.innerHTML = ''");
			Function.AppendLine("return true;");
			Function.AppendLine("}");
			return Function;
		}
		public string ValidateRadioOption(string ControlID, string Message, bool ShowToolTip = false)
		{
			if (this.Function["CheckRadioOption"] == null) {
				this.AddRadioOptionValidation(ShowToolTip);
			}
			return "CheckRadioOption('" + ControlID + "','" + Message + "')";
		}
		public Function AddRadioOptionValidation(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckRadioOption", "", "Control, Message");
			Function.AppendLine("var TemplateOption = -1;");
			Function.AppendLine("var Element = document.getElementsByName(Control);");
			Function.AppendLine("for (var i=Element.length -1;i > -1;i--)");
			Function.AppendLine("{");
			Function.AppendLine("if (Element[i].checked)");
			Function.AppendLine("{");
			Function.AppendLine("var TemplateOption=i; i=-1;");
			Function.AppendLine("}");
			Function.AppendLine("}");
			Function.AppendLine("if (TemplateOption == -1) {");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message)");
			}
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("return true;");
			return Function;
		}
		public Function AddNumericCheckFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckNumeric", "", "Control, ValidationType, Message,AllowInf");
			Function.AppendLine("try { ");
			Function.AppendLine("var tempValue = Control.value;");
			Function.AppendLine("tempValue = tempValue.split(',').join('.');");
			Function.AppendLine("if (tempValue.indexOf('.') > -1){");
			Function.AppendLine("    tempValue = tempValue.substring(0,tempValue.lastIndexOf('.')).split('.').join('') + tempValue.substring(tempValue.lastIndexOf('.'));}");
			Function.AppendLine("if (AllowInf == 1 && tempValue== '*') return true;");
			Function.AppendLine("if(isNaN(tempValue)){");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message);");
			}
			Function.AppendLine("Control.value=0;");
			Function.AppendLine("var elementfocusString = \"document.getElementById('\" + Control.id + \"').select();\" ");
			Function.AppendLine("setTimeout(elementfocusString,1);");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("else {");
			Function.AppendLine("return true;");
			Function.AppendLine("}");
			Function.AppendLine("}");
			Function.AppendLine("catch (err) { return true; }");
			return Function;
		}
		public Function AddDateValidatorFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckDate", "", "Control,Message");
			Function.AppendLine("if (Control.value != ''){");
			Function.AppendLine("var validformat=/^\\d{2}\\.\\d{2}\\.\\d{4}$/;//Basic check for format validity");
			Function.AppendLine("var returnval=false;");
			Function.AppendLine("if (!validformat.test(Control.value)) ");
			Function.AppendLine("     {");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("     alert(Message);");
			}
			Function.AppendLine("     Control.value = ''; ");
			Function.AppendLine("     }");
			Function.AppendLine(" else{ //Detailed check for valid date ranges");
			Function.AppendLine("   var dayfield=Control.value.split('.')[0] ");
			Function.AppendLine("   var monthfield=Control.value.split('.')[1] ");
			Function.AppendLine("   var yearfield=Control.value.split('.')[2] ");
			Function.AppendLine("   var dayobj = new Date(yearfield, monthfield-1, dayfield) ");
			Function.AppendLine("   if ((dayobj.getMonth()+1!=monthfield)||(dayobj.getDate()!=dayfield)||(dayobj.getFullYear()!=yearfield))");
			Function.AppendLine("     {");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("     alert(Message);");
			}
			Function.AppendLine("     Control.value = ''; ");
			Function.AppendLine("     }");
			Function.AppendLine("    else ");
			Function.AppendLine("      {returnval=true} ");
			Function.AppendLine("    } ");
			Function.AppendLine("if (returnval==false) { ");
			Function.AppendLine("     var elementfocus = \"document.getElementById('\" + Control.id + \"').focus();\"");
			Function.AppendLine("     setTimeout(elementfocus,1);");
			Function.AppendLine("    }");
			Function.AppendLine("return returnval ");
			Function.AppendLine("} ");
			return Function;
		}
		public Function AddAllowNumericFunction(bool AllowDecimal)
		{
			Function Function = this.AddFunction("AllowNumeric", "", "e,controlID");
			Function.AppendLine("var charCode = (e.which) ? e.which : e.keyCode");
			Function.AppendLine("if ((charCode>=48 && charCode<=57) || (charCode>=96 && charCode<=105)  || charCode==46  || charCode==8 || charCode==9 || charCode==36 || charCode==35 || charCode==37 || charCode==39 || charCode == 188 || charCode == 110 || charCode == 190) ");
			Function.AppendLine("{");
			if (!AllowDecimal) {
				Function.AppendLine("if (charCode == 188 || charCode == 110 || charCode == 190 || charCode==46 || charCode==44 ) { ");
				Function.AppendLine(" return false;");
				Function.AppendLine("}");
			} else {
				Function.AppendLine("if (controlID!=undefined && controlID!='' && (charCode == 188 || charCode == 110 || charCode == 190 || charCode==46 || charCode==44 )) { ");
				Function.AppendLine("  if (!(document.getElementById(controlID).value.indexOf(String.fromCharCode(44)) == -1  && document.getElementById(controlID).value.indexOf(String.fromCharCode(46)) == -1  && document.getElementById(controlID).value.indexOf(String.fromCharCode(188)) == -1  && document.getElementById(controlID).value.indexOf(String.fromCharCode(110)) == -1 && document.getElementById(controlID).value.indexOf(String.fromCharCode(190)) == -1))");
				Function.AppendLine("        return false;");
				Function.AppendLine("}");
			}
			Function.AppendLine("return true;");
			Function.AppendLine("}");
			Function.AppendLine("return false;");
			return Function;
		}
		public Function AddDisableEnterKeyFunction()
		{
			Function Function = this.AddFunction("DisableEnterKey", "", "e");
			Function.AppendLine("var key;");
			Function.AppendLine("if(window.event) {");
			Function.AppendLine("key = window.event.keyCode; }");
			//IE
			Function.AppendLine("else {");
			Function.AppendLine("key = e.which;}");
			Function.AppendLine("return (key != 13);");
			return Function;
		}
		public Function AddNumericalRangeFunction(bool ShowToolTip = false)
		{
			Function Function = this.AddFunction("CheckNumericRange", "", "Control, ValidationType, Message,MaxValue,MinValue,AllowInf");
			Function.AppendLine("if (AllowInf == 1  && Control.value == '*') return true;");
			Function.AppendLine("if (Control.value == \"\"){");
			Function.AppendLine("if(ValidationType==1){");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message);");
			}
			Function.AppendLine("Control.value=MinValue;");
			Function.AppendLine("var elementfocus = \"document.getElementById('\" + Control.id + \"').select();\"");
			Function.AppendLine("setTimeout(elementfocus,1);");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("}");
			Function.AppendLine("else if(Control.value < MinValue ){");
			Function.AppendLine("Control.value=MinValue;");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message);");
			}
			Function.AppendLine("var elementfocus = \"document.getElementById('\" + Control.id + \"').select();\"");
			Function.AppendLine("setTimeout(elementfocus,1);");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("else if(Control.value > MaxValue){");
			Function.AppendLine("Control.value= MaxValue ;");
			if (ShowToolTip) {
				this.AddToolTipScriptFiles();
				Function.ReturnErrorTooltip();
			} else {
				Function.AppendLine("alert(Message);");
			}
			Function.AppendLine("var elementfocus = \"document.getElementById('\" + Control.id + \"').select();\"");
			Function.AppendLine("setTimeout(elementfocus,1);");
			Function.AppendLine("return false;");
			Function.AppendLine("}");
			Function.AppendLine("else {");
			Function.AppendLine("return true;");
			Function.AppendLine("}");
			return Function;
		}
		public string AddFunctionToOnload(string FunctionWithParameters, bool AppendLine = true)
		{
			Content Content = new Content();
			Content.Add("if (window.addEventListener)");
			Content.Add("{");
			Content.Add("window.addEventListener('load', function () {" + FunctionWithParameters + ";},false);");
			Content.Add("}");
			Content.Add("else if (window.attachEvent)");
			Content.Add("{");
			Content.Add("window.attachEvent('onload', function () {" + FunctionWithParameters + ";})");
			Content.Add("}");
			if (AppendLine) {
				this.AppendLine(Content.Value);
			}
			return Content.Value;
		}
		public void AppendLine(string Value)
		{
			if (!string.IsNullOrEmpty(Value)) {
				if (this.DebugMode) {
					this.ContentBuilder.Add(Value + Constants.vbCrLf);
				} else {
					this.ContentBuilder.Add(Value);
				}
			}
		}
		public Content Append(string Value)
		{
			this.AppendLine(Value);
			return this.ContentBuilder;
		}
		public string Draw(bool CanBeCached = true)
		{
			if (this.Enabled) {
				if (!string.IsNullOrEmpty(this.Path)) {
					return "<script type='text/javascript' src='" + this.Path + "'" + (this.Async ? " async" : "") + (this.IsDefered ? " defer" : "") + (string.IsNullOrEmpty(this.OnLoadFunction) ? "" : " onload=\"" + this.OnLoadFunction + "\" ") + "></script>";
				}
				string CacheValue = "";
				if (!this.HasCachedContent) {
					foreach (DictionaryEntry GlobalVariableDictionaryEntry in this.GlobalVariables) {
						if (GlobalVariableDictionaryEntry.Value == null || string.IsNullOrEmpty(GlobalVariableDictionaryEntry.Value))
							GlobalVariableDictionaryEntry.Value = "''";
						this.AppendLine("var " + GlobalVariableDictionaryEntry.Key + " = " + GlobalVariableDictionaryEntry.Value + ";");
					}
					this.OnBeforeDraw();
					int Counter = 0;
					for (int i = 0; i <= this.Functions.Count - 1; i += 1) {
						this.AppendLine(this.Functions[i].Draw);
						Counter += 1;
					}
					for (int i = Counter; i <= this.Functions.Count - 1; i += 1) {
						this.AppendLine(this.Functions[i].Draw);
					}
					CacheValue = this.ContentBuilder.Value;
					if (this.HttpObjectCount > 0) {
						CacheValue = this.AddXMLHttpRequestFunction() + CacheValue;
					}
				}
				if (!CanBeCached) {
					if (!string.IsNullOrEmpty(CacheValue)) {
						return "<script type='text/javascript'> " + CacheValue + "</script>";
					}
					return "";
				}
				if (!string.IsNullOrEmpty(CacheValue)) {
					CacheManager.AddCache(this.CacheName, CacheValue, 60);
					return "<script type='text/javascript' src='?WriteScript=" + this.CacheName + "'></script>";
				}
			}
			return "";
		}

		protected virtual void OnBeforeDraw()
		{
		}
		protected string AddXMLHttpRequestFunction()
		{
			string ReturnString = "";
			ReturnString += "function createobject() {" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "var object;" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "var browserName = navigator.appName;" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "if (browserName == 'Microsoft Internet Explorer') {" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "object = new ActiveXObject('Microsoft.XMLHTTP');" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "} else {" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "object = new XMLHttpRequest();" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "}" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "return object;" + (this.DebugMode ? Constants.vbCrLf : "");
			ReturnString += "}" + (this.DebugMode ? Constants.vbCrLf : "");
			return ReturnString;
		}
		public Script(string Name, ScriptCollection ScriptCollection)
		{
			this.oScriptCollection = ScriptCollection;
			this.sName = Name;
			//Me.DebugMode = System.Configuration.ConfigurationManager.AppSettings("debug") IsNot Nothing AndAlso System.Configuration.ConfigurationManager.AppSettings("debug") = "true"
		}
	}
}

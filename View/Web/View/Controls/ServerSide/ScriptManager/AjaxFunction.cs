using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.ServerSide.ScriptManager
{
	public class AjaxFunction : Function
	{
		private string sPageName = "";
		private string sName;
		private bool bPageIsSecure = false;
		private bool bNeedApplication = true;
		private string bControlIDs = "";
		private string sHttpObject = "";
		private Hashtable oAjaxRequestParameter = new Hashtable();
		private string sHttpObjectConstructorString = "";
		private string sOnBeforeRequestStarted = "";
		private string sOnRequestFinished = "";
		private string sDisplayEventName = "";
		private bool bShowOverlay = true;
		private string sOverlayElementID = "";
		private string sOverlayHtml = "<img style=\"z-index:99999; position:absolute; left:48%; top:48%; \" src=\"" + Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("AjaxLoader", , , "gif") + "\"/>";
		private Style oOverlayStyle;
		private string sOnAfterRequestFinished = "";
		private AjaxQueueManagementType eAjaxQueueType = AjaxQueueManagementType.Inherits;
		public string HttpObject {
			get {
				if (string.IsNullOrEmpty(this.sHttpObject)) {
					if (this.AjaxQueueType == AjaxQueueManagementType.UseQueue) {
						sHttpObject = "httpobjectforajaxqueue";
					} else {
						if (!string.IsNullOrEmpty(this.HttpObjectConstructorString)) {
							this.sHttpObject = this.Script.GetHttpObject(-1);
						}
					}
				}
				return this.sHttpObject;
			}
		}
		public string HttpObjectConstructorString {
			get {
				if (this.AjaxQueueType == AjaxQueueManagementType.UseQueue) {
					if (Ophelia.Web.View.UI.Current.Session != null) {
						if (Ophelia.Web.View.UI.Current.Session("HttpObjectCount") == null) {
							Ophelia.Web.View.UI.Current.Session("HttpObjectCount") = 1;
						} else {
							Ophelia.Web.View.UI.Current.Session("HttpObjectCount") += 1;
						}
					}
					return "";
				}
				if (string.IsNullOrEmpty(this.sHttpObjectConstructorString)) {
					this.sHttpObjectConstructorString = this.Script.AddHttpObject(false);
					this.sHttpObject = this.Script.GetHttpObject(-1);
				}
				return this.sHttpObjectConstructorString;
			}
		}
		protected internal string HttpObjectResult {
			get { return this.HttpObject + ".responseText" + (UseEndBracket ? ";" : ""); }
		}
		public bool NeedApplication {
			get { return this.bNeedApplication; }
			set { this.bNeedApplication = value; }
		}
		public AjaxQueueManagementType AjaxQueueType {
			get {
				if (this.eAjaxQueueType == AjaxQueueManagementType.Inherits) {
					if (this.Script.AjaxQueueType != AjaxQueueManagementType.Inherits) {
						return this.Script.AjaxQueueType;
					} else {
						return AjaxQueueManagementType.UseMultipleRequest;
					}
				}
				return this.eAjaxQueueType;
			}
			set { this.eAjaxQueueType = value; }
		}
		public string ControlIDs {
			get { return this.bControlIDs; }
			set { this.bControlIDs = value; }
		}
		public bool PageIsSecure {
			get { return this.bPageIsSecure; }
			set { this.bPageIsSecure = value; }
		}
		public string OverlayHtml {
			get { return UI.Current.AjaxConfiguration.OverlayHtml; }
			set { UI.Current.AjaxConfiguration.OverlayHtml = value; }
		}
		public Style OverlayStyle {
			get { return UI.Current.AjaxConfiguration.OverlayStyle; }
			set { UI.Current.AjaxConfiguration.OverlayStyle = value; }
		}
		public bool ShowOverlay {
			get { return UI.Current.AjaxConfiguration.ShowOverlay; }
			set { UI.Current.AjaxConfiguration.ShowOverlay = value; }
		}
		public string OverlayElementID {
			get { return this.sOverlayElementID; }
			set { this.sOverlayElementID = value; }
		}
		public string OnBeforeRequestStarted {
			get { return this.sOnBeforeRequestStarted; }
			set { this.sOnBeforeRequestStarted = value; }
		}
		public string OnRequestFinished {
			get { return this.sOnRequestFinished; }
			set { this.sOnRequestFinished = value; }
		}
		public string OnAfterRequestFinished {
			get { return this.sOnAfterRequestFinished; }
			set { this.sOnAfterRequestFinished = value; }
		}
		public Hashtable AjaxRequestParameter {
			get { return this.oAjaxRequestParameter; }
		}
		public string AjaxRequestParameterInText {
			get {
				string ReturnString = ",,";
				foreach (DictionaryEntry Entry in this.AjaxRequestParameter) {
					if (ReturnString != ",,") {
						ReturnString += "$$$";
					}
					ReturnString += Entry.Key.ToString().Replace(" ", "") + ",," + Entry.Value;
				}
				return ReturnString;
			}
		}
		public string DisplayEventName {
			get {
				if (string.IsNullOrEmpty(this.sDisplayEventName)) {
					this.sDisplayEventName = this.Name;
				}
				return this.sDisplayEventName;
			}
			set { this.sDisplayEventName = value; }
		}
		public string PageName {
			get {
				string ReturnPageName = "";
				if (string.IsNullOrEmpty(this.sPageName)) {
					ReturnPageName = "document.location";
				} else {
					string ApplicationBase = "";
					if ((this.Script.Manager != null)) {
						ApplicationBase = this.Script.Manager.ApplicationBase;
					} else if (this.Script.Manager == null && System.Configuration.ConfigurationManager.AppSettings("ApplicationBase") != null) {
						ApplicationBase = System.Configuration.ConfigurationManager.AppSettings("ApplicationBase");
					}
					ReturnPageName += "document.location.protocol + '//' + document.location.host + '" + ApplicationBase + sPageName + "' ";
				}
				if (this.PageIsSecure) {
					if ((this.Script.Manager != null && !this.Script.Manager.IgnoreSecurity) || (System.Configuration.ConfigurationManager.AppSettings("IgnoreSecurity") != null && System.Configuration.ConfigurationManager.AppSettings("IgnoreSecurity") != true)) {
						ReturnPageName = this.AddVariable("AjaxPageUrl", ReturnPageName);
						if (this.Script.DebugMode)
							ReturnPageName += Constants.vbCrLf;
						ReturnPageName += this.AddCondition("document.host.indexOf('https://') == -1", "AjaxPageUrl = AjaxPageUrl.replace('http://','https://')");
					}
				}
				return ReturnPageName;
			}
			set { this.sPageName = value; }
		}
		public string AddConditionAboutHttpObject(string CheckedValue, bool IsVariable, string TruePart, string FalsePart = "", bool AppendLine = true)
		{
			return this.AddCondition(this.HttpObjectResult + " == " + View.Web.Controls.ServerSide.ScriptManager.Utility.FormattedValue(CheckedValue, IsVariable) + "", TruePart, FalsePart);
		}
		public string AddConditionAboutHttpObject(string CheckedValue, bool IsVariable, string TruePart, bool IsDependent, bool AppendLine = true)
		{
			return this.AddCondition(this.HttpObjectResult + " == " + View.Web.Controls.ServerSide.ScriptManager.Utility.FormattedValue(CheckedValue, IsVariable) + "", TruePart, IsDependent);
		}
		public string UpdateInnerHtml(string UpdatingElementID, bool IsVariable = false, bool AppendLine = true)
		{
			string ReturnString = View.Web.Controls.ServerSide.ScriptManager.Utility.UpdateInnerHtml(UpdatingElementID, this.HttpObjectResult, IsVariable);
			if (AppendLine) {
				this.AppendLine(ReturnString);
			}
			return ReturnString;
		}
		private bool AddQueueFunctionality()
		{
			if (this.AjaxQueueType == AjaxQueueManagementType.UseQueue) {
				if (this.Script.Function("AddOpheliaAjaxQueue") == null) {
					this.Script.AppendLine("if (OpheliaAjaxQueueManager == undefined || OpheliaAjaxQueueManager == null){");
					this.Script.AppendLine("    var OpheliaAjaxQueueManager = null;");
					this.Script.AppendLine("    var OpheliaAjaxQueue = null;");
					this.Script.AppendLine("    var OpheliaAjaxQueueManagerHasProcessingAjax = false;");
					this.Script.AppendLine("    var httpobjectforajaxqueue=createobject();");
					this.Script.AppendLine("}");
					Function AddOpheliaAjaxQueue = this.Script.AddFunction("AddOpheliaAjaxQueue", "", "pagename,resultfunction,variables");
					AddOpheliaAjaxQueue.AppendLine("if (OpheliaAjaxQueue == null)");
					AddOpheliaAjaxQueue.AppendLine("    OpheliaAjaxQueue = new Array();");
					AddOpheliaAjaxQueue.AppendLine("var OpheliaAjaxFunction = new Object();");
					AddOpheliaAjaxQueue.AppendLine("OpheliaAjaxFunction.pagename = pagename;");
					AddOpheliaAjaxQueue.AppendLine("OpheliaAjaxFunction.resultfunction = resultfunction;");
					AddOpheliaAjaxQueue.AppendLine("OpheliaAjaxFunction.variables = variables;");
					AddOpheliaAjaxQueue.AppendLine("OpheliaAjaxQueue.push(OpheliaAjaxFunction);");
					AddOpheliaAjaxQueue.AppendLine("ExecuteNextAjaxFunction();");
				}
				if (this.Script.Function("ExecuteNextAjaxFunction") == null) {
					Function ExecuteNextAjaxFunction = this.Script.AddFunction("ExecuteNextAjaxFunction", "", "");
					ExecuteNextAjaxFunction.AppendLine("if (OpheliaAjaxQueueManagerHasProcessingAjax){");
					ExecuteNextAjaxFunction.AppendLine("    if (OpheliaAjaxQueueManager == null){");
					ExecuteNextAjaxFunction.AppendLine("        OpheliaAjaxQueueManager = setInterval('ExecuteNextAjaxFunction()',10);}");
					ExecuteNextAjaxFunction.AppendLine("}");
					ExecuteNextAjaxFunction.AppendLine("else{");
					ExecuteNextAjaxFunction.AppendLine("   OpheliaAjaxQueueManagerHasProcessingAjax = true;");
					ExecuteNextAjaxFunction.AppendLine("   var currentFunction = OpheliaAjaxQueue[0];");
					ExecuteNextAjaxFunction.AppendLine("   if (currentFunction != null) {");
					ExecuteNextAjaxFunction.AppendLine("     httpobjectforajaxqueue.open('POST',currentFunction.pagename, 'true');");
					ExecuteNextAjaxFunction.AppendLine("     httpobjectforajaxqueue.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');");
					ExecuteNextAjaxFunction.AppendLine("     httpobjectforajaxqueue.onreadystatechange = currentFunction.resultfunction;");
					ExecuteNextAjaxFunction.AppendLine("     httpobjectforajaxqueue.send(currentFunction.variables);");
					ExecuteNextAjaxFunction.AppendLine("   }else{");
					ExecuteNextAjaxFunction.AppendLine("     OpheliaAjaxQueueManagerHasProcessingAjax = false;}");
					ExecuteNextAjaxFunction.AppendLine("}");
				}
				if (this.Script.Function("EndLastAjaxFunction") == null) {
					Function EndLastAjaxFunction = this.Script.AddFunction("EndLastAjaxFunction", "", "");
					EndLastAjaxFunction.AppendLine("OpheliaAjaxQueue.splice(0,1);");
					EndLastAjaxFunction.AppendLine("OpheliaAjaxQueueManagerHasProcessingAjax = false;");
					EndLastAjaxFunction.AppendLine("if (OpheliaAjaxQueue.length == 0)");
					EndLastAjaxFunction.AppendLine("    clearInterval('OpheliaAjaxQueueManager');");
				}
			}
		}
		public override string Draw()
		{
			System.Text.StringBuilder ReturnString = new System.Text.StringBuilder();
			ReturnString.AppendLine(this.HttpObjectConstructorString);
			ReturnString.AppendLine("function " + this.Name + "(" + this.ParametersInText + ") {");
			if (!string.IsNullOrEmpty(this.ControlIDs)) {
				ReturnString.AppendLine("var IDs = new String('" + this.ControlIDs + "')");
				ReturnString.AppendLine("var IDList = IDs.split('-');");
				ReturnString.AppendLine("var AjaxParameters='';");
				ReturnString.AppendLine("for (var n in IDList)");
				ReturnString.AppendLine("{");
				ReturnString.AppendLine("   if (IDList[n]!=''){");
				ReturnString.AppendLine("       AjaxParameters += '$$$' + IDList[n] + ',,';");
				ReturnString.AppendLine("       AjaxParameters += document.getElementById(IDList[n]).value ;");
				ReturnString.AppendLine("   }");
				ReturnString.AppendLine("}");
			}
			ReturnString.AppendLine(this.OnBeforeRequestStarted);
			ReturnString.AppendLine(this.DrawOverlayElement());

			if (this.AjaxQueueType == AjaxQueueManagementType.UseQueue) {
				this.AddQueueFunctionality();
				string Str = "'rnd=" + (Math.Floor(new Random().NextDouble() * 10000)).ToString() + "&' + '" + "AjaxRequest" + (NeedApplication ? "WithApplication" : "") + "=" + this.DisplayEventName + this.AjaxRequestParameterInText + "'";
				if (!string.IsNullOrEmpty(this.ControlIDs))
					Str += "+ AjaxParameters";
				ReturnString.AppendLine("AddOpheliaAjaxQueue(" + this.PageName + "," + this.Name + "_result," + Str + ");");
			} else {
				ReturnString.AppendLine(this.HttpObject + ".open('post', " + this.PageName + ",'false');");
				ReturnString.AppendLine(this.HttpObject + ".setRequestHeader('Content-type', 'application/x-www-form-urlencoded');");
				ReturnString.AppendLine(this.HttpObject + ".onreadystatechange = " + this.Name + "_result;");
				ReturnString.AppendLine(this.HttpObject + ".send('rnd=" + (Math.Floor(new Random().NextDouble() * 10000)).ToString() + "&' + '" + "AjaxRequest" + (NeedApplication ? "WithApplication" : "") + "=" + this.DisplayEventName + this.AjaxRequestParameterInText + "'");
				if (!string.IsNullOrEmpty(this.ControlIDs))
					ReturnString.AppendLine("+ AjaxParameters");
				ReturnString.Append(");");
			}

			ReturnString.AppendLine("}");
			ReturnString.AppendLine("function " + this.Name + "_result() {");
			string ResponsableHttpobject = this.HttpObject;
			ReturnString.AppendLine("if (" + ResponsableHttpobject + ".readyState == 4) {");
			ReturnString.AppendLine("   if (" + ResponsableHttpobject + ".responseText.indexOf('redirectedurl') == 0)");
			ReturnString.AppendLine("{");

			if (Ophelia.Web.View.UI.Current.GetType().IsSubclassOf(Type.GetType("Ophelia.Web.View.Mobile.MobilePage"))) {
				ReturnString.AppendLine(this.HideOverlayElement());
				ReturnString.AppendLine("navigateUrl(" + ResponsableHttpobject + ".responseText.substring(14));");
			} else {
				ReturnString.AppendLine("window.location = " + ResponsableHttpobject + ".responseText.substring(14);");
			}

			ReturnString.AppendLine("}");
			ReturnString.AppendLine("  else{");
			if (!string.IsNullOrEmpty(this.OnAfterRequestFinished)) {
				ReturnString.AppendLine(" var " + this.Name + "_ProcessResultvar = " + this.Name + "_ProcessResult();");
				ReturnString.AppendLine(" if (" + this.Name + "_ProcessResultvar == true || " + this.Name + "_ProcessResultvar == undefined)");
				ReturnString.AppendLine(" {");
				ReturnString.AppendLine(OnAfterRequestFinished);
				ReturnString.AppendLine(" }");
			} else {
				ReturnString.Append(this.Name + "_ProcessResult();");
			}
			ReturnString.Append("}");
			ReturnString.AppendLine("}");
			ReturnString.AppendLine("}");
			ReturnString.AppendLine("function " + this.Name + "_ProcessResult() {");
			if (this.AjaxQueueType == AjaxQueueManagementType.UseQueue) {
				ReturnString.AppendLine("EndLastAjaxFunction();");
			}

			ReturnString.AppendLine(this.HideOverlayElement());
			ReturnString.AppendLine(this.Content.Value);
			ReturnString.AppendLine(this.OnRequestFinished);
			ReturnString.AppendLine("}");
			return ReturnString.ToString();
		}
		public string HideOverlayElement()
		{
			System.Text.StringBuilder ReturnString = new System.Text.StringBuilder();
			if (this.ShowOverlay) {
				if (string.IsNullOrEmpty(this.OverlayElementID)) {
					//ReturnString.AppendLine("setTimeout(function(){ ")
					ReturnString.AppendLine("var body = document.getElementsByTagName('body')[0];");
					ReturnString.AppendLine("var overlay = document.getElementById('AjaxOverlay');");
					ReturnString.AppendLine("if (overlay != null){");
					ReturnString.AppendLine(" body.removeChild(overlay);}");
				//ReturnString.AppendLine("},0);")
				} else {
					//ReturnString.AppendLine("setTimeout(function(){ ")
					ReturnString.AppendLine("var element = document.getElementById('" + this.OverlayElementID + "');");
					ReturnString.AppendLine("var overlay = document.getElementById('AjaxOverlay');");
					ReturnString.AppendLine("if (element != null){");
					ReturnString.AppendLine("element.removeChild(overlay);}");
					ReturnString.AppendLine("else{");
					ReturnString.AppendLine("var body = document.getElementsByTagName('body')[0];");
					ReturnString.AppendLine("body.removeChild(overlay);}");
					//ReturnString.AppendLine("},0);")
				}
			}
			return ReturnString.ToString();
		}
		private string DrawOverlayElement()
		{
			System.Text.StringBuilder ReturnString = new System.Text.StringBuilder();
			if (this.ShowOverlay) {
				//ReturnString.AppendLine("setTimeout(function(){ ")
				ReturnString.AppendLine("var OverlayElement = document.createElement('div'); ");
				ReturnString.AppendLine("OverlayElement.id = 'AjaxOverlay'; ");
				if (this.OverlayStyle.IsCustomized == false) {
					this.OverlayStyle.PositionStyle = Position.Fixed;
					this.OverlayStyle.WidthInPercent = 100;
					this.OverlayStyle.HeightInPercent = 100;
					this.OverlayStyle.BackgroundColor = "gray";
					this.OverlayStyle.Opacity = 0.5;
					this.OverlayStyle.ZIndex = 99999;
					this.OverlayStyle.Filter = "alpha(opacity=50)";
				}

				if (this.OverlayStyle.Width == 0)
					this.OverlayStyle.Width = -1;
				if (this.OverlayStyle.Height == 0)
					this.OverlayStyle.Height = -1;

				ReturnString.AppendLine("OverlayElement.setAttribute('style','" + this.OverlayStyle.Draw(true) + "'); ");
				ReturnString.AppendLine("OverlayElement.innerHTML = '" + this.OverlayHtml + "'; ");
				if (string.IsNullOrEmpty(this.OverlayElementID)) {
					ReturnString.AppendLine("var body = document.getElementsByTagName('body')[0]; ");
					ReturnString.AppendLine("body.insertBefore(OverlayElement, body.childNodes[0]); ");
				} else {
					ReturnString.AppendLine("var element = document.getElementById('" + this.OverlayElementID + "'); ");
					ReturnString.AppendLine("if (element != null && element.childNodes[0] != null){");
					ReturnString.AppendLine("element.insertBefore(OverlayElement, element.childNodes[0]);} ");
					ReturnString.AppendLine(" else ");
					ReturnString.AppendLine("{var body = document.getElementsByTagName('body')[0]; ");
					ReturnString.AppendLine("body.insertBefore(OverlayElement, body.childNodes[0]); }");
				}
				//ReturnString.AppendLine("},0);")
			}
			return ReturnString.ToString();
		}
		public AjaxFunction(string Name, Script Script) : base(Name, Script)
		{
		}
		public enum AjaxQueueManagementType
		{
			Inherits = 0,
			UseQueue = 1,
			UseMultipleRequest = 2
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.ServerSide.ScriptManager
{
	public class Utility
	{
		public static string AddVariable(string Name, string DefaultValue = "")
		{
			return "var " + Name + (!string.IsNullOrEmpty(DefaultValue) ? "=" + DefaultValue : "") + ";";
		}
		public static string FormattedValue(string Value, bool IsVariable)
		{
			string ReturnString = "";
			if (!IsVariable) {
				ReturnString = "'";
			}
			ReturnString += Value;
			if (!IsVariable) {
				ReturnString += "'";
			}
			return ReturnString;
		}
		public static string GetElement(string ID, bool IsVariable, bool UseEndBracket = false)
		{
			string ReturnString = "document.getElementById(" + FormattedValue(ID, IsVariable) + ")";
			if (UseEndBracket) {
				ReturnString += ";";
			}
			return ReturnString;
		}
		public static string GetElementValue(string ID, bool IsVariable, bool UseEndBracket = true)
		{
			string ReturnString = GetElement(ID, IsVariable, false) + ".value";
			if (UseEndBracket) {
				ReturnString += ";";
			}
			return ReturnString;
		}
		public static string UpdateInnerHtml(string UpdatingElementID, string Value, bool IsVariable = false)
		{
			return GetElement(UpdatingElementID, IsVariable) + ".innerHTML = " + Value + ";";
		}
		public static string FocusElement(string ID, bool IsVariable)
		{
			return GetElement(ID, IsVariable, false) + ".focus;";
		}
		public static string AddMessage(string Message)
		{
			return "alert('" + Message + "');";
		}
	}
}

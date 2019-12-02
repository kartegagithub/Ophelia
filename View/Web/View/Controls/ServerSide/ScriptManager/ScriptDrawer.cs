using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.ServerSide.ScriptManager
{
	public class ScriptDrawer
	{
		public static void WriteScript(System.Web.UI.Page Form)
		{
			string ScriptName = Form.Request["WriteScript"];
			Form.Response.ContentType = "text/javascript";
			string ScriptInText = CacheManager.GetCachedObject(ScriptName);
			Form.Response.Write(ScriptInText);
		}
		public static Web.Controls.QueryString ArrangeQueryStringForAjaxRequest(string Key, ref Web.Controls.QueryString QueryString)
		{
			if (string.IsNullOrEmpty(QueryString(Key)))
				return QueryString;
			string QueryParsedString = QueryString.Item(Key, "", true);
			//QueryParsedString = QueryParsedString.Replace(",,", "=")
			//QueryParsedString = QueryParsedString.Replace("$$$", "&")
			ArrayList Parameters = ClearEmptyElements(ref QueryParsedString.Split("$$$"), 3, "$");

			string Parameter = "";
			ArrayList ParsedParameter = null;
			QueryString.Remove(Key);
			for (int i = 0; i <= Parameters.Count - 1; i++) {
				Parameter = Parameters[i];
				ParsedParameter = ClearEmptyElements(ref Parameter.Split(",,"), 2, ",");
				if (i == 0 && ParsedParameter.Count == 3) {
					QueryString.Add("EventName", ParsedParameter[0]);
					QueryString.Add(ParsedParameter[1], ParsedParameter[2]);
				} else if (i == 0 && ParsedParameter.Count == 1) {
					QueryString.Add("EventName", ParsedParameter[0]);
				} else if (i == 0 && ParsedParameter.Count == 2 && string.IsNullOrEmpty(ParsedParameter[1])) {
					QueryString.Add("EventName", ParsedParameter[0]);
				}
				if (ParsedParameter.Count == 2) {
					QueryString.Add(ParsedParameter[0], ParsedParameter[1]);
				}
			}
			return QueryString;
		}
		private static ArrayList ClearEmptyElements(ref string[] Parameters, int SplitCharacterCount, char SplitChar)
		{
			ArrayList List = new ArrayList();
			string ParameterValueInString = "";
			bool SumParamaterValue = false;
			int TempWhiteSpaceCount = 0;
			int Counter = 1;
			for (int i = 0; i <= Parameters.Count - 1; i++) {
				ParameterValueInString = Parameters[i];
				TempWhiteSpaceCount = 1;
				Counter = 1;
				while (!((TempWhiteSpaceCount == SplitCharacterCount))) {
					if (Parameters.Count > (i + Counter)) {
						if (string.IsNullOrEmpty(Parameters[i + Counter])) {
							TempWhiteSpaceCount += 1;
						} else if (!string.IsNullOrEmpty(Parameters[i + Counter]) && TempWhiteSpaceCount > 0) {
							for (int k = 1; k <= TempWhiteSpaceCount - 1; k++) {
								ParameterValueInString += SplitChar;
							}
							ParameterValueInString += SplitChar + Parameters[i + Counter];
							TempWhiteSpaceCount = 1;
						} else {
							ParameterValueInString += SplitChar + Parameters[i + Counter];
						}
						Counter += 1;
					} else {
						for (int k = 1; k <= TempWhiteSpaceCount - 1; k++) {
							ParameterValueInString += SplitChar;
						}
						break; // TODO: might not be correct. Was : Exit Do
					}
				}
				i += Counter - 1;
				List.Add(ParameterValueInString);
			}
			return List;
		}

		public static Web.Controls.QueryString ArrangeQueryStringForAjaxRequest(string Key, System.Web.HttpRequest Request)
		{
			QueryString QueryString = new QueryString(Request);
			return ArrangeQueryStringForAjaxRequest(Key, ref QueryString);
		}
	}
}

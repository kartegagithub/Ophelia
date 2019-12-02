using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View
{
	public class StyleSheet
	{
		private RuleCollection oRules;
		internal RuleCollection Rules {
			get {
				if (this.oRules == null)
					this.oRules = new RuleCollection();
				return this.oRules;
			}
		}
		public void AddIDBasedRule(string ControlIDs, Style Style)
		{
			if (!string.IsNullOrEmpty(ControlIDs) && Style != null) {
				string[] Controls = ControlIDs.Split(",");
				string TextSelectors = "";
				for (int i = 0; i <= Controls.Length - 1; i++) {
					if (!string.IsNullOrEmpty(TextSelectors))
						TextSelectors += ",";
					TextSelectors += "#" + Controls[i];
				}
				this.Rules.Add(TextSelectors, Style);
			}
		}
		public void AddIDBasedRule(string ControlIDs, string CustomStyle)
		{
			if (!string.IsNullOrEmpty(ControlIDs) && !string.IsNullOrEmpty(CustomStyle)) {
				string[] Controls = ControlIDs.Split(",");
				string TextSelectors = "";
				for (int i = 0; i <= Controls.Length - 1; i++) {
					if (!string.IsNullOrEmpty(TextSelectors))
						TextSelectors += ",";
					TextSelectors += "#" + Controls[i];
				}
				this.Rules.Add(TextSelectors, CustomStyle);
			}
		}
		public void AddClassBasedRule(string ClassNames, Style Style)
		{
			if (!string.IsNullOrEmpty(ClassNames) && Style != null) {
				string[] Classes = ClassNames.Split(",");
				string TextSelectors = "";
				for (int i = 0; i <= Classes.Length - 1; i++) {
					if (!string.IsNullOrEmpty(TextSelectors))
						TextSelectors += ",";
					TextSelectors += "." + Classes[i];
				}
				this.Rules.Add(TextSelectors, Style);
			}
		}
		public void AddClassBasedRule(string ClassNames, string CustomStyle)
		{
			if (!string.IsNullOrEmpty(ClassNames) && !string.IsNullOrEmpty(CustomStyle)) {
				string[] Classes = ClassNames.Split(",");
				string TextSelectors = "";
				for (int i = 0; i <= Classes.Length - 1; i++) {
					if (!string.IsNullOrEmpty(TextSelectors))
						TextSelectors += ",";
					TextSelectors += "." + Classes[i];
				}
				this.Rules.Add(TextSelectors, CustomStyle);
			}
		}
		public void AddRule(string ParentElementTag, string ParentElementID, string EffectiveElementTag, Style Style, string RestrictedStyleElements = "", string IgnoredStyleElements = "")
		{
			this.Rules.Add(ParentElementTag + "#" + ParentElementID + " " + EffectiveElementTag, Style.Draw(true, RestrictedStyleElements, IgnoredStyleElements));
		}
		public void AddCustomRule(string TextSelector, Style Style)
		{
			this.Rules.Add(TextSelector, Style);
		}
		public void AddCustomRule(string TextSelector, string CustomStyle)
		{
			if (!string.IsNullOrEmpty(CustomStyle)) {
				this.Rules.Add(TextSelector, CustomStyle);
			}
		}
		public string Draw()
		{
			Content Content = new Content();
			for (int i = 0; i <= this.Rules.Count - 1; i++) {
				Content.Add(this.Rules(i).Draw());
			}
			if (!string.IsNullOrEmpty(Content.Value)) {
				return "<style type=\"text/css\">" + Content.Value + "</style>";
			}
			return "";
		}
	}
}

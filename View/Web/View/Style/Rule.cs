using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View
{
	internal class Rule
	{
		private Style oStyle;
		private string sSelectorText = "";
		private string sCustomStyle = "";
		public Style Style {
			get { return this.oStyle; }
			set { this.oStyle = value; }
		}
		public string CustomStyle {
			get { return this.sCustomStyle; }
			set { this.sCustomStyle = value; }
		}
		public string SelectorText {
			get { return this.sSelectorText; }
			set { this.sSelectorText = value; }
		}
		internal string Draw()
		{
			if (!string.IsNullOrEmpty(this.SelectorText)) {
				if (this.Style != null) {
					string StyleInText = this.Style.Draw(true);
					if (!string.IsNullOrEmpty(StyleInText)) {
						return this.SelectorText + "{" + StyleInText + "}";
					}
				} else if (!string.IsNullOrEmpty(this.CustomStyle)) {
					return this.SelectorText + "{" + this.CustomStyle + "}";
				}
			}
			return "";
		}
		public Rule(string SelectorText, Style Style)
		{
			this.oStyle = Style;
			this.sSelectorText = SelectorText;
		}
		public Rule(string SelectorText, string CustomStyle)
		{
			this.sCustomStyle = CustomStyle;
			this.sSelectorText = SelectorText;
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class Title
	{
		private TitleConfiguration oStyle = new TitleConfiguration();
		private string sText;
		private Section oSection;
		public Section Section {
			get { return this.oSection; }
		}
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public TitleConfiguration Style {
			get { return this.oStyle; }
		}
		public string Draw()
		{
			string ReturnString = "";
			if (!string.IsNullOrEmpty(this.Text)) {
				if (this.ValidStyle.Spacing > 0) {
					ReturnString += "<DIV STYLE=padding-bottom:" + this.ValidStyle.Spacing + "px;>";
				}
				ReturnString += "<DIV";
				ReturnString += this.ValidStyle.GetStyle;
				ReturnString += ">" + this.Text + "</DIV>";
				if (this.ValidStyle.Spacing > 0) {
					ReturnString += "</DIV>";
				}
			}
			return ReturnString;
		}
		private TitleConfiguration ValidStyle {
			get {
				if (this.Style.Customized) {
					return this.Style;
				} else {
					return this.Section.Layout.Titles;
				}
			}
		}
		public Title(Section Section)
		{
			this.oSection = Section;
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class TitleConfiguration
	{
		private FontConfiguration oFont = new FontConfiguration();
		private BordersConfiguration oBorders = new BordersConfiguration();
		private VerticalAlignment eVerticalAlignment = VerticalAlignment.None;
		private HorizontalAlignment eHorizontalAlignment = HorizontalAlignment.None;
		private int nPadding = 0;
		private string sBackgroundColor = "";
		private bool bCustomized;
		private byte nSpacing = 0;
		internal bool Customized {
			get { return this.bCustomized || this.Font.Customized || this.Borders.Customized; }
		}
		public FontConfiguration Font {
			get { return this.oFont; }
		}
		public BordersConfiguration Borders {
			get { return this.oBorders; }
		}
		public VerticalAlignment VerticalAlignment {
			get { return this.eVerticalAlignment; }
			set { this.eVerticalAlignment = value; }
		}
		public HorizontalAlignment HorizontalAlignment {
			get { return this.eHorizontalAlignment; }
			set { this.eHorizontalAlignment = value; }
		}
		public string BackgroundColor {
			get { return this.sBackgroundColor; }
			set { this.sBackgroundColor = value; }
		}
		public int Padding {
			get { return this.nPadding; }
			set { this.nPadding = value; }
		}
		public int Spacing {
			get { return this.nSpacing; }
			set { this.nSpacing = value; }
		}
		private string GetAlignmentStyle()
		{
			string ReturnString = "";
			if (this.HorizontalAlignment != HorizontalAlignment.None) {
				ReturnString += "text-align:" + this.HorizontalAlignment.ToString.Replace("HorizontalAlignment.", "").ToLower;
				ReturnString += ";";
			}
			if (this.VerticalAlignment != VerticalAlignment.None) {
				ReturnString += "vertical-align:" + this.VerticalAlignment.ToString.Replace("VerticalAlignment.", "").ToLower;
				ReturnString += ";";
			}
			return ReturnString;
		}
		private string GetColorStyle()
		{
			string ReturnString = "";
			if (!string.IsNullOrEmpty(this.BackgroundColor)) {
				ReturnString += "background-color:" + this.BackgroundColor + ";";
			}
			return ReturnString;
		}
		private string GetPaddingStyle()
		{
			return "padding:" + this.Padding + "px;";
		}
		internal string GetStyle()
		{
			string ReturnString = " STYLE=\"";
			ReturnString += this.GetAlignmentStyle();
			ReturnString += this.GetColorStyle();
			ReturnString += this.Borders.GetStyle;
			ReturnString += this.Font.GetStyle;
			ReturnString += this.GetPaddingStyle();
			if (ReturnString == " STYLE=\"") {
				ReturnString = "";
			} else {
				ReturnString += "\"";
			}
			return ReturnString;
		}
	}
}

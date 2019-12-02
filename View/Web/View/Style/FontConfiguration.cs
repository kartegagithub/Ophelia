using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class FontConfiguration
	{
		private double nSize = -1;
		private TextDecoration eDecoration = TextDecoration.None;
		private FontStyle eStyle = FontStyle.None;
		private FontWeight eWeight = FontWeight.None;
		private string sFamily = "";
		private string sColor = "";
		private bool bIsLink = false;
		internal bool Customized = false;
		internal FontUnits eFontUnit = FontUnits.Point;
		public string Family {
			get { return this.sFamily; }
			set {
				this.sFamily = value;
				Customized = true;
			}
		}
		public bool IsLink {
			get { return this.bIsLink; }
			set { this.bIsLink = value; }
		}
		public string Color {
			get { return this.sColor; }
			set {
				this.sColor = value;
				Customized = true;
			}
		}
		public double Size {
			get { return this.nSize; }
			set {
				this.nSize = value;
				Customized = true;
			}
		}
		public FontUnits Unit {
			get { return this.eFontUnit; }
			set {
				this.eFontUnit = value;
				Customized = true;
			}
		}
		public FontStyle Style {
			get { return this.eStyle; }
			set {
				this.eStyle = value;
				Customized = true;
			}
		}
		public TextDecoration Decoration {
			get { return this.eDecoration; }
			set {
				this.eDecoration = value;
				Customized = true;
			}
		}
		public FontWeight Weight {
			get { return this.eWeight; }
			set {
				this.eWeight = value;
				Customized = true;
			}
		}
		internal string GetStyle()
		{
			string ReturnString = "";
			if (this.Customized) {
				if (!string.IsNullOrEmpty(this.Family)) {
					ReturnString += "font-family:" + this.Family + ";";
				}
				if (!string.IsNullOrEmpty(this.Color)) {
					ReturnString += "color:" + this.Color + ";";
				}

				if (this.Style != FontStyle.None) {
					ReturnString += "font-style:" + this.Style.ToString().Replace("FontStyle.", "").ToLower(new System.Globalization.CultureInfo("en-US")) + ";";
				}

				if (this.Weight != FontWeight.None) {
					ReturnString += "font-weight:" + this.Weight.ToString().Replace("FontWeight.", "").Replace("W", "").ToLower(new System.Globalization.CultureInfo("en-US")) + ";";
				}
				if (this.Size > 0) {
					ReturnString += "font-size:" + this.Size.ToString().Replace(",", ".") + this.GetFontUnit();
				}
				if ((this.Decoration != TextDecoration.None || this.IsLink) && (this.Decoration != TextDecoration.Underline || !this.IsLink)) {
					if (this.Decoration == TextDecoration.LineThrough) {
						ReturnString += "text-decoration:line-through;";
					} else {
						ReturnString += "text-decoration:" + this.Decoration.ToString().Replace("TextDecoration.", "").ToLower(new System.Globalization.CultureInfo("en-US")) + ";";
					}
				}
			}
			return ReturnString;
		}
		public string GetFontUnit()
		{
			switch (this.Unit) {
				case FontUnits.Em:
					return "em;";
				case FontUnits.Pixel:
					return "px;";
				case FontUnits.Point:
					return "pt;";
				case FontUnits.Percent:
					return "%;";
				default:
					return "pt;";
			}
		}
	}
	public enum TextDecoration
	{
		None,
		Underline,
		Overline,
		LineThrough,
		Blink
	}
	public enum FontStyle
	{
		None,
		Normal,
		Italic
	}
	public enum FontUnits
	{
		Em = 1,
		Pixel = 2,
		Point = 3,
		Percent = 4
	}
	public enum FontWeight
	{
		None,
		Normal,
		Bold,
		Bolder,
		Lighter,
		W100,
		W200,
		W300,
		W400,
		W500,
		W600,
		W700,
		W800,
		W900
	}
}

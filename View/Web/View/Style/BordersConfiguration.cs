using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class BordersConfiguration
	{
		private string sColor = "#000";
		private int nWidth = -1;
		private int nRadius = -1;
		private BorderStyle eStyle = BorderStyle.None;
		private BorderConfiguration oLeft = new BorderConfiguration(this, BorderSide.Left);
		private BorderConfiguration oRight = new BorderConfiguration(this, BorderSide.Right);
		private BorderConfiguration oTop = new BorderConfiguration(this, BorderSide.Top);
		private BorderConfiguration oBottom = new BorderConfiguration(this, BorderSide.Bottom);
		internal bool Customized = false;
		public string Color {
			get { return this.sColor; }
			set {
				this.sColor = value;
				this.Left.Color = value;
				this.Right.Color = value;
				this.Top.Color = value;
				this.Bottom.Color = value;
				this.Customized = true;
			}
		}
		public int Radius {
			get { return this.nRadius; }
			set {
				this.nRadius = value;
				this.Left.Radius = value;
				this.Right.Radius = value;
				this.Top.Radius = value;
				this.Bottom.Radius = value;
				this.Customized = true;
			}
		}
		public int Width {
			get { return this.nWidth; }
			set {
				this.nWidth = value;
				this.Left.Width = value;
				this.Right.Width = value;
				this.Top.Width = value;
				this.Bottom.Width = value;
				this.Customized = true;
			}
		}
		public BorderStyle Style {
			get { return this.eStyle; }
			set {
				this.eStyle = value;
				this.Left.Style = value;
				this.Right.Style = value;
				this.Top.Style = value;
				this.Bottom.Style = value;
				this.Customized = true;
			}
		}
		public BorderConfiguration Left {
			get { return this.oLeft; }
		}
		public BorderConfiguration Right {
			get { return this.oRight; }
		}
		public BorderConfiguration Top {
			get { return this.oTop; }
		}
		public BorderConfiguration Bottom {
			get { return this.oBottom; }
		}
		public void Set(int Width, BorderStyle Style)
		{
			this.Width = Width;
			this.Style = Style;
		}
		public void Set(int Width, BorderStyle Style, string Color)
		{
			this.Set(Width, Style);
			this.Color = Color;
		}
		public void Set(int Width, BorderStyle Style, string Color, int Radius)
		{
			this.Set(Width, Style, Color);
			this.Radius = Radius;
		}
		internal string GetStyle()
		{
			string ReturnString = "";
			if (this.Customized) {
				if (this.Width > -1) {
					if (this.Style != BorderStyle.Inherited) {
						ReturnString += "border:" + this.Width + "px";
						ReturnString += " " + this.Style.ToString().Replace("BorderStyle.", "").ToLower();
						if (!string.IsNullOrEmpty(this.Color)) {
							ReturnString += " " + this.Color;
						}
						ReturnString += ";";
					} else {
						ReturnString += "border-width:" + this.Width + "px;";
						if (!string.IsNullOrEmpty(this.Color)) {
							ReturnString += "border-color:" + this.Color + ";";
						}
					}

					if (this.Radius > -1) {
						ReturnString += "border-radius:" + Radius + "px;";
					}
				}
				ReturnString += this.Left.GetStyle;
				ReturnString += this.Right.GetStyle;
				ReturnString += this.Top.GetStyle;
				ReturnString += this.Bottom.GetStyle;
			}
			return ReturnString;
		}
	}
	public enum BorderStyle
	{
		None,
		Dotted,
		Dashed,
		Solid,
		Double,
		Groove,
		Ridge,
		Inset,
		Outset,
		Inherited
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class BorderConfiguration
	{
		private string sColor = "#000000";
		private int nWidth = -1;
		private BorderStyle eStyle = BorderStyle.Solid;
		private BorderSide Side;
		private int nRadius = -1;
		private BordersConfiguration Parent;
		public string Color {
			get { return this.sColor; }
			set {
				this.sColor = value;
				this.Parent.Customized = true;
			}
		}
		public int Width {
			get { return this.nWidth; }
			set {
				this.nWidth = value;
				this.Parent.Customized = true;
			}
		}
		public int Radius {
			get { return this.nRadius; }
			set {
				this.nRadius = value;
				this.Parent.Customized = true;
			}
		}
		public BorderStyle Style {
			get { return this.eStyle; }
			set {
				if (value == BorderStyle.Inherited)
					value = this.Parent.Style;
				this.eStyle = value;
				this.Parent.Customized = true;
			}
		}
		public void SetInput(string Color, int Width, BorderStyle Style)
		{
			this.Color = Color;
			this.Width = Width;
			this.Style = Style;
		}
		internal string GetStyle()
		{
			string ReturnString = "";
			if ((this.Width > -1 && (this.Width != this.Parent.Width || this.Color != this.Parent.Color || this.Style != this.Parent.Style))) {
				if (this.Style != BorderStyle.Inherited) {
					ReturnString += "border-" + this.Side.ToString().Replace("Side.", "").ToLower() + ":" + this.Width + "px";
					ReturnString += " " + this.Style.ToString.Replace("BorderStyle.", "").ToLower;
					if (!string.IsNullOrEmpty(this.Color)) {
						ReturnString += " " + this.Color;
					}
					ReturnString += ";";
				} else {
					ReturnString += "border-" + this.Side.ToString().Replace("Side.", "").ToLower() + "-width:" + this.Width + "px;";
					if (!string.IsNullOrEmpty(this.Color)) {
						ReturnString += "border-" + this.Side.ToString().Replace("Side.", "").ToLower() + "-color:" + this.Color + ";";
					}
				}
			}

			if (this.Radius > -1) {
				switch (this.Side.ToString().Replace("Side.", "").ToLower()) {
					case "top":
						ReturnString += "border-top-left-radius:" + Radius + "px;";
						break;
					case "left":
						ReturnString += "border-bottom-left-radius:" + Radius + "px;";
						break;
					case "right":
						ReturnString += "border-top-right-radius:" + Radius + "px;";
						break;
					case "bottom":
						ReturnString += "border-bottom-right-radius:" + Radius + "px;";
						break;
				}
			}
			return ReturnString;
		}
		public BorderConfiguration(BordersConfiguration Parent, BorderSide Side)
		{
			this.Parent = Parent;
			this.Side = Side;
		}
	}
	public enum BorderSide
	{
		Left,
		Right,
		Top,
		Bottom
	}
}

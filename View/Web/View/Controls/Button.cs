using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Button : InputDataControl
	{
		private ButtonType eType = ButtonType.Submit;
		private string sImageSource = "";
		public string ImageSource {
			get { return this.sImageSource; }
			set {
				this.sImageSource = value;
				if (!string.IsNullOrEmpty(value)) {
					this.Type = ButtonType.Image;
				}
			}
		}
		public ButtonType Type {
			get { return this.eType; }
			set { this.eType = value; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			Content.Clear();
			if (this.Type != ButtonType.AjaxButton) {
				Content.Add("<input ");
				switch (this.Type) {
					case ButtonType.Image:
						Content.Add(" type=\"image\" ");
						break;
					case ButtonType.Submit:
						Content.Add(" type=\"submit\" ");
						break;
				}
				if (!string.IsNullOrEmpty(this.Name)) {
					Content.Add(" name=\"" + this.Name + "\"");
				}
				if (this.ReadOnly) {
					Content.Add(" disabled ");
				}
				if (!string.IsNullOrEmpty(this.ID)) {
					Content.Add(" id=\"" + this.ID + "\"");
					if (string.IsNullOrEmpty(this.Name)) {
						Content.Add(" name=\"" + this.ID + "\"");
					}
				}
				switch (this.Type) {
					case ButtonType.Image:
						if (!string.IsNullOrEmpty(this.ImageSource)) {
							Content.Add(" src='" + this.ImageSource + "'");
						}
						Content.Add(" alt=\"" + this.ID + "\"");
						break;
				}
				if (!string.IsNullOrEmpty(this.Value)) {
					Content.Add(" value=\"" + this.Value + "\"");
				}
				if (!string.IsNullOrEmpty(this.Title))
					Content.Add(" title=\"" + this.Title + "\"");
				this.DrawEvents(Content);
				Content.Add(this.Style.Draw());
				Content.Add(" >");
			} else {
				Image Image = new Image(this.ID, this.ImageSource);
				Image.SetStyle(this.Style);
				Image.CloneEventsFrom(this);
				Content.Add(Image.Draw);
			}
		}
		public Button(string MemberName)
		{
			this.ID = MemberName;
		}
		public Button(string MemberName, string Value) : this(MemberName)
		{
			this.Value = Value;
		}
		public Button(string MemberName, string ImageSource, ButtonType Type, string Value = "") : this(MemberName)
		{
			this.Value = Value;
			this.ImageSource = ImageSource;
			this.Type = Type;
		}
		public enum ButtonType
		{
			None = 0,
			Submit = 1,
			Image = 2,
			AjaxButton = 3
		}
	}
}

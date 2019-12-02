using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class TextBox : InputDataControl
	{
		private string sSuffix = "";
		private int nMaxLength = -1;
		private string sText = "";
		private Label oTextControl;
		private bool sAutoComplete = true;
		private string sPlaceholder = string.Empty;
		public bool ReadOnlyAndDrawInput { get; set; }
		public string Placeholder {
			get { return this.sPlaceholder; }
			set { this.sPlaceholder = value; }
		}
		public virtual bool AutoComplete {
			get { return this.sAutoComplete; }
			set { this.sAutoComplete = value; }
		}
		public Label TextControl {
			get {
				if (this.oTextControl == null) {
					this.oTextControl = new Label(this.ID + "_Label");
					this.oTextControl.DrawingType = LabelDrawingType.Span;
				}
				return this.oTextControl;
			}
		}
		public virtual string Text {
			get { return this.sText; }
			set {
				this.sText = value;
				this.TextControl.Value = value;
			}
		}
		public string Suffix {
			get { return this.sSuffix; }
			set { this.sSuffix = value; }
		}
		public int MaxLength {
			get { return this.nMaxLength; }
			set { this.nMaxLength = value; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			if (this.ReadOnly && (this.GetType().Name == "TextBox" || this.GetType().Name == "NumberBox" || this.GetType().Name == "InfinityNumberBox" || this.GetType().Name == "DateTimePicker") && this.ReadOnlyAndDrawInput == false) {
				Label Label = new Label(this.ID);
				Label.Value = this.Value;
				Label.Title = this.Title;
				Label.SetStyle(this.Style);
				Content.Add(Label.Draw);
			} else {
				Content.Add("<input ");
				if (!string.IsNullOrEmpty(this.ID)) {
					Content.Add(" id=\"" + this.ID + "\"");
					Content.Add(" name=\"" + this.Name + "\"");
				}
				if (this.ReadOnlyAndDrawInput == true) {
					Content.Add(" readonly");
				}
				if (!string.IsNullOrEmpty(this.Language)) {
					Content.Add(" lang=\"" + this.Language + "\"");
				}
				if (!string.IsNullOrEmpty(this.Value)) {
					Content.Add(" value=\"" + this.Value + "\"");
				}
				if (!string.IsNullOrEmpty(this.Title))
					Content.Add(" title=\"" + this.Title + "\"");
				if (!this.AutoComplete) {
					Content.Add(" autocomplete=\"off\"");
				}
				if (!string.IsNullOrEmpty(this.Placeholder)) {
					Content.Add(" placeholder=\"" + this.Placeholder + "\"");
				}
				if (this.MaxLength > -1) {
					Content.Add(" maxlength=\"" + this.MaxLength + "\"");
				}
				if (this.Disabled) {
					Content.Add(" disabled=\"true\"");
				}
				if (this.oAttributes != null) {
					for (int i = 0; i <= this.Attributes.Count - 1; i++) {
						Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
					}
				}
				this.DrawEvents(Content);
				this.OnInputDrawn(Content);
				if (Page.UseHtml5) {
					Content.Add(" type=\"text\" ");
				} else {
					Content.Add(" type=\"" + this.GetHtml5Type() + "\" ");
				}

				Content.Add(this.Style.Draw());
				Content.Add(" >");
			}
			if (!string.IsNullOrEmpty(this.Suffix)) {
				Content.Add("<span style=\"padding-left:" + (this.ReadOnly ? "13" : "8") + "px\">" + this.Suffix + "</span>");
			}
			if (!string.IsNullOrEmpty(this.Text)) {
				Content.Add(" " + this.TextControl.Draw);
			}
		}
		protected virtual string GetHtml5Type()
		{
			return "text";
		}

		protected virtual void OnInputDrawn(Ophelia.Web.View.Content Content)
		{
		}
		public TextBox(string MemberName)
		{
			this.ID = MemberName;
		}
		public TextBox(string MemberName, string Value) : this(MemberName)
		{
			this.Value = Value;
		}
	}
}

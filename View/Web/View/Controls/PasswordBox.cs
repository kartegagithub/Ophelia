using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class PasswordBox : TextBox
	{
		private Label oTextControl;
		private bool sAutoComplete = false;
		public override bool AutoComplete {
			get { return this.sAutoComplete; }
			set { this.sAutoComplete = value; }
		}
		public new Label TextControl {
			get {
				if (this.oTextControl == null) {
					this.oTextControl = new Label(this.ID + "_Label");
					this.oTextControl.DrawingType = LabelDrawingType.Span;
				}
				return this.oTextControl;
			}
		}
		public override string Text {
			get { return base.Text; }
			set {
				base.Text = value;
				this.TextControl.Value = value;
			}
		}
		protected override string InitilizeValue()
		{
			return string.Empty;
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content.Clear();
			Content.Add("<input ");
			if (!string.IsNullOrEmpty(this.ID)) {
				Content.Add(" id=\"" + this.ID + "\"");
				Content.Add(" name=\"" + this.Name + "\"");
			}
			if (!string.IsNullOrEmpty(this.Language)) {
				Content.Add(" lang=\"" + this.Language + "\"");
			}
			if (this.ReadOnly)
				Content.Add(" readonly ");
			if (!string.IsNullOrEmpty(this.Value))
				Content.Add(" value=\"" + this.Value + "\"");
			if (!string.IsNullOrEmpty(this.Title))
				Content.Add(" title=\"" + this.Title + "\"");
			if (this.Disabled)
				Content.Add(" disable=\"true\"");
			if (!this.AutoComplete)
				Content.Add(" autocomplete=\"off\"");
			if (!string.IsNullOrEmpty(this.Placeholder))
				Content.Add(" placeholder=\"" + this.Placeholder + "\"");
			if (this.MaxLength > -1)
				Content.Add(" maxlength=\"").Add(this.MaxLength).Add("\"");
			Content.Add(" type=\"password\" ");
			Content.Add(this.Style.Draw());
			this.DrawEvents(Content);
			Content.Add(" >");
			if (!string.IsNullOrEmpty(this.Text))
				Content.Add(" " + this.TextControl.Draw);
			if (!string.IsNullOrEmpty(this.Suffix))
				Content.Add("<span style=\"padding-left:" + (this.ReadOnly ? "13" : "8") + "px\">" + this.Suffix + "</span>");
		}
		public PasswordBox(string MemberName) : base(MemberName)
		{
		}
		public PasswordBox(string MemberName, string Value) : this(MemberName)
		{
			this.Value = Value;
		}
	}
}

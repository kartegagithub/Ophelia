using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class TextArea : InputDataControl
	{
		private int nRowCount;
		private int nMaxLength;
		private string sText;
		private Label oTextControl;
		public virtual string Text {
			get { return this.sText; }
			set {
				this.sText = value;
				this.TextControl.Value = value;
			}
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
		public int MaxLength {
			get { return this.nMaxLength; }
			set { this.nMaxLength = value; }
		}
		public int RowCount {
			get { return this.nRowCount; }
			set { this.nRowCount = value; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			if (this.ReadOnly) {
				Label Label = new Label(this.ID, this.Value);
				Label.CloneEventsFrom(this);
				Label.SetStyle(this.Style);
				Content.Add(Label.Draw);
				return;
			}
			Content.Add("<textarea ");
			if (!string.IsNullOrEmpty(this.ID)) {
				Content.Add(" id=\"").Add(this.ID).Add("\"");
				Content.Add(" name=\"").Add(this.Name).Add("\"");
			}
			if (!string.IsNullOrEmpty(this.Language)) {
				Content.Add(" lang=\"" + this.Language + "\"");
			}
			if (this.MaxLength > -1) {
				Content.Add(" maxlength=\"").Add(this.MaxLength).Add("\"");
				this.Validators.AddMaxLengthValidator();
			}
			if (this.RowCount > 0)
				Content.Add(" rows=\"").Add(this.RowCount).Add("\"");
			if (!string.IsNullOrEmpty(this.Title))
				Content.Add(" title=\"").Add(this.Title).Add("\"");
			if (this.Disabled)
				Content.Add(" disabled=\"true\"");
			if (this.oAttributes != null) {
				for (int i = 0; i <= this.Attributes.Count - 1; i++) {
					Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
				}
			}
			this.DrawEvents(Content);
			Content.Add(this.Style.Draw());
			Content.Add(">");
			Content.Add(this.Value);
			Content.Add("</textarea>");
			if (!string.IsNullOrEmpty(this.Text))
				Content.Add(" ").Add(this.TextControl.Draw);
		}
		public TextArea(string MemberName, string Value) : this(MemberName)
		{
			base.Value = Value;
		}
		public TextArea(string MemberName)
		{
			this.ID = MemberName;
			this.nRowCount = 0;
			this.nMaxLength = -1;
			this.sText = string.Empty;
			this.oTextControl = null;
		}
	}
}

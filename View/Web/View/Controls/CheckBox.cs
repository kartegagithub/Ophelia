using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class CheckBox : InputDataControl
	{
		private bool bChecked = false;
		private string sText = string.Empty;
		private string sSortOrder = string.Empty;
		private Label oTextControl;
		private bool Drawing = false;
		public Label TextControl {
			get {
				if (this.oTextControl == null) {
					this.oTextControl = new Label(this.ID + "_Label");
					this.oTextControl.DrawingType = LabelDrawingType.Span;
				}
				return this.oTextControl;
			}
		}
		public string Text {
			get { return this.sText; }
			set {
				this.sText = value;
				this.TextControl.Value = value;
			}
		}
		public string SortOrder {
			get { return this.sSortOrder; }
			set { this.sSortOrder = value; }
		}
		public new bool Value {
			get {
				if (base.Value != null && base.Value.ToUpper == "TRUE") {
					return true;
				}
				int ParsedInteger = 0;
				Int32.TryParse(base.Value, out ParsedInteger);
				if (ParsedInteger > 0) {
					return true;
				}
				return false;
			}
			set { base.Value = value.ToString(); }
		}
		public override string OnChangeEvent {
			get {
				if (this.Drawing)
					return "";
				return this.OnClickEvent;
			}
			set { base.OnClickEvent = value; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			this.Drawing = true;
			Content.Clear();
			if (string.IsNullOrEmpty(this.ID))
				this.ID = "CheckboxControl";
			Content.Add("<input name=\"" + this.Name + "\"");
			Content.Add(" id=\"" + this.ID + "\"");
			Content.Add(" type=\"checkbox\" ");
			Content.Add(this.Style.Draw);
			if (this.Value) {
				Content.Add("checked=\"checked\"");
			}
			if (!string.IsNullOrEmpty(this.Title))
				Content.Add(" title=\"" + this.Title + "\"");
			if (this.SortOrder != string.Empty) {
				Content.Add(" sortorder=\"" + this.SortOrder + "\"");
			}
			if (this.oAttributes != null) {
				for (int i = 0; i <= this.Attributes.Count - 1; i++) {
					Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
				}
			}
			this.DrawEvents(Content);
			if (this.ReadOnly || this.Disabled) {
				Content.Add(" disabled=\"true\" ");
			}
			Content.Add(" >");
			if (this.Text != string.Empty) {
				if (this.Style.Display == DisplayMethod.Hidden) {
					this.TextControl.Style.Display = DisplayMethod.Hidden;
				}
				Content.Add(" " + this.TextControl.Draw);
				this.TextControl.OnClickEvent = "document.getElementById('" + this.ID + "').checked =!document.getElementById('" + this.ID + "').checked;" + this.TextControl.OnClickEvent;
			}
			this.Drawing = false;
		}
		public CheckBox(string MemberName)
		{
			this.ID = MemberName.Replace(".", "_");
		}
	}
}

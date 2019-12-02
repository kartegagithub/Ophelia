using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class ConditionalTextBox : TextBox
	{
		private string sConditionText = string.Empty;
		private CheckBox oConditionCheckBox;
		public string ConditionText {
			get { return this.sConditionText; }
			set { this.sConditionText = value; }
		}
		public CheckBox ConditionCheckBox {
			get {
				if (this.oConditionCheckBox == null) {
					this.oConditionCheckBox = new CheckBox(this.ID + "_ConditionCheckBox");
				}
				return this.oConditionCheckBox;
			}
		}
		private void AddConditionalFieldCheckBoxClickedEvent()
		{
			if (this.Script.Function("ConditionalTextBoxFieldCheckBoxClickedEvent") == null) {
				System.Text.StringBuilder FunctionString = new System.Text.StringBuilder();
				FunctionString.AppendLine("var controlID = element.id.replace('_ConditionCheckBox','');");
				FunctionString.AppendLine("");
				FunctionString.AppendLine("if (!element.checked){ ");
				FunctionString.AppendLine("    document.getElementById(controlID).style.display = 'none';");
				FunctionString.AppendLine("    document.getElementById(controlID + '_ConditionText').style.display = 'none';}");
				FunctionString.AppendLine(" else{");
				FunctionString.AppendLine("    document.getElementById(controlID).style.display = 'block';");
				FunctionString.AppendLine("    document.getElementById(controlID + '_ConditionText').style.display = 'block';}");
				this.Script.AddFunction("ConditionalTextBoxFieldCheckBoxClickedEvent", FunctionString.ToString(), "element", "");
			}
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			this.AddConditionalFieldCheckBoxClickedEvent();
			if (this.ReadOnly && (this.GetType().Name == "TextBox" || this.GetType().Name == "NumberBox" || this.GetType().Name == "InfinityNumberBox" || this.GetType().Name == "DateTimePicker")) {
				Label Label = new Label(this.ID);
				Label.Value = this.Value;
				Label.Title = this.Title;
				Label.SetStyle(this.Style);
				Content.Add(Label.Draw);
			} else {
				Content.Add("<div id=\"" + this.ID + "_Container" + "\">");

				this.ConditionCheckBox.ID = this.ID + "_ConditionCheckBox";
				if (this.ConditionCheckBox.Value == true) {
					this.ConditionCheckBox.Style.Display = DisplayMethod.Block;
				}
				this.ConditionCheckBox.Style.Left = 0;
				this.ConditionCheckBox.Style.VerticalAlignment = VerticalAlignment.Middle;
				this.ConditionCheckBox.OnClickEvent += "ConditionalTextBoxFieldCheckBoxClickedEvent(this);";
				Content.Add(this.ConditionCheckBox.Draw());

				Content.Add("<span id=\"" + this.ID + "_ConditionText" + "\" ");
				if (this.ConditionCheckBox.Value == false) {
					Content.Add(" style=\"display:none;\" ");
				}
				Content.Add(">");
				Content.Add(this.ConditionText);
				Content.Add("</span>");

				Content.Add("<input ");
				if (!string.IsNullOrEmpty(this.ID)) {
					Content.Add(" id=\"" + this.ID + "\"");
					Content.Add(" name=\"" + this.Name + "\"");
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
				Content.Add(" type=\"text\" ");
				if (this.ConditionCheckBox.Value == false) {
					this.Style.Display = DisplayMethod.Hidden;
				} else {
					this.Style.Display = DisplayMethod.Block;
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
			Content.Add("</div>");
		}
		public ConditionalTextBox(string MemberName) : base(MemberName)
		{
		}
		public ConditionalTextBox(string MemberName, string Value) : base(MemberName, Value)
		{
		}
	}
}

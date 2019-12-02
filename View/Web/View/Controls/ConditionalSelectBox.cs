using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using Ophelia.Web.View.Forms;
using Ophelia.Web.View.Controls.DataGrid;
namespace Ophelia.Web.View.Controls
{
	public class ConditionalSelectBox : SelectBox
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
			if (this.Script.Function("ConditionalSelectBoxFieldCheckBoxClickedEvent") == null) {
				System.Text.StringBuilder FunctionString = new System.Text.StringBuilder();
				FunctionString.AppendLine("var controlID = element.id.replace('_ConditionCheckBox','');");
				FunctionString.AppendLine("");
				FunctionString.AppendLine("if (!element.checked){ ");
				FunctionString.AppendLine("    document.getElementById(controlID).selectedIndex = 0;");
				FunctionString.AppendLine("    document.getElementById(controlID).style.display = 'none';");
				FunctionString.AppendLine("    document.getElementById(controlID + '_ConditionText').style.display = 'none';}");
				FunctionString.AppendLine(" else{");
				FunctionString.AppendLine("    document.getElementById(controlID).style.display = 'block';");
				FunctionString.AppendLine("    document.getElementById(controlID + '_ConditionText').style.display = 'block';}");
				this.Script.AddFunction("ConditionalSelectBoxFieldCheckBoxClickedEvent", FunctionString.ToString(), "element", "");
			}
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			if (this.DataSource != null) {
				this.Options.Clear();
				this.DataGrid.Bind();
				this.Options.SelectedValue = "";
				for (int i = 0; i <= this.DataGrid.Rows.Count - 1; i++) {
					this.Options.Add(this.DataGrid.Rows(i).ItemID.ToString(), this.DataGrid.Rows(i).Cells(this.DisplayMember).Text());
					if (this.SelectedRow != null) {
						if (this.DataGrid.Rows(i).ItemID > 0) {
							if (this.DataGrid.Rows(i).ItemID == this.SelectedRow.ItemID) {
								Options.SelectedValue = this.DataGrid.Rows(i).ItemID.ToString();
							}
						} else {
							if (object.ReferenceEquals(this.DataGrid.Rows(i), this.SelectedRow)) {
								Options.SelectedValue = this.DataGrid.Rows(i).ItemID.ToString();
							}
						}
					}
				}
			} else {
				if (!string.IsNullOrEmpty(this.Value)) {
					this.Options.SelectedValue = this.Value;
				}
			}
			if (string.IsNullOrEmpty(this.ID))
				this.ID = "SelectboxControl";
			if (this.ReadOnly) {
				if (this.Options.SelectedOption == null) {
					Content.Add("&nbsp;");
				} else {
					Label OptionLabel = new Label("SelectedOption");
					OptionLabel.Value = this.Options.SelectedOption.Text;
					OptionLabel.Style.Top = 3;
					Content.Add(OptionLabel.Draw);
					HiddenBox SelectedValue = new HiddenBox(this.ID);
					SelectedValue.Value = this.Options.SelectedOption.Value;
					Content.Add(SelectedValue.Draw);
				}
			} else {
				this.AddConditionalFieldCheckBoxClickedEvent();
				Content.Add("<div id=\"" + this.ID + "_Container" + "\">");
				this.ConditionCheckBox.ID = this.ID + "_ConditionCheckBox";
				if (this.ConditionCheckBox.Value == true) {
					this.ConditionCheckBox.Style.Display = DisplayMethod.InlineBlock;
				}
				this.ConditionCheckBox.Style.Left = 0;
				this.ConditionCheckBox.Style.VerticalAlignment = VerticalAlignment.Middle;
				this.ConditionCheckBox.OnClickEvent += "ConditionalSelectBoxFieldCheckBoxClickedEvent(this);";
				Content.Add(this.ConditionCheckBox.Draw());

				Content.Add("<span id=\"" + this.ID + "_ConditionText" + "\" ");
				if (this.ConditionCheckBox.Value == false) {
					Content.Add(" style=\"display:none;\" ");
				} else {
					Content.Add(" style=\"display:block;\" ");
				}
				Content.Add(">");
				Content.Add(this.ConditionText);
				Content.Add("</span>");

				Content.Add("<select name=\"" + this.Name + "\"");
				Content.Add(" id=\"" + this.ID + "\"");
				if (this.ConditionCheckBox.Value == false) {
					this.Style.Display = DisplayMethod.Hidden;
				} else {
					this.Style.Display = DisplayMethod.Block;
				}
				Content.Add(Style.Draw);
				if (this.oAttributes != null) {
					for (int i = 0; i <= this.Attributes.Count - 1; i++) {
						Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
					}
				}
				this.DrawEvents(Content);
				if (this.Disable)
					Content.Add(" disabled=\"disabled\"");
				if (this.Multiple)
					Content.Add(" multiple");
				Content.Add(">");
				Content.Add(this.Options.Draw);
				Content.Add("</select>");
				Content.Add("</div>");
			}
		}
		public ConditionalSelectBox(string Name, string Value) : base(Name, Value)
		{
		}
		public ConditionalSelectBox(string Name) : base(Name)
		{
		}
	}
}

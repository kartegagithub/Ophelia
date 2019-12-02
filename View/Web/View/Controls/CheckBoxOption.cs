using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class CheckBoxOption : InputDataControl
	{
		private CheckBoxOptionCollection oCollection;
		private string sText = "";
		private Label oLabel = null;
		private bool bChecked = false;
		private int iTextCharacterSize = -1;
		private LabelPositionType eLabelPosition = LabelPositionType.Right;
		public int TextCharacterSize {
			get { return this.iTextCharacterSize; }
			set { this.iTextCharacterSize = value; }
		}
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public bool Checked {
			get { return this.bChecked; }
			set { this.bChecked = value; }
		}
		public CheckBoxOptionCollection Collection {
			get { return this.oCollection; }
		}
		public LabelPositionType LabelPosition {
			get { return this.eLabelPosition; }
			set { this.eLabelPosition = value; }
		}
		public Label Label {
			get {
				if (this.oLabel == null) {
					this.oLabel = new Label("Option");
					this.oLabel.DrawingType = LabelDrawingType.Span;
				}
				return this.oLabel;
			}
		}
		public CheckBoxOption(CheckBoxOptionCollection Collection)
		{
			this.oCollection = Collection;
			this.iTextCharacterSize = 18;
		}
		public CheckBoxOption(CheckBoxOptionCollection Collection, int TextCharacterSize)
		{
			this.oCollection = Collection;
			this.iTextCharacterSize = TextCharacterSize;
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content TempContent = new Content();
			TempContent.Add("<input name=\"" + this.Collection.CheckBoxList.ID + "\" type=\"checkbox\" ");
			if (this.Collection != null && this.Collection.Count > 0) {
				TempContent.Add(" id= \"" + this.Collection.CheckBoxList.ID + "_" + this.Value + "\"");
			} else {
				TempContent.Add(" id= \"" + this.Collection.CheckBoxList.ID + "\"");
			}
			if (this.ReadOnly || this.Disabled) {
				TempContent.Add(" disabled=\"disabled\" ");
			}
			this.Style.Class = " " + this.Collection.CheckBoxList.ID + "_CheckBoxOptionClass";
			int TempLeftMargin = this.Style.Left;
			this.Style.Left = 0;
			TempContent.Add(this.Style.Draw);
			this.Style.Left = TempLeftMargin;
			if (!string.IsNullOrEmpty(this.Value)) {
				TempContent.Add(" value=\"" + this.Value + "\"");
			}
			TempContent.Add(" onclick=\"" + this.Collection.CheckBoxList.ID + "_CheckBoxOptionClicked();\" ");
			if (this.Checked) {
				TempContent.Add("checked");
			}
			if (this.Collection != null && this.Collection.CheckBoxList != null) {
				this.CloneEventsFrom(this.Collection.CheckBoxList);
			}
			if (this.oAttributes != null) {
				for (int i = 0; i <= this.Attributes.Count - 1; i++) {
					TempContent.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
				}
			}
			this.DrawEvents(TempContent);
			TempContent.Add(" >");
			this.Label.ID = this.ID;
			if (this.Label.Style.Top == int.MinValue) {
				this.Label.Style.Top = 3;
			}
			if (TextCharacterSize > -1 && this.Text.Length > TextCharacterSize) {
				this.Label.Title = this.Text;
				this.Label.Value = Strings.Left(this.Text, TextCharacterSize) + "...";
			} else {
				this.Label.Value = this.Text;
			}
			if (this.LabelPosition == LabelPositionType.Left) {
				Content.Add(this.Label.Draw);
				Content.Add(TempContent.Value);
			} else {
				Content.Add(TempContent.Value);
				Content.Add(this.Label.Draw);
			}
			if (this.Script.Function(this.Collection.CheckBoxList.ID + "_CheckBoxOptionClicked") == null) {
				System.Text.StringBuilder FunctionString = new System.Text.StringBuilder();
				FunctionString.AppendLine("var elements = document.getElementsByClassName('" + this.Collection.CheckBoxList.ID + "_CheckBoxOptionClass');");
				FunctionString.AppendLine("var selectedCheckboxes = new Array(); ");
				FunctionString.AppendLine("for (var i=0; i< elements.length;i++){");
				FunctionString.AppendLine("    if (elements[i].checked == true){");
				FunctionString.AppendLine("        selectedCheckboxes.push(elements[i].value);");
				FunctionString.AppendLine("     }");
				FunctionString.AppendLine("}");
				FunctionString.AppendLine("document.getElementById('" + this.Collection.CheckBoxList.ID + "').value = selectedCheckboxes.join(); ");
				this.Script.AddFunction(this.Collection.CheckBoxList.ID + "_CheckBoxOptionClicked", FunctionString.ToString());
			}
		}
	}
}

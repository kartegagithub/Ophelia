using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class RadioOption : InputDataControl
	{
		private RadioOptionCollection oCollection;
		private string sText = "";
		private Label oLabel = null;
		private LabelPositionType eLabelPosition = LabelPositionType.Right;
		private bool bLabelCanBeClicked = false;
		private bool bUseOptionStyleOnLabel = false;
		public bool UseOptionStyleOnLabel {
			get { return this.bUseOptionStyleOnLabel; }
			set { this.bUseOptionStyleOnLabel = value; }
		}
		public bool LabelCanBeClicked {
			get { return this.bLabelCanBeClicked; }
			set { this.bLabelCanBeClicked = value; }
		}
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public RadioOptionCollection Collection {
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
		public RadioOption(RadioOptionCollection Collection)
		{
			this.oCollection = Collection;
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content TempContent = new Content();
			TempContent.Add("<input name=\"" + this.Collection.RadioButton.ID + "\" type=\"radio\" ");
			if (this.Collection != null && this.Collection.Count > 0) {
				TempContent.Add(" id= \"" + this.Collection.RadioButton.ID + "_" + this.Value + "\"");
			} else {
				TempContent.Add(" id= \"" + this.Collection.RadioButton.ID + "\"");
			}
			if (this.ReadOnly || this.Disabled) {
				TempContent.Add(" disabled=\"disabled\" ");
			}
			TempContent.Add(this.Style.Draw);
			if (!string.IsNullOrEmpty(this.Value)) {
				TempContent.Add(" value=\"" + this.Value + "\"");
			}
			if (this.Collection.SelectedValue == this.Value) {
				TempContent.Add("checked");
			}
			if (this.Collection != null && this.Collection.RadioButton != null) {
				this.CloneEventsFrom(this.Collection.RadioButton);
			}
			this.DrawEvents(TempContent);
			TempContent.Add(" >");
			this.Label.ID = this.ID;
			this.Label.Value = this.Text;
			if (this.UseOptionStyleOnLabel) {
				this.Label.SetStyle(this.Style.Clone);
			}
			if (this.LabelCanBeClicked) {
				this.Label.OnClickEvent = "document.getElementById('" + this.Collection.RadioButton.ID + "_" + this.Value + "').checked=true;" + this.OnChangeEvent;
				this.Label.Style.CursorStyle = Cursor.Pointer;
			}
			if (this.LabelPosition == LabelPositionType.Left) {
				Content.Add(this.Label.Draw);
				Content.Add(TempContent.Value);
			} else {
				Content.Add(TempContent.Value);
				Content.Add(this.Label.Draw);
			}
		}
	}
	public enum LabelPositionType : byte
	{
		Left = 1,
		Right = 2
	}
}

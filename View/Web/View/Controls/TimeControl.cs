using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
namespace Ophelia.Web.View.Controls
{
	public class TimeControl : InputDataControl
	{
		private string sCalenderImage = string.Empty;
		private SelectBox oHourSelectBox;
		private SelectBox oMinuteSelectBox;
		private string sTimeSeperator = ":";
		public string TimeSeperator {
			get { return this.sTimeSeperator; }
			set { this.sTimeSeperator = value; }
		}
		public SelectBox HourSelectBox {
			get { return this.oHourSelectBox; }
			set { this.oHourSelectBox = value; }
		}
		public SelectBox MinuteSelectBox {
			get { return this.oMinuteSelectBox; }
			set { this.oMinuteSelectBox = value; }
		}
		public new DateTime Value {
			get {
				if (!Information.IsDate(base.Value))
					return DateTime.MinValue;
				return base.Value;
			}
			set { base.Value = value; }
		}
		private SimpleCollection GetTimeCollection(bool IsMinute = false)
		{
			SimpleCollection oTimeCollection = new SimpleCollection();
			for (int i = 0; i <= (IsMinute ? 59 : 23); i++) {
				oTimeCollection.Add(i, (i < 10 ? "0" : "") + i.ToString());
			}
			return oTimeCollection;
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.oHourSelectBox = new SelectBox(this.ID + "_HourSelectBox", "");
			this.oHourSelectBox.CreateBlankOption = false;
			this.oHourSelectBox.Style.Float = FloatType.Left;
			this.oHourSelectBox.DataSource = this.GetTimeCollection();

			this.oMinuteSelectBox = new SelectBox(this.ID + "_MinuteSelectBox", "");
			this.oMinuteSelectBox.CreateBlankOption = false;
			this.oMinuteSelectBox.Style.Float = FloatType.Left;
			this.oMinuteSelectBox.DataSource = this.GetTimeCollection(true);

			if (this.ReadOnly) {
				this.oHourSelectBox.ReadOnly = true;
				this.oMinuteSelectBox.ReadOnly = true;
			}
			this.oHourSelectBox.OnChangeEvent = this.SetValueEvent();
			this.oMinuteSelectBox.OnChangeEvent = this.SetValueEvent();
			if (this.Value == DateTime.MinValue)
				base.Value = string.Empty;
			if (base.Value != string.Empty) {
				this.HourSelectBox.SelectedItem = this.Value.Hour;
				this.MinuteSelectBox.SelectedItem = this.Value.Minute;
			}
			Content.Add("<div id=\"" + this.ID + "_Container\">");
			Content.Add("<input type=\"hidden\" ");
			Content.Add("id=\"" + this.ID + "\" ");
			Content.Add("value=\"" + (this.Value.Hour < 10 ? "0" : "") + this.Value.Hour + this.TimeSeperator + (this.Value.Minute < 10 ? "0" : "") + this.Value.Minute + "\" ");
			Content.Add(">");
			Content.Add(this.HourSelectBox.Draw());
			Content.Add("<span style=\"float:left;font-weight:bold;" + (!this.ReadOnly ? "margin-top:2px;" : "") + "\">&nbsp;" + this.TimeSeperator + "&nbsp;</span>");
			Content.Add(this.MinuteSelectBox.Draw());
			Content.Add("</div>");
		}
		private string SetValueEvent()
		{
			if (this.Script.Function("SetTimeFieldValue") == null) {
				System.Text.StringBuilder returnString = new System.Text.StringBuilder();
				returnString.AppendLine("var hour = document.getElementById('" + this.ID + "_HourSelectBox').value;");
				returnString.AppendLine("var minute = document.getElementById('" + this.ID + "_MinuteSelectBox').value;");
				returnString.AppendLine("if (hour == '') {hour = '0';}");
				returnString.AppendLine("if (hour < 10) { hour = '0' + hour;}");
				returnString.AppendLine("if (minute == '') {minute = '0';}");
				returnString.AppendLine("if (minute < 10) { minute = '0' + minute;}");
				returnString.AppendLine("document.getElementById('" + this.ID + "').value = hour + ':' + minute; ");
				this.Script.AddFunction("SetTimeFieldValue", returnString.ToString());
			}
			return "SetTimeFieldValue();";
		}
		public TimeControl(string MemberName) : base()
		{
			this.ID = MemberName;
		}
	}
}

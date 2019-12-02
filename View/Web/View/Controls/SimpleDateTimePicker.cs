using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class SimpleDateTimePicker : DataControl
	{
		private SimpleDateTimeType eType = SimpleDateTimeType.Default;
		private DateTime oMinDate;
		private DateTime oMaxDate;
		public SimpleDateTimeType Type {
			get { return this.eType; }
			set { this.eType = value; }
		}
		public new DateTime Value {
			get {
				if (!Information.IsDate(base.Value))
					return DateTime.MinValue;
				return base.Value;
			}
			set { base.Value = value; }
		}
		public DateTime MinDate {
			get { return oMinDate; }
			set { oMinDate = value; }
		}
		public DateTime MaxDate {
			get { return this.oMaxDate; }
			set { this.oMaxDate = value; }
		}
		private Style oControlStyle;
		public Style ControlStyle {
			get {
				if (this.oControlStyle == null) {
					this.oControlStyle = new Style();
					this.oControlStyle.Float = FloatType.Left;
					this.oControlStyle.HorizontalAlignment = HorizontalAlignment.Center;
				}
				return this.oControlStyle;
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			SelectBox YearSelectBox = new SelectBox(this.ID + "year");
			SelectBox MonthSelectBox = new SelectBox(this.ID + "month");
			SelectBox DaySelectBox = new SelectBox(this.ID + "day");
			HiddenBox HiddenValue = new HiddenBox(this.ID);
			Panel Panel = new Panel(this.ID + "container");
			Panel.SetStyle(this.Style);
			if (Value == DateTime.MinValue)
				Value = DateAndTime.Now;
			for (int i = MinDate.Year; i <= MaxDate.Year; i++) {
				YearSelectBox.Options.Add(i);
			}
			if (MaxDate.Subtract(MinDate).TotalDays > 366) {
				for (int i = 1; i <= 12; i++) {
					MonthSelectBox.Options.Add(i, i);
				}
			} else {
				int FirstMonth = 0;
				int LastMonth = 0;
				if (MinDate.Month > MaxDate.Month) {
					FirstMonth = MaxDate.Month;
					LastMonth = MinDate.Month;
				} else {
					LastMonth = MaxDate.Month;
					FirstMonth = MinDate.Month;
				}
				for (int i = FirstMonth; i <= LastMonth; i++) {
					MonthSelectBox.Options.Add(i, i);
				}
			}
			for (int i = 1; i <= 31; i++) {
				DaySelectBox.Options.Add(i, i);
			}
			YearSelectBox.Value = Value.Year;
			YearSelectBox.CreateBlankOption = false;
			YearSelectBox.Style.WidthInPercent = 50;

			MonthSelectBox.Value = Value.Month;
			MonthSelectBox.CreateBlankOption = false;
			MonthSelectBox.Style.WidthInPercent = 24;

			this.StyleSheet.AddIDBasedRule(MonthSelectBox.ID, "margin-right:1%;");
			this.StyleSheet.AddIDBasedRule(DaySelectBox.ID, "margin-right:1%;");

			DaySelectBox.Value = Value.Day;
			DaySelectBox.CreateBlankOption = false;
			DaySelectBox.Style.WidthInPercent = 24;

			YearSelectBox.OnChangeEvent = "SelectSimpleDateTimePicker('" + this.ID + "')";
			MonthSelectBox.OnChangeEvent = YearSelectBox.OnChangeEvent;
			DaySelectBox.OnChangeEvent = YearSelectBox.OnChangeEvent;

			this.StyleSheet.AddCustomRule("#" + this.ID + "container select", this.ControlStyle);
			Panel.Controls.Add(DaySelectBox);
			Panel.Controls.Add(MonthSelectBox);
			Panel.Controls.Add(YearSelectBox);
			HiddenValue.Value = DateAndTime.Now.Day.ToString() + "." + DateAndTime.Now.Month.ToString() + "." + DateAndTime.Now.Year.ToString();
			Panel.Controls.Add(HiddenValue);
			Content.Add(Panel.Draw);
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			if (Script.Function("SelectSimpleDateTimePicker") == null) {
				ServerSide.ScriptManager.Function SelectSimpleDateTimePickerFunction = this.Script.AddFunction("SelectSimpleDateTimePicker", "", "id");
				SelectSimpleDateTimePickerFunction.AppendLine("var dayelement = document.getElementById(id + 'day');");
				SelectSimpleDateTimePickerFunction.AppendLine("var monthelement = document.getElementById(id + 'month');");
				SelectSimpleDateTimePickerFunction.AppendLine("var yearelement = document.getElementById(id + 'year');");
				SelectSimpleDateTimePickerFunction.AppendLine("var year = parseInt(yearelement.options[yearelement.selectedIndex].value);");
				SelectSimpleDateTimePickerFunction.AppendLine("var month = parseInt(monthelement.options[monthelement.selectedIndex].value);");
				SelectSimpleDateTimePickerFunction.AppendLine("var day = parseInt(dayelement.options[dayelement.selectedIndex].value);");
				SelectSimpleDateTimePickerFunction.AppendLine("var daycount=31;");
				SelectSimpleDateTimePickerFunction.AppendLine("if(month==2){");
				SelectSimpleDateTimePickerFunction.AppendLine("   if(year%4 ==0){daycount=29;} else{daycount=28;}");
				SelectSimpleDateTimePickerFunction.AppendLine("}");
				SelectSimpleDateTimePickerFunction.AppendLine("else if(month==4 || month==6 || month==9 || month==11){");
				SelectSimpleDateTimePickerFunction.AppendLine("   daycount=30;");
				SelectSimpleDateTimePickerFunction.AppendLine("}");
				SelectSimpleDateTimePickerFunction.AppendLine("var selectedday = dayelement.options[dayelement.selectedIndex].value;");
				SelectSimpleDateTimePickerFunction.AppendLine("for (var i=dayelement.options.length;i>=0;i--)");
				SelectSimpleDateTimePickerFunction.AppendLine("{");
				SelectSimpleDateTimePickerFunction.AppendLine("     dayelement.remove(i);");
				SelectSimpleDateTimePickerFunction.AppendLine("}");
				SelectSimpleDateTimePickerFunction.AppendLine("for (var i=1;i<=daycount;i++)");
				SelectSimpleDateTimePickerFunction.AppendLine("{");
				SelectSimpleDateTimePickerFunction.AppendLine("     var option = document.createElement('option');");
				SelectSimpleDateTimePickerFunction.AppendLine("     option.text = i.toString(); ");
				SelectSimpleDateTimePickerFunction.AppendLine("     option.value = i;");
				SelectSimpleDateTimePickerFunction.AppendLine("     if (i == selectedday)");
				SelectSimpleDateTimePickerFunction.AppendLine("         option.selected = true;");
				SelectSimpleDateTimePickerFunction.AppendLine("     dayelement.add(option);");
				SelectSimpleDateTimePickerFunction.AppendLine("}");
				SelectSimpleDateTimePickerFunction.AppendLine("var valueelement = document.getElementById(id);");
				SelectSimpleDateTimePickerFunction.AppendLine("valueelement.value =  dayelement.options[dayelement.selectedIndex].value.toString() + '.' + monthelement.options[monthelement.selectedIndex].value.toString() + '.' + yearelement.options[yearelement.selectedIndex].value.toString();");
			}
		}
		public SimpleDateTimePicker(string MemberName) : base()
		{
			this.ID = MemberName;
			this.MinDate = DateAndTime.Now.AddYears(-10);
			this.MaxDate = DateAndTime.Now.AddYears(10);
		}
		public SimpleDateTimePicker(string MemberName, DateTime Value) : this(MemberName)
		{
			this.Value = Value;
		}
	}
	public enum SimpleDateTimeType
	{
		Default = 1
	}
}

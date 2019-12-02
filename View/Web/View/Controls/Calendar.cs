using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Controls
{
	public class Calendar : WebControl
	{
		private System.DateTime dVisibleDate = DateAndTime.Now;
		private string sTodayFontColor = "#000000";
		private string sTodayBackColor = "#FFFFFF";
		private string sFontColor = "";
		private string sBackColor = "";
		private string sTitleFontColor = "#000000";
		private string sTitleBackColor = "#6699CC";
		private string sSelectedDateFontColor = "#000000";
		private string sSelectedDateBackColor = "#6699CC";
		private string sDayHeaderFontColor = "";
		private string sDayHeaderBackColor = "#C3CDD9";
		private string sBorderColor = "#000000";
		private int nWidth = 200;
		private string sUrl = "";
		private int nVisibleYear = DateAndTime.Now.Year;
		private int nVisibleMonth = DateAndTime.Now.Month;
		private HttpRequest oRequest;
		private string sOnValueChanged = "";
		private string sDateParameterName = "Date";
		private bool bShowWeekIndex = false;
		private bool bShowShortCuts = false;
		protected string LastDayDrawn = "";
		private int nFutureSpan = 10;
		private int nPastSpan = 10;
		public bool ShowWeekIndex {
			get { return this.bShowWeekIndex; }
			set { this.bShowWeekIndex = value; }
		}
		public bool ShowShortCuts {
			get { return this.bShowShortCuts; }
			set { this.bShowShortCuts = value; }
		}
		public string DateParameterName {
			get { return this.sDateParameterName; }
		}
		public string Url {
			get {
				if (this.sUrl.IndexOf("?") < 0) {
					this.sUrl += "?";
				}
				return this.sUrl;
			}
			set { this.sUrl = value; }
		}
		public System.DateTime VisibleDate {
			get { return this.dVisibleDate; }
			set {
				if (Information.IsDate(value)) {
					this.dVisibleDate = value;
				}
			}
		}
		public int VisibleYear {
			get {
				if (this.nVisibleYear == DateAndTime.Now.Year && (this.Request != null) && Information.IsNumeric(this.Request("SelectedYear"))) {
					this.nVisibleYear = this.Request("SelectedYear");
				} else if (this.Request("SelectedYear") == null) {
					this.nVisibleYear = this.dVisibleDate.Year;
				}
				return this.nVisibleYear;
			}
			set {
				this.nVisibleYear = value;
				this.VisibleDate = Convert.ToDateTime(this.dVisibleDate.Day + "/" + this.VisibleMonth + "/" + value);
			}
		}
		public int VisibleMonth {
			get {
				if ((this.Request != null) && Information.IsNumeric(this.Request("SelectedMonth"))) {
					this.nVisibleMonth = this.Request("SelectedMonth");
				} else {
					this.nVisibleMonth = this.dVisibleDate.Month;
				}
				return this.nVisibleMonth;
			}
			set {
				this.nVisibleMonth = value;
				this.VisibleDate = Convert.ToDateTime(this.dVisibleDate.Day + "/" + value + "/" + this.VisibleYear);
			}
		}
		public string OnValueChanged {
			get { return this.sOnValueChanged; }
			set { this.sOnValueChanged = value; }
		}
		public string FontColor {
			get { return this.sFontColor; }
			set { this.sFontColor = value; }
		}
		public string BackColor {
			get { return this.sBackColor; }
			set { this.sBackColor = value; }
		}
		public string TodayFontColor {
			get { return this.sTodayFontColor; }
			set { this.sTodayFontColor = value; }
		}
		public string TodayBackColor {
			get { return this.sTodayBackColor; }
			set { this.sTodayBackColor = value; }
		}
		public string TitleBackColor {
			get { return this.sTitleBackColor; }
			set { this.sTitleBackColor = value; }
		}
		public string TitleFontColor {
			get { return this.sTitleFontColor; }
			set { this.sTitleFontColor = value; }
		}
		public string SelectedDateFontColor {
			get { return this.sSelectedDateFontColor; }
			set { this.sSelectedDateFontColor = value; }
		}
		public string SelectedDateBackColor {
			get { return this.sSelectedDateBackColor; }
			set { this.sSelectedDateBackColor = value; }
		}
		public string DayHeaderBackColor {
			get { return this.sDayHeaderBackColor; }
			set { this.sDayHeaderBackColor = value; }
		}
		public string DayHeaderFontColor {
			get { return this.sDayHeaderFontColor; }
			set { this.sDayHeaderFontColor = value; }
		}
		public string BorderColor {
			get { return this.sBorderColor; }
			set { this.sBorderColor = value; }
		}
		public int Width {
			get { return this.nWidth; }
			set { this.nWidth = value; }
		}
		public int FutureSpan {
			get { return this.nFutureSpan; }
			set { this.nFutureSpan = value; }
		}
		public int PastSpan {
			get { return this.nPastSpan; }
			set { this.nPastSpan = value; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			this.GetStyles();
			int n = 0;
			byte ColCount = 7;
			if (this.ShowWeekIndex)
				ColCount = 8;
			//style=""" & Me.GetBorderString(Me.BorderColor) & """ 
			Content.Add("<table id=\"" + this.ID + "_CalendarControl\" border=\"0\" " + this.Style.Draw() + " cellpadding=\"3\" cellspacing=\"0\"  width=\"" + this.Width + "\" " + this.GetBackColorString(this.BackColor) + ">");
			Content.Add("<tr " + this.GetBackColorString(this.TitleBackColor) + ">");
			Content.Add("<td colspan=\"" + ColCount + "\" align=\"center\" class=\"TitleFontColor\">&nbsp;&nbsp;&nbsp;");
			if (this.VisibleDate.AddMonths(-1) > DateAndTime.Now().AddYears(-1 * this.PastSpan)) {
				Content.Add("<a class=\"TitleFontColor\" href=\"" + this.Url + "Date=01." + this.VisibleDate.AddMonths(-1).Month + "." + this.VisibleDate.AddMonths(-1).Year + "&DateSpan=Month\"><<</a>&nbsp;&nbsp;&nbsp;");
			}
			Content.Add(this.GetMonthSelectBox() + "&nbsp;&nbsp;" + this.GetYearSelectBox() + "&nbsp;" + this.GetGoToMonthButton() + "&nbsp;&nbsp;&nbsp;");
			if (this.VisibleDate.AddMonths(1) < DateAndTime.Now().AddYears(this.FutureSpan)) {
				Content.Add("<a class=\"TitleFontColor\" href=\"" + this.Url + "Date=01." + this.VisibleDate.AddMonths(1).Month + "." + this.VisibleDate.AddMonths(1).Year + "&DateSpan=Month\">>></a>");
			}
			Content.Add("</td></tr>");

			Content.Add("<tr " + this.GetBackColorString(this.DayHeaderBackColor) + " class=\"DayHeaderFontColor\" style=\"text-align:center\"><td>Pts</td><td>S</td><td>Ç</td><td>Prş</td><td>C</td><td>Cts</td><td>Pzr</td>");
			if (this.ShowWeekIndex) {
				Content.Add("<td align=\"right\"><b>Hafta#</b></td>");
			}
			Content.Add("</tr>");
			int nBegin = this.GetMonthFirstDayIndex(this.VisibleDate);
			int nEnd = DateTime.DaysInMonth(this.VisibleDate.Year, this.VisibleDate.Month);
			int nLastMonthDays = DateTime.DaysInMonth(this.VisibleDate.AddMonths(-1).Year, this.VisibleDate.AddMonths(-1).Month);
			Content.Add("<tr>");
			int DayIndex = 0;
			if (nBegin == 1) {
				for (n = (nLastMonthDays - nBegin - 5); n <= nLastMonthDays; n++) {
					Content.Add(this.DrawCell(n, this.VisibleDate.AddMonths(-1).Month, this.VisibleDate.AddMonths(-1).Year));
					DayIndex += 1;
				}
			} else {
				for (n = (nLastMonthDays - nBegin + 2); n <= nLastMonthDays; n++) {
					Content.Add(this.DrawCell(n, this.VisibleDate.AddMonths(-1).Month, this.VisibleDate.AddMonths(-1).Year));
					DayIndex += 1;
				}
				for (n = 1; n <= 8 - nBegin; n++) {
					Content.Add(this.DrawCell(n, this.VisibleDate.Month, this.VisibleDate.Year));
					DayIndex += 1;
				}
			}
			byte WeekIndex = 1;
			byte StartIndex = 0;
			if (nBegin == 1) {
				StartIndex = 1;
			} else {
				StartIndex = 9 - nBegin;
			}
			for (n = StartIndex; n <= nEnd; n++) {
				if (DayIndex % 7 == 0) {
					if (this.ShowWeekIndex) {
						Content.Add(this.DrawWeekIndex(WeekIndex));
						WeekIndex += 1;
					}
					Content.Add("</tr>");
					Content.Add("<tr>");
				}
				Content.Add(this.DrawCell(n, this.VisibleDate.Month, this.VisibleDate.Year));
				DayIndex += 1;
			}
			for (n = 1; n <= 42 - DayIndex; n++) {
				if (DayIndex % 7 == 0) {
					Content.Add(this.DrawWeekIndex(WeekIndex));
					WeekIndex += 1;
					Content.Add("</tr><tr>");
				}
				Content.Add(this.DrawCell(n, this.VisibleDate.AddMonths(1).Month, this.VisibleDate.AddMonths(1).Year));
				DayIndex += 1;
			}
			if (this.ShowWeekIndex) {
				Content.Add(this.DrawWeekIndex(WeekIndex));
				WeekIndex += 1;
			}
			Content.Add("</tr>");
			if (this.ShowShortCuts) {
				Content.Add("<tr><td colspan=\"" + ColCount + "\"><table width=\"100%\" cellspacing=\"1\">");
				Content.Add("<tr bgcolor=\"" + this.DayHeaderBackColor + "\"><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.ToShortDateString() && string.IsNullOrEmpty(this.Page.QueryString("DateSpan"))) {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.Day + "." + DateAndTime.Now.Month + "." + DateAndTime.Now.Year + "\">Bugün</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.AddDays(-1).ToShortDateString() && string.IsNullOrEmpty(this.Page.QueryString("DateSpan"))) {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.AddDays(-1).Day + "." + DateAndTime.Now.AddDays(-1).Month + "." + DateAndTime.Now.AddDays(-1).Year + "\">Dün</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.AddDays(1).ToShortDateString() && string.IsNullOrEmpty(this.Page.QueryString("DateSpan"))) {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				if (this.FutureSpan > 0) {
					Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.AddDays(1).Day + "." + DateAndTime.Now.AddDays(1).Month + "." + DateAndTime.Now.AddDays(1).Year + "\">Yarın</a>");
				}
				Content.Add("</td></tr>");
				System.DateTime WeekStart = this.GetWeekStart(DateAndTime.Now);
				Content.Add("<tr bgcolor=\"" + this.DayHeaderBackColor + "\"><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == WeekStart.ToShortDateString() && this.Page.QueryString("DateSpan") == "Week") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + WeekStart.Day + "." + WeekStart.Month + "." + WeekStart.Year + "&DateSpan=Week\">Bu hafta</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == new System.DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, 1) && this.Page.QueryString("DateSpan") == "Month") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=01." + DateAndTime.Now.Month + "." + DateAndTime.Now.Year + "&DateSpan=Month\">Bu ay</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == new System.DateTime(DateAndTime.Now.Year, 1, 1) && this.Page.QueryString("DateSpan") == "Year") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=01.01." + DateAndTime.Now.Year + "&DateSpan=Year\">Bu yıl</a>");
				Content.Add("</TD></TR>");

				Content.Add("<tr bgcolor=\"" + this.DayHeaderBackColor + "\"><td align=\"center\"");
				WeekStart = this.GetWeekStart(DateAndTime.Now.AddDays(-7));
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == WeekStart.ToShortDateString() && this.Page.QueryString("DateSpan") == "Week") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + WeekStart.Day + "." + WeekStart.Month + "." + WeekStart.Year + "&DateSpan=Week\">Geçen hafta</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == new DateTime(DateAndTime.Now.AddMonths(-1).Year, DateAndTime.Now.AddMonths(-1).Month, 1) && this.Page.QueryString("DateSpan") == "Month") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=01." + DateAndTime.Now.AddMonths(-1).Month + "." + DateAndTime.Now.AddMonths(-1).Year + "&DateSpan=Month\">Geçen ay</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == new DateTime(DateAndTime.Now.AddYears(-1).Year, 1, 1) && this.Page.QueryString("DateSpan") == "Year") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=01.01." + DateAndTime.Now.AddYears(-1).Year + "&DateSpan=Year\">Geçen yıl</a>");
				Content.Add("</td></tr>");

				WeekStart = DateAndTime.Now.AddDays(-7);
				Content.Add("<tr bgcolor=\"" + this.DayHeaderBackColor + "\"><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == WeekStart.ToShortDateString() && this.Page.QueryString("DateSpan") == "Week") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + WeekStart.Day + "." + WeekStart.Month + "." + WeekStart.Year + "&DateSpan=Week\">Son bir hafta</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.AddMonths(-1).ToShortDateString() && this.Page.QueryString("DateSpan") == "Month") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.AddMonths(-1).Day + "." + DateAndTime.Now.AddMonths(-1).Month + "." + DateAndTime.Now.AddMonths(-1).Year + "&DateSpan=Month\">Son bir ay</a>");
				Content.Add("</td><td align=\"center\"");
				if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.AddYears(-1).ToShortDateString() && this.Page.QueryString("DateSpan") == "Year") {
					Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
				}
				Content.Add(">");
				Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.AddYears(-1).Day + "." + DateAndTime.Now.AddYears(-1).Month + "." + DateAndTime.Now.AddYears(-1).Year + "&DateSpan=Year\">Son bir yıl</a>");
				Content.Add("</td></tr>");

				if (this.FutureSpan > 0) {
					WeekStart = this.GetWeekStart(DateAndTime.Now.AddDays(7));
					Content.Add("<tr bgcolor=\"" + this.DayHeaderBackColor + "\"><td align=\"center\"");
					if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == WeekStart.ToShortDateString() && this.Page.QueryString("DateSpan") == "Week") {
						Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
					}
					Content.Add(">");
					Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + WeekStart.Day + "." + WeekStart.Month + "." + WeekStart.Year + "&DateSpan=Week\">Gelecek hafta</a>");
					Content.Add("</td><td align=\"center\"");
					if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == new DateTime(DateAndTime.Now.AddMonths(1).Year, DateAndTime.Now.AddMonths(1).Month, 1) && this.Page.QueryString("DateSpan") == "Month") {
						Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
					}
					Content.Add(">");
					Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=01." + DateAndTime.Now.AddMonths(1).Month + "." + DateAndTime.Now.AddMonths(1).Year + "&DateSpan=Month\">Gelecek ay</a>");
					Content.Add("</td><td align=\"center\"");
					if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == new DateTime(DateAndTime.Now.AddYears(1).Year, 1, 1) && this.Page.QueryString("DateSpan") == "Year") {
						Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
					}
					Content.Add(">");
					Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=01.01." + DateAndTime.Now.AddYears(1).Year + "&DateSpan=Year\">Gelecek yıl</a>");
					Content.Add("</td></tr>");


					Content.Add("<tr bgcolor=\"" + this.DayHeaderBackColor + "\"><td align=\"center\"");
					if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.ToShortDateString() && this.Page.QueryString("DateSpan") == "Week") {
						Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
					}
					Content.Add(">");
					Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.Day + "." + DateAndTime.Now.Month + "." + DateAndTime.Now.Year + "&DateSpan=Week\">Gelecek bir hafta</a>");
					Content.Add("</td><td align=\"center\"");
					if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.ToShortDateString() && this.Page.QueryString("DateSpan") == "Month") {
						Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
					}
					Content.Add(">");
					Content.Add("<a HREF=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.Day + "." + DateAndTime.Now.Month + "." + DateAndTime.Now.Year + "&DateSpan=Month\">Gelecek bir ay</a>");
					Content.Add("</td><td align=\"center\"");
					if (this.Page.QueryString.DateTimeValue(this.DateParameterName) == DateAndTime.Now.ToShortDateString() && this.Page.QueryString("DateSpan") == "Year") {
						Content.Add(" bgcolor=\"" + this.SelectedDateBackColor + "\"");
					}
					Content.Add(">");
					Content.Add("<a href=\"" + this.Url + this.DateParameterName + "=" + DateAndTime.Now.Day + "." + DateAndTime.Now.Month + "." + DateAndTime.Now.Year + "&DateSpan=Year\">Gelecek bir yıl</a>");
					Content.Add("</td></tr>");
				}

				Content.Add("</table></td></tr>");
			}
			Content.Add("</table>");
		}
		protected string DrawWeekIndex(byte WeekIndex)
		{
			if (this.ShowWeekIndex) {
				System.DateTime StartDate = Convert.ToDateTime(this.LastDayDrawn).AddDays(-6);
				string sReturnString = "<td align=\"right\">";
				sReturnString += "<a class=\"FontColor\" href=\"" + this.Url + this.DateParameterName + "=" + StartDate.Day + "." + StartDate.Month + "." + StartDate.Year + "&DateSpan=Week\"";
				sReturnString += "><b>(" + WeekIndex + ")</b></a></td>";
				return sReturnString;
			}
			return "";
		}
		protected void GetStyles()
		{
			if (!string.IsNullOrEmpty(this.SelectedDateFontColor))
				this.StyleSheet.AddCustomRule(".SelectedDateFontColor", "color:" + this.SelectedDateFontColor + "");
			if (!string.IsNullOrEmpty(this.TodayFontColor))
				this.StyleSheet.AddCustomRule(".TodayFontColor", "color:" + this.TodayFontColor + "");
			if (!string.IsNullOrEmpty(this.FontColor))
				this.StyleSheet.AddCustomRule(".FontColor", "color:" + this.FontColor + "");
			if (!string.IsNullOrEmpty(this.DayHeaderFontColor))
				this.StyleSheet.AddCustomRule(".DayHeaderFontColor", "color:" + this.DayHeaderFontColor + "");
			if (!string.IsNullOrEmpty(this.TitleFontColor))
				this.StyleSheet.AddCustomRule(".TitleFontColor", "color:" + this.TitleFontColor + "");
			this.StyleSheet.AddCustomRule(".OutOfDate", "color:#CCCCCC");
		}
		public virtual string DrawCell(int Day, int Month, int Year)
		{
			string sReturnString = "";
			sReturnString += "<td align=\"center\" " + (DateAndTime.Now().Day == Day && DateAndTime.Now().Month == Month ? this.GetBackColorString(this.TodayBackColor) : (this.VisibleDate.Day == Day && this.VisibleDate.Month == Month ? this.GetBackColorString(this.SelectedDateBackColor) : "")) + ">";
			sReturnString += this.DrawDateString(Day, Month, Year);
			sReturnString += "</td>";
			this.LastDayDrawn = Day + "." + Month + "." + Year;
			return sReturnString;
		}
		protected virtual string GetGoToMonthButton()
		{
			string sReturnString = "";
			sReturnString += "<input type=\"button\" name=\"GoToMonthButton\" id=\"GoToMonthButton\" style=\"font-size:10px;\" value=\">\" onclick=\"location.href='" + this.Page.Client.ApplicationBase + this.Url.Substring(1, this.Url.Length - 1) + this.DateParameterName + "=01.'+document.getElementById('CalendarMonth').value+'.'+document.getElementById('CalendarYear').value+'&DateSpan=Month'\">";

			return sReturnString;
		}
		protected string GetYearSelectBox()
		{
			string sReturnString = "";
			sReturnString += "<select name=\"SelectedYear\" id=\"CalendarYear\" style=\"font-size:10px;\">";
			int n = 0;
			for (n = DateAndTime.Now.AddYears(this.FutureSpan).Year; n >= DateAndTime.Now.AddYears(1).Year; n += -1) {
				sReturnString += "<option value=\"" + n + "\" " + (this.VisibleYear == n ? "selected" : "") + ">" + n + "</option>";
			}
			for (n = DateAndTime.Now.Year; n >= DateAndTime.Now.AddYears(-1 * this.PastSpan).Year; n += -1) {
				sReturnString += "<option value=\"" + n + "\" " + (this.VisibleYear == n ? "selected" : "") + ">" + n + "</option>";
			}
			sReturnString += "</select>";
			return sReturnString;
		}
		protected string GetMonthSelectBox()
		{
			string sReturnString = "";
			sReturnString += "<select name=\"SelectedMonth\" id=\"CalendarMonth\" style=\"font-size:10px;\">";
			int n = 0;
			for (n = 1; n <= 12; n++) {
				sReturnString += "<option value=\"" + n + "\" " + (this.VisibleMonth == n ? "selected" : "") + ">" + this.GetMonthName(n) + "</option>";
			}
			sReturnString += "</select>";
			return sReturnString;
		}
		protected string GetBackColorString(string Value)
		{
			if (!string.IsNullOrEmpty(Value)) {
				return "bgcolor=\"" + Value + "\"";
			}
			return "";
		}
		protected string GetBorderString(string Value)
		{
			if (!string.IsNullOrEmpty(Value)) {
				return "border:1px solid " + Value + ";";
			}
			return "";
		}
		private string DrawDateString(int Day, int Month, int Year)
		{
			string sReturnString = "<a " + (this.VisibleDate.Month == Month ? "class=\"FontColor\"" : "class=\"OutOfDate\"");
			sReturnString += " href=\"" + this.Url + this.DateParameterName + "=" + Day + "." + Month + "." + Year + "\" ";
			sReturnString += ">" + Day + "</a>";
			return sReturnString;
		}
		private string GetMonthName(int Value)
		{
			switch (Value) {
				case 1:
					return "Ocak";
				case 2:
					return "Şubat";
				case 3:
					return "Mart";
				case 4:
					return "Nisan";
				case 5:
					return "Mayıs";
				case 6:
					return "Haziran";
				case 7:
					return "Temmuz";
				case 8:
					return "Ağustos";
				case 9:
					return "Eylül";
				case 10:
					return "Ekim";
				case 11:
					return "Kasım";
				case 12:
					return "Aralık";
				default:
					return "";
			}
		}
		protected System.DateTime GetWeekStart(System.DateTime Value)
		{
			return Value.AddDays(-this.GetWeekDayIndex(Value) + 1);
		}
		private int GetWeekDayIndex(System.DateTime Value)
		{
			switch (Value.DayOfWeek) {
				case DayOfWeek.Monday:
					return 1;
				case DayOfWeek.Tuesday:
					return 2;
				case DayOfWeek.Wednesday:
					return 3;
				case DayOfWeek.Thursday:
					return 4;
				case DayOfWeek.Friday:
					return 5;
				case DayOfWeek.Saturday:
					return 6;
				case DayOfWeek.Sunday:
					return 7;
			}
		}
		protected int GetMonthFirstDayIndex(System.DateTime Value)
		{
			return this.GetWeekDayIndex(Value.AddDays((-Value.Day) + 1));
		}
		public Calendar(string DateParameterName)
		{
			this.sDateParameterName = DateParameterName;
			this.Url = this.Request.ServerVariables("Script_Name");
			if (!string.IsNullOrEmpty(Request(this.DateParameterName)))
				this.VisibleDate = Request(this.DateParameterName);
			if (this.Url.IndexOf("?") == -1)
				this.Url += "?";
			if (!(Url.StartsWith("/") || Url.StartsWith("http:"))) {
				this.Url = "/" + this.Url;
			}
		}
		public static EntityCollection ArrangeEntityCollection(EntityCollection EntityCollection, string FilterNameProperty, string QueryStringParameter = "Date")
		{
			DateTime FirstDate = DateTime.MinValue;
			DateTime LastDate = DateTime.MinValue;
			if (Ophelia.Web.View.UI.Current.QueryString.DateTimeValue(QueryStringParameter) > DateTime.MinValue) {
				FirstDate = Ophelia.Web.View.UI.Current.QueryString.DateTimeValue(QueryStringParameter);
				switch (Ophelia.Web.View.UI.Current.QueryString("DateSpan")) {
					case "Week":
						LastDate = FirstDate.AddDays(7);
						break;
					case "Month":
						LastDate = FirstDate.AddMonths(1);
						break;
					case "Year":
						LastDate = FirstDate.AddYears(1);
						break;
					default:
						LastDate = FirstDate;
						break;
				}
				LastDate = LastDate.AddDays(1);
			}
			if (FirstDate > DateTime.MinValue) {
				EntityCollection = EntityCollection.FilterBy(FilterNameProperty, FirstDate.ToString(), FilterComparison.GreaterOrEqual);
				EntityCollection = EntityCollection.FilterBy(FilterNameProperty, LastDate.ToString(), FilterComparison.SmallerOrEqual);
			}
			return EntityCollection;
		}
		public Calendar() : this("Date")
		{
		}
	}
}

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
	public class DateTimePicker : TextBox
	{
		private Ophelia.View.Windows.Controls.DateFormat eDateFormat = Ophelia.View.Windows.Controls.DateFormat.ShortDate;
		private string sCalenderImage = string.Empty;
		private bool bInitializeByCssClass = true;
		public string CalenderImage {
			get { return this.sCalenderImage; }
			set { this.sCalenderImage = value; }
		}
		public bool InitializeByCssClass {
			get { return this.bInitializeByCssClass; }
			set { this.bInitializeByCssClass = value; }
		}
		public DateFormat DateFormat {
			get { return this.eDateFormat; }
			set { this.eDateFormat = value; }
		}
		public new DateTime Value {
			get {
				if (!Information.IsDate(base.Value))
					return DateTime.MinValue;
				return base.Value;
			}
			set { base.Value = value; }
		}
		private DateTime oMinDate;
		public DateTime MinDate {
			get { return oMinDate; }
			set { oMinDate = value; }
		}
		private DateTime oMaxDate;
		public DateTime MaxDate {
			get { return oMaxDate; }
			set { oMaxDate = value; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			if (Information.IsDate(this.Value)) {
				if (this.DateFormat == View.Web.Controls.DateFormat.ShortDate) {
					this.Value = Strings.FormatDateTime(this.Value, Microsoft.VisualBasic.DateFormat.ShortDate);
				} else if (this.DateFormat == View.Web.Controls.DateFormat.ShortTime) {
					this.Value = Strings.FormatDateTime(this.Value, Microsoft.VisualBasic.DateFormat.ShortTime);
				} else if (this.DateFormat == View.Web.Controls.DateFormat.LongTime) {
					this.Value = Strings.FormatDateTime(this.Value, Microsoft.VisualBasic.DateFormat.LongTime);
				} else if (this.DateFormat == View.Web.Controls.DateFormat.ShortTimeWithoutSecond) {
					if (this.Value == DateTime.MinValue) {
						base.Value = "00:00";
					} else {
						base.Value = this.Value.ToString("HH:mm");
					}
				}
				if (this.Value == DateTime.MinValue)
					base.Value = string.Empty;
			}
			this.MaxLength = 25;
			this.StyleSheet.AddCustomRule("div.ui-datepicker", " font-size: 10px;");
			if (!this.ReadOnly) {
				if (string.IsNullOrEmpty(this.Style.Class)) {
					this.Style.Class = "datetimepicker";
				} else {
					this.Style.Class = "datetimepicker " + this.Style.Class;
				}
				string ApplicationPath = "/";
				if (this.Page != null) {
					ApplicationPath = this.Page.ContentManager.ApplicationPath;
				}
				Ophelia.Web.View.UI.Header Header = this.Page.Header;
				Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuicore.css", this.Page.FileUsageType), UI.HeadLink.ReleationShipType.StyleSheet);
				Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuidatepicker.css", this.Page.FileUsageType), UI.HeadLink.ReleationShipType.StyleSheet);
				Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuitheme.css", this.Page.FileUsageType), UI.HeadLink.ReleationShipType.StyleSheet);

				//Header.ScriptManager.AddGeneralJQueryLibrary()
				Header.ScriptManager.Add("jqueryuicore", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuicore.js", this.Page.FileUsageType));
				Header.ScriptManager.Add("jqueryuiwidget", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuiwidget.js", this.Page.FileUsageType));
				Header.ScriptManager.Add("jqueryuidatepicker", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuidatepicker.js", this.Page.FileUsageType));

				if (this.Request("AjaxRequestWithApplication") == null) {
					Header.ScriptManager.Add("jqueryuidatepickertr", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuidatepickertr.js", this.Page.FileUsageType));
				}
			}
			base.OnBeforeDraw(Content);
		}
		//Private Function InitializationScript() As String
		//    Dim Str As String = "" & _
		//        "$('.datetimepicker').datepicker({" & _
		//        "showOn: 'button',"
		//    If String.IsNullOrEmpty(Me.sCalenderImage) Then
		//        Str &= "buttonImage: '" & Me.Page.Client.ApplicationBase & "?DisplayImage=ResourceName,,Calendar.gif$$$Namespace,,Ophelia',"
		//    Else
		//        Str &= "buttonImage: '" & sCalenderImage & "',"
		//    End If
		//    Str &= "buttonImageOnly: true"
		//    If MinDate > DateTime.MinValue Then
		//        Str &= ",minDate: new Date(" & MinDate.Year & ", " & MinDate.Month - 1 & ", " & MinDate.Day & ")"
		//    End If
		//    Str &= "});"

		//    Str &= "jQuery(function($){"
		//    Str &= "$.datepicker.regional['tr'] = {"
		//    Str &= "closeText: 'kapat',"
		//    Str &= "prevText: '&#x3c;geri',"
		//    Str &= "nextText: 'ileri&#x3e',"
		//    Str &= "currentText: 'bug&#252;n',"
		//    Str &= "monthNames: ['Ocak','&#350;ubat','Mart','Nisan','May&#305;s','Haziran',"
		//    Str &= "'Temmuz','A&#287;ustos','Eyl&#252;l','Ekim','Kas&#305;m','Aral&#305;k'],"
		//    Str &= "monthNamesShort: ['Oca','&#350;ub','Mar','Nis','May','Haz',"
		//    Str &= "'Tem','A&#287;u','Eyl','Eki','Kas','Ara'],"
		//    Str &= "dayNames: ['Pazar', 'Pazartesi', 'Sal&#305;', '&#199;ar&#351;amba', 'Per&#351;embe', 'Cuma', 'Cumartesi'],"
		//    Str &= "dayNamesShort: ['Pz', 'Pt', 'Sa', '&#199;a', 'Pe', 'Cu', 'Ct'],"
		//    Str &= "dayNamesMin: ['Pz', 'Pt', 'Sa', '&#199;a', 'Pe', 'Cu', 'Ct'],"
		//    Str &= "weekHeader: 'Hf',"
		//    Str &= "dateFormat: 'dd.mm.yy',"
		//    Str &= "firstDay: 1,"
		//    Str &= "isRTL: false,"
		//    Str &= "showMonthAfterYear: false,"
		//    Str &= "yearSuffix: ''};"
		//    Str &= "$.datepicker.setDefaults($.datepicker.regional['tr']);"
		//    Str &= "});"
		//    Return Str
		//End Function
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			//"$('.datetimepicker').datepicker({" & _

			if (!this.ReadOnly) {
				string Str = "";
				if (this.InitializeByCssClass) {
					Str = "$('.datetimepicker').datepicker({";
				} else {
					Str = "$('#" + this.ID + "').datepicker({";
				}
				Str += "showOn: 'button',";
				if (string.IsNullOrEmpty(this.sCalenderImage)) {
					Str += "buttonImage: '" + this.Page.Client.ApplicationBase + "?DisplayImage=ResourceName,,Calendar.gif$$$Namespace,,Ophelia',";
				} else {
					Str += "buttonImage: '" + sCalenderImage + "',";
				}
				Str += "buttonImageOnly: true";
				if (MinDate > DateTime.MinValue) {
					Str += ",minDate: new Date(" + MinDate.Year + ", " + MinDate.Month - 1 + ", " + MinDate.Day + ")";
				}
				if (MaxDate > DateTime.MinValue) {
					Str += ",maxDate: new Date(" + MaxDate.Year + ", " + MaxDate.Month + 1 + ", " + MaxDate.Day + ")";
				}
				Str += "});";
				if (!(this.Request("AjaxRequestWithApplication") != null && !string.IsNullOrEmpty(this.Request("AjaxRequestWithApplication")))) {
					Script.AddFunctionToOnload(Str);
				} else {
					//Script.AppendLine(Str)
				}
			}
		}
		public DateTimePicker(string MemberName) : base(MemberName)
		{
			this.DateFormat = View.Web.Controls.DateFormat.ShortDate;
		}
	}
	public enum DateFormat
	{
		None = 0,
		ShortDate = 1,
		LongDate = 2,
		ShortTime = 3,
		LongTime = 4,
		MonthAndYear = 5,
		YearOnly = 6,
		LongDateAndTime = 7,
		LongDateAndShortTime = 8,
		ShortTimeWithoutSecond = 9
	}
}

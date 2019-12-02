using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
using Ophelia.Web.View;
namespace Ophelia.Web.View.Controls
{
	public class DatePicker : Ophelia.Web.View.Controls.TextBox
	{
		private string sMemberName = string.Empty;
		private string sDateFormat = "dd.mm.yyyy";
		private DatePickerLanguage lLanguge = DatePickerLanguage.Turkish;
		private FirstDayOfWeek iWeekStart = FirstDayOfWeek.Monday;
		public string MemberName {
			get { return this.sMemberName; }
			set { this.sMemberName = value; }
		}
		public string DateFormat {
			get { return this.sDateFormat; }
			set { this.sDateFormat = value; }
		}
		public FirstDayOfWeek WeekStart {
			get { return this.iWeekStart; }
			set { this.iWeekStart = value; }
		}
		public DatePickerLanguage PickerLanguage {
			get { return this.lLanguge; }
			set { this.lLanguge = value; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			if (string.IsNullOrEmpty(this.Style.Class)) {
				this.Style.Class = "datetimepicker";
			} else {
				this.Style.Class = "datetimepicker " + this.Style.Class;
			}
			Ophelia.Web.View.UI.Header Header = this.Page.Header;
			Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuicore.css", this.Page.FileUsageType), UI.HeadLink.ReleationShipType.StyleSheet);
			Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuitheme.css", this.Page.FileUsageType), UI.HeadLink.ReleationShipType.StyleSheet);

			Header.ScriptManager.Add("jqueryuicore", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuicore.js", this.Page.FileUsageType));
			Header.ScriptManager.Add("jqueryuiwidget", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryuiwidget.js", this.Page.FileUsageType));


			Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "bootstrap.css", this.Page.FileUsageType), UI.HeadLink.ReleationShipType.StyleSheet);
			Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "datepicker.css", this.Page.FileUsageType), UI.HeadLink.ReleationShipType.StyleSheet);
			// Header.ScriptManager.Add("bootstrapdatepicker", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "bootstrap-datepicker.js", Me.Page.FileUsageType))
			this.Page.Header.ScriptManager.Add("bootstrapdatepicker", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "bootstrap-datepicker.js", this.Page.FileUsageType));
			//   Me.Page.Header.ScriptManager.Add("bootstrapdatepicker", "/assets/content-manager/autocomplete-box/script.js")
			//  Me.Script.AppendLine("$('#" + Me.MemberName + "').datepicker2({  format:'" + Me.DateFormat + "',weekStart:" + CInt(Me.WeekStart).ToString() + ",language:'" + CInt(Me.PickerLanguage).ToString() + "'});")
			//  $('#datepicker').live('click', function(){$(this).datepicker($.datepicker.regional['en']);})
			// Me.Script.AppendLine("$('#" + Me.MemberName + "').on('click', );")

			this.Script.AppendLine(" $( document ).on( 'click', '#" + this.MemberName + "', function(){$(this).datepicker2({  format:'" + this.DateFormat + "',weekStart:" + Convert.ToInt32(this.WeekStart).ToString() + ",language:'" + Convert.ToInt32(this.PickerLanguage).ToString() + "'});});");

			base.OnBeforeDraw(Content);
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			// Script.AppendLine("$('#" + Me.MemberName + "').datepicker2({  format:'" + Me.DateFormat + "',weekStart:" + CInt(Me.WeekStart).ToString() + ",language:'" + CInt(Me.PickerLanguage).ToString() + "'});")



		}
		public DatePicker(string MemberName, DatePickerLanguage Language = DatePickerLanguage.Turkish) : base(MemberName)
		{
			this.MemberName = MemberName;
			this.PickerLanguage = Language;
		}
		public enum DatePickerLanguage
		{
			English = 0,
			Turkish = 1
		}
	}
}

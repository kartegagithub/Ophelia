using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Forms
{
	public class Layout : Section
	{
		private BaseForm oForm;
		private TitleConfiguration oTitles = new TitleConfiguration();
		private LinkConfiguration oLinks = new LinkConfiguration();
		private LayoutTechnique eTechnique = LayoutTechnique.Tables;
		private Browser eBrowser = View.Web.Browser.UnIdentified;
		private string sFrameColor = "";
		private string sScript = "";
		private string sExtraMetaTags = "";
		private Grid oGrid;
		private string sXmlnsUrl = "";
		public TitleConfiguration Titles {
			get { return this.oTitles; }
		}
		public LinkConfiguration Links {
			get { return this.oLinks; }
		}
		public new BaseForm Form {
			get { return this.oForm; }
		}
		public Grid Grid {
			get { return this.oGrid; }
		}
		public string Script {
			get { return this.sScript; }
			set { this.sScript = value; }
		}
		public string XmlnsUrl {
			get { return this.sXmlnsUrl; }
			set { this.sXmlnsUrl = value; }
		}
		public string ExtraMetaTags {
			get { return this.sExtraMetaTags; }
			set { this.sExtraMetaTags = value; }
		}
		public LayoutTechnique Technique {
			get { return this.eTechnique; }
			set { this.eTechnique = value; }
		}
		public View.Web.Browser Browser {
			get {
				if (this.eBrowser == View.Web.Browser.UnIdentified) {
					if ((this.Request != null) && !string.IsNullOrEmpty(this.Request.UserAgent)) {
						if (this.Request.UserAgent.IndexOf("MSIE") > -1) {
							this.eBrowser = View.Web.Browser.Explorer;
						} else {
							this.eBrowser = View.Web.Browser.Other;
						}
					} else {
						this.eBrowser = View.Web.Browser.Other;
					}
				}
				return this.eBrowser;
			}
		}
		public string FrameColor {
			get {
				if (string.IsNullOrEmpty(this.sFrameColor)) {
					return this.BackgroundColor;
				} else {
					return this.sFrameColor;
				}
			}
			set { this.sFrameColor = value; }
		}
		private int FooterSpaceCount;
		public void AddSpace(int Count = 2)
		{
			this.FooterSpaceCount = Count;
		}
		public override string Draw()
		{
			this.OnBeforeDraw();
			this.Form.OnBeforeLayoutDraw();
			string ReturnString = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\">";
			ReturnString += "<html " + sXmlnsUrl + ">";
			ReturnString += "<head>";
			ReturnString += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1254\">";
			ReturnString += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=ISO-8859-9\">";
			ReturnString += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">";
			if (this.Form.AutoRefresh)
				ReturnString += "<meta http-equiv=\"refresh\" content=\"" + this.Form.RefreshPeriod + "\">";
			ReturnString += "<meta http-equiv=\"Content-Language\" content=\"" + this.Form.Header.ContentLanguage + "\">";
			ReturnString += this.ExtraMetaTags;
			ReturnString += "<title>" + this.Form.PageTitle + "</title>";
			ReturnString += this.Form.Scripts.Draw;
			ReturnString += this.Script;
			string LinkStyle = this.Links.GetStyle;
			string HoverStyle = this.Links.Hover.GetStyle;
			string VisitedStyle = this.Links.Visited.GetStyle;
			if (this.Technique != LayoutTechnique.Custom) {
				ReturnString += "<style>";
				ReturnString += "INPUT{" + this.Font.GetStyle + "}";
				ReturnString += "TEXTAREA{" + this.Font.GetStyle + "}";
				ReturnString += "SELECT{" + this.Font.GetStyle + "}";
				if (!string.IsNullOrEmpty(LinkStyle))
					ReturnString += "A{" + LinkStyle + "}";
				if (!string.IsNullOrEmpty(HoverStyle))
					ReturnString += "A:hover{" + HoverStyle + "}";
				if (!string.IsNullOrEmpty(VisitedStyle))
					ReturnString += "A:visited{" + VisitedStyle + "}";
				ReturnString += "</style>";
				ReturnString += "</head>";
				ReturnString += "<script ID=\"clientEventHandlersJS\" LANGUAGE=\"javascript\">" + Constants.vbCrLf;
				ReturnString += "function ConfirmAction(sMessage, sAction) {" + Constants.vbCrLf;
				ReturnString += "if (confirm(sMessage) == true) {" + Constants.vbCrLf;
				ReturnString += "window.self.location.replace(sAction);" + Constants.vbCrLf;
				ReturnString += "}" + Constants.vbCrLf;
				ReturnString += "}" + Constants.vbCrLf;
				ReturnString += "</script>" + Constants.vbCrLf;
				ReturnString += "<body " + this.GetBodyStyle() + " leftmargin=\"0\" topmargin=\"0\">";
			} else {
				ReturnString += "</head>";
				ReturnString += "<body>";
			}

			if (this.Grid.Rows.Count > 0) {
				ReturnString += "<table cellpadding=\"0\" cellspacing=\"0\" align=\"" + this.HorizontalAlignment.ToString.Replace("HorizontalAlignment.", "").ToLower + "\"";
				ReturnString += this.GetStyle;
				ReturnString += ">";
				for (int i = 0; i <= this.Grid.Rows.Count - 1; i++) {
					ReturnString += "<tr>";
					for (int j = 0; j <= this.Grid.Columns.Count - 1; j++) {
						if (!(this.Layout.Technique == LayoutTechnique.Tables)) {
							ReturnString += "<td>";
						}
						ReturnString += this.Grid.Sections(this.Grid.Rows(i), this.Grid.Columns(j)).Draw();
						if (!(this.Layout.Technique == LayoutTechnique.Tables)) {
							ReturnString += "</td>";
						}
					}
					ReturnString += "</tr>";
				}
				ReturnString += "</table>";
			} else {
				if (this.Technique == LayoutTechnique.Tables) {
					ReturnString += "<table cellpadding=\"0\" cellspacing=\"0\" align=\"" + this.HorizontalAlignment.ToString.Replace("HorizontalAlignment.", "").ToLower + "\"";
					ReturnString += this.GetStyle;
					ReturnString += "><tr><td>";
				}
				ReturnString += this.Content.Value;
				ReturnString += this.Sections.Draw();
				if (this.Technique == LayoutTechnique.Tables) {
					ReturnString += "</td></tr></table>";
				}
			}
			for (int i = 0; i <= this.FooterSpaceCount - 1; i++) {
				ReturnString += "<br>";
			}
			ReturnString += "</body>";
			ReturnString += "</html>";
			this.OnAfterDraw();
			this.Form.OnAfterLayoutDraw();
			return ReturnString;
		}

		public virtual string BuildMailBody(string Message)
		{
			string ReturnString = "";
			ReturnString += "<style>";
			ReturnString += "a{text-decoration:none;color:#000000;}";
			ReturnString += "a:hover{text-decoration:underline;}";
			ReturnString += "a:visited{color:#000000;}";
			ReturnString += "</style>";
			ReturnString += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" bgcolor=\"" + this.BackgroundColor + "\" >";
			ReturnString += "<tr><td align=\"left\"><a href=\"http://" + this.Form.Request.Url.Authority + this.Client.ApplicationBase + "\"><img src=\"http://" + this.Form.Request.Url.Authority + this.Client.SiteLogo + "\" border=\"0\" alt=\"\"></a><br><br></td></tr>";
			ReturnString += "<tr><td style=\"font-family:" + this.Font.Family + "; font-size:8pt;padding-left:10px;\">";
			ReturnString += Message;
			ReturnString += "<br><a style=\"font-family:" + this.Font.Family + "; font-size:12px;\" href=\"http://" + this.Form.Request.Url.Authority + this.Client.ApplicationBase + "\">" + this.Form.Request.Url.Authority + "</a><br><br>";
			ReturnString += "</td></tr></table>";
			return ReturnString;
		}
		public virtual string DrawTitle(string Title)
		{
			return this.DrawHorizantalSpacer(10) + "<div style=\"font-size:14pt;color:#444444;\">" + Title + "</div>" + this.DrawHorizantalSpacer(10);
		}
		public virtual string DrawLine()
		{
			return "<div style=\"border-bottom:solid 1px #000000;\">&nbsp;</div>";
		}
		private string GetBodyStyle()
		{
			string ReturnString = "style=\"margin-top:0;margin-left:0;margin-right:0;";
			if (!string.IsNullOrEmpty(this.FrameColor)) {
				ReturnString += "background-color:" + this.FrameColor + ";";
			}
			if (!string.IsNullOrEmpty(this.BackgroundImage)) {
				ReturnString += "background-image:url('" + this.Layout.Client.ImageBase + this.BackgroundImage + "');";
				if (!this.RepeatBackroundImage) {
					ReturnString += "background-repeat:no-repeat;";
				}
			}
			ReturnString += "\"";
			return ReturnString;
		}
		public string DrawImage(string FileName)
		{
			return this.DrawImage(FileName, 0);
		}
		public string DrawImage(string FileName, string AlternateText)
		{
			return this.DrawImage(FileName, 0, AlternateText);
		}
		public string DrawImage(string FileName, int Border)
		{
			return this.DrawImage(FileName, Border, "");
		}
		public string DrawImage(string FileName, int Border, string AlternateText)
		{
			return "<img src=\"" + this.Client.ImageBase + FileName + "\" border=\"" + Border + "\" alt=\"" + AlternateText + "\">";
		}
		public string DrawHorizantalSpacer(int Height)
		{
			return "<div style=\"height:" + Height + "px;font-size:1pt;\">&nbsp;</div>";
		}
		public string DrawErrorMessage(string Message)
		{
			return "<div style=\"color:red;\">" + Message + "</div>";
		}
		public string DrawLink(string Url, string Body, bool Blank = false, bool ExcludeApplicationBase = false, string StyleClass = "", string OnClick = "", string ID = "")
		{
			string ReturnString = null;
			string FileName = null;
			if (Url.StartsWith("http") || ExcludeApplicationBase) {
				FileName = Url;
			} else {
				FileName = Strings.Replace(this.Client.ApplicationBase + Url, "//", "/");
			}
			ReturnString = "<a href=\"" + FileName + "\"";
			if (Blank)
				ReturnString += " target=\"_blank\"";
			if (!string.IsNullOrEmpty(StyleClass))
				ReturnString += " class=\"" + StyleClass + "\" ";
			if (!string.IsNullOrEmpty(OnClick)) {
				ReturnString += " onclick=\"" + OnClick + ";\"";
			}
			if (!string.IsNullOrEmpty(ID)) {
				ReturnString += " id=\"" + ID + "\" ";
			}
			ReturnString += ">" + Body + "</a>";
			return ReturnString;
		}
		public string DrawConfirmationLink(string Url, string Body, string Message)
		{
			return this.DrawLink("javascript:ConfirmAction('" + Message + "','" + Url + "')", Body, false, true);
		}
		public string DrawDateSelector(string Name, DateTime SelectedDate, bool YearAhead = false)
		{
			string ReturnString = "";
			int n = 0;
			string IsSelected = "";

			ReturnString += "<select name=\"" + Name + "Day\">";
			for (n = 1; n <= 31; n++) {
				IsSelected = "";
				if (SelectedDate > DateTime.MinValue && n == SelectedDate.Day)
					IsSelected = " selected";
				ReturnString += "<option value=\"" + n + "\"" + IsSelected + ">" + n + "</option>";
			}
			ReturnString += "</select>";

			ReturnString += "<select name=\"" + Name + "Month\">";
			for (n = 1; n <= 12; n++) {
				IsSelected = "";
				//If SelectedDate > DateTime.MinValue AndAlso n = SelectedDate.Month Then IsSelected = " SELECTED"
				if (n == SelectedDate.Month)
					IsSelected = " selected";
				ReturnString += "<option value=\"" + n + "\"" + IsSelected + ">" + n + "</option>";
			}
			ReturnString += "</select>";

			ReturnString += "<select name=\"" + Name + "Year\">";
			if (YearAhead) {
				for (n = DateAndTime.Now().AddYears(-5).Year; n <= 2020; n++) {
					IsSelected = "";
					//If SelectedDate > DateTime.MinValue AndAlso n = SelectedDate.Year Then IsSelected = " SELECTED"
					if (n == SelectedDate.Year)
						IsSelected = " selected";
					ReturnString += "<option value=\"" + n + "\"" + IsSelected + ">" + n + "</option>";
				}
			} else {
				int BeginingYear = 0;
				if (SelectedDate.Year > DateAndTime.Now.Year) {
					BeginingYear = SelectedDate.Year;
				} else {
					BeginingYear = DateAndTime.Now.Year;
				}
				for (n = BeginingYear; n >= 1930; n += -1) {
					IsSelected = "";
					//If SelectedDate > DateTime.MinValue AndAlso n = SelectedDate.Year Then IsSelected = " SELECTED"
					if (n == SelectedDate.Year)
						IsSelected = " selected";
					ReturnString += "<option value=\"" + n + "\"" + IsSelected + ">" + n + "</option>";
				}
			}
			ReturnString += "</select>";

			ReturnString += " (G�n / Ay / Y�l)";
			return ReturnString;
		}
		public int GetCheckBoxValue(string RequestValue)
		{
			if (RequestValue == "on") {
				return 1;
			} else {
				return 0;
			}
		}
		public string DrawCheckBox(string Name, string Text, int Checked)
		{
			return this.DrawCheckBox(Name, Text, Convert.ToBoolean(Checked));
		}
		public string DrawCheckBox(string Name, string Text, bool Checked = false)
		{
			string IsSelected = "";
			if (Checked)
				IsSelected = " checked";
			return "<input type=\"checkbox\" name=\"" + Name + "\"" + IsSelected + "> " + Text;
		}
		public string DrawRadio(string Name, string Text, string Value, string SelectedValue = null, string OnClickFunction = "")
		{
			string IsSelected = "";
			if (Value == SelectedValue)
				IsSelected = " checked";
			if (!string.IsNullOrEmpty(OnClickFunction))
				OnClickFunction = " onclick=\"" + OnClickFunction + "\" ";
			return "<input type=\"radio\" name=\"" + Name + "\" id=\"" + Name + Value + "\" value=\"" + Value + "\"" + IsSelected + " " + OnClickFunction + "> " + Text;
		}
		public string DrawOption(string Text, string Value, string SelectedValue = null)
		{
			string IsSelected = "";
			if (Value == SelectedValue)
				IsSelected = " selected";
			return "<option value=\"" + Value + "\"" + IsSelected + ">" + Text + "</option>";
		}
		public string DrawAnimation(string FileName, int Width, int Height)
		{
			string ReturnString = "";
			ReturnString += "<object codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0'";
			ReturnString += "height='" + Height + "' width='" + Width + "' classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' viewastext>";
			ReturnString += "<param name='_cx' value='20902'>";
			ReturnString += "<param name='_cy' value='3440'>";
			ReturnString += "<param name='FlashVars' value='" + this.Client.ImageBase + FileName + "'>";
			ReturnString += "<param name='Movie' value='" + this.Client.ImageBase + FileName + "'>";
			ReturnString += "<param name='Src' value=''>";
			ReturnString += "<param name='WMode' value='Transparent'>";
			ReturnString += "<param name='Play' value='-1'>";
			ReturnString += "<param name='Loop' value='-1'>";
			ReturnString += "<param name='Quality' value='Best'>";
			ReturnString += "<param name='SAlign' value=''>";
			ReturnString += "<param name='Menu' value='-1'>";
			ReturnString += "<param name='Base' value=''>";
			ReturnString += "<param name='AllowScriptAccess' value='always'>";
			ReturnString += "<param name='Scale' value='ShowAll'>";
			ReturnString += "<param name='DeviceFont' value='0'>";
			ReturnString += "<param name='EmbedMovie' value='0'>";
			ReturnString += "<param name='BGColor' value='CCCCCC'>";
			ReturnString += "<param name='SWRemote' value=''>";
			ReturnString += "<param name='MovieData' value=''>";
			ReturnString += "<param name='SeamlessTabbing' value='1'>";
			ReturnString += "<embed SRC='" + this.Client.ImageBase + FileName + "' quailty='Best' BGCOLOR='#CCCCCC' width='" + Width + "' height='" + Height + "'";
			ReturnString += "name='HozkaXMLFotoSlide' align='' type='application/x-shockwave-flash' PLUGINSPAGE='http://www.macromedia.com/go/getflashplayer'";
			ReturnString += "wmode='transparent'> </embed>";
			ReturnString += "</object>";
			return ReturnString;
		}
		private void CreateGridSections()
		{
			for (int i = 0; i <= this.Grid.Rows.Count - 1; i++) {
				for (int j = 0; j <= this.Grid.Columns.Count - 1; j++) {
					Section Section = this.CreateSection(i, j);
					if (Section == null) {
						Section = new Section(this.Sections);
					}
					this.Grid.SetGridSection(this.Grid.Rows(i), this.Grid.Columns(j), Section);
				}
			}
		}
		protected virtual Section CreateSection(int RowNumber, int ColumnNumber)
		{
			return null;
		}

		protected virtual void Configure()
		{
		}

		protected virtual void DefineLayoutGrid()
		{
		}
		public Layout(BaseForm Form) : base(null, false)
		{
			this.oForm = Form;
			this.oGrid = new Grid(this.Form);
			this.DefineLayoutGrid();
			this.CreateGridSections();
			this.CreateSections();
			this.Configure();
		}
	}
}

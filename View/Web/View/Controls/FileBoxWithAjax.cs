using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
using System.IO;
namespace Ophelia.Web.View.Controls
{
	public class FileBoxWithAjax : InputDataControl
	{
		private bool bHasShowButton = true;
		private string sShowButtonImage = string.Empty;
		private string sSelectButtonImage = string.Empty;
		//Private sSaveButtonImage As String = String.Empty
		private string sFileID = string.Empty;
		public string FileID {
			get { return this.sFileID; }
			set { this.sFileID = value; }
		}
		public bool HasShowButton {
			get { return this.bHasShowButton; }
			set { this.bHasShowButton = value; }
		}
		public string ShowButtonImage {
			get {
				if (string.IsNullOrEmpty(this.sShowButtonImage))
					return ServerSide.ImageDrawer.GetImageUrl("ShowIcon");
				return this.sShowButtonImage;
			}
			set { this.sShowButtonImage = value; }
		}
		public string SelectButtonImage {
			get {
				if (string.IsNullOrEmpty(this.sSelectButtonImage))
					return ServerSide.ImageDrawer.GetImageUrl("SelectIcon");
				return this.sSelectButtonImage;
			}
			set { this.sSelectButtonImage = value; }
		}
		//Public Property SaveButtonImage() As String
		//    Get
		//        If String.IsNullOrEmpty(Me.sSaveButtonImage) Then Return ServerSide.ImageDrawer.GetImageUrl("WebSave")
		//        Return Me.sSaveButtonImage
		//    End Get
		//    Set(ByVal value As String)
		//        Me.sSaveButtonImage = value
		//    End Set
		//End Property
		private string HideOverlayElementEvent()
		{
			if (this.Script.Function("HideOverlayElement") == null) {
				System.Text.StringBuilder ReturnString = new System.Text.StringBuilder();
				ReturnString.AppendLine("function HideOverlayElement(){");
				ReturnString.AppendLine("var body = document.getElementsByTagName('body')[0];");
				ReturnString.AppendLine("var overlay = document.getElementById('AjaxOverlay');");
				ReturnString.AppendLine("body.removeChild(overlay);");
				ReturnString.AppendLine("}");
				this.Script.AppendLine(ReturnString.ToString());
			}
			return "HideOverlayElement();";
		}
		private string ShowOverlayElementEvent()
		{
			if (this.Script.Function("ShowOverlayElement") == null) {
				System.Text.StringBuilder ReturnString = new System.Text.StringBuilder();
				Style OverlayStyle = new Style();

				ReturnString.AppendLine("function ShowOverlayElement(){");
				ReturnString.AppendLine("var OverlayElement = document.createElement('div'); ");
				ReturnString.AppendLine("OverlayElement.id = 'AjaxOverlay'; ");
				if (OverlayStyle.IsCustomized == false) {
					OverlayStyle.PositionStyle = Position.Fixed;
					OverlayStyle.WidthInPercent = 100;
					OverlayStyle.HeightInPercent = 100;
					OverlayStyle.BackgroundColor = "gray";
					OverlayStyle.Opacity = 0.5;
					OverlayStyle.ZIndex = 99999;
					OverlayStyle.Filter = "alpha(opacity=50)";
				}
				ReturnString.AppendLine("OverlayElement.setAttribute('style','" + OverlayStyle.Draw(true) + "'); ");
				ReturnString.AppendLine("OverlayElement.innerHTML = '" + "<img style=\"z-index:99999; position:absolute; left:48%; top:48%; \" src=\"" + Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("AjaxLoader", , , "gif") + "\"/>" + "'; ");
				ReturnString.AppendLine("var body = document.getElementsByTagName('body')[0]; ");
				ReturnString.AppendLine("body.insertBefore(OverlayElement, body.childNodes[0]); ");
				ReturnString.AppendLine("}");

				this.Script.AppendLine(ReturnString.ToString());
			}
			return "ShowOverlayElement();";
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			HideOverlayElementEvent();
			//To add function definition to the page
			string ScriptUrl = string.Empty;
			ScriptUrl = UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "FileSelector.js", this.Page.FileUsageType);
			this.Script.AddBuildScript();
			Script.AppendLine("BuildScript('" + ScriptUrl + "');");
			Content.Clear();
			Content.Add("<div style=\"position:relative;\">");
			Style.Float = FloatType.Left;
			Style.Padding = 0;
			if (this.Style.Top == int.MinValue)
				this.Style.Top = 4;
			if (this.Style.Height == int.MinValue)
				this.Style.Height = 16;
			if (this.Style.Width == int.MinValue)
				this.Style.Width = 160;
			Style HiddenInputStyle = this.Style.Clone;
			HiddenInputStyle.PositionStyle = Position.Absolute;
			HiddenInputStyle.ZIndex = 1;
			HiddenInputStyle.Filter = "alpha(opacity=0)";
			HiddenInputStyle.Opacity = 0;
			HiddenInputStyle.PositionLeft = 0;
			HiddenInputStyle.Height = this.Style.Height + 4;

			Content.Add("<input type=\"" + (this.ReadOnly ? "text" : "file") + "\" " + (this.ReadOnly ? "readonly=\"true\"" : "") + " id=\"").Add(this.ID).Add("_real\" name=\"").Add(this.ID).Add("_real\"  ").Add(HiddenInputStyle.Draw).Add(" size=\"1\"").Add(" onchange=\"" + this.ShowOverlayElementEvent() + " SelectFile('" + this.ID + "'); UploadFile('").Add(this.ID).Add("'); ").Add(!string.IsNullOrEmpty(this.OnChangeEvent) ? this.OnChangeEvent + "\"" : "").Add("\">");
			Content.Add("<input class=\"FileBox\" type=\"text\" id=\"").Add(this.ID).Add("_fake\" name=\"").Add(this.ID).Add("_fake\" ").Add(this.Style.Draw()).Add(" readonly=\"true\" value=\"").Add(this.Value).Add("\" >");
			Content.Add("<input type=\"hidden\" id=\"").Add(this.ID).Add("_FileID\" name=\"").Add(this.ID).Add("_FileID\" value=\"").Add(FileID).Add("\">");
			if (!this.ReadOnly) {
				Image SelectImage = new Image(this.ID + "_select", this.SelectButtonImage);
				SelectImage.Style.Top = 1;
				SelectImage.Style.ZIndex = 3;
				SelectImage.Style.Float = FloatType.Left;
				SelectImage.Style.Left = -24;
				Content.Add(SelectImage.Draw);

			//Dim SaveImage As New Image(Me.ID & "_save", Me.SaveButtonImage)
			//SaveImage.Style.Top = 5
			//SaveImage.Style.ZIndex = 3
			//SaveImage.Style.Float = FloatType.Left
			//SaveImage.Style.PaddingLeft = 15
			//SaveImage.OnClickEvent = "UploadFile('" & Me.ID & "');"
			//Content.Add(SaveImage.Draw)
			} else {
				if (this.HasShowButton && !string.IsNullOrEmpty(this.FileID)) {
					Image ShowImage = new Image(this.ID + "_show", this.SelectButtonImage, "Show", "", "", true);
					ShowImage.Style.Float = FloatType.Left;
					ShowImage.Style.PaddingLeft = 5;
					ShowImage.Style.Top = 1;
					ShowImage.OnClickEvent = "DownloadFile('" + this.ID + "_FileID');";
					Content.Add(ShowImage.Draw());
				}
			}
			Content.Add("</div>");
		}
		public FileBoxWithAjax(string MemberName, string Value) : this(MemberName)
		{
			base.Value = Value;
		}
		public FileBoxWithAjax(string MemberName)
		{
			this.ID = MemberName;
		}
	}
}

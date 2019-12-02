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
	public class FileBox : InputDataControl
	{
		private bool bHasShowButton = true;
		private bool bHasResetButton = true;
		private string sResetToolTip = "Reset";
		private string sShowFileToolTip = "ShowFile";
		private string sShowButtonImage = string.Empty;
		private string sResetButtonImage = string.Empty;
		private string sSelectButtonImage = string.Empty;
		public bool HasShowButton {
			get { return this.bHasShowButton; }
			set { this.bHasShowButton = value; }
		}
		public bool HasResetButton {
			get { return this.bHasResetButton; }
			set { this.bHasResetButton = value; }
		}
		public string ShowButtonImage {
			get {
				if (string.IsNullOrEmpty(this.sShowButtonImage))
					return ServerSide.ImageDrawer.GetImageUrl("ShowIcon");
				return this.sShowButtonImage;
			}
			set { this.sShowButtonImage = value; }
		}
		public string ResetButtonImage {
			get {
				if (string.IsNullOrEmpty(this.sResetButtonImage))
					return ServerSide.ImageDrawer.GetImageUrl("DeleteIcon");
				return this.sResetButtonImage;
			}
			set { this.sResetButtonImage = value; }
		}
		public string SelectButtonImage {
			get {
				if (string.IsNullOrEmpty(this.sSelectButtonImage))
					return ServerSide.ImageDrawer.GetImageUrl("SelectIcon");
				return this.sSelectButtonImage;
			}
			set { this.sSelectButtonImage = value; }
		}
		public string ResetToolTip {
			get { return this.sResetToolTip; }
			set { this.sResetToolTip = value; }
		}
		public string ShowFileToolTip {
			get { return this.sShowFileToolTip; }
			set { this.sShowFileToolTip = value; }
		}
		public static FileBoxData GetFile(HttpRequest Request, string ID)
		{
			FileBoxData FileBoxData = null;
			if ((Request(ID + "_real_Stream") != null)) {
				string[] TempString = Request(ID + "_real_Stream").Split(",");
				byte[] Data = new byte[TempString.Length + 1];
				for (int i = 0; i <= TempString.Length - 1; i++) {
					Data[i] = TempString[i];
				}
				Stream Stream = new MemoryStream(Data);
				FileBoxData = new FileBoxData(Stream, Request(ID + "_fake"));
			} else if ((Request.Files(ID + "_real") != null)) {
				FileBoxData = new FileBoxData(Request.Files(ID + "_real"), Request(ID + "_fake"));
			}
			return FileBoxData;
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			Content.Clear();
			Content.Add("<div style=\"position:relative;\">");
			Style.Float = FloatType.Left;
			Style.Padding = 0;
			if (this.Style.Top == int.MinValue)
				this.Style.Top = 4;
			if (this.Style.Height == int.MinValue)
				this.Style.Height = 16;
			Style HiddenInputStyle = this.Style.Clone;
			HiddenInputStyle.PositionStyle = Position.Absolute;
			HiddenInputStyle.ZIndex = 1;
			HiddenInputStyle.Filter = "alpha(opacity=0)";
			HiddenInputStyle.Opacity = 0;
			HiddenInputStyle.PositionLeft = 0;
			HiddenInputStyle.Height = this.Style.Height + 4;
			HiddenInputStyle.Width = int.MinValue;
			HiddenInputStyle.WidthInPercent = int.MinValue;

			int Size = 1;
			int ClearButtonPaddingLeft = 2;

			Content.Add("<input type=\"file\" id=\"").Add(this.ID).Add("_real\" name=\"").Add(this.ID).Add("_real\"  ").Add(HiddenInputStyle.Draw).Add(" onMouseOver=\"ArrangeFileBoxHiddenInput('").Add(this.ID).Add("');\" size=\"").Add(Size).Add("\" onchange=\"SelectFile('").Add(this.ID).Add("'); ").Add(!string.IsNullOrEmpty(this.OnChangeEvent) ? this.OnChangeEvent + "\"" : "").Add("\">");
			Content.Add("<input class=\"FileBox\" type=\"text\" id=\"").Add(this.ID).Add("_fake\" name=\"").Add(this.ID).Add("_fake\" ").Add(this.Style.Draw()).Add(" readonly=\"true\" value=\"").Add(this.Value).Add("\" onMouseOver=\"ArrangeFileBoxHiddenInput('").Add(this.ID).Add("');\">");
			Image SelectImage = new Image(this.ID + "_select", this.SelectButtonImage);
			SelectImage.Style.Top = 5;
			SelectImage.Style.ZIndex = 3;
			SelectImage.Style.Float = FloatType.Left;
			SelectImage.Style.PaddingLeft = 8;
			SelectImage.OnMouseOverEvent = "ArrangeFileBoxHiddenInput('" + this.ID + "');";
			Content.Add(SelectImage.Draw);
			if (this.HasResetButton) {
				Image ResetImage = new Image(this.ID + "_reset", this.ResetButtonImage, this.ResetToolTip);
				ResetImage.Style.Float = FloatType.Left;
				ResetImage.Style.PaddingLeft = ClearButtonPaddingLeft;
				ResetImage.Style.Top = 5;
				ResetImage.OnClickEvent = "ResetFile('" + this.ID + "'); " + !string.IsNullOrEmpty(this.OnClickEvent) ? this.OnClickEvent + "\"" : "";
				if (string.IsNullOrEmpty(this.Value))
					ResetImage.Style.Display = DisplayMethod.Hidden;
				Content.Add(ResetImage.Draw());
			}
			if (this.HasShowButton && !string.IsNullOrEmpty(this.Value)) {
				Image ShowImage = new Image(this.ID + "_show", this.ShowButtonImage, "Show Image", this.ShowFileToolTip, this.Value, true);
				ShowImage.Style.Float = FloatType.Left;
				ShowImage.Style.PaddingLeft = 2;
				ShowImage.Style.Top = 5;
				Content.Add(ShowImage.Draw());
			}
			Content.Add("</div>");
			this.Script.AddFunctionToOnload("ArrangeFileBoxHiddenInput('" + this.ID + "');");
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			if (Script.Function("ArrangeFileBoxHiddenInput") == null) {
				string ScriptUrl = string.Empty;
				ScriptUrl = UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "FileSelector.js", this.Page.FileUsageType);
				this.Script.AddBuildScript();
				Script.AppendLine("BuildScript('" + ScriptUrl + "');");
				ServerSide.ScriptManager.Function Function = Script.AddFunction("ArrangeFileBoxHiddenInput", "", "OpheliaElementID");
				Function.AppendLine("var FakeFileID = document.getElementById(OpheliaElementID + '_fake');");
				Function.AppendLine("var OpenFileButton = document.getElementById(OpheliaElementID + '_select');");
				Function.AppendLine("var FileID = document.getElementById(OpheliaElementID + '_real');");
				Function.AppendLine("var ClearButton = document.getElementById(OpheliaElementID + '_reset');");
				Function.AppendLine("var currentDisplay = FakeFileID.style.display;");
				Function.AppendLine("FakeFileID.style.display = '';");
				Function.AppendLine("FileID.size = 1;");
				Function.AppendLine("FileID.style.width = '';");
				Function.AppendLine("var FileIDoffset = FileID.offsetWidth - 6;");
				Function.AppendLine("var width = FakeFileID.offsetWidth + OpenFileButton.offsetWidth;");
				Function.AppendLine("FakeFileID.style.display = currentDisplay;");
				Function.AppendLine("FileID.style.zIndex = 1001;");
				Function.AppendLine("if (width > FileIDoffset) {");
				Function.AppendLine("var resize = width - FileIDoffset;");
				Function.AppendLine("var size = Math.ceil(resize / 6);");
				Function.AppendLine("FileID.style.width = width + 'px';");
				Function.AppendLine("FileID.size = size;");
				Function.AppendLine("if (ClearButton != undefined && ClearButton != null){");
				Function.AppendLine("if (resize % 6 != 0) {");
				Function.AppendLine("");
				Function.AppendLine("ClearButton.style.paddingLeft = parseInt(parseInt((resize % 6)) + parseInt(4))  + 'px';");
				Function.AppendLine("}");
				Function.AppendLine("}");
				Function.AppendLine("}");
				Function.AppendLine("else {");
				Function.AppendLine("  FileID.style.width = width + 'px';");
				Function.AppendLine("  if (ClearButton != undefined && ClearButton != null)");
				Function.AppendLine("      ClearButton.style.paddingLeft = '4px';");
				Function.AppendLine("}");
			}
		}
		public FileBox(string MemberName, string Value) : this(MemberName)
		{
			base.Value = Value;
		}
		public FileBox(string MemberName)
		{
			this.ID = MemberName;
		}
	}
	public class FileBoxData
	{
		private System.IO.Stream oInputStream;
		private bool bHasValue;
		private string sFileName = string.Empty;
		public System.IO.Stream InputStream {
			get {
				if (HasValue)
					return this.oInputStream;
				return null;
			}
		}
		public string FileName {
			get {
				if (HasValue)
					return this.sFileName;
				return null;
			}
		}
		public bool HasValue {
			get { return bHasValue; }
		}
		public FileBoxData(System.IO.Stream InputStream, string FileName)
		{
			this.oInputStream = InputStream;
			this.bHasValue = !string.IsNullOrEmpty(FileName);
			this.sFileName = FileName;
		}
		public FileBoxData(HttpPostedFile File, string FileName)
		{
			this.oInputStream = File.InputStream;
			this.bHasValue = !string.IsNullOrEmpty(FileName);
			this.sFileName = FileName;
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class ColorPicker : InputDataControl
	{
		private string sSelectPanelText = "";
		private string sCancelPanelText = "";
		public string SelectPanelText {
			get { return sSelectPanelText; }
			set { sSelectPanelText = value; }
		}
		public string CancelPanelText {
			get { return sCancelPanelText; }
			set { sCancelPanelText = value; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			Content.Add("<input ");
			if (!string.IsNullOrEmpty(this.ID)) {
				Content.Add(" id=\"" + this.ID + "\"");
				Content.Add(" name=\"" + this.ID + "\"");
				Content.Add(" class=\"ColorPicker\"");
			}
			if (!string.IsNullOrEmpty(this.Value)) {
				Content.Add(" value=\"" + this.Value + "\"");
			}
			this.DrawEvents(Content);
			Content.Add(" type=\"text\" ");
			Content.Add(" >");

			Content.Add("<span style=\"padding-left:8px;vertical-align: bottom;\">");
			if (this.Page.Body != null && this.Page.Body.HasColorPicker) {
				Button ColorPickerButton = new Button("ColorPickerButton", ServerSide.ImageDrawer.GetImageUrl("ColorActive"), Button.ButtonType.AjaxButton);
				ColorPickerButton.OnClickEvent = "ShowColorSelectionPanel('" + this.ID + "');return false;";
				ColorPickerButton.Style.VerticalAlignment = VerticalAlignment.Top;
				ColorPickerButton.Style.CursorStyle = Cursor.Pointer;
				if (this.Page.QueryString.Request.Browser.Browser == "AppleMAC-Safari")
					ColorPickerButton.Style.PaddingTop = 2;
				Content.Add(ColorPickerButton.Draw());
			} else {
				PopupButton PopupButton = new PopupButton("colorpicker", this.Page.ContentManager.GetFile("Ophelia", "ColorPicker", "htm", false), PopupControl.PopupAutoClosingType.Hide);
				PopupButton.ImageSource = ServerSide.ImageDrawer.GetImageUrl("ColorActive");
				PopupButton.OnClickEvent = "ShowColorSelectionPanel('" + this.ID + "');return false;";
				PopupButton.Type = Button.ButtonType.AjaxButton;
				PopupButton.Style.VerticalAlignment = VerticalAlignment.Top;
				PopupButton.Style.CursorStyle = Cursor.Pointer;
				if (this.Page.QueryString.Request.Browser.Browser == "AppleMAC-Safari")
					PopupButton.Style.PaddingTop = 2;
				Content.Add(PopupButton.Draw);
				if (this.Page.Body != null)
					this.Page.Body.HasColorPicker = true;
			}
			Content.Add("</span>");
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			this.Script.Manager.Add("ColorPicker", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "ColorPicker.js", this.Page.FileUsageType));

			base.CustomizeScript(Script);
			this.Script.AddGlobalVariable("OpheliaElementID", "");
			if (Script.Function("ShowColorSelectionPanel") == null) {
				ServerSide.ScriptManager.Function ShowColorSelectionPanelFunction = Script.AddFunction("ShowColorSelectionPanel", "", "ElementID");
				ShowColorSelectionPanelFunction.AppendLine("var Selector = document.getElementById(ElementID + '_ColorSelector');");
				if (!string.IsNullOrEmpty(SelectPanelText))
					ShowColorSelectionPanelFunction.AppendLine("ColorPickerSelectPanelText='" + SelectPanelText + "';");
				if (!string.IsNullOrEmpty(CancelPanelText))
					ShowColorSelectionPanelFunction.AppendLine("ColorPickerCancelPanelText='" + CancelPanelText + "';");
				ShowColorSelectionPanelFunction.AppendLine("OpheliaElementID = ElementID;ShowColorPage();");
			}

			if (Script.Function("ShowColorPage") == null) {
				ServerSide.ScriptManager.AjaxFunction OpenColorPage = Script.AddAjaxFunction("ShowColorPage", "", "", "", false, false);
				OpenColorPage.DisplayEventName = "ShowColorPage";
				OpenColorPage.UpdateInnerHtml("'colorpickerppcr'", true);
				string ScriptUrl = "";
				string ApplicationPath = "/";
				if (this.Page != null) {
					ApplicationPath = this.Page.ContentManager.ApplicationPath;
				}
				this.Script.AddBuildScript();
				ScriptUrl = UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "ColorPicker.js", this.Page.FileUsageType);
				OpenColorPage.AppendLine("BuildScript('" + ScriptUrl + "');");
				OpenColorPage.AppendLine("ShowPopup('colorpicker', 0);");
				OpenColorPage.AppendLine("if (typeof init == 'function')");
				OpenColorPage.AppendLine("{init();}");
			}
		}
		public ColorPicker(string ID)
		{
			this.ID = ID;
			this.Style.Width = 50;
			this.Style.PaddingLeft = 3;
			this.Style.PaddingRight = 3;
			this.Style.Borders.Width = 1;
		}
	}
}

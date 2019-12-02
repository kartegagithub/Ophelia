using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class PopupControl : ComplexWebControl
	{
		private Panel oPanel;
		private Panel oContentRegion;
		private Image oCloseImage;
		private decimal nWidth;
		private PopupAutoClosingType eAutoClosingType = PopupAutoClosingType.Hide;
		private bool bHasContainer = true;
		private bool bHasCloseImage = true;
		private bool bOverlayPanelHasCloseEvent = true;
		private PopupContentRegionDisplayType eContentRegionDisplayType = PopupContentRegionDisplayType.TopAndCenter;
		private string sControlID = "";
		private int nAutoCloseDuration = -1;
		public bool HasContainer {
			get { return this.bHasContainer; }
			set { this.bHasContainer = value; }
		}
		public int AutoCloseDuration {
			get { return this.nAutoCloseDuration; }
			set { this.nAutoCloseDuration = value; }
		}
		private string ControlID {
			get { return this.sControlID; }
		}
		public void SetDependentControl(string ControlID)
		{
			this.sControlID = ControlID;
		}
		public bool HasCloseImage {
			get { return this.bHasCloseImage; }
			set { this.bHasCloseImage = value; }
		}
		public bool OverlayPanelHasCloseEvent {
			get { return this.bOverlayPanelHasCloseEvent; }
			set { this.bOverlayPanelHasCloseEvent = value; }
		}
		public PopupContentRegionDisplayType ContentRegionDisplayType {
			get {
				if (!string.IsNullOrEmpty(this.ControlID))
					return PopupContentRegionDisplayType.ControlPosition;
				return this.eContentRegionDisplayType;
			}
			set {
				if (value != PopupContentRegionDisplayType.ControlPosition) {
					this.eContentRegionDisplayType = value;
				}
			}
		}
		public Panel Panel {
			get { return this.oPanel; }
		}
		public Container ContentRegion {
			get { return this.oContentRegion; }
		}
		public Image CloseImage {
			get {
				if (this.oCloseImage == null) {
					this.oCloseImage = new Image("", ServerSide.ImageDrawer.GetImageUrl("WebClose"));
					this.oCloseImage.Style.PositionStyle = Position.Absolute;
					this.oCloseImage.Style.Right = -10;
					this.oCloseImage.Style.Top = -10;
					this.oCloseImage.Style.PositionTop = 0;
					this.oCloseImage.Style.PositionRight = 0;
					this.oCloseImage.Style.CursorStyle = Cursor.Pointer;
				}
				return this.oCloseImage;
			}
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			if (this.Script.Function("ShowPopup") == null) {
				Controls.ServerSide.ScriptManager.Function ShowPopup = this.Script.AddFunction("ShowPopup", "", "elementid,type,controlid,duration");
				ShowPopup.AppendLine("if (this.showedPopup == undefined) this.showedPopup = new Array();");
				ShowPopup.AppendLine("if (this.CurrentPopUpZIndex == undefined) this.CurrentPopUpZIndex = 1000;");
				ShowPopup.AppendLine("if (this.CurrentPopUpContainerZIndex == undefined) this.CurrentPopUpContainerZIndex = 100;");
				ShowPopup.AppendLine("OnBeforeShowPopup(elementid);");
				ShowPopup.AppendLine("if (type == undefined) type = 0;");
				ShowPopup.AppendLine("if (duration == undefined) duration = -1;");
				ShowPopup.AppendLine("var popup = document.getElementById(elementid + 'ppcr');");
				ShowPopup.AppendLine("var popupcontainer = document.getElementById(elementid + 'pp');");
				ShowPopup.AppendLine("popup.style.display = 'block';");
				ShowPopup.AppendLine("if (popupcontainer != undefined) popupcontainer.style.display = 'block';");
				ShowPopup.AppendLine("this.showedPopup.push(elementid);");
				ShowPopup.AppendLine("popup.style.left = '0px';");
				ShowPopup.AppendLine("var popupwidth = popup.offsetWidth;");
				ShowPopup.AppendLine("var calculatedLeft = -1 * popupwidth / 2;");
				ShowPopup.AppendLine("if (type ==0)");
				ShowPopup.AppendLine("{");
				ShowPopup.AppendLine("      popup.style.left = '50%';");
				ShowPopup.AppendLine("      popup.style.top= '15%';");
				ShowPopup.AppendLine("      popup.style.marginLeft = calculatedLeft + 'px';");
				ShowPopup.AppendLine("}");
				ShowPopup.AppendLine("else if (type==1)");
				ShowPopup.AppendLine("{");
				ShowPopup.AppendLine("      popup.style.left = '50%';");
				ShowPopup.AppendLine("      popup.style.top = this.MouseTop + 'px';");
				ShowPopup.AppendLine("      popup.style.marginLeft = calculatedLeft + 'px';");
				ShowPopup.AppendLine("}");
				ShowPopup.AppendLine("else if (type == 2)");
				ShowPopup.AppendLine("{");
				ShowPopup.AppendLine("      popup.style.left = this.MouseLeft + 'px';");
				ShowPopup.AppendLine("      popup.style.top = this.MouseTop + 'px';");
				ShowPopup.AppendLine("}");
				ShowPopup.AppendLine("else if (type == 3)");
				ShowPopup.AppendLine("{");
				ShowPopup.AppendLine("      var point = CalculateControlPosition(document.getElementById(controlid));");
				ShowPopup.AppendLine("      popup.style.left = point.x + 'px';");
				ShowPopup.AppendLine("      popup.style.top = point.y + 'px';");
				ShowPopup.AppendLine("}");
				//Z-index ayarları. Browser'lar için 
				ShowPopup.AppendLine("popup.style.zIndex = this.CurrentPopUpZIndex;");
				ShowPopup.AppendLine("if (popupcontainer != undefined) popupcontainer.style.zIndex = this.CurrentPopUpContainerZIndex;");
				ShowPopup.AppendLine("this.CurrentPopUpZIndex = this.CurrentPopUpZIndex + 1000;");
				ShowPopup.AppendLine("this.CurrentPopUpContainerZIndex = this.CurrentPopUpContainerZIndex + 1000;");

				ShowPopup.AppendLine("if (popupcontainer != undefined)");
				ShowPopup.AppendLine("{");
				ShowPopup.AppendLine("    if (popup.style.top + popup.offsetHeight > popupcontainer.offsetHeight)");
				ShowPopup.AppendLine("             popup.style.top = (popupcontainer.offsetHeight - popup.offsetHeight -20) + 'px';");
				ShowPopup.AppendLine("}");

				ShowPopup.AppendLine("if (duration > 0) ClosePopupFromInterval(elementid,duration);");
			}

			if (this.Script.Function("CalculateControlPosition") == null) {
				Controls.ServerSide.ScriptManager.Function CalculateControlPosition = this.Script.AddFunction("CalculateControlPosition", "", "control");
				CalculateControlPosition.AppendLine("var point = {x:control.offsetWidth,y:control.offsetHeight};");
				CalculateControlPosition.AppendLine("while (control)");
				CalculateControlPosition.AppendLine("{");
				CalculateControlPosition.AppendLine("       if (control.style.position == 'absolute')");
				CalculateControlPosition.AppendLine("           break;");
				CalculateControlPosition.AppendLine("       point.x += control.offsetLeft;");
				CalculateControlPosition.AppendLine("       point.y += control.offsetTop;");
				CalculateControlPosition.AppendLine("       control = control.offsetParent;");
				CalculateControlPosition.AppendLine("}");
				CalculateControlPosition.AppendLine("return point;");
			}


			if (this.Script.Function("FindMouseCoordinates") == null) {
				Script.AppendLine("if (this.MouseTop == undefined) var MouseTop=-1;");
				Script.AppendLine("if (this.MouseLeft == undefined) var MouseLeft=-1;");

				Controls.ServerSide.ScriptManager.Function FindMouseCoordinates = this.Script.AddFunction("FindMouseCoordinates", "", "e");
				this.Page.Body.OnMouseOver += "FindMouseCoordinates(event);";
				FindMouseCoordinates.AppendLine("var IE = document.all?true:false;");
				FindMouseCoordinates.AppendLine("if (IE)");
				FindMouseCoordinates.AppendLine("{");
				FindMouseCoordinates.AppendLine("   this.MouseLeft = event.clientX + document.body.scrollLeft;");
				FindMouseCoordinates.AppendLine("   this.MouseTop = event.clientY + document.body.scrollTop;");
				FindMouseCoordinates.AppendLine("}");
				FindMouseCoordinates.AppendLine("else {");
				FindMouseCoordinates.AppendLine("    this.MouseLeft = e.pageX;");
				FindMouseCoordinates.AppendLine("    this.MouseTop = e.pageY;");
				FindMouseCoordinates.AppendLine("}");
				FindMouseCoordinates.AppendLine("if (this.MouseLeft < 0){MouseLeft = 0;}");
				FindMouseCoordinates.AppendLine("if (this.MouseTop < 0){MouseTop = 0;}");
				FindMouseCoordinates.AppendLine("return true;");
			}
			if (this.Script.Function("HandleEcsapeForClosePopup") == null) {
				Controls.ServerSide.ScriptManager.Function HandleEcsapeForClosePopup = this.Script.AddFunction("HandleEcsapeForClosePopup", "", "e");
				HandleEcsapeForClosePopup.AppendLine("var code = e.keyCode ? e.keyCode : e.which;");
				HandleEcsapeForClosePopup.AppendLine("if (code == 27){");
				HandleEcsapeForClosePopup.AppendLine("    if (this.showedPopup.length > 0)");
				HandleEcsapeForClosePopup.AppendLine("        HidePopup(this.showedPopup[showedPopup.length-1]);");
				HandleEcsapeForClosePopup.AppendLine("}");
			}
			if (this.Script.Function("ClosePopupFromInterval") == null) {
				Controls.ServerSide.ScriptManager.Function ClosePopupFromInterval = this.Script.AddFunction("ClosePopupFromInterval", "", "elementid,duration");
				ClosePopupFromInterval.AppendLine("setTimeout('HidePopup(\\''+ elementid + '\\')',duration);");
			}
			if (this.Script.Function("HidePopup") == null)
				this.Script.AddFunction("HidePopup", "try{clearTimeout();}catch(e){}OnBeforeHidePopup(elementid);this.showedPopup.pop(); if (document.getElementById(elementid + 'pp') != undefined && document.getElementById(elementid + 'pp') != null) {document.getElementById(elementid + 'pp').style.display = 'none';document.getElementById(elementid + 'ppcr').style.display = 'none';}", "elementid");
			if (this.Script.Function("RemovePopup") == null)
				this.Script.AddFunction("RemovePopup", "try{clearTimeout();}catch(e){}this.showedPopup.pop(); if (document.getElementById(elementid + 'pp') != undefined) document.body.removeChild(document.getElementById(elementid + 'pp'));document.body.removeChild(document.getElementById(elementid + 'ppcr'));", "elementid");
			if (this.Script.Function("OnBeforeShowPopup") == null)
				this.Script.AddFunction("OnBeforeShowPopup", "return true;", "PopupID");
			if (this.Script.Function("OnBeforeHidePopup") == null)
				this.Script.AddFunction("OnBeforeHidePopup", "return true;", "PopupID");
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.Page.Body.PopUpControlsInnerHtml.Add(this.DrawToBody());
		}
		public new Style Style {
			get { return this.ContentRegion.Style; }
		}
		internal string DrawToBody()
		{
			this.Panel.ID = ID + "pp";
			this.ContentRegion.ID = ID + "ppcr";
			this.StyleSheet.AddIDBasedRule(ID + "pp", Panel.Style);
			this.StyleSheet.AddCustomRule("#" + ID + "ppcr", this.ContentRegion.Style);

			if (this.AutoClosingType == PopupAutoClosingType.Hide) {
				this.Panel.OnClickEvent += (this.OverlayPanelHasCloseEvent ? this.HideEvent : "");
				CloseImage.OnClickEvent += this.HideEvent;
			} else if (this.AutoClosingType == PopupAutoClosingType.Remove) {
				this.Panel.OnClickEvent += (this.OverlayPanelHasCloseEvent ? this.RemoveEvent : "");
				CloseImage.OnClickEvent += this.RemoveEvent;
			}
			string TempClass = "";
			this.Page.Body.OnKeyUp = "HandleEcsapeForClosePopup(event);";
			TempClass = Panel.Style.Class;
			Panel.SetStyle(new Style());
			// Çizmemesi için
			Panel.Style.Class = TempClass;
			if (this.HasCloseImage) {
				ContentRegion.Controls.Add(CloseImage);
			}
			TempClass = ContentRegion.Style.Class;
			ContentRegion.SetStyle(new Style());
			// Çizmemesi için
			ContentRegion.Style.Class = TempClass;
			if (this.HasContainer) {
				return this.Panel.Draw + this.ContentRegion.Draw;
			} else {
				return this.ContentRegion.Draw;
			}
		}
		public PopupAutoClosingType AutoClosingType {
			get { return this.eAutoClosingType; }
			set { this.eAutoClosingType = value; }
		}
		public void Hide()
		{
			this.Panel.Style.Display = DisplayMethod.Hidden;
			this.ContentRegion.Style.Display = DisplayMethod.Hidden;
		}
		public void Show()
		{
			this.Panel.Style.Display = DisplayMethod.Block;
			this.ContentRegion.Style.Display = DisplayMethod.Block;
		}
		public string RemoveEvent {
			get { return "RemovePopup('" + this.ID + "');"; }
		}
		public string HideEvent {
			get { return "HidePopup('" + this.ID + "');"; }
		}
		public string ShowEvent {
			get {
				if (this.ContentRegionDisplayType == PopupContentRegionDisplayType.TopAndCenter) {
					return "ShowPopup('" + this.ID + "',0,'" + this.ControlID + "'," + this.AutoCloseDuration + ");";
				} else if (this.ContentRegionDisplayType == PopupContentRegionDisplayType.MouseTopCenter) {
					return "ShowPopup('" + this.ID + "',1,'" + this.ControlID + "'," + this.AutoCloseDuration + ");";
				} else if (this.ContentRegionDisplayType == PopupContentRegionDisplayType.MousePosition) {
					return "ShowPopup('" + this.ID + "',2,'" + this.ControlID + "'," + this.AutoCloseDuration + ");";
				} else {
					return "ShowPopup('" + this.ID + "',3,'" + this.ControlID + "'," + this.AutoCloseDuration + ");";
				}
			}
		}
		public PopupControl()
		{
			this.Configure();
		}
		public void UseForSimpleToolTip()
		{
			this.HasCloseImage = false;
			this.HasContainer = false;
			this.ContentRegionDisplayType = PopupContentRegionDisplayType.MousePosition;
			this.ContentRegion.SetStyle(new Style());
			this.ContentRegion.Style.PositionStyle = Position.Fixed;
		}
		private void Configure()
		{
			this.oPanel = new Panel(ID + "pp");
			this.Panel.Style.BackgroundColor = "black";
			this.Panel.Style.Opacity = 0.7;
			this.Panel.Style.PositionStyle = Position.Fixed;
			this.Panel.Style.ZIndex = 999;
			this.Panel.Style.Filter = "alpha(opacity=70)";
			this.Panel.Style.PositionLeft = 0;
			this.Panel.Style.PositionTop = 0;
			this.Panel.Style.Dock = DockStyle.Fill;
			this.oContentRegion = new Panel(ID + "ppcr");
			this.oContentRegion.Style.BackgroundColor = "white";
			this.oContentRegion.Style.ZIndex = 99999;
			this.oContentRegion.Style.PositionStyle = Position.Fixed;
			this.oContentRegion.Style.Padding = 15;
			this.oContentRegion.Style.Borders.Width = 10;
			this.oContentRegion.Style.Borders.Color = "gray";
			this.oContentRegion.Style.Borders.Style = Forms.BorderStyle.Solid;
			this.oContentRegion.Style.Borders.Radius = 10;
		}
		public PopupControl(string ID, bool IsToolTip, Container Content) : this(ID, Content)
		{
			if (IsToolTip) {
				this.UseForSimpleToolTip();
			}
		}
		public PopupControl(string ID, Container Content, PopupAutoClosingType AutoClosingType) : this(ID, Content)
		{
			this.AutoClosingType = AutoClosingType;
		}
		public PopupControl(string ID, Container Content) : this(ID)
		{
			this.ContentRegion.Controls.Add(Content);
		}
		public PopupControl(string ID, bool IsToolTip, string Content) : this(ID, Content)
		{
			if (IsToolTip) {
				this.UseForSimpleToolTip();
			}
		}
		public PopupControl(string ID, string Content, PopupAutoClosingType AutoClosingType) : this(ID, Content)
		{
			this.AutoClosingType = AutoClosingType;
		}
		public PopupControl(string ID, string Content) : this(ID)
		{
			this.ContentRegion.Content.Add(Content);
		}
		public PopupControl(string ID, bool IsToolTip) : this(ID)
		{
			if (IsToolTip) {
				this.UseForSimpleToolTip();
			}
		}
		public PopupControl(string ID)
		{
			this.ID = ID;
			this.Configure();
		}
		public enum PopupAutoClosingType
		{
			None = 0,
			Hide = 1,
			Remove = 2
		}
		public enum PopupContentRegionDisplayType
		{
			TopAndCenter = 0,
			MouseTopCenter = 1,
			MousePosition = 2,
			ControlPosition = 3
		}
	}
}

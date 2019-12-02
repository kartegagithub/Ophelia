using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class Body
	{
		private Style oStyle;
		private string sID;
		private string sLanguageCode;
		private string sTitle;
		private string sOnUnload;
		private string sOnKeyUp;
		private string sOnKeyPress;
		private string sOnKeyDown;
		private string sOnMouseUp;
		private string sOnMouseOver;
		private string sOnMouseOut;
		private string sOnMouseMove;
		private string sOnMouseDown;
		private string sOnDoubleClick;
		private string sOnLoad;
		private string sOnClick;
		private IPage oPage;
		private Content oInnerHtml;
		private Controls.WebControlCollection oControls;
		private bool bHasPopupContainer = false;
		private bool bHasColorPicker = false;
		internal Content PopUpControlsInnerHtml = new Content();
		public IPage Page {
			get { return this.oPage; }
		}
		public Controls.WebControlCollection Controls {
			get {
				if (this.oControls == null) {
					this.oControls = new Controls.WebControlCollection(this.Page);
				}
				return this.oControls;
			}
		}
		public string ID {
			get { return this.sID; }
			set { this.sID = value; }
		}
		public string LanguageCode {
			get { return this.sLanguageCode; }
			set { this.sLanguageCode = value; }
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public Content InnerHtml {
			get {
				if (this.oInnerHtml == null)
					this.oInnerHtml = new Content();
				return this.oInnerHtml;
			}
		}
		internal bool HasPopupContainer {
			get { return this.bHasPopupContainer; }
			set { this.bHasPopupContainer = value; }
		}
		internal bool HasColorPicker {
			get { return this.bHasColorPicker; }
			set { this.bHasColorPicker = value; }
		}
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = new Style();
					this.oStyle.Top = 0;
					this.oStyle.Left = 0;
					this.oStyle.Right = 0;
				}
				return this.oStyle;
			}
		}
		#region "Events"
		public string OnClick {
			get { return this.sOnClick; }
			set { this.sOnClick = value; }
		}
		public string OnDoubleClick {
			get { return this.sOnDoubleClick; }
			set { this.sOnDoubleClick = value; }
		}
		public string OnLoad {
			get { return this.sOnLoad; }
			set { this.sOnLoad = value; }
		}
		public string OnMouseDown {
			get { return this.sOnMouseMove; }
			set { this.sOnMouseMove = value; }
		}
		public string OnMouseMove {
			get { return this.sOnMouseMove; }
			set { this.sOnMouseMove = value; }
		}
		public string OnMouseOut {
			get { return this.sOnMouseOut; }
			set { this.sOnMouseOut = value; }
		}
		public string OnMouseOver {
			get { return this.sOnMouseOver; }
			set { this.sOnMouseOver = value; }
		}
		public string OnMouseUp {
			get { return this.sOnMouseUp; }
			set { this.sOnMouseUp = value; }
		}
		public string OnKeyDown {
			get { return this.sOnKeyDown; }
			set { this.sOnKeyDown = value; }
		}
		public string OnKeyPress {
			get { return this.sOnKeyPress; }
			set { this.sOnKeyPress = value; }
		}
		public string OnKeyUp {
			get { return this.sOnKeyUp; }
			set { this.sOnKeyUp = value; }
		}
		public string OnUnload {
			get { return this.sOnUnload; }
			set { this.sOnUnload = value; }
		}
		private void DrawEvents(ref Content Content)
		{
			this.CheckEvent(ref this.OnLoad);
			this.CheckEvent(ref this.OnClick);
			this.CheckEvent(ref this.OnDoubleClick);
			this.CheckEvent(ref this.OnKeyUp);
			this.CheckEvent(ref this.OnKeyDown);
			this.CheckEvent(ref this.OnKeyPress);
			this.CheckEvent(ref this.OnMouseMove);
			this.CheckEvent(ref this.OnMouseOut);
			this.CheckEvent(ref this.OnMouseOver);
			this.CheckEvent(ref this.OnUnload);
			this.CheckEvent(ref this.OnMouseUp);
			this.CheckEvent(ref this.OnMouseDown);

			if (!string.IsNullOrEmpty(this.OnLoad))
				Content.Add(" onload=\"" + this.OnLoad + "\"");
			if (!string.IsNullOrEmpty(this.OnClick))
				Content.Add(" onclick=\"" + this.OnClick + "\"");
			if (!string.IsNullOrEmpty(this.OnDoubleClick))
				Content.Add(" ondblclick=\"" + this.OnDoubleClick + "\"");
			if (!string.IsNullOrEmpty(this.OnKeyUp))
				Content.Add(" onkeyup=\"" + this.OnKeyUp + "\"");
			if (!string.IsNullOrEmpty(this.OnKeyDown))
				Content.Add(" onkeydown=\"" + this.OnKeyDown + "\"");
			if (!string.IsNullOrEmpty(this.OnKeyPress))
				Content.Add(" onkeypress=\"" + this.OnKeyPress + "\"");
			if (!string.IsNullOrEmpty(this.OnMouseMove))
				Content.Add(" onmousemove=\"" + this.OnMouseMove + "\"");
			if (!string.IsNullOrEmpty(this.OnMouseOut))
				Content.Add(" onmouseout=\"" + this.OnMouseOut + "\"");
			if (!string.IsNullOrEmpty(this.OnMouseOver))
				Content.Add(" onmouseover=\"" + this.OnMouseOver + "\"");
			if (!string.IsNullOrEmpty(this.OnUnload))
				Content.Add(" onunload=\"" + this.OnUnload + "\"");
			if (!string.IsNullOrEmpty(this.OnMouseUp))
				Content.Add(" onmouseup=\"" + this.OnMouseUp + "\"");
			if (!string.IsNullOrEmpty(this.OnMouseDown))
				Content.Add(" onmousedown=\"" + this.OnMouseDown + "\"");
		}
		private void CheckEvent(ref string Value)
		{
			if (Value != null && !string.IsNullOrEmpty(Value)) {
				Value = Value.Trim();
				Value = Value.Replace(";;", ";");
				if (!Value.EndsWith(";")) {
					Value += ";";
				} else if (Value.Length == 1) {
					Value = "";
				}
			}
		}
		#endregion
		internal void Draw(ref Content Content)
		{
			Content ControlsContent = new Content();
			for (int i = 0; i <= this.Controls.Count - 1; i++) {
				ControlsContent.Add(this.Controls(i).Draw);
			}
			this.Page.StyleSheet.AddCustomRule("Body", this.Style);
			Content.Add("<body");
			if ((this.oStyle != null) && !string.IsNullOrEmpty(this.oStyle.Class)) {
				Content.Add(" class=\"").Add(this.oStyle.Class).Add("\"");
			}
			this.DrawEvents(ref Content);
			Content.Add(">");
			Content.Add(this.PopUpControlsInnerHtml.Value);
			Content.Add(this.InnerHtml.Value);
			Content.Add(ControlsContent.Value);
			Content.Add("</body>");
		}
		public Body(IPage Page)
		{
			this.oPage = Page;
		}
	}
}

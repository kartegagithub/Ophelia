using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
using Ophelia.Web.View.Controls;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
using Ophelia.Web.View.Controls.Validator;
namespace Ophelia.Web.View.Controls
{
	public abstract class WebControl : IWebControl
	{
		private string sID = string.Empty;
		private Style oStyle;
		private IContainer oContainer;
		private IComplexWebControl oParentControl;
		private StyleSheet oStyleSheet;
		private Script oScript;
		private string sLanguage = string.Empty;
			//Performans ama�l� public tan�mland�.
		public Hashtable oAttributes = null;
		public Hashtable Attributes {
			get {
				if (oAttributes == null)
					oAttributes = new Hashtable();
				return oAttributes;
			}
		}
		public UI.IPage Page {
			get { return Ophelia.Web.View.UI.PageContext.Current; }
		}
		public IContainer Container {
			get { return this.oContainer; }
			set { this.oContainer = value; }
		}
		public IComplexWebControl ParentControl {
			get { return this.oParentControl; }
			set { this.oParentControl = value; }
		}
		public virtual string ID {
			get { return this.sID; }
			set { this.sID = value; }
		}
		public Script Script {
			get {
				if (this.oScript == null) {
					if (this.Page != null) {
						this.oScript = this.Page.ScriptManager.FirstScript;
					} else {
						this.oScript = new Script(this.GetType().FullName, null);
					}
					this.CustomizeScript(this.oScript);
				}
				return this.oScript;
			}
		}
		public StyleSheet StyleSheet {
			get {
				if (this.oStyleSheet == null) {
					if (this.Page != null) {
						this.oStyleSheet = this.Page.StyleSheet;
					} else {
						this.oStyleSheet = new StyleSheet();
					}
				}
				return this.oStyleSheet;
			}
		}
		public string Language {
			get { return this.sLanguage; }
			set { this.sLanguage = value; }
		}

		protected virtual void CustomizeScript(Script Script)
		{
		}
		internal void SetScript(Script Script)
		{
			this.oScript = Script;
		}
		#region "Events"
		private string sOnMouseOverEvent = string.Empty;
		private string sOnKeyUpEvent = string.Empty;
		private string sOnKeyDownEvent = string.Empty;
		private string sOnKeyPressEvent = string.Empty;
		private string sOnClickEvent = string.Empty;
		private string sOnDoubleClickEvent = string.Empty;
		private string sOnMouseMoveEvent = string.Empty;
		private string sOnMouseOutEvent = string.Empty;
		private string sOnMouseUpEvent = string.Empty;
		public string OnDoubleClickEvent {
			get { return this.sOnDoubleClickEvent; }
			set { this.sOnDoubleClickEvent = value; }
		}
		public string OnMouseMoveEvent {
			get { return this.sOnMouseMoveEvent; }
			set { this.sOnMouseMoveEvent = value; }
		}
		public string OnMouseOutEvent {
			get { return this.sOnMouseOutEvent; }
			set { this.sOnMouseOutEvent = value; }
		}
		public string OnMouseOverEvent {
			get { return this.sOnMouseOverEvent; }
			set { this.sOnMouseOverEvent = value; }
		}
		public string OnMouseUpEvent {
			get { return this.sOnMouseUpEvent; }
			set { this.sOnMouseUpEvent = value; }
		}
		public string OnClickEvent {
			get { return this.sOnClickEvent; }
			set { this.sOnClickEvent = value; }
		}
		public string OnKeyUpEvent {
			get { return this.sOnKeyUpEvent; }
			set { this.sOnKeyUpEvent = value; }
		}
		public string OnKeyPressEvent {
			get { return this.sOnKeyPressEvent; }
			set { this.sOnKeyPressEvent = value; }
		}
		public string OnKeyDownEvent {
			get { return this.sOnKeyDownEvent; }
			set { this.sOnKeyDownEvent = value; }
		}
		#endregion
		protected internal WebControl ConfigureSubControls(WebControl WebControl)
		{
			if (this.ParentControl != null) {
				WebControl.ParentControl = this.ParentControl;
			}
			if (this.Container != null) {
				WebControl.Container = this.Container;
			}
			return WebControl;
		}
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = this.CreateStyle();
				}
				return this.oStyle;
			}
		}
		public virtual Style CreateStyle()
		{
			return new Style();
		}
		public void SetStyle(Style Style)
		{
			this.oStyle = Style;
		}
		public HttpRequest Request {
			get { return this.Page.Request; }
		}
		public abstract void OnBeforeDraw(Ophelia.Web.View.Content Content);
		public string Draw()
		{
			Content DrawnContent = new Content();
			this.OnBeforeDraw(DrawnContent);
			if (this.Script != null && this.Script.Manager == null) {
				DrawnContent.Add(this.Script.Draw(false));
			}
			if (this.Page == null || !object.ReferenceEquals(this.Page.StyleSheet, this.StyleSheet)) {
				DrawnContent.Add(this.StyleSheet.Draw);
			}
			return DrawnContent.Value;
		}
		protected void CheckEvent(ref string Value)
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
		public virtual void CloneEventsFrom(WebControl WebControl)
		{
			this.OnClickEvent = WebControl.OnClickEvent;
			this.OnDoubleClickEvent = WebControl.OnDoubleClickEvent;
			this.OnKeyUpEvent = WebControl.OnKeyUpEvent;
			this.OnKeyPressEvent = WebControl.OnKeyPressEvent;
			this.OnKeyDownEvent = WebControl.OnKeyDownEvent;
			this.OnMouseMoveEvent = WebControl.OnMouseMoveEvent;
			this.OnMouseOutEvent = WebControl.OnMouseOutEvent;
			this.OnMouseOverEvent = WebControl.OnMouseOverEvent;
			this.OnMouseUpEvent = WebControl.OnMouseUpEvent;
		}
		protected virtual void DrawEvents(Content Content)
		{
			this.CheckEvent(ref this.OnClickEvent);
			this.CheckEvent(ref this.OnDoubleClickEvent);
			this.CheckEvent(ref this.OnKeyUpEvent);
			this.CheckEvent(ref this.OnKeyDownEvent);
			this.CheckEvent(ref this.OnKeyPressEvent);
			this.CheckEvent(ref this.OnMouseMoveEvent);
			this.CheckEvent(ref this.OnMouseOutEvent);
			this.CheckEvent(ref this.OnMouseOverEvent);
			this.CheckEvent(ref this.OnMouseUpEvent);

			if (!string.IsNullOrEmpty(this.OnClickEvent))
				Content.Add(" onclick=\"").Add(this.OnClickEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnDoubleClickEvent))
				Content.Add(" ondblclick=\"").Add(this.OnDoubleClickEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnKeyUpEvent))
				Content.Add(" onkeyup=\"").Add(this.OnKeyUpEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnKeyDownEvent))
				Content.Add(" onkeydown=\"").Add(this.OnKeyDownEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnKeyPressEvent))
				Content.Add(" onkeypress=\"").Add(this.OnKeyPressEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnMouseMoveEvent))
				Content.Add(" onmousemove=\"").Add(this.OnMouseMoveEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnMouseOutEvent))
				Content.Add(" onmouseout=\"").Add(this.OnMouseOutEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnMouseOverEvent))
				Content.Add(" onmouseover=\"").Add(this.OnMouseOverEvent).Add("\"");
			if (!string.IsNullOrEmpty(this.OnMouseUpEvent))
				Content.Add(" onmouseup=\"").Add(this.OnMouseUpEvent).Add("\"");
		}
	}
}

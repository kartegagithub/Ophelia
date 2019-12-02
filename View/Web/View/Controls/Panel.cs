using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Panel : Container
	{
		private PanelDrawingType eDrawingType = PanelDrawingType.Div;
		private string sTitle = string.Empty;
		public Panel(string ID)
		{
			this.ID = ID;
		}
		public Panel(string ID, string Class)
		{
			this.ID = ID;
			this.Style.Class = Class;
		}
		public Panel(string ID, PanelDrawingType DrawingType)
		{
			this.ID = ID;
			this.eDrawingType = DrawingType;
		}
		public PanelDrawingType DrawingType {
			get { return this.eDrawingType; }
			set { this.eDrawingType = value; }
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			if (this.eDrawingType == LabelDrawingType.Div) {
				Content.Add("<div");
			} else {
				Content.Add("<span");
			}
			if (!string.IsNullOrEmpty(this.ID))
				Content.Add(" id=\"").Add(this.ID).Add("\"");
			if (!string.IsNullOrEmpty(this.Title)) {
				Content.Add(" title=\"").Add(this.Title).Add("\"");
			}
			if (this.oAttributes != null) {
				for (int i = 0; i <= this.Attributes.Count - 1; i++) {
					Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
				}
			}
			this.DrawEvents(Content);
			Content.Add(this.Style.Draw).Add(">").Add(this.Content.Value);
			base.DrawControls(Content);
			if (this.eDrawingType == LabelDrawingType.Div) {
				Content.Add("</div>");
			} else {
				Content.Add("</span>");
			}
		}
	}
	public enum PanelDrawingType : byte
	{
		Div = 1,
		Span = 2
	}
}

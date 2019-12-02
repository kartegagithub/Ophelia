using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Label : DataControl
	{
		private LabelDrawingType eDrawingType = LabelDrawingType.Div;
		public Label(string MemberName, string Value = "")
		{
			this.ID = MemberName.Replace(".", "_");
			this.Value = Value;
		}
		public LabelDrawingType DrawingType {
			get { return this.eDrawingType; }
			set { this.eDrawingType = value; }
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
			if (!string.IsNullOrEmpty(this.Title))
				Content.Add(" title=\"").Add(this.Title).Add("\"");
			if (!string.IsNullOrEmpty(this.Style.Class))
				Content.Add(" class=\"").Add(this.Style.Class).Add("\"");
			if (this.oAttributes != null) {
				for (int i = 0; i <= this.Attributes.Count - 1; i++) {
					Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
				}
			}

			this.DrawEvents(Content);
			Content.Add(this.Style.Draw + ">" + this.Value);
			if (this.eDrawingType == LabelDrawingType.Div) {
				Content.Add("</div>");
			} else {
				Content.Add("</span>");
			}
		}
	}
	public enum LabelDrawingType : byte
	{
		Div = 1,
		Span = 2
	}
}

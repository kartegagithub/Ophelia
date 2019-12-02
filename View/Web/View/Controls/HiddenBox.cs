using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class HiddenBox : InputDataControl
	{
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			Content.Clear();
			Content.Add("<input ");
			if (!string.IsNullOrEmpty(this.ID)) {
				Content.Add(" id=\"" + this.ID + "\"");
				Content.Add(" name=\"" + this.ID + "\"");
			}
			this.DrawEvents(Content);
			if (!string.IsNullOrEmpty(this.Value)) {
				Content.Add(" value=\"" + this.Value + "\"");
			}
			Content.Add(" type=\"hidden\" >");
		}
		public HiddenBox(string MemberName)
		{
			this.ID = MemberName;
		}
		public HiddenBox(string MemberName, string Value) : this(MemberName)
		{
			this.Value = Value;
		}
	}
}

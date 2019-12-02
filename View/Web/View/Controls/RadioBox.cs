using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class RadioBox : InputDataControl
	{
		private bool bChecked;
		private string sSortOrder = string.Empty;
		public bool Checked {
			get { return this.bChecked; }
			set { this.bChecked = value; }
		}
		public string SortOrder {
			get { return this.sSortOrder; }
			set { this.sSortOrder = value; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			Content.Clear();
			Content.Add("<input name=\"" + this.Name + "\" id=\"" + this.ID + "\" type=\"radio\" ");
			Content.Add(this.Style.Draw);
			Content.Add(" value=\"" + this.Value + "\"");
			if (this.Checked) {
				Content.Add("checked=\"checked\"");
			}
			if (this.SortOrder != string.Empty) {
				Content.Add("sortorder=\"" + this.SortOrder + "\"");
			}
			if (!string.IsNullOrEmpty(this.Title))
				Content.Add(" title=\"" + this.Title + "\"");
			if (!string.IsNullOrEmpty(this.OnClickEvent)) {
				Content.Add(" onclick=\"" + this.OnClickEvent + ";\"");
			}
			if (!string.IsNullOrEmpty(this.OnChangeEvent)) {
				Content.Add(" onchange=\"" + this.OnChangeEvent + ";\"");
			}
			if (this.ReadOnly) {
				Content.Add(" disabled=\"true\" ");
			}
			Content.Add(" >");
		}
		public RadioBox(string MemberName)
		{
			this.ID = MemberName;
		}
		public RadioBox(string MemberName, string ControlName) : this(MemberName)
		{
			this.Name = ControlName;
		}
	}
}

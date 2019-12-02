using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class TextBoxText : TextBox
	{
		protected override void OnInputDrawn(Content Content)
		{
			if (this.ReadOnly) {
				this.Style.Borders.Width = 0;
				Content.Add(" readonly=\"true\"");
			}
		}
		public TextBoxText(string MemberName) : base(MemberName)
		{
		}
		public TextBoxText(string MemberName, string Value) : this(MemberName)
		{
			this.Value = Value;
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class NumberBoxText : NumberBox
	{
		protected override void OnInputDrawn(Content Content)
		{
			if (this.ReadOnly) {
				this.Style.Borders.Width = 0;
				Content.Add(" readonly=\"true\"");
			}
		}
		public NumberBoxText(string MemberName) : base(MemberName)
		{
		}
		public NumberBoxText(string MemberName, int Value) : this(MemberName)
		{
			this.Value = Value;
		}
	}
}

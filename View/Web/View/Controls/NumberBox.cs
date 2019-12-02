using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class NumberBox : TextBox
	{
		public override void OnBeforeDraw(Content Content)
		{
			if (!this.Style.Class.Contains("NumberBoxClass")) {
				this.Style.Class = "NumberBoxClass" + this.Style.Class;
			}
			base.OnBeforeDraw(Content);
		}
		protected override string GetHtml5Type()
		{
			return "number";
		}
		public NumberBox(string MemberName, string Message = "Sayısal değer giriniz.") : base(MemberName)
		{
			this.Validators.AddNumericValidator(Message);
		}
		public NumberBox(string MemberName, decimal Value) : this(MemberName)
		{
			this.Value = Value;
			this.Style.HorizontalAlignment = HorizontalAlignment.Right;
		}
	}
}

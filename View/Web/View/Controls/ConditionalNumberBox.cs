using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class ConditionalNumberBox : ConditionalTextBox
	{
		public override void OnBeforeDraw(Content Content)
		{
			if (!this.Style.Class.Contains("NumberBoxClass")) {
				this.Style.Class = "NumberBoxClass" + this.Style.Class;
			}
			base.OnBeforeDraw(Content);
		}
		public ConditionalNumberBox(string MemberName, string Message = "Sayısal değer giriniz.") : base(MemberName)
		{
			this.Validators.AddNumericValidator(Message);
		}
		public ConditionalNumberBox(string MemberName, decimal Value) : this(MemberName)
		{
			this.Value = Value;
			this.Style.HorizontalAlignment = HorizontalAlignment.Right;
		}
	}
}

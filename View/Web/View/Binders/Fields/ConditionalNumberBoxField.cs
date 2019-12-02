using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class ConditionalNumberBoxField : ConditionalTextBoxField
	{
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.ConditionalNumberBox(this.MemberName, "Sayısal değer giriniz.");
			this.Control.Style.HorizontalAlignment = HorizontalAlignment.Right;
		}
		public override void Bind()
		{
			base.Bind();
		}
		public ConditionalNumberBoxField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

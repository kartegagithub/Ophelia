using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class InfinityNumberBoxField : NumberBoxField
	{
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.InfinityNumberBox(this.MemberName, "Sayısal değer giriniz.");
		}
		public override void Bind()
		{
			base.Bind();
		}
		public InfinityNumberBoxField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

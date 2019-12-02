using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class HiddenField : Field
	{
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.HiddenBox(this.MemberName);
		}
		public new View.Web.Controls.HiddenBox Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			this.Control.Value = this.Binding.Value;
		}
		public HiddenField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

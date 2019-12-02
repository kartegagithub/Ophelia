using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class LabelField : Field
	{
		public new View.Web.Controls.Label Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			if (this.Binding.Value != null) {
				this.Control.Value = this.Binding.Value;
			}
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.Label(this.MemberName);
		}
		public LabelField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

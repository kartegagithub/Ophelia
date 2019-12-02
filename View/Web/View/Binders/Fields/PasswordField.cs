using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Binders.Fields
{
	public class PasswordField : Field
	{
		public new View.Web.Controls.PasswordBox Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			this.Control.Text = this.Binding.Value;
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.PasswordBox(this.MemberName);
		}
		public PasswordField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

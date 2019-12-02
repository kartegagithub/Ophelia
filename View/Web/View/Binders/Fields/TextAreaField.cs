using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class TextAreaField : Field
	{
		public new View.Web.Controls.TextArea Control {
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
			this.oControl = new View.Web.Controls.TextArea(this.MemberName);
		}
		public TextAreaField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

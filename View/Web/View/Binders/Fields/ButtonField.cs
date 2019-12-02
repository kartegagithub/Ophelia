using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class ButtonField : Field
	{
		public new View.Web.Controls.Button Control {
			get { return base.Control; }
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.Button(this.MemberName);
		}
		public ButtonField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, "", MemberName)
		{
		}
	}
}

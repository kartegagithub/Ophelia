using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class CheckBoxListField : Field
	{
		public new View.Web.Controls.CheckBoxList Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			if ((this.Binding.Value != null) && Information.IsNumeric(this.Binding.Value)) {
				switch (Convert.ToInt32(this.Binding.Value)) {
					case 0:
						this.Control.Value = false;
						break;
					case 1:
						this.Control.Value = true;
						break;
				}
			}
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.CheckBoxList(this.MemberName);
		}
		public CheckBoxListField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

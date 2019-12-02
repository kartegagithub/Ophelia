using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class ComboBoxField : Field
	{
		private HttpRequest oRequest;
		public new View.Web.Controls.ComboBox Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			this.Control.SelectedItem = this.Binding.Value;
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.ComboBox(this.MemberName);
		}
		public ComboBoxField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

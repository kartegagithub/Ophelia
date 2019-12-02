using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Binders.Fields
{
	public class TextBoxField : Field
	{
		public new View.Web.Controls.TextBox Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			this.Control.Value = this.Binding.Value;
		}
		public override void BindReverse(string Value)
		{
			if (!this.Binding.Value.ToString().Equals(Value)) {
				this.Binding.Value = Value;
			}
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new TextBox(this.MemberName);
		}
		public TextBoxField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

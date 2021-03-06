using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Binders.Fields
{
	public class FileBoxField : Field
	{
		public new View.Web.Controls.FileBox Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			this.Control.Value = this.Binding.Value.Name;
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new FileBox(this.MemberName);
		}
		public FileBoxField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
			this.Header.Style.Top = 2;
		}
	}
}

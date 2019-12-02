using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class GridField : Field
	{
		public new View.Web.Controls.Grid Control {
			get { return base.Control; }
		}
		public View.Web.Base.DataGrid.ColumnCollection Columns {
			//Kolaylık olsun diye eklendi.
			get { return this.Control.Binder.Columns; }
		}
		public override void Bind()
		{
			base.Bind();
			this.Control.Collection = this.Binding.Value;
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.Grid(this.MemberName);
		}
		public GridField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

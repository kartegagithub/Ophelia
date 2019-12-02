using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class NumberBoxTextColumn : NumberBoxColumn
	{
		public new Ophelia.Web.View.Controls.NumberBoxText DataControl {
			get { return base.DataControl; }
		}
		public override Cell CreateCell(Row Row)
		{
			NumberBoxTextCell NumberBoxTextCell = new NumberBoxTextCell(Row, this);
			NumberBoxTextCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(NumberBoxTextCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new TextBox(this.MemberName);
			this.oDataControl.Style.Width = this.Style.Width;
		}
		public NumberBoxTextColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
		}
	}
}

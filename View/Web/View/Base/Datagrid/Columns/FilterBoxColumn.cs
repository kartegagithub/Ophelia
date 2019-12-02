using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class FilterBoxColumn : Column
	{
		private object oDataSource;
		public new Ophelia.Web.View.Controls.FilterBox DataControl {
			get { return base.DataControl; }
		}
		public object DataSource {
			get { return this.oDataSource; }
			set {
				this.oDataSource = value;
				if (this.DataControl.DataSource == null || (!object.ReferenceEquals(this.DataControl.DataSource, value))) {
					this.DataControl.DataSource = value;
				}
			}
		}
		public override Cell CreateCell(Row Row)
		{
			FilterBoxCell ComboBoxCell = new FilterBoxCell(Row, this);
			ComboBoxCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(ComboBoxCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new FilterBox(this.MemberName);
		}
		public FilterBoxColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
		}
	}
}


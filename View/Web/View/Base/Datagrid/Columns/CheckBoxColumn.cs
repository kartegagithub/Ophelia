using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class CheckBoxColumn : Column
	{
		private bool bAllowSelectAll = false;
		private Ophelia.Web.View.Controls.CheckBox oSelectAllControl;
		public new Ophelia.Web.View.Controls.CheckBox DataControl {
			get { return base.DataControl; }
		}
		public new Ophelia.Web.View.Controls.CheckBox SelectAllControl {
			get { return this.oSelectAllControl; }
		}
		public bool AllowSelectAll {
			get { return this.bAllowSelectAll; }
			set { this.bAllowSelectAll = value; }
		}
		public override Cell CreateCell(Row Row)
		{
			CheckBoxCell CheckBoxCell = new CheckBoxCell(Row, this);
			CheckBoxCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(CheckBoxCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new Ophelia.Web.View.Controls.CheckBox(this.MemberName);
			this.oSelectAllControl = new Ophelia.Web.View.Controls.CheckBox(this.MemberName + "_SelectAll");
			this.oSelectAllControl.OnChangeEvent = "SelectAll_ValueChanged(this);";
			this.Style.HorizontalAlignment = HorizontalAlignment.Left;
		}
		public CheckBoxColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
			this.Style.HorizontalAlignment = HorizontalAlignment.Center;
			this.CellStyle.HorizontalAlignment = HorizontalAlignment.Center;
		}
	}
}

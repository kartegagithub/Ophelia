using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class FileBoxColumn : Column
	{
		public new Ophelia.Web.View.Controls.FileBoxWithAjax DataControl {
			get { return base.DataControl; }
		}
		public override Cell CreateCell(Row Row)
		{
			FileBoxCell FileBoxCell = new FileBoxCell(Row, this);
			FileBoxCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(FileBoxCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new FileBoxWithAjax(this.MemberName);
			this.oDataControl.Style.Width = this.Style.Width;
		}
		public FileBoxColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
		}
	}
}

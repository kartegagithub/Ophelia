using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.Structure.Columns;
using Ophelia.Web.View.Controls.Structure.Rows;
namespace Ophelia.Web.View.Controls.Structure.Cells
{
	public class RowCellCollection : Cells.CellCollection
	{
		private Row oRow;
		private Row Row {
			get { return this.oRow; }
		}
		private object CreateCells()
		{
			Column Column = default(Column);
			foreach ( Column in this.Row.Collection.Structure.Columns) {
				this.Add(Column.AddCell(this.Row));
			}
			return this;
		}
		public RowCellCollection(Row Row)
		{
			this.oRow = Row;
			this.CreateCells();
		}
	}
}

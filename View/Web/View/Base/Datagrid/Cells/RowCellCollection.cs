using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class RowCellCollection : CellCollection
	{
		private Row oRow;
		public Row Row {
			get { return this.oRow; }
		}
		private object CreateCells()
		{
			Column Column = default(Column);
			foreach ( Column in this.Row.Collection.DataGrid.Columns) {
				this.Add(Column.CreateCell(this.Row));
			}
			return this;
		}
		public Cell this[string ColumnMemberName] {
			get {
				for (int i = 0; i <= this.Row.Cells.Count - 1; i++) {
					if (this.Row.Cells(i).Column.MemberName == ColumnMemberName) {
						return this.Row.Cells(i);
					}
				}
				return null;
			}
		}
		internal RowCellCollection(Row Row) : base(Row.DataGrid)
		{
			this.oRow = Row;
			this.CreateCells();
		}
	}
}

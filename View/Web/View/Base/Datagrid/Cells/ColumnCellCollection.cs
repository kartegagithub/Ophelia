using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ColumnCellCollection : CellCollection
	{
		private Column oColumn;
		internal Column Column {
			get { return this.oColumn; }
		}
		internal ColumnCellCollection(Column Column) : base(Column.DataGrid)
		{
			this.oColumn = Column;
		}
	}
}

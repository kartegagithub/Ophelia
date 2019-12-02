using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.Structure.Columns;
namespace Ophelia.Web.View.Controls.Structure.Cells
{
	public class ColumnCellCollection : Cells.CellCollection
	{
		private Column oColumn;
		private Column Columns {
			get { return this.oColumn; }
		}
		public ColumnCellCollection(Column Column)
		{
			this.oColumn = Column;
		}
	}
}

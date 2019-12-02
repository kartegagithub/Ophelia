using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.DataGrid
{
	public class DataGridStyle : Style
	{
		private DataGrid oDataGrid;
		private int nCellSpacing = 2;
		private int nCellPadding = 2;
		public int CellSpacing {
			get { return this.nCellSpacing; }
			set { this.nCellSpacing = value; }
		}
		public int CellPadding {
			get { return this.nCellPadding; }
			set { this.nCellPadding = value; }
		}
		public DataGrid DataGrid {
			get { return this.oDataGrid; }
		}
		public DataGridStyle(DataGrid DataGrid)
		{
			this.oDataGrid = DataGrid;
			this.CellSpacing = 2;
			this.CellPadding = 2;
			this.BackgroundColor = "#dcddde";
			this.Font.Color = "#000000";
		}
	}
}

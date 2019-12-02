using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	[Serializable()]
	public class ColumnEventArgs : Ophelia.View.Base.Controls.CancelEventArgs
	{
		private Column oColumn;
		public Column Column {
			get { return this.oColumn; }
		}
		public ColumnEventArgs(Column Column)
		{
			this.oColumn = Column;
		}
	}
}

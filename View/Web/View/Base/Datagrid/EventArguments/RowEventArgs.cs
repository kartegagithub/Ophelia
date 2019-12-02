using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	[Serializable()]
	public class RowEventArgs : Ophelia.View.Base.Controls.CancelEventArgs
	{
		private Row oRow;
		public Row Row {
			get { return this.oRow; }
		}
		public RowEventArgs(Row Row)
		{
			this.oRow = Row;
		}
	}
}

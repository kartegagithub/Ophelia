using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class FilterBoxCell : Cell
	{
		public new Ophelia.Web.View.Controls.FilterBox DataControl {
			get { return base.DataControl; }
		}
		public FilterBoxCell(Row Row, FilterBoxColumn Column) : base(Row, Column)
		{
		}
	}
}

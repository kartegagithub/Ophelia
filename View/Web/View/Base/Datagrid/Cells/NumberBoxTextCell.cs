using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class NumberBoxTextCell : Cell
	{
		public new Ophelia.Web.View.Controls.NumberBoxText DataControl {
			get { return base.DataControl; }
		}
		public NumberBoxTextCell(Row Row, Column Column) : base(Row, Column)
		{
		}
	}
}

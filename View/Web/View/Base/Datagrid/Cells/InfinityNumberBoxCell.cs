using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class InfinityNumberBoxCell : NumberBoxCell
	{
		public new Ophelia.Web.View.Controls.InfinityNumberBox DataControl {
			get { return base.DataControl; }
		}
		public InfinityNumberBoxCell(Row Row, NumberBoxColumn Column) : base(Row, Column)
		{
			this.DataControl.Style.HorizontalAlignment = HorizontalAlignment.Right;
		}
	}
}

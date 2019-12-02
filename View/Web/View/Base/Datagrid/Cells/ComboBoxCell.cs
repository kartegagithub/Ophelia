using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ComboBoxCell : Cell
	{
		public new Ophelia.Web.View.Controls.SelectBox DataControl {
			get { return base.DataControl; }
		}
		public ComboBoxCell(Row Row, ComboBoxColumn Column) : base(Row, Column)
		{
		}
	}
}

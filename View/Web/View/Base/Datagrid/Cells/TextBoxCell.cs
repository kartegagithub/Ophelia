using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class TextBoxCell : Cell
	{
		public new Ophelia.Web.View.Controls.TextBox DataControl {
			get { return base.DataControl; }
		}
		public TextBoxCell(Row Row, Column Column) : base(Row, Column)
		{
		}
	}
}

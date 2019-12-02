using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class TextBoxTextCell : Cell
	{
		public new Ophelia.Web.View.Controls.TextBoxText DataControl {
			get { return base.DataControl; }
		}
		public TextBoxTextCell(Row Row, Column Column) : base(Row, Column)
		{
		}
	}
}

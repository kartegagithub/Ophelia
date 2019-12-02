using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class LinkCell : Cell
	{
		public new Ophelia.Web.View.Controls.Link DataControl {
			get { return base.DataControl; }
		}
		public LinkCell(Row Row, LinkColumn Column) : base(Row, Column)
		{
		}
	}
}

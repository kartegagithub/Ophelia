using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class FileBoxCell : Cell
	{
		public new Ophelia.Web.View.Controls.FileBoxWithAjax DataControl {
			get { return base.DataControl; }
		}
		public FileBoxCell(Row Row, Column Column) : base(Row, Column)
		{
		}
	}
}

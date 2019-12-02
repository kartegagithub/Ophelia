using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class CheckBoxCell : Cell
	{
		public new Ophelia.Web.View.Controls.CheckBox DataControl {
			get { return base.DataControl; }
		}
		public bool Checked {
			get { return Convert.ToBoolean(this.Value); }
			set {
				switch (value) {
					case true:
						this.Value = 1;
						break;
					case false:
						this.Value = 0;
						break;
				}
			}
		}
		public CheckBoxCell(Row Row, CheckBoxColumn Column) : base(Row, Column)
		{
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class NumberBoxCell : TextBoxCell
	{
		private int nDecimalDigits = -1;
		public new NumberBoxColumn Column {
			get { return base.Column; }
		}
		public int DecimalDigits {
			get { return this.nDecimalDigits; }
			set { this.nDecimalDigits = value; }
		}
		public override string Text {
			get {
				if (this.Value != null && (!Ophelia.Application.Base.IsText(this.Value) || !string.IsNullOrEmpty(this.Value.ToString))) {
					if (!this.Column.ShowAlwaysDecimalPart && (Int64)this.Value == this.Value) {
						return (Int64)this.Value;
					}
					return Strings.FormatNumber(Value, this.Column.DecimalDigits, TriState.True, TriState.False, TriState.True);
				} else {
					return base.Text;
				}
			}
			set { base.Text = value; }
		}
		public new Ophelia.Web.View.Controls.NumberBox DataControl {
			get { return base.DataControl; }
		}
		public NumberBoxCell(Row Row, NumberBoxColumn Column) : base(Row, Column)
		{
			this.DataControl.Style.HorizontalAlignment = HorizontalAlignment.Right;
		}
	}
}

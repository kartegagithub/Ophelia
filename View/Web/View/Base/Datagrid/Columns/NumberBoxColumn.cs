using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class NumberBoxColumn : Column
	{
		private int nDecimalDigits = 2;
		private bool bShowAlwaysDecimalPart = false;
		private bool bCalculateBySum = true;
		private bool bHasTotals = true;
		private decimal dTotalValue = 0;
		private Style oTotalCellStyle;
		public bool HasTotals {
			get { return this.bHasTotals; }
			set { this.bHasTotals = value; }
		}
		public bool ShowAlwaysDecimalPart {
			get { return this.bShowAlwaysDecimalPart; }
			set { this.bShowAlwaysDecimalPart = value; }
		}
		public Style TotalCellStyle {
			get {
				if (this.oTotalCellStyle == null) {
					this.oTotalCellStyle = new Style();
				}
				return this.oTotalCellStyle;
			}
		}
		public decimal TotalValue {
			get { return this.dTotalValue; }
		}
		public void FormatValue(int DecimalDigits, string Suffix, bool ShowAlwaysDecimalPart = true)
		{
			this.DecimalDigits = DecimalDigits;
			this.ShowAlwaysDecimalPart = ShowAlwaysDecimalPart;
			this.Suffix = Suffix;
		}
		public void AddValueToTotalValue(decimal Value, bool ClearTotal = false)
		{
			if (ClearTotal) {
				this.dTotalValue = Value;
			} else {
				this.dTotalValue += Value;
			}
		}
		public new Ophelia.Web.View.Controls.NumberBox DataControl {
			get { return base.DataControl; }
		}
		public bool CalculateBySum {
			get { return this.bCalculateBySum; }
			set { this.bCalculateBySum = value; }
		}
		public int DecimalDigits {
			get { return this.nDecimalDigits; }
			set {
				this.nDecimalDigits = value;
				int i = 0;
				for (i = 0; i <= this.Cells.Count - 1; i++) {
					((NumberBoxCell)this.Cells(i)).DecimalDigits = value;
				}
			}
		}
		public override Cell CreateCell(Row Row)
		{
			NumberBoxCell NumberBoxCell = new NumberBoxCell(Row, this);
			NumberBoxCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(NumberBoxCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new NumberBox(this.MemberName, "Sayısal değer giriniz.");
			this.oDataControl.Style.Width = this.Style.Width;
		}
		public NumberBoxColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
			this.Style.HorizontalAlignment = HorizontalAlignment.Right;
			this.CellStyle.HorizontalAlignment = HorizontalAlignment.Right;
		}
	}
}

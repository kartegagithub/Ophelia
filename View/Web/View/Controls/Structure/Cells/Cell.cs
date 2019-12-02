using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.Structure.Rows;
using Ophelia.Web.View.Controls.Structure.Columns;
namespace Ophelia.Web.View.Controls.Structure.Cells
{
	public class Cell : Container
	{
		private Column oColumn;
		private Row oRow;
		private int nColumnSpan = 1;
		private int nRowSpan = 1;
		private CellCollection oSpannedCells;
		private Cell oDependentCell;
		private VAlignEnum eVAlign = VAlignEnum.None;
		private AlignEnum eAlign = AlignEnum.None;
		public Column Column {
			get { return this.oColumn; }
		}
		public Row Row {
			get { return this.oRow; }
		}
		public AlignEnum Align {
			get { return this.eAlign; }
			set { this.eAlign = value; }
		}
		public VAlignEnum VAlign {
			get { return this.eVAlign; }
			set {
				if (this.ColumnSpan != value) {
					this.eVAlign = value;
				}
			}
		}
		public override string ID {
			get { return this.Row.Structure.ID + "_Cell_" + Row.Index + "_" + Column.Index; }

			set { }
		}
		public int ColumnSpan {
			get { return this.nColumnSpan; }
			set {
				if (this.ColumnSpan != value) {
					this.nColumnSpan = value;
					this.ArrangeSpannedCells();
				}
			}
		}
		public int RowSpan {
			get { return this.nRowSpan; }
			set {
				if (this.RowSpan != value) {
					this.nRowSpan = value;
					this.ArrangeSpannedCells();
				}
			}
		}
		public Cell DependentCell {
			get { return this.oDependentCell; }
			set {
				if ((!object.ReferenceEquals(value, this))) {
					if (value != null) {
						this.ClearSpannedCells();
					}
					this.oDependentCell = value;
				}
			}
		}
		public CellCollection SpannedCells {
			get {
				if (this.oSpannedCells == null) {
					oSpannedCells = new CellCollection();
				}
				return this.oSpannedCells;
			}
		}
		private void ClearSpannedCells()
		{
			for (int i = 0; i <= this.SpannedCells.Count - 1; i++) {
				this.SpannedCells(i, true).DependentCell = null;
			}
			this.SpannedCells.Reset();
		}
		public void ArrangeSpannedCells()
		{
			this.ClearSpannedCells();
			ArrayList ColumnSpanIndexes = new ArrayList();
			if (this.ColumnSpan > 1) {
				for (int i = 0; i <= this.Row.Cells.Count - 1; i++) {
					if (object.ReferenceEquals(this.Row.Cells(i), this)) {
						for (int j = 1; j <= this.ColumnSpan - 1; j++) {
							if (this.Row.Cells.Count > i + j) {
								ColumnSpanIndexes.Add(i + j);
								this.Row.Cells(i + j, true).DependentCell = this;
								this.oSpannedCells.Add(this.Row.Cells(i + j, true));
							}
						}
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
			if (this.RowSpan > 1) {
				for (int i = 0; i <= this.Column.Cells.Count - 1; i++) {
					if (object.ReferenceEquals(this.Column.Cells(i), this)) {
						Row Row = this.Row;
						for (int j = 1; j <= this.RowSpan - 1; j++) {
							Row = Row.NextRow;
							if (Row == null)
								break; // TODO: might not be correct. Was : Exit For
							for (int m = 0; m <= ColumnSpanIndexes.Count - 1; m++) {
								Row.Cells(ColumnSpanIndexes[m], true).DependentCell = this;
								this.oSpannedCells.Add(Row.Cells(ColumnSpanIndexes[m], true));
							}
							if (this.Column.Cells.Count > i + j) {
								this.Column.Cells(i + j, true).DependentCell = this;
								this.oSpannedCells.Add(this.Column.Cells(i + j, true));
							}
						}
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			base.OnBeforeDraw(Content);
			if (!string.IsNullOrEmpty(this.Content.Value)) {
				Content.Add(this.Content.Value);
			} else if (this.Controls.Count == 0) {
				if (this.Row.Structure.Technique == LayoutTechnique.Tables) {
					Content.Add("&nbsp;");
				}
			}
		}
		public Cell(Row Row, Column Column)
		{
			this.oColumn = Column;
			this.oRow = Row;
			if (Row.Structure.Container == null) {
				this.Container = Row.Structure.Container;
			} else {
				this.Container = Row.Structure.Container;
			}
			this.ParentControl = Row.Structure;
		}
		public enum VAlignEnum
		{
			None = 0,
			Baseline = 1,
			Bottom = 2,
			Middle = 3,
			Top = 4
		}
		public enum AlignEnum
		{
			None = 0,
			Left = 1,
			Center = 2,
			Right = 3
		}
	}
}

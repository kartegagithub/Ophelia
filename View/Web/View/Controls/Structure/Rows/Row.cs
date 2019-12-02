using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.Structure.Cells;
namespace Ophelia.Web.View.Controls.Structure.Rows
{
	public class Row
	{
		private RowCellCollection oCells;
		private RowCollection oCollection;
		private Style oStyle = new Style();
		private int nIndex;
		private Style oCellStyle = new Style();
		public int Index {
			get { return this.nIndex; }
		}
		public Style Style {
			get { return this.oStyle; }
		}
		public Style CellStyle {
			get { return this.oCellStyle; }
		}
		public void SetStyle(Style Style)
		{
			this.oStyle = Style;
		}
		public void SetCellStyle(Style Style)
		{
			this.oCellStyle = Style;
		}
		public RowCollection Collection {
			get { return this.oCollection; }
		}
		public Structure Structure {
			get { return this.Collection.Structure; }
		}
		public Cells.Cell this[int Index] {
			get { return this.Cells(Index); }
		}
		public RowCellCollection Cells {
			get { return this.oCells; }
		}
		public Row NextRow {
			get {
				for (int i = 0; i <= this.Collection.Count - 1; i++) {
					if (object.ReferenceEquals(this.Collection(i), this)) {
						if (this.Collection.Count > i + 1) {
							return this.Collection(i + 1);
						} else {
							break; // TODO: might not be correct. Was : Exit For
						}
					}
				}
				return null;
			}
		}
		public Row(RowCollection Collection)
		{
			this.oCollection = Collection;
			this.oCells = new RowCellCollection(this);
			this.nIndex = this.Collection.Count;
		}
	}
}

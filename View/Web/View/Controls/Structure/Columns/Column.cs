using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.Structure.Rows;
using Ophelia.Web.View.Controls.Structure.Cells;
namespace Ophelia.Web.View.Controls.Structure.Columns
{
	public class Column
	{
		private Structure oStructure;
		private ColumnCellCollection oCells;
		private string sName;
		private ColumnCollection oCollection;
		private int nIndex;
		private Style oStyle = new Style();
		public Style Style {
			get { return this.oStyle; }
		}
		public void SetStyle(Style Style)
		{
			this.oStyle = Style;
		}
		public int Index {
			get { return this.nIndex; }
		}
		public ColumnCollection Collection {
			get { return this.oCollection; }
		}
		public Structure Structure {
			get { return this.oStructure; }
		}
		public ColumnCellCollection Cells {
			get {
				if (this.oCells == null) {
					this.oCells = new ColumnCellCollection(this);
				}
				return this.oCells;
			}
		}
		public Cell this[int Row] {
			get { return this.Cells(Row); }
		}
		public Column NextColumn {
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
		protected virtual Cell CreateCell(Row Row)
		{
			return new Cell(Row, this);
		}
		public Cell AddCell(Row Row)
		{
			return this.Cells.Add(this.CreateCell(Row));
		}
		public Column(ColumnCollection Collection)
		{
			this.oCollection = Collection;
			this.nIndex = Collection.Count;
			this.oStructure = Collection.Structure;
		}
	}
}

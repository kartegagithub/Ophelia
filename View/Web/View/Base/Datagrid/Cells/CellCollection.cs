using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class CellCollection : Ophelia.Application.Base.CollectionBase
	{
		private DataGrid oDataGrid;
		private bool bReadOnly = true;
		internal DataGrid DataGrid {
			get { return this.oDataGrid; }
		}
		public new Cell this[int Index] {
			get {
				if (Index > -1 && Index < this.List.Count) {
					return this.List(Index);
				}
				return null;
			}
		}
		public Cell FirstCell {
			get {
				if (this.Count > 0) {
					return this[0];
				}
				return null;
			}
		}
		public Cell LastCell {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1];
				}
				return null;
			}
		}
		internal Cell Add(Cell Cell)
		{
			this.List.Add(Cell);
			return Cell;
		}
		public bool Remove(Cell Cell)
		{
			try {
				this.List.Remove(Cell);
				return true;
			} catch {
				return false;
			}
		}
		public bool ReadOnly {
			get { return this.bReadOnly; }
			set {
				this.bReadOnly = value;
				Cell Cell = default(Cell);
				foreach ( Cell in this) {
					Cell.ReadOnly = value;
				}
			}
		}
		internal CellCollection(DataGrid DataGrid)
		{
			this.oDataGrid = DataGrid;
		}
	}
}

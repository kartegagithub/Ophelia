using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class AnyRowCollection : Ophelia.Application.Base.CollectionBase
	{
		private DataGrid oDataGrid;
		public DataGrid DataGrid {
			get { return this.oDataGrid; }
		}
		public new Row this[int Index] {
			get { return base.Item(Index); }
			set { base.Item(Index) = value; }
		}
		public Row FirstRow {
			get {
				if (this.Count > 0) {
					return this[0];
				}
				return null;
			}
		}
		public Row LastRow {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1];
				}
				return null;
			}
		}
		public virtual Row Add(Row Row)
		{
			this.List.Add(Row);
			return Row;
		}
		public virtual bool Remove(Row Row)
		{
			if (this.List.Contains(Row))
				this.List.Remove(Row);
			return true;
		}
		public AnyRowCollection(DataGrid DataGrid)
		{
			this.oDataGrid = DataGrid;
		}
	}
}

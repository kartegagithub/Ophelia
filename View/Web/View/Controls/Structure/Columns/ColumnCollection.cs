using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Structure.Columns
{
	public class ColumnCollection : Ophelia.Application.Base.CollectionBase
	{
		private Structure oStructure;
		public Structure Structure {
			get { return this.oStructure; }
		}
		public new Column this[int index] {
			get { return base.Item(index); }
		}
		public Column LastColumn {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1];
				}
				return null;
			}
		}
		public Column Add(int Width)
		{
			Column Column = this.Add();
			Column.Style.Width = Width;
			return Column;
		}
		public Column Add()
		{
			Column Column = new Column(this);
			this.List.Add(Column);
			if (Structure.Rows.Count > 0) {
				for (int i = 0; i <= Structure.Rows.Count - 1; i++) {
					Structure.Rows(i).Cells.Add(Column.AddCell(Structure.Rows(i)));
				}
			}
			return Column;
		}
		public Column Add(Column Column)
		{
			this.List.Add(Column);
			return Column;
		}
		public ArrayList AddColumns(int Count)
		{
			ArrayList ArrayList = new ArrayList();
			for (int i = 0; i <= Count - 1; i++) {
				Column Column = this.Add();
				ArrayList.Add(Column);
			}
			return ArrayList;
		}
		public ColumnCollection(Structure Structure)
		{
			this.oStructure = Structure;
		}
	}
}

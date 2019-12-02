using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Structure.Rows
{
	public class RowCollection : Ophelia.Application.Base.CollectionBase
	{
		private Structure oStructure;
		public Structure Structure {
			get { return this.oStructure; }
		}
		public new Row this[int index] {
			get { return base.Item(index); }
		}
		public Row LastRow {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1];
				}
				return null;
			}
		}
		public Row Add()
		{
			Row Row = new Row(this);
			this.List.Add(Row);
			return Row;
		}
		public void AddRows(int Count)
		{
			for (int i = 0; i <= Count - 1; i++) {
				this.Add();
			}
		}
		public RowCollection(Structure Structure)
		{
			this.oStructure = Structure;
		}
	}
}

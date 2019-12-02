using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Structure.Cells
{
	public class CellCollection : Ophelia.Application.Base.CollectionBase
	{
		public new Cell this[int index, bool IgnoreDependency = false] {
			get {
				if (!IgnoreDependency && base.Item(index) != null && ((Cell)base.Item(index)).DependentCell != null) {
					return ((Cell)base.Item(index)).DependentCell;
				}
				return base.Item(index);
			}
		}
		public bool Remove(Cell Cell)
		{
			for (int i = 0; i <= Count - 1; i++) {
				if (object.ReferenceEquals(this[i], Cell)) {
					this.List.Remove(Cell);
				}
				return true;
			}
			return false;
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
		public Cell Add(Cell Cell)
		{
			this.List.Add(Cell);
			return Cell;
		}
		public bool Reset()
		{
			this.List.Clear();
		}
	}
}

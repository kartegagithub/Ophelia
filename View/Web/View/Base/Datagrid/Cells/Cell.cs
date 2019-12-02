using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class Cell
	{
		private Column oColumn;
		private Row oRow;
		private View.Base.Binders.Binding oBinding;
		private bool bReadonly = true;
		private Style oStyle;
		public Controls.DataControl DataControl {
			get { return this.Column.DataControl; }
		}
		public Column Column {
			get { return this.oColumn; }
		}
		public string ID {
			get { return this.Row.ID + "_" + this.Column.ID; }
		}
		public View.Base.Binders.Binding Binding {
			get { return this.oBinding; }
		}
		public Row Row {
			get { return this.oRow; }
		}
		public Style Style {
			get {
				if (this.oStyle == null)
					this.oStyle = new Style();
				return this.oStyle;
			}
		}
		internal Datagrid DataGrid {
			get { return this.Column.DataGrid; }
		}
		public virtual object Value {
			get { return this.Binding.Value; }
			set {
				if ((Functions.IsNothing(value) && !Functions.IsNothing(this.Binding.Value)) || (Ophelia.Application.Base.Functions.IsEntity(value.GetType()) && (!object.ReferenceEquals(value, this.Binding.Value))) || (!Ophelia.Application.Base.Functions.IsEntity(value.GetType()) && (Functions.IsNothing(this.Binding.Value) || value != this.Binding.Value))) {
					this.Binding.Value = value;
				}
			}
		}
		public bool ReadOnly {
			get { return this.bReadonly; }
			set { this.bReadonly = value; }
		}
		public virtual string Text {
			get {
				if (!Functions.IsNothing(this.Value)) {
					return this.Value.ToString();
				} else {
					return "&nbsp;";
				}
			}
			set {
				if (this.Text != value) {
					this.Value = value;
				}
			}
		}
		private void CreateBinding(Row Row, Column Column)
		{
			this.oBinding = new View.Base.Binders.Binding();
			this.Binding.Item = Row.Item;
			this.Binding.MemberName = Column.MemberName;
		}
		public Cell(Row Row, Column Column)
		{
			this.oRow = Row;
			this.oColumn = Column;
			this.CreateBinding(Row, Column);
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class AnyColumnCollection : Ophelia.Application.Base.CollectionBase
	{
		private DataGrid oDataGrid;
		private Style oStyle;
		private Style oCellStyle;
		private Style oAlternativeCellStyle;
		public Style Style {
			get { return this.oStyle; }
		}
		public Style CellStyle {
			get { return this.oCellStyle; }
		}
		public Style AlternativeCellStyle {
			get { return this.oAlternativeCellStyle; }
		}
		internal DataGrid DataGrid {
			get { return this.oDataGrid; }
		}
		public Column FirstColumn {
			get {
				if (this.Count > 0) {
					return this[0];
				}
				return null;
			}
		}
		public Column LastColumn {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1];
				}
				return null;
			}
		}
		public virtual bool Remove()
		{
			int i = 0;
			for (i = this.Count - 1; i >= 0; i += -1) {
				this[i].Remove();
			}
			this.Clear();
			return true;
		}
		public new Column this[int Index] {
			get {
				if (Index > -1 && Index < this.List.Count) {
					return this.List(Index);
				}
				return null;
			}
			set {
				if (Index > -1 && Index < this.List.Count) {
					this.List(Index) = value;
				}
			}
		}
		public new Column this[string MemberName] {
			get {
				Column Column = default(Column);
				foreach ( Column in this) {
					if (Column.MemberName == MemberName) {
						return Column;
					}
				}
				return null;
			}
		}
		public Column ItemByName {
			get {
				Column Column = default(Column);
				foreach ( Column in this) {
					if (Column.Name == Name) {
						return Column;
					}
				}
				return null;
			}
		}
		public Column ItemByMemberNameToBeDrawn {
			get {
				Column Column = default(Column);
				foreach ( Column in this) {
					if (Column.MemberNameToBeDrawn == MemberNameToBeDrawn) {
						return Column;
					}
				}
				return null;
			}
		}
		public virtual bool Remove(string MemberName)
		{
			Column Column = this[MemberName];
			if ((Column != null)) {
				this.Remove(Column);
				return true;
			}
			return false;
		}
		public virtual bool Remove(Column Column)
		{
			this.List.Remove(Column);
			return true;
		}
		public virtual Column Add(Column Column)
		{
			this.List.Add(Column);
			this.oStyle = Column.Collection.Style;
			return Column;
		}
		protected void ConfigureStyle()
		{
			this.Style.VerticalAlignment = VerticalAlignment.Middle;
			this.Style.Font.Weight = Forms.FontWeight.Bold;
			this.Style.HorizontalAlignment = SectionAlignment.Left;
			this.Style.BackgroundColor = "#F1F2F2";
			this.Style.Font.Color = "black";
		}
		internal AnyColumnCollection(DataGrid DataGrid, Style Style = null, Style CellStyle = null, Style AlternativeCellStyle = null)
		{
			this.oDataGrid = DataGrid;
			this.oStyle = (Style == null ? new Style() : Style);
			this.oCellStyle = (CellStyle == null ? new Style() : CellStyle);
			this.oAlternativeCellStyle = (AlternativeCellStyle == null ? new Style() : AlternativeCellStyle);
			this.ConfigureStyle();
		}
	}
}

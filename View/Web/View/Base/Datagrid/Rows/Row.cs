using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class Row
	{
		private RowCollection oCollection;
		private RowCellCollection oCells;
		private object oItem;
		private int nIndex = 0;
		private bool bSelected = false;
		private bool bReadOnly = false;
		private bool bCanBeSelected = true;
		private string sSelectedBehavior = "";
		private string sEditLinkUrl = "";
		private string sExtraData = "";
		private Style oStyle;
		private string sOnRowClick = string.Empty;
		private ContextMenu oContextMenu;
		public ContextMenu ContextMenu {
			get {
				if (this.oContextMenu == null) {
					this.oContextMenu = new ContextMenu(this);
				}
				return this.oContextMenu;
			}
		}

		public string ExtraData {
			get { return this.sExtraData; }
		}
		public void SetExtraData(string Data)
		{
			this.sExtraData = Data;
		}
		public Style Style {
			get {
				if (this.oStyle == null)
					this.oStyle = new Style();
				return this.oStyle;
			}
		}
		public int Index {
			get { return this.nIndex; }
		}
		private string sDeleteLinkUrl = "";
		public string DeleteLinkUrl {
			get {
				if (string.IsNullOrEmpty(sDeleteLinkUrl)) {
					string Url = this.Collection.DataGrid.Request.RawUrl;
					if (Url.IndexOf("?") > -1) {
						Url += "&";
					} else {
						Url += "?";
					}
					Url += "ChildID=" + this.ItemID + "&Delete=1&Type=" + this.DataGrid.ID;
					return Url;
				} else {
					return this.sDeleteLinkUrl;
				}
			}
			set { this.sDeleteLinkUrl = value; }
		}
		public string EditLinkUrl {
			get {
				if (this.Collection.AllowEdit && !string.IsNullOrEmpty(this.Collection.EditLinkUrl)) {
					if (!this.Item.GetType().IsSubclassOf(Type.GetType("Ophelia.Application.Base.Entity")) || (this.Item.GetType().IsSubclassOf(Type.GetType("Ophelia.Application.Base.Entity")) && this.Item.CanBeEdited)) {
						if (!string.IsNullOrEmpty(this.Collection.QueryStringKey)) {
							return this.Collection.EditLinkUrl.Insert(this.Collection.EditLinkUrl.IndexOf(this.Collection.QueryStringKey) + this.Collection.QueryStringKey.Length + 1, this.LinkItemID);
						} else if (this.Collection.EditLinkUrl.IndexOf("?") > -1) {
							return this.Collection.EditLinkUrl.Insert(this.Collection.EditLinkUrl.IndexOf("?"), this.LinkItemID);
						} else {
							return this.Collection.EditLinkUrl + this.LinkItemID;
						}
					}
				}
				return "";
			}
			set { this.sEditLinkUrl = value; }
		}
		public bool CanBeDeleted {
			get {
				if ((this.ReadOnly == false || this.Collection.AllowDelete)) {
					if (this.Collection.AllowDelete) {
						if ((!object.ReferenceEquals(this, this.DataGrid.ExtraRow))) {
							if (!this.Item.GetType().IsSubclassOf(Type.GetType("Ophelia.Application.Base.Entity")) || (this.Item.GetType().IsSubclassOf(Type.GetType("Ophelia.Application.Base.Entity")) && this.Item.CanBeDeleted)) {
								return true;
							}
						}
					}
				}
				return false;
			}
		}
		public DataGrid DataGrid {
			get { return this.Collection.DataGrid; }
		}
		public RowCollection Collection {
			get { return this.oCollection; }
		}
		public object Item {
			get { return this.oItem; }
			set {
				this.oItem = value;
				if ((this.Cells != null)) {
					Cell Cell = default(Cell);
					foreach ( Cell in this.Cells) {
						Cell.Binding.Item = value;
					}
				}
			}
		}
		public string OnRowClick {
			get {
				if (this.sOnRowClick.Length == 0)
					return this.DataGrid.OnRowClick;
				return this.sOnRowClick;
			}
			set { this.sOnRowClick = value; }
		}
		public bool IsSelected {
			get { return this.bSelected; }
			set { this.bSelected = value; }
		}
		public bool ReadOnly {
			get { return this.bReadOnly; }
			set {
				this.bReadOnly = value;
				this.Cells.ReadOnly = value;
			}
		}
		public bool CanBeSelected {
			get { return this.bCanBeSelected; }
			set { this.bCanBeSelected = value; }
		}
		public string SelectedBehavior {
			get { return this.sSelectedBehavior; }
			set { this.sSelectedBehavior = value; }
		}
		public Row PrecedingRow {
			get {
				if (this.Index == 0) {
					return this;
				} else {
					return this.Collection(this.Index - 1);
				}
			}
		}
		public Cell this[int ColumnIndex] {
			get { return this.Cells(ColumnIndex); }
		}
		public Cell this[string Name] {
			get { return this.Cells(Name); }
		}
		public Cell CellByColumnName {
			get {
				Column Column = this.DataGrid.Columns.ItemByName(ColumnName);
				if ((Column != null)) {
					return Column(this.Index);
				}
				return null;
			}
		}
		public Cell CellByColumnMemberNameToBeDrawn {
			get {
				Column Column = this.DataGrid.Columns.ItemByMemberNameToBeDrawn(MemberNameToBeDrawn);
				if ((Column != null)) {
					return Column(this.Index);
				}
				return null;
			}
		}
		public RowCellCollection Cells {
			get { return this.oCells; }
		}
		public string ItemID {
			get {
				if ((this.Item != null) && !this.Collection.DataGrid.ItemIDProperty.Contains(".") && (this.Item.GetType().GetProperty(this.Collection.DataGrid.ItemIDProperty) != null)) {
					return this.Item.GetType().GetProperty(this.Collection.DataGrid.ItemIDProperty).GetValue(this.Item, null);
				} else if ((this.Item != null) && this.Collection.DataGrid.ItemIDProperty.Contains(".")) {
					View.Base.Binders.Binding Binding = new View.Base.Binders.Binding();
					Binding.MemberName = this.Collection.DataGrid.ItemIDProperty;
					Binding.Item = this.Item;
					return Binding.Value;
				} else {
					return this.Index;
				}
			}
		}
		public int LinkItemID {
			get {
				if (string.IsNullOrEmpty(this.Collection.EditLinkMemberName)) {
					return this.ItemID;
				}
				View.Base.Binders.Binding Binding = new View.Base.Binders.Binding();
				Binding.MemberName = this.Collection.EditLinkMemberName;
				Binding.Item = this.Item;
				return Binding.Value;
			}
		}
		public string ID {
			get { return "CB_" + this.DataGrid.ID + "Form_Row_ID_" + this.ItemID; }
		}
		public string GetID()
		{
			return " ID=\"" + this.ID + "\"";
		}
		public string GetClassName()
		{
			string ClassName = "EvenRow";
			if (!(Index % 2 == 0)) {
				ClassName = "OddRow";
			}
			return this.DataGrid.ID + "RowStyle " + ClassName;
		}
		public string GetSelectedRowClassName()
		{
			string ClassName = "EvenRow";
			if (!(Index % 2 == 0)) {
				ClassName = "OddRow";
			}
			return this.DataGrid.ID + "SelectedRowStyle " + this.GetClassName();
		}
		public Row(RowCollection Collection, ref object Item)
		{
			this.oCollection = Collection;
			this.oItem = Item;
			this.nIndex = this.Collection.Count;
			this.SelectedBehavior = Collection.DataGrid.RowSelectedChangeBehaivor;
			this.oCells = new RowCellCollection(this);
			Collection.ExpandedRows.Add(this);
		}
	}
}

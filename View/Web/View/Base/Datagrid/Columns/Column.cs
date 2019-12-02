using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class Column
	{
		private ColumnCollection oCollection;
		private string sName = "";
		private string sMemberName = "";
		internal int nIndex;
		private int nVisibilityIndex;
		private ColumnCellCollection oCells;
		private Style oStyle = new Style();
		private Style oCellStyle = new Style();
		protected WebControl oDataControl;
		private bool bReadOnly = false;
		private bool bVisible = false;
		private string sEditLink = "";
		private string sSuffix = "";
		private bool bSortable = false;
		private bool bDrawHiddenValues = false;
		private bool bDrawHiddenColumn = false;
		private bool bHideExtraRowCell = false;
		private string sEditLinkMemberName = "";
		private string sMemberNameToBeDrawn = "";
		public bool Sortable {
			get { return this.bSortable; }
			set { this.bSortable = value; }
		}
		public string EditLink {
			get { return this.sEditLink; }
		}
		public string EditLinkMemberName {
			get { return this.sEditLinkMemberName; }
			set { this.sEditLinkMemberName = value; }
		}
		public int LinkItemID {
			get {
				if (string.IsNullOrEmpty(this.EditLinkMemberName)) {
					return this[Index].Row.ItemID;
				}
				View.Base.Binders.Binding Binding = new View.Base.Binders.Binding();
				Binding.MemberName = this.EditLinkMemberName;
				Binding.Item = this[Index].Row.Item;
				return Binding.Value;
			}
		}
		internal string ID {
			get {
				//If Me.MemberName.IndexOf(".") > -1 Then
				//    Return "CB_" & Me.DataGrid.ID & "Form_Column_" & Me.MemberName.Substring(Me.MemberName.LastIndexOf(".") + 1, Me.MemberName.Length - Me.MemberName.LastIndexOf(".") - 1)
				//End If
				if (!string.IsNullOrEmpty(this.MemberNameToBeDrawn)) {
					return "CB_" + this.DataGrid.ID + "Form_Column_" + this.MemberNameToBeDrawn.Replace(".", "");
				}
				return "CB_" + this.DataGrid.ID + "Form_Column_" + this.MemberName.Replace(".", "");
			}
		}
		public string GetID()
		{
			return " NAME=\"" + this.ID + "\" ID=\"" + this.ID + "\"";
		}
		public DataControl DataControl {
			get { return this.oDataControl; }
		}
		internal DataGrid DataGrid {
			get { return this.Collection.DataGrid; }
		}
		internal ColumnCollection Collection {
			get { return this.oCollection; }
		}
		public string Name {
			get { return this.sName; }
			set { this.sName = value; }
		}
		public string Suffix {
			get { return this.sSuffix; }
			set { this.sSuffix = value; }
		}
		public string MemberNameToBeDrawn {
			get { return this.sMemberNameToBeDrawn; }
			set { this.sMemberNameToBeDrawn = value; }
		}
		public string MemberName {
			get { return this.sMemberName; }
			set { this.sMemberName = value; }
		}
		public bool DrawHiddenValues {
			get { return this.bDrawHiddenValues; }
			set { this.bDrawHiddenValues = value; }
		}
		public bool DrawHiddenColumn {
			get { return this.bDrawHiddenColumn; }
			set { this.bDrawHiddenColumn = value; }
		}
		public bool HideExtraRowCell {
			get { return this.bHideExtraRowCell; }
			set { this.bHideExtraRowCell = value; }
		}
		public bool ReadOnly {
			get {
				if (this.DataGrid.Readonly || !string.IsNullOrEmpty(this.EditLink)) {
					return true;
				} else {
					return this.bReadOnly;
				}
			}
			set { this.bReadOnly = value; }
		}
		public bool Visible {
			get { return this.bVisible; }
			set { this.bVisible = value; }
		}
		public int Index {
			get { return this.nIndex; }
		}
		public int VisibilityIndex {
			get { return this.nVisibilityIndex; }
		}
		public Style Style {
			get { return this.oStyle; }
		}
		public Cell this[int Index] {
			get { return this.Cells(Index); }
		}
		public Style CellStyle {
			get { return this.oCellStyle; }
		}
		public ColumnCellCollection Cells {
			get { return this.oCells; }
		}
		public Validator.ValidatorCollection Validators {
			get { return this.DataControl.Validators; }
		}
		protected virtual int DecreaseIndex(bool ForVisibility = false)
		{
			if (ForVisibility) {
				this.nVisibilityIndex -= 1;
				return this.nVisibilityIndex;
			} else {
				this.nIndex -= 1;
				return this.nIndex;
			}
		}
		protected virtual int IncreaseIndex(bool ForVisibility = false)
		{
			if (ForVisibility) {
				this.nVisibilityIndex += 1;
				return this.nVisibilityIndex;
			} else {
				this.nIndex += 1;
				return this.nIndex;
			}
		}
		public virtual void Remove()
		{
			this.Collection.Remove(this);
			if (this.Collection.ExpandedColumns.Contains(this)) {
				this.Collection.ExpandedColumns.Remove(this);
			}
			if (this.Collection.CollapsedColumns.Contains(this)) {
				this.Collection.CollapsedColumns.Remove(this);
			}
		}
		protected virtual void SetDataControl()
		{
			if (this.oDataControl == null) {
				this.oDataControl = new Label(this.ID);
			}
		}
		protected virtual void OnAfterSetDataControl()
		{
			if ((this.DataControl != null))
				this.DataControl.NeedToValidate = this.DataGrid.NeedToValidate;
		}
		public virtual Cell CreateCell(Row Row)
		{
			Cell Cell = new Cell(Row, this);
			Cell.ReadOnly = this.ReadOnly;
			return Cell;
		}
		public virtual bool LinkForEdit(string Url, string EditLinkMemberName = "")
		{
			this.sEditLink = Url;
			this.EditLinkMemberName = EditLinkMemberName;
			if (!string.IsNullOrEmpty(this.sEditLink)) {
				this.ReadOnly = true;
			}
		}
		protected virtual void CustomizeStyle()
		{
			this.Style.bCustomized = false;
		}
		protected virtual void SetDefaultCellStyle()
		{
			this.CellStyle.HorizontalAlignment = SectionAlignment.Left;
			this.CellStyle.VerticalAlignment = VerticalAlignment.Middle;
			this.Style.bCustomized = false;
		}
		public Column(ColumnCollection ColumnCollection, string Name, string MemberName)
		{
			this.oCollection = ColumnCollection;
			this.Name = Name;
			this.MemberName = MemberName;
			ColumnCollection.ExpandedColumns.Add(this);
			this.nIndex = ColumnCollection.Count;
			this.oCells = new ColumnCellCollection(this);
			this.oStyle = new Style();
			this.CustomizeStyle();
			this.oCellStyle = new Style();
			this.SetDefaultCellStyle();
			this.SetDataControl();
			this.OnAfterSetDataControl();
		}
	}
}

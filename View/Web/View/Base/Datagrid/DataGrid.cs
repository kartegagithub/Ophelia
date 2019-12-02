using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public abstract class DataGrid : Binders.Binder
	{
		private RowCollection oRows;
		private ColumnCollection oColumns;
		private int nBindingStartIndex = -1;
		private int nBindingEndIndex = -1;
		public event RowAddedEventHandler RowAdded;
		public delegate void RowAddedEventHandler(object Sender, ref Ophelia.Web.View.Base.DataGrid.RowEventArgs e);
		public event BeforeRowDrawnEventHandler BeforeRowDrawn;
		public delegate void BeforeRowDrawnEventHandler(object Sender, ref Ophelia.Web.View.Base.DataGrid.RowEventArgs e);
		public event BeforeColumnDrawnEventHandler BeforeColumnDrawn;
		public delegate void BeforeColumnDrawnEventHandler(object Sender, ref Ophelia.Web.View.Base.DataGrid.ColumnEventArgs e);
		public event ColumnTotalRequestedEventHandler ColumnTotalRequested;
		public delegate void ColumnTotalRequestedEventHandler(object sender, ref ColumnEventArgs e);
		private bool bHasForm = true;
		private Controls.Label oSubmitButton;
		private string sNewLinkUrl = string.Empty;
		private string sNoRowMessage = string.Empty;
		protected Row oExtraRow;
		private string sItemIDProperty = "ID";
		private string sRowSelectedChangeBehaivor = "";
		private bool bHierarchicDisplay = false;
		private Controls.WebControl oDeleteWebControl;
		private Style oDeleteLinkStyle;
		private Style oDeleteColumnStyle;
		private Style oSaveColumnStyle;
		private string sOnRowClick;
		protected ToolBar oToolBar;
		private bool bNeedToValidate = true;
		public ToolBar ToolBar {
			get {
				if (this.oToolBar == null) {
					this.oToolBar = new ToolBar(this);
					this.ConfigureToolbar();
				}
				return this.oToolBar;
			}
		}

		public virtual void ConfigureToolbar()
		{
		}
		internal virtual int DrawnTableColumnCount {
			get { return this.Columns.Count; }
		}
		public bool NeedToValidate {
			get { return this.bNeedToValidate; }
			set { this.bNeedToValidate = value; }
		}
		public string OnRowClick {
			get { return this.sOnRowClick; }
			set { this.sOnRowClick = value; }
		}
		public string RowSelectedChangeBehaivor {
			get { return this.sRowSelectedChangeBehaivor; }
			set { this.sRowSelectedChangeBehaivor = value; }
		}
		public ColumnCollection Columns {
			get { return this.oColumns; }
		}
		public RowCollection Rows {
			get { return this.oRows; }
		}
		public Cell this[int RowIndex, int ColumnIndex] {
			get {
				if (this.Rows.Count > RowIndex && RowIndex > -1 && this.Rows(RowIndex).Cells.Count > ColumnIndex && ColumnIndex > -1) {
					return this.Rows(RowIndex)(ColumnIndex);
				}
				return null;
			}
		}
		public int BindingStartIndex {
			get { return this.nBindingStartIndex; }
			set { this.nBindingStartIndex = value; }
		}
		public int BindingEndIndex {
			get { return this.nBindingEndIndex; }
			set { this.nBindingEndIndex = value; }
		}
		public string NewLinkUrl {
			get { return this.sNewLinkUrl; }
			set {
				this.sNewLinkUrl = value;
				this.ToolBar.NewButton.Url = value;
			}
		}
		public string ItemIDProperty {
			get { return this.sItemIDProperty; }
			set { this.sItemIDProperty = value; }
		}
		public string NoRowMessage {
			get { return this.sNoRowMessage; }
			set { this.sNoRowMessage = value; }
		}
		public bool HasForm {
			get { return this.bHasForm; }
			set { this.bHasForm = value; }
		}
		public bool HierarchicDisplay {
			get { return this.bHierarchicDisplay; }
			set { this.bHierarchicDisplay = value; }
		}
		public Controls.WebControl DeleteWebControl {
			get {
				if (this.oDeleteWebControl == null) {
					this.oDeleteWebControl = new Controls.Image("", Controls.ServerSide.ImageDrawer.GetImageUrl("WebDelete"));
					this.oDeleteWebControl.Style.Padding = 0;
					this.oDeleteColumnStyle = null;
					this.oDeleteLinkStyle = null;
				}
				return this.oDeleteWebControl;
			}
			set { this.oDeleteWebControl = value; }
		}
		public Style DeleteColumnStyle {
			get {
				if (this.oDeleteColumnStyle == null) {
					this.oDeleteColumnStyle = new Style();
					this.oDeleteColumnStyle.Padding = 0;
					this.oDeleteColumnStyle.Font.Size = 1;
					this.oDeleteColumnStyle.Class = "DeleteCell";
					this.oDeleteColumnStyle.HorizontalAlignment = HorizontalAlignment.Center;
					this.oDeleteColumnStyle.VerticalAlignment = VerticalAlignment.Middle;
				}
				return this.oDeleteColumnStyle;
			}
		}
		public Style SaveColumnStyle {
			get {
				if (this.oSaveColumnStyle == null) {
					this.oSaveColumnStyle = new Style();
					this.oSaveColumnStyle.Padding = 0;
					this.oSaveColumnStyle.Font.Size = 1;
					this.oSaveColumnStyle.HorizontalAlignment = HorizontalAlignment.Center;
					this.oSaveColumnStyle.VerticalAlignment = VerticalAlignment.Middle;
				}
				return this.oSaveColumnStyle;
			}
		}
		public Style DeleteLinkStyle {
			get {
				if (this.oDeleteLinkStyle == null) {
					this.oDeleteLinkStyle = new Style();
					this.oDeleteLinkStyle.Padding = 0;
				}
				return this.oDeleteLinkStyle;
			}
		}
		public Controls.Label SubmitButton {
			get {
				if (this.oSubmitButton == null) {
					this.oSubmitButton = new Controls.Label(this.ID + "_Save");
					this.oSubmitButton.Style.CursorStyle = Cursor.Pointer;
					this.oSubmitButton.Style.Float = FloatType.Right;
					this.oSubmitButton.Style.Top = 2;
					this.oSubmitButton.Style.Bottom = 2;
				}
				return this.oSubmitButton;
			}
		}
		public override void Bind()
		{
			if (this.BindState == BinderState.Pending && (this.Binding.Value != null)) {
				bool Rebinding = false;
				if (this.Rows.Count > 0)
					Rebinding = true;
				this.BindState = BinderState.Binding;
				this.Rows.Clear();
				this.OnBindingStarted(new BindingEventArgs(this.Binding, Rebinding));
				if (this.BindingStartIndex == -1)
					this.BindingStartIndex = 0;
				if (this.BindingEndIndex == -1)
					this.BindingEndIndex = this.Binding.Value.Count - 1;
				for (int n = BindingStartIndex; n <= BindingEndIndex; n++) {
					if (n < this.Binding.Value.Count) {
						if ((this.Binding.Value(n) != null))
							this.Rows.Bind(this.Binding.Value(n));
					}
				}
				this.CreateExtraRow();
				this.BindingStartIndex = -1;
				this.BindingEndIndex = -1;
				this.BindState = BinderState.Binded;
				this.OnBindingCompleted(new BindingEventArgs(this.Binding, Rebinding));
			} else if (this.Binding.Value == null) {
				this.Rows.Clear();
			}
			for (int i = 0; i <= this.Columns.Count - 1; i++) {
				this.UpdateColumnTotal(this.Columns(i));
			}
		}
		public virtual void OnRowAdded(RowEventArgs e)
		{
			if (RowAdded != null) {
				RowAdded(this, e);
			}
		}
		public virtual void OnBeforeRowDrawn(RowEventArgs e)
		{
			if (BeforeRowDrawn != null) {
				BeforeRowDrawn(this, e);
			}
		}
		public virtual void OnBeforeColumnDrawn(ColumnEventArgs e)
		{
			if (BeforeColumnDrawn != null) {
				BeforeColumnDrawn(this, e);
			}
		}
		public void Bind(ICollection Data)
		{
			this.BindState = BinderState.Pending;
			this.Binding.UpdateValue(Data);
			this.Bind();
		}
		protected internal bool HasRowEditButton {
			get {
				if (this.Rows.AllowEdit && !string.IsNullOrEmpty(this.Rows.EditLinkUrl)) {
					return true;
				}
				return false;
			}
		}
		internal virtual void UpdateColumnTotal(Column Column)
		{
			if (Column.GetType.FullName == "Ophelia.Web.View.Base.DataGrid.NumberBoxColumn" || Column.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Base.DataGrid.NumberBoxColumn"))) {
				NumberBoxColumn NumberBoxColumn = (NumberBoxColumn)Column;
				if (NumberBoxColumn.CalculateBySum) {
					NumberBoxColumn.AddValueToTotalValue(0, true);
					Row Row = default(Row);
					foreach ( Row in this.Rows) {
						if ((!object.ReferenceEquals(Row, this.ExtraRow))) {
							NumberBoxColumn.AddValueToTotalValue(Convert.ToDecimal(Column.Cells(Row.Index).Text));
						}
					}
				}
			}
		}
		public void OnColumnTotalRequested(ref ColumnEventArgs e)
		{
			if (ColumnTotalRequested != null) {
				ColumnTotalRequested(this, e);
			}
		}
		protected string SetRowDescription(int RowIndex, Cell Cell)
		{
			string Description = "";
			if ((Cell.Row.Item != null) && (Cell.Row.Item.GetType.GetProperty("ID") != null)) {
				return "ID_" + ((object)Cell.Row.Item).ID + "_" + Cell.Column.MemberName;
			} else {
				return "ID_" + RowIndex + "_" + Cell.Column.MemberName;
			}
			return Description;
		}
		protected virtual object CreateColumnCollection()
		{
			return new ColumnCollection(this);
		}
		private Row CreateExtraRow()
		{
			if (this.Rows.AllowNew) {
				this.oExtraRow = this.OnCreatingExtraRow();
			}
			return this.ExtraRow;
		}
		public Row ExtraRow {
			get { return this.oExtraRow; }
		}
		protected virtual Row OnCreatingExtraRow()
		{
			return this.Rows.Add();
		}
		public DataGrid(string Title, string ID) : this(null, Title, "", ID)
		{
		}
		public DataGrid(string Title, string MemberName, string ID) : this(null, Title, MemberName, ID)
		{
		}
		public DataGrid(object Data, string MemberName, string ID)
		{
			this.oColumns = this.CreateColumnCollection();
			this.oRows = new RowCollection(this);
			this.ID = ID;
			this.ReadOnly = true;
			if (Data != null) {
				this.Binding.Item = Data;
			}
			this.Binding.MemberName = MemberName;
			this.Customize(MemberName);
		}
		public DataGrid(object Data, string Title, string MemberName, string ID)
		{
			this.oColumns = this.CreateColumnCollection();
			this.oRows = new RowCollection(this);
			this.ID = ID;
			this.ReadOnly = true;
			if (Data != null) {
				this.Binding.Value = Data;
			}
			if (string.IsNullOrEmpty(MemberName))
				MemberName = Title;
			this.Binding.MemberName = MemberName;
			this.Binding.MemberName = Title;
			this.Customize(Title);
		}

		protected virtual void Customize(string Title)
		{
		}
		public DataGrid(string Title, ICollection Data, string ID) : this(Data, Title, "", ID)
		{
		}
		public DataGrid(ICollection Data, string ID) : this(Data, "", "", ID)
		{
		}
		public DataGrid(string ID) : this(null, "", "", ID)
		{
		}
		public static int GetSelectedRowItemID(string DataGridName, HttpRequest Request)
		{
			if (Information.IsNumeric(Request(DataGridName + "RadioBox"))) {
				return Request(DataGridName + "RadioBox");
			}
			return -1;
		}
	}
}

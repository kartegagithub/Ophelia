using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class RowCollection : AnyRowCollection
	{
		private AnyRowCollection oCheckedRows;
		private AnyRowCollection oCollapsedRows;
		private AnyRowCollection oExpandedRows;
		private Style oRowStyle;
		private Style oSelectedRowStyle;
		private bool bAllowDelete = false;
		private bool bAllowNew = false;
		private bool bAllowEdit = false;
		private string sEditLinkUrl = "";
		private string sEditLinkMemberName = "";
		private string sDeleteLinkUrl = "";
		private string sParameterUrl = "";
		private string sQueryStringKey = "";
		private bool bUseCustomizedSelectedRowStyle = false;
		public bool UseCustomizedSelectedRowStyle {
			get { return this.bUseCustomizedSelectedRowStyle; }
			set { this.bUseCustomizedSelectedRowStyle = value; }
		}
		public Style RowStyle {
			get { return this.oRowStyle; }
		}
		public Style SelectedRowStyle {
			get { return this.oSelectedRowStyle; }
		}
		public AnyRowCollection CheckedRows {
			get { return this.oCheckedRows; }
		}
		public AnyRowCollection CollapsedRows {
			get { return this.oCollapsedRows; }
		}
		public AnyRowCollection ExpandedRows {
			get { return this.oExpandedRows; }
		}
		public bool AllowNew {
			get { return this.bAllowNew; }
			set { this.bAllowNew = value; }
		}
		public bool AllowDelete {
			get { return this.bAllowDelete; }
			set { this.bAllowDelete = value; }
		}
		public bool AllowEdit {
			get { return this.bAllowEdit; }
			set { this.bAllowEdit = value; }
		}
		public string EditLinkUrl {
			get { return this.sEditLinkUrl; }
			set {
				this.sEditLinkUrl = value;
				this.AllowEdit = (!string.IsNullOrEmpty(value));
			}
		}
		public string EditLinkMemberName {
			get { return this.sEditLinkMemberName; }
			set { this.sEditLinkMemberName = value; }
		}
		public string QueryStringKey {
			get { return this.sQueryStringKey; }
			set { this.sQueryStringKey = value; }
		}
		public Row Bind(ref object Item)
		{
			return this.Add(this.CreateRow(ref Item));
		}
		public Row Add()
		{
			return this.Add(this.CreateRow(ref null));
		}
		protected virtual object CreateRow(ref object Item)
		{
			return new Row(this, Item);
		}
		public override Row Add(Row Row)
		{
			RowEventArgs EventArgs = new RowEventArgs(Row);
			if (!EventArgs.Cancel) {
				base.Add(Row);
				this.DataGrid.OnRowAdded(new RowEventArgs(Row));
				return Row;
			} else {
				if (this.ExpandedRows.Contains(Row))
					this.ExpandedRows.Remove(Row);
				return null;
			}
		}
		protected virtual void CustomizeRowStyle()
		{
			this.RowStyle.VerticalAlignment = VerticalAlignment.Top;
			this.RowStyle.BackgroundColor = "#ffffff";
			this.RowStyle.Font.Color = "#000000";
		}
		public RowCollection(DataGrid DataGrid) : base(DataGrid)
		{
			this.oCheckedRows = new AnyRowCollection(DataGrid);
			this.oCollapsedRows = new AnyRowCollection(DataGrid);
			this.oExpandedRows = new AnyRowCollection(DataGrid);
			this.oRowStyle = new Style();
			this.oSelectedRowStyle = new Style();
			this.CustomizeRowStyle();
		}
	}
}

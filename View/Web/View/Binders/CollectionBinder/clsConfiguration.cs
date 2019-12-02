using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinderConfiguration
	{
		private CollectionBinder oBinder;
		private bool bAllowNew = false;
		private bool bAllowEdit = false;
		private bool bAllowDelete = false;
		private bool bAllowRefresh = false;
		private bool bAllowPrint = false;
		private bool bAllowReport = false;
		private bool bAllowCreateExcelDocument = false;
		private bool bAllowConfiguration = false;
		private bool bAllowHelp = false;
		private bool bAllowTotals = false;
		private string sEditLinkUrl = "";
		private string sNewLinkUrl = "";
		private string sQueryStringKey = "";
		private int nPageSize = 20;
		private bool bPaging = true;
		internal CollectionBinder Binder {
			get { return this.oBinder; }
		}
		public bool AllowNew {
			get { return this.bAllowNew; }
			set {
				this.bAllowNew = value;
				this.Binder.Rows.AllowNew = value;
			}
		}
		public bool AllowDelete {
			get { return this.bAllowDelete; }
			set {
				this.bAllowDelete = value;
				this.Binder.Rows.AllowDelete = value;
			}
		}
		public bool AllowEdit {
			get { return this.bAllowEdit; }
			set {
				this.bAllowEdit = value;
				this.Binder.Rows.AllowEdit = value;
			}
		}
		public bool AllowRefresh {
			get { return this.bAllowRefresh; }
			set { this.bAllowRefresh = value; }
		}
		public bool AllowPrint {
			get { return this.bAllowPrint; }
			set { this.bAllowPrint = value; }
		}
		public bool AllowReport {
			get { return this.bAllowReport; }
			set { this.bAllowReport = value; }
		}
		public bool AllowCreateExcelDocument {
			get { return this.bAllowCreateExcelDocument; }
			set { this.bAllowCreateExcelDocument = value; }
		}
		public bool AllowConfiguration {
			get { return this.bAllowConfiguration; }
			set { this.bAllowConfiguration = value; }
		}
		public bool AllowHelp {
			get { return this.bAllowHelp; }
			set { this.bAllowHelp = value; }
		}
		public bool AllowTotals {
			get { return this.bAllowTotals; }
			set { this.bAllowTotals = value; }
		}
		public string EditLinkUrl {
			get { return this.sEditLinkUrl; }
			set {
				this.sEditLinkUrl = value;
				this.Binder.Rows.EditLinkUrl = value;
			}
		}
		public string NewLinkUrl {
			get { return this.sNewLinkUrl; }
			set {
				this.sNewLinkUrl = value;
				this.Binder.NewLinkUrl = value;
				this.Binder.Rows.AllowNew = true;
			}
		}
		public string QueryStringKey {
			get { return this.sQueryStringKey; }
			set {
				this.sQueryStringKey = value;
				this.Binder.Rows.QueryStringKey = value;
			}
		}
		public int PageSize {
			get { return this.nPageSize; }
			set { this.nPageSize = value; }
		}
		public bool Paging {
			get { return this.bPaging; }
			set { this.bPaging = value; }
		}
		public CollectionBinderConfiguration(CollectionBinder Binder)
		{
			this.oBinder = Binder;
		}
	}
}

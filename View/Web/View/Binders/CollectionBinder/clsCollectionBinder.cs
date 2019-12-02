using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
using Ophelia.Web.View;
using Microsoft.Office.Interop;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinder : Ophelia.Web.View.Controls.DataGrid.DataGrid, IEntityBinder
	{
		private int nPageSize = 0;
		private Controls.Pager oPager;
		private CollectionBinderConfiguration oConfiguration;
		private bool bBindAllPages;
		private Filtering.FilterCollection oFilters;
		private View.Base.Controls.DataGrid.ViewStyle eViewStyle;
		public bool AlwaysHidePager = false;
		public bool PagerWithMoreButton = false;
		private CollectionBinderCustomization oCustomization;
		private CollectionBinderColumnCustomizationCollection oColumnCustomizations = null;
		private CollectionBinderAjaxDelegate oAjaxDelegate = null;
		private Forms.EntityForm oEntityForm = null;
		public Forms.EntityForm EntityForm {
			get { return this.oEntityForm; }
			set { this.oEntityForm = value; }
		}
		public override void ConfigureToolbar()
		{
			base.ConfigureToolbar();
		}
		public Binders.Toolbar.ToolBar ToolBar {
			get { return this.oToolBar; }
		}
		private Hashtable oExtraDefinitions;
		public Hashtable ExtraDefinitions {
			get {
				if (this.oExtraDefinitions == null) {
					this.oExtraDefinitions = new Hashtable();
				}
				return this.oExtraDefinitions;
			}
		}
		//ExtraDefinition set edildiğinde raise edilecek.
		public virtual void ExtraDefinitionIsSet(Hashtable ExtraDefinition)
		{

		}
		public CollectionBinderAjaxDelegate AjaxDelegate {
			get {
				if (this.oAjaxDelegate == null) {
					this.oAjaxDelegate = new CollectionBinderAjaxDelegate();
					this.oAjaxDelegate.Binder = this;
				}
				return this.oAjaxDelegate;
			}
			set { this.oAjaxDelegate = value; }
		}
		private bool bHasAjaxDelegate = false;
		public bool HasAjaxDelegate {
			get { return this.bHasAjaxDelegate; }
			set {
				this.bHasAjaxDelegate = value;
				if (value == true && this.ToolBar != null) {
					this.ToolBar.RefreshButton.Url = "";
					this.ToolBar.RefreshButton.OnClickEvent = this.AjaxDelegate.RedrawEvent;

					this.ToolBar.PrintButton.Url = "";
					this.ToolBar.PrintButton.OnClickEvent = this.AjaxDelegate.PrintEvent;
				}
			}
		}
		internal void ExportToExcel()
		{
			bool IsDone = false;
			try {
				Microsoft.Office.Interop.Excel.Application ExcelDocument = new Microsoft.Office.Interop.Excel.Application();
				Microsoft.Office.Interop.Excel.Workbook wBook = default(Microsoft.Office.Interop.Excel.Workbook);
				Microsoft.Office.Interop.Excel.Worksheet wSheet = default(Microsoft.Office.Interop.Excel.Worksheet);
				System.Globalization.CultureInfo myCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
				wBook = ExcelDocument.Workbooks.Add;
				wSheet = wBook.ActiveSheet();

				this.Bind();
				System.Data.DataTable dt = this.Collection.GetDataTable();
				Ophelia.Web.View.Base.DataGrid.Column dc = default(Ophelia.Web.View.Base.DataGrid.Column);
				int colIndex = 0;
				int rowIndex = 0;

				foreach ( dc in this.Columns.ExpandedColumns) {
					colIndex = colIndex + 1;
					ExcelDocument.Cells(1, colIndex) = this.Client.Dictionary.GetWord(dc.Name);
				}

				for (int n = 0; n <= this.Rows.Count - 1; n++) {
					rowIndex = rowIndex + 1;
					colIndex = 0;
					for (int k = 0; k <= this.Columns.ExpandedColumns.Count - 1; k++) {
						colIndex = colIndex + 1;
						if (this.Columns(this.Columns.ExpandedColumns(k).MemberName).GetType.FullName == "Ophelia.Web.View.Base.DataGrid.DateTimeColumn") {
							ExcelDocument.Cells(rowIndex + 1, colIndex) = this.Rows(n).Cells(k).Value;
						} else {
							ExcelDocument.Cells(rowIndex + 1, colIndex) = this.Rows(n).Cells(k);
						}

					}
					if (rowIndex == this.Rows.Count) {
						break; // TODO: might not be correct. Was : Exit For
					}
				}

				wSheet.Columns.AutoFit();
				string strFileName = System.IO.Path.GetTempPath() + "t_" + VBMath.Rnd().ToString().Replace(".", "") + "_shv.xls";
				if (System.IO.File.Exists(strFileName)) {
					System.IO.File.Delete(strFileName);
				}
				wBook.SaveAs(strFileName);
				wBook.Close();
				ExcelDocument.Quit();
				System.Threading.Thread.CurrentThread.CurrentCulture = myCultureInfo;
				UI.Current.Response.Clear();
				UI.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Document.xls");
				UI.Current.Response.ContentType = "application/vnd.ms-excel";
				UI.Current.Response.Charset = "";
				UI.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding(65001);
				UI.Current.Response.WriteFile(strFileName);
				System.Threading.Thread DeleteThread = new System.Threading.Thread(DeleteExcelFile);
				DeleteThread.Start(strFileName);
				IsDone = true;
				UI.Current.Response.End();
			} catch (Exception ex) {
				if (IsDone) {
					throw ex;
				}
				try {
					base.BindAllDataForDocuments();
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
					System.Globalization.CultureInfo oldCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
					System.Globalization.CultureInfo NewCultureInfo = new System.Globalization.CultureInfo("en-US", false);
					System.Threading.Thread.CurrentThread.CurrentCulture = NewCultureInfo;
					this.Bind();
					try {
						System.Text.StringBuilder sbDelimited = new System.Text.StringBuilder();
						Ophelia.Web.View.Base.DataGrid.Column Column = null;
						for (int Index = 0; Index <= this.Columns.ExpandedColumns.Count - 1; Index++) {
							Column = this.Columns.ExpandedColumns(Index);
							sbDelimited.Append(Strings.Chr(34) + this.GetColumnTitle(Column.MemberName) + Strings.Chr(34));
							sbDelimited.Append(";");
						}
						sbDelimited.Remove(sbDelimited.Length - 1, 1);
						sbDelimited.AppendLine();
						System.Data.DataTable dt = this.Collection.GetDataTable();
						Ophelia.Web.View.Base.DataGrid.Column dc = default(Ophelia.Web.View.Base.DataGrid.Column);
						Ophelia.Web.View.Base.DataGrid.Row dr = default(Ophelia.Web.View.Base.DataGrid.Row);
						int colIndex = 0;
						int rowIndex = 0;
						for (int Index = 0; Index <= this.Rows.Count - 1; Index++) {
							dr = this.Rows(Index);
							rowIndex = rowIndex + 1;
							colIndex = 0;
							for (int k = 0; k <= this.Columns.ExpandedColumns.Count - 1; k++) {
								dc = this.Columns.ExpandedColumns(k);
								colIndex = colIndex + 1;
								if (dr(dc.MemberName).Value != null) {
									if (this.Columns(dc.MemberName).GetType.FullName == "Ophelia.Web.View.Base.DataGrid.DateTimeColumn") {
										if (Information.IsDate(dr(dc.MemberName).Value) && dr(dc.MemberName).Value == DateTime.MinValue) {
											sbDelimited.Append("");
										} else {
											sbDelimited.Append(dr(dc.MemberName).Value.ToString().Replace(Constants.vbCr, "").Replace(Constants.vbLf, ""));
										}
										sbDelimited.Append(";");
									} else {
										sbDelimited.Append(dr(dc.MemberName).Value.ToString().Replace(Constants.vbCr, "").Replace(Constants.vbLf, ""));
										sbDelimited.Append(";");
									}
								} else {
									sbDelimited.Append(" ");
									sbDelimited.Append(";");
								}
							}
							if (rowIndex == this.Rows.Count) {
								break; // TODO: might not be correct. Was : Exit For
							}
							sbDelimited.Remove(sbDelimited.Length - 1, 1);
							sbDelimited.AppendLine();
						}

						string strFileName = System.IO.Path.GetTempPath() + "t_" + VBMath.Rnd().ToString().Replace(".", "") + "_shv.csv";
						if (System.IO.File.Exists(strFileName)) {
							System.IO.File.Delete(strFileName);
						}
						System.IO.File.WriteAllText(strFileName, sbDelimited.ToString(), System.Text.UTF8Encoding.Default);

						UI.Current.Response.Clear();
						UI.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Document.csv");
						UI.Current.Response.ContentType = "application/vnd.ms-excel";
						UI.Current.Response.Charset = "";
						UI.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding(65001);
						UI.Current.Response.WriteFile(strFileName);
						System.Threading.Thread DeleteThread = new System.Threading.Thread(DeleteExcelFile);
						DeleteThread.Start(strFileName);
						UI.Current.Response.End();
					} catch (Exception ex3) {
						throw ex3;
					} finally {
						System.Threading.Thread.CurrentThread.CurrentCulture = oldCultureInfo;
						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
					}
				} catch (Exception ex2) {
					throw ex2;
				}
			}
		}
		private void DeleteExcelFile(string Data)
		{
			System.Threading.Thread.Sleep(1000);
			System.IO.File.Delete(Data);
		}
		private string GetColumnTitle(string MemberName)
		{
			string Word = null;
			if (MemberName.IndexOf(".") > -1) {
				string[] Words = MemberName.Split(".");
				if (Words[Words.Length - 1] == "Name" || Words[Words.Length - 1] == "Count") {
					Word = Words[Words.Length - 2];
				} else {
					Word = Words[Words.Length - 1];
				}
			} else {
				Word = MemberName;
			}
			return Strings.Replace(this.Page.Client.Dictionary.GetWord("Concept." + Word), "Concept.", "");
		}
		public CollectionBinderCustomization Customization {
			get {
				if ((this.Client != null) && this.Client.Session != null && this.Client.Session.User != null && this.Client.Session.User.ID > 0 && this.oCustomization == null && this.GetType().FullName != "Ophelia.Web.View.Binders.CollectionBinder") {
					this.oCustomization = this.Client.Application.GetCollection("Ophelia.Web.View.Binders.CollectionBinderCustomization").FilterBy("BinderUniqueName", this.GetType().FullName).FilterBy("User.ID", this.Client.Session.User.ID).FirstEntity;
					if (this.oCustomization == null) {
						this.oCustomization = this.Client.Application.GetEntity("Ophelia.Web.View.Binders.CollectionBinderCustomization", 0);
						this.oCustomization.BinderUniqueName = this.GetType().FullName;
						this.oCustomization.User = this.Client.Session.User;
						this.oCustomization.Save();
					}
				}
				return this.oCustomization;
			}
		}
		//Public ReadOnly Property ColumnCustomizations() As CollectionBinderColumnCustomizationCollection
		//    Get
		//        If Not Me.Client Is Nothing AndAlso Me.oColumnCustomizations Is Nothing AndAlso Not Me.Client.Session Is Nothing AndAlso Me.Client.Session.User.ID > 0 AndAlso Me.GetType.FullName <> "Ophelia.Web.View.Binders.CollectionBinder" Then
		//            Me.oColumnCustomizations = Me.Client.Application.GetCollection("Ophelia.Web.View.Binders.CollectionBinderColumnCustomization").FilterBy("CollectionBinderCustomization.BinderUniqueName", Me.GetType.FullName).FilterBy("CollectionBinderCustomization.User.ID", Me.Client.Session.User.ID)
		//            Me.oColumnCustomizations.Definition.RequestProperty("CollectionBinderCustomization.IsDefaultCustomization", AccessType.OverRelation)
		//            Me.oColumnCustomizations.Definition.Sorters.Add("Indis", SortDirection.Ascending)
		//        End If
		//        Return Me.oColumnCustomizations
		//    End Get
		//End Property
		protected virtual void SetPersistentColumnVisibilities()
		{
			if ((this.Client != null)) {
				if (this.Customization != null && this.Customization.ColumnCustomizations != null && this.Customization.ColumnCustomizations.Count > 0) {
					CollectionBinderColumnCustomization ColumnCustomization = this.Customization.ColumnCustomizations.FirstEntity;
					if ((ColumnCustomization != null) && ColumnCustomization.CollectionBinderCustomization.IsDefaultCustomization == 0) {
						int n = 0;
						Ophelia.Web.View.Base.DataGrid.Column Column = default(Ophelia.Web.View.Base.DataGrid.Column);
						int LastIndex = (this.Customization.ColumnCustomizations.LastEntity == null ? -1 : ((Binders.CollectionBinderColumnCustomization)this.Customization.ColumnCustomizations.LastEntity).Indis);
						for (n = 0; n <= this.Columns.Count - 1; n++) {
							Column = this.Columns(n);
							ColumnCustomization = this.Customization.ColumnCustomizations((!string.IsNullOrEmpty(Column.MemberName) ? Column.MemberName : Column.MemberNameToBeDrawn));
							if (ColumnCustomization == null) {
								this.Customization.ColumnCustomizations.Add((!string.IsNullOrEmpty(Column.MemberName) ? Column.MemberName : Column.MemberNameToBeDrawn), LastIndex + 1, (Column.Style.Display == DisplayMethod.Hidden ? 0 : 1), this.Customization);
							}
						}
						this.Columns.ExpandedColumns.Clear();
						this.Columns.CollapsedColumns.Clear();
						for (n = 0; n <= this.Customization.ColumnCustomizations.Count - 1; n++) {
							Column = this.Columns(this.Customization.ColumnCustomizations(n).MemberName);
							if (Column == null) {
								Column = this.Columns.ItemByMemberNameToBeDrawn(this.Customization.ColumnCustomizations(n).MemberName);
							}
							if ((Column != null)) {
								if (this.Customization.ColumnCustomizations(n).Visible > 0) {
									if (!this.Columns.ExpandedColumns.Contains(Column)) {
										this.Columns.ExpandedColumns.Add(Column);
									}
								} else {
									if (!this.Columns.CollapsedColumns.Contains(Column)) {
										this.Columns.CollapsedColumns.Add(Column);
									}
								}
							}
						}
					}
				}
			}
		}
		public CollectionBinderConfiguration Configuration {
			get {
				if (this.oConfiguration == null) {
					this.oConfiguration = new CollectionBinderConfiguration(this);
				}
				return this.oConfiguration;
			}
		}
		public new EntityColumnCollection Columns {
			get { return base.Columns; }
		}
		public Application.Base.Entity Entity {
			get { return this.Binding.Item; }
		}
		public Application.Base.EntityCollection Collection {
			get { return this.Binding.Value; }
		}
		public Pager Pager {
			get {
				if (this.oPager == null) {
					this.oPager = new Pager(this.Binding.Value);
					this.oPager.ID = this.ID + "pager";
				}
				return this.oPager;
			}
		}
		public Client Client {
			get { return this.Page.Client; }
		}
		public int PageSize {
			get { return this.nPageSize; }
			set { this.nPageSize = value; }
		}
		public bool BindAllPages {
			get { return this.bBindAllPages; }
			set { this.bBindAllPages = value; }
		}
		protected override string GetHierarchicalSortOrder(Base.DataGrid.Row Row)
		{
			if (Row != null) {
				if ((Row.Item != null) && (Row.Item.GetType.GetProperty("SortOrder") != null)) {
					return Row.Item.GetType.GetProperty("SortOrder").GetValue(Row.Item, null);
				}
			}
			return base.GetHierarchicalSortOrder(Row);
		}
		public Application.Framework.Dictionary Dictionary {
			get { return this.Client.Dictionary; }
		}
		protected override object CreateColumnCollection()
		{
			return new EntityColumnCollection(this);
		}
		public override void Binding_ValueInitiliazed(ref object Value)
		{
			base.Binding_ValueInitiliazed(Value);
			if (this.Collection.IsHierarchical)
				this.HierarchicDisplay = true;
			if (this.PageSize > 0) {
				if (this.Collection.IsHierarchical && this.PageSize < this.Collection.Count)
					this.PageSize = this.Collection.Count;
				this.Collection.Definition.PageSize = this.PageSize;
			}
		}
		internal override void UpdateColumnTotal(Base.DataGrid.Column Column)
		{
			if (this.ShowTotals) {
				if (Column.GetType.FullName == "Ophelia.Web.View.Base.DataGrid.NumberBoxColumn" || Column.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Base.DataGrid.NumberBoxColumn"))) {
					Ophelia.Web.View.Base.DataGrid.NumberBoxColumn NumberBoxColumn = (Ophelia.Web.View.Base.DataGrid.NumberBoxColumn)Column;
					NumberBoxColumn.AddValueToTotalValue(0, true);
					if (NumberBoxColumn.CalculateBySum) {
						NumberBoxColumn.AddValueToTotalValue(this.Binding.Value.Sum(Column.MemberName));
					} else {
						this.OnColumnTotalRequested(new Ophelia.Web.View.Base.DataGrid.ColumnEventArgs(Column));
					}
				}
			}
		}
		public override void Bind()
		{
			if (this.Collection.Application != null && this.Collection.Application.DataSource != null) {
				bool Required = true;
				if (this.Collection.Count >= this.PageSize)
					Required = false;
				if (!Required) {
					if (this.Binding.Item == null) {
						if ((this.Binding.Value != null))
							((EntityCollection)this.Binding.Value).Refresh();
					} else {
						this.Binding.Refresh();
					}
				}
			}
			base.Bind();
		}
		protected virtual void CollectionBinder_BindingStarted(object Sender, ref BindingEventArgs e)
		{
			this.ArrangeForBinding();
		}
		protected override Base.DataGrid.Row OnCreatingExtraRow()
		{
			Base.DataGrid.Row Row = base.OnCreatingExtraRow();
			Row.Item = this.Collection.Add;
			return Row;
		}
		private void ArrangeForBinding()
		{
			this.SetPersistentColumnVisibilities();
			if (this.PageSize > 0) {
				this.Collection.Definition.PageSize = this.PageSize;
				this.BindingStartIndex = this.Pager.LowerIndex;
				this.BindingEndIndex = this.Pager.UpperIndex;
				if (this.BindingEndIndex >= this.Collection.Count) {
					this.BindingEndIndex = this.Collection.Count - 1;
				} else if (this.BindAllPages) {
					this.BindingEndIndex = this.Collection.Count - 1;
				}
			}
		}
		public virtual string GetRowsInString()
		{
			this.ArrangeForBinding();
			this.Bind();
			Content Content = new Content();
			this.DrawRows(Content, new Hashtable(), "");
			return Content.Value;
		}
		private Style oMoreDataLinkStyle;
		public Style MoreDataLinkStyle {
			get {
				if (this.oMoreDataLinkStyle == null) {
					this.oMoreDataLinkStyle = new Style();
					this.oMoreDataLinkStyle.CursorStyle = Cursor.Pointer;
					this.oMoreDataLinkStyle.Font.Color = "gray";
					this.oMoreDataLinkStyle.Font.Weight = Forms.FontWeight.Bold;
				}
				return this.oMoreDataLinkStyle;
			}
		}

		public override void OnBeforeDraw(Content Content)
		{
			this.ArrangeForBinding();
			if (this.PageSize > 0 && this.Collection.Pages.Count > 1 && !AlwaysHidePager) {
				if (HasAjaxDelegate && PagerWithMoreButton) {
					Panel MoreButton = new Panel(this.ID + "_NextPageButton");
					MoreButton.Content.Add(this.Page.Client.Dictionary.GetWord("Concept.MoreRecord"));
					MoreButton.SetStyle(this.MoreDataLinkStyle);
					MoreButton.OnClickEvent = this.AjaxDelegate.DrawByPageEvent();
					this.Footer.DefaultRegion.Controls.Add(MoreButton);
					this.Footer.DefaultRegion.Controls.AddHiddenBox(this.ID + "_NextPageIndex", (this.Pager.CurrentPage + 1).ToString());
				} else {
					this.Footer.DefaultRegion.Controls.Add(Pager);
				}
			}
			if (this.HasAjaxDelegate) {
				Content.Add(this.AjaxDelegate.Draw());
				//Şimdilik sadece selectedRow'lar için işlem yapıldığından bu şart kalsın.
				if (this.CheckBoxes) {
					this.BeforeRowDrawn += BinderBeforeRowDrawn;
					DrawRowSelectedScript();
				}
			}
			base.OnBeforeDraw(Content);
		}
		private string[] SelectedRowItemIDs;
		private void BinderBeforeRowDrawn(object Sender, ref Ophelia.Web.View.Base.DataGrid.RowEventArgs e)
		{
			SelectedRowItemIDs = this.Page.QueryString("BinderSelectedRowItemIDs").Split(",");
			for (int i = 0; i <= SelectedRowItemIDs.Length - 1; i++) {
				if (SelectedRowItemIDs[i] == e.Row.ItemID) {
					e.Row.IsSelected = true;
					this.Script.AppendLine(" DataGridRowSelectedItemChanged(document.getElementById('" + e.Row.ID + "CheckBox" + "'),'" + e.Row.ItemID + "'); ");
				}
			}
			e.Row.SelectedBehavior += " DataGridRowSelectedItemChanged(this,'" + e.Row.ItemID + "'); ";
		}
		private void DrawRowSelectedScript()
		{
			if (this.Script.Function("DataGridRowSelectedItemChanged") == null) {
				System.Text.StringBuilder returnString = new System.Text.StringBuilder();
				returnString.AppendLine("var selectedIDs = document.getElementById('" + this.ID + "_SelectedRowItemIDs').innerHTML.split(','); ");
				returnString.AppendLine("var IDsArray = new Array(); ");
				returnString.AppendLine("for (var i=0; i< selectedIDs.length;i++){");
				returnString.AppendLine("    if (selectedIDs[i] != clickedItemID && selectedIDs[i] != ''){");
				returnString.AppendLine("        IDsArray.push(selectedIDs[i]); ");
				returnString.AppendLine("    }  ");
				returnString.AppendLine("}  ");
				returnString.AppendLine("if (element.checked == true){ ");
				returnString.AppendLine("    IDsArray.push(clickedItemID); ");
				returnString.AppendLine("} ");
				returnString.AppendLine("document.getElementById('" + this.ID + "_SelectedRowItemIDs').innerHTML = IDsArray.join(','); ");

				this.Script.AddFunction("DataGridRowSelectedItemChanged", returnString.ToString(), "element,clickedItemID");
			}
		}
		protected override void Customize(string Title)
		{
			base.Customize(Title);
			this.oToolBar = new Binders.Toolbar.ToolBar(this);
			this.ConfigureToolbar();
		}
		protected override void OnAfterFormDraw(Content Content)
		{
			base.OnAfterFormDraw(Content);
		}
		protected override void AddConfirmDeleteActionFunction()
		{
			if ((this.Client != null))
				this.Script.AddConfirmFunction(this.ID + "_DeleteConfirmation", this.Client.Dictionary.GetWord("Message.DeleteConfirmation"));
		}
		public CollectionBinder(string ID) : base(ID)
		{
			BindingStarted += CollectionBinder_BindingStarted;
		}
		public CollectionBinder(string Title, string ID) : base(Title, ID)
		{
			BindingStarted += CollectionBinder_BindingStarted;
		}
		public CollectionBinder(string Title, string MemberName, string ID) : base(Title, MemberName, ID)
		{
			BindingStarted += CollectionBinder_BindingStarted;
		}
		public CollectionBinder(Entity Entity, string MemberName, string ID) : base(Entity, MemberName, ID)
		{
			BindingStarted += CollectionBinder_BindingStarted;
		}
		public CollectionBinder(Entity Entity, string Title, string MemberName, string ID) : base(Entity, Title, MemberName, ID)
		{
			BindingStarted += CollectionBinder_BindingStarted;
		}
		public CollectionBinder(string Title, EntityCollection Collection, string ID) : base(Title, Collection, ID)
		{
			BindingStarted += CollectionBinder_BindingStarted;
			if (this.Collection.IsHierarchical)
				this.HierarchicDisplay = true;
		}
		public CollectionBinder(EntityCollection Collection, string ID) : base(Collection, ID)
		{
			BindingStarted += CollectionBinder_BindingStarted;
			if (this.Collection.IsHierarchical)
				this.HierarchicDisplay = true;
		}
		public CollectionBinder(EntityCollection Collection) : base(Collection, "")
		{
			BindingStarted += CollectionBinder_BindingStarted;
			this.ID = this.GetType().Name;
			if (this.Collection.IsHierarchical)
				this.HierarchicDisplay = true;
		}
		public CollectionBinder() : base("", "")
		{
			BindingStarted += CollectionBinder_BindingStarted;
		}
	}
}

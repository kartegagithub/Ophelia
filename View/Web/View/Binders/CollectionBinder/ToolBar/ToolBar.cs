using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Base.DataGrid;
namespace Ophelia.Web.View.Binders.Toolbar
{
	public class ToolBar : View.Web.Base.DataGrid.ToolBar
	{
		private CollectionBinder oCollectionBinder;
		private ToolBarButton oSearchButton;
		private ToolBarButton oReportButton;
		private ToolBarButton oTotalsButton;
		private ToolBarButton oCreateExcelDocumentButton;
		private ToolBarButton oConfigureButton;
		private Web.Binders.CollectionBinderViewCustomization oCollectionBinderViewCustomization;
		internal CollectionBinder CollectionBinder {
			get { return this.oCollectionBinder; }
		}
		public Web.Binders.CollectionBinderViewCustomization CollectionBinderViewCustomization {
			get {
				if (this.oCollectionBinderViewCustomization == null) {
					this.oCollectionBinderViewCustomization = new Web.Binders.CollectionBinderViewCustomization(this.DataGrid);
				}
				return this.oCollectionBinderViewCustomization;
			}
		}
		public ToolBarButton ReportButton {
			get {
				if (this.oReportButton == null) {
					this.oReportButton = this.Buttons.AddButton(this.DataGrid.ID + "ReportButton", "", "", "");
				}
				return this.oReportButton;
			}
		}
		public ToolBarButton SearchButton {
			get {
				if (this.oSearchButton == null) {
					this.oSearchButton = this.Buttons.AddButton(this.DataGrid.ID + "SearchButton", "Search", "", "");
				}
				return this.oSearchButton;
			}
		}
		public ToolBarButton CreateExcelDocumentButton {
			get {
				if (this.oCreateExcelDocumentButton == null) {
					this.oCreateExcelDocumentButton = this.Buttons.AddButton(this.DataGrid.ID + "CreateExcelDocumentButton", "SendListToExcel", "", "");
					this.oCreateExcelDocumentButton.OnClickEvent = this.CollectionBinder.AjaxDelegate.ExportToExcelEvent();
				}
				return this.oCreateExcelDocumentButton;
			}
		}
		public ToolBarButton ConfigureButton {
			get {
				if (this.oConfigureButton == null) {
					this.oConfigureButton = this.Buttons.AddButton(this.DataGrid.ID + "ConfigureButton", "ListCustomization", "", "");
					this.oConfigureButton.OnClickEvent = this.CollectionBinderViewCustomization.ShowEvent();
				}
				return this.oConfigureButton;
			}
		}
		public ToolBarButton TotalsButton {
			get {
				if (this.oTotalsButton == null) {
					this.oTotalsButton = this.Buttons.AddButton(this.DataGrid.ID + "TotalsButton", "", "", "");
				}
				return this.oTotalsButton;
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			base.OnBeforeDraw(Content);
			Content.Add(this.CollectionBinderViewCustomization.Draw());
		}
		internal override void UpdateButtonVisibilities()
		{
			base.UpdateButtonVisibilities();
			//Buray� if'li �ekilde yazmam�n nedeni, e�er butonlar g�r�nmeyecekse, �zellik �zerinden Visible property'sini �a��rd���mda instance'� olu�mas�n.
			if (this.CollectionBinder.Configuration.AllowNew) {
				this.NewButton.Visible = true;
			}
			if (this.CollectionBinder.Configuration.AllowRefresh) {
				this.RefreshButton.Visible = true;
			}
			if (this.CollectionBinder.Configuration.AllowPrint) {
				this.PrintButton.Visible = true;
			}
			if (this.CollectionBinder.Configuration.AllowReport) {
				this.ReportButton.Visible = true;
			}
			if (this.CollectionBinder.Configuration.AllowCreateExcelDocument) {
				this.CreateExcelDocumentButton.Visible = true;
			}
			if (this.CollectionBinder.Configuration.AllowConfiguration) {
				this.ConfigureButton.Visible = true;
			}
			if (this.CollectionBinder.Configuration.AllowHelp) {
				this.HelpButton.Visible = true;
			}
		}
		public ToolBar(CollectionBinder CollectionBinder) : base(CollectionBinder)
		{
			this.oCollectionBinder = CollectionBinder;
		}
	}
}

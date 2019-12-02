using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ToolBar : WebControl
	{
		private ToolBarButton oNewButton;
		private ToolBarButton oPrintButton;
		private ToolBarButton oHelpButton;
		private ToolBarButton oRefreshButton;
		private ToolBarButtonCollection oButtons;
		private DataGrid oDataGrid;
		private bool bVisible = false;
		public bool Visible {
			get { return this.bVisible && this.Buttons.Count > 0; }
			set { this.bVisible = value; }
		}
		public DataGrid DataGrid {
			get { return this.oDataGrid; }
		}
		public ToolBarButtonCollection Buttons {
			get { return this.oButtons; }
		}
		public ToolBarButton NewButton {
			get {
				if (this.oNewButton == null) {
					this.oNewButton = this.Buttons.AddButton(this.DataGrid.ID + "NewButton", "CreateNewEntry", this.DataGrid.NewLinkUrl, ServerSide.ImageDrawer.GetImageUrl("WebNew2"));
					this.oNewButton.ViewOrder = 1;
				}
				return this.oNewButton;
			}
		}
		public ToolBarButton RefreshButton {
			get {
				if (this.oRefreshButton == null) {
					this.oRefreshButton = this.Buttons.AddButton(this.DataGrid.ID + "RefreshButton", "Refresh", UI.Current.Request.Url.AbsoluteUri, "");
					this.oRefreshButton.ViewOrder = 2;
				}
				return this.oRefreshButton;
			}
		}
		public ToolBarButton PrintButton {
			get {
				if (this.oPrintButton == null) {
					this.oPrintButton = this.Buttons.AddButton(this.DataGrid.ID + "PrintButton", "SendListToPrinter", "", "");
					this.oPrintButton.ViewOrder = 3;
				}
				return this.oPrintButton;
			}
		}
		public ToolBarButton HelpButton {
			get {
				if (this.oHelpButton == null) {
					this.oHelpButton = this.Buttons.AddButton(this.DataGrid.ID + "HelpButton", "", "", "");
					this.oHelpButton.OnClickEvent = "alert('You are on your own. Try to succeed without help!');";
					this.oHelpButton.ViewOrder = 4;
				}
				return this.oHelpButton;
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.UpdateButtonVisibilities();
			if (this.Buttons.Count > 0) {
				this.Buttons.SortButtonsInOrder();
				Content.Add("<td name=\"CB_" + this.DataGrid.ID + "_TOOLBAR\" id=\"CB_" + this.DataGrid.ID + "_TOOLBAR\" colspan=\"" + this.DataGrid.DrawnTableColumnCount + "\" " + this.DataGrid.Columns.Style.Draw + " >");
				for (int i = 0; i <= this.Buttons.Count - 1; i++) {
					Content.Add(this.Buttons(i).Draw());
				}
				Content.Add("</td>");
			}
			//Return Content.Value
		}
		//Public Overrides Sub OnBeforeDrawn(ByRef Content As Content)

		//End Sub
		//Public Function Draw() As String
		//    Dim Content As New Content
		//    Me.OnBeforeDrawn(Content)

		//End Function

		internal virtual void UpdateButtonVisibilities()
		{
			if (!string.IsNullOrEmpty(this.DataGrid.NewLinkUrl)) {
				this.oNewButton.Visible = true;
			}
		}
		public ToolBar(DataGrid DataGrid)
		{
			this.oDataGrid = DataGrid;
			this.oButtons = new ToolBarButtonCollection(this);
		}
	}
}

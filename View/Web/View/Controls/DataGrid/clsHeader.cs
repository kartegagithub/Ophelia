using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.DataGrid
{
	public class Header
	{
		private DataGrid oDataGrid;
		private Structure.Structure oControl;
		private string sTitle;
		private Style oStyle;
		private bool bShowCount = false;
		private bool bShowTitle = true;
		public readonly string CountSeperator = "|";
		public bool ShowTitle {
			get {
				if (string.IsNullOrEmpty(this.Title))
					return false;
				return this.bShowTitle;
			}
			set { this.bShowTitle = value; }
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public bool ShowCount {
			get { return this.bShowCount; }
			set { this.bShowCount = value; }
		}
		internal string Draw()
		{
			string HeaderText = string.Empty;
			if (this.ShowTitle && this.ShowCount) {
				HeaderText = this.Title + " " + this.CountSeperator + " ";
				if (this.DataGrid.Binding.Value.GetType.IsSubclassOf(Type.GetType("Ophelia.Application.Base.EntityCollection"))) {
					EntityCollection EntityCollection = (Ophelia.Application.Base.EntityCollection)this.DataGrid.Binding.Value;
					if (EntityCollection.Definition.PageSize > -1) {
						if (EntityCollection.Definition.Page > 0) {
							if (DataGrid.ExtraRow != null) {
								HeaderText += (EntityCollection.Definition.Page - 1) * EntityCollection.Definition.PageSize + 1 + " - " + (EntityCollection.Definition.Page - 1) * EntityCollection.Definition.PageSize + EntityCollection.Pages((EntityCollection.Definition.Page - 1)).Count - 1 + " / " + EntityCollection.Count - 1;
							} else {
								HeaderText += (EntityCollection.Definition.Page - 1) * EntityCollection.Definition.PageSize + 1 + " - " + (EntityCollection.Definition.Page - 1) * EntityCollection.Definition.PageSize + EntityCollection.Pages((EntityCollection.Definition.Page - 1)).Count + " / " + EntityCollection.Count;
							}
						} else {
							HeaderText += "0";
						}
					} else {
						if (this.DataGrid.ExtraRow != null) {
							HeaderText += this.DataGrid.Binding.Value.Count() - 1;
						} else {
							HeaderText += this.DataGrid.Binding.Value.Count();
						}
					}
				} else {
					if (this.DataGrid.ExtraRow != null) {
						HeaderText += this.DataGrid.Binding.Value.Count() - 1;
					} else {
						HeaderText += this.DataGrid.Binding.Value.Count();
					}
				}
			} else if (this.ShowTitle) {
				HeaderText = this.Title;
			} else if (this.ShowCount) {
				if (this.DataGrid.ExtraRow != null) {
					HeaderText = this.DataGrid.Binding.Value.Count - 1;
				} else {
					HeaderText = this.DataGrid.Binding.Value.Count;
				}
			}
			if (!string.IsNullOrEmpty(HeaderText)) {
				Content Content = new Content();
				this.DefaultRegion.Content.Clear();
				this.DefaultRegion.Content.Add(HeaderText);
				Content.Add("<tr>");
				Content.Add("<td name=\"CB_" + this.DataGrid.ID + "FormHeader\" id=\"CB_" + this.DataGrid.ID + "FormHeader\" colspan=\"" + DataGrid.DrawnTableColumnCount + "\" " + this.Style.Draw + " >");
				Content.Add(this.Control.Draw());
				Content.Add("</td>");
				Content.Add("</tr>");
				return Content.Value;
			}
			return "";
		}
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = new Style();
					this.oStyle.BackgroundColor = "#dcddde";
				}
				return this.oStyle;
			}
		}
		public Structure.Structure Control {
			get {
				if (this.oControl == null) {
					this.oControl = new Structure.Structure("Header");
					this.oControl.ParentControl = this.DataGrid.ParentControl;
					this.oControl.Container = this.DataGrid.Container;
					this.oControl.Style.HorizontalAlignment = SectionAlignment.Left;
					this.oControl.Style.Font.Color = "black";
					this.oControl.Style.Font.Weight = Forms.FontWeight.Bold;
					this.oControl.Style.Dock = DockStyle.Fill;
				}
				return this.oControl;
			}
		}
		public Structure.Cells.Cell DefaultRegion {
			get { return this.Control(0, 0); }
		}
		public DataGrid DataGrid {
			get { return this.oDataGrid; }
		}
		public Header(DataGrid DataGrid)
		{
			this.oDataGrid = DataGrid;
		}
	}
}

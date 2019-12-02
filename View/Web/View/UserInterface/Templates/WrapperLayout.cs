using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.Structure;
using Ophelia.Web.View.Controls.Structure.Cells;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.UI.Templates
{
	public abstract class WrapperLayout : UI.Page
	{
		private Structure oHeaderLayout;
		private Cell oHeader;
		private Structure oFooter;
		private Structure ContentRegionLayout;
		private Structure oLayout;
		private Menu.Menu oMenu;
		private Menu.Menu oSubMenu;
		public Menu.Menu Menu {
			get {
				if (this.oMenu == null) {
					this.oMenu = new Menu.Menu("menu");
					this.MenuRegion.Controls.Add(this.oMenu);
					this.ConfigureMenu();
				}
				return this.oMenu;
			}
		}
		public Menu.Menu SubMenu {
			get {
				if (this.Menu != null && this.oSubMenu == null) {
					this.oSubMenu = new Menu.Menu("submenu");
					this.MenuRegion.Controls.Add(this.oSubMenu);
					this.ConfigureSubMenu();
				}
				return this.oSubMenu;
			}
		}
		protected virtual void ConfigureMenu()
		{
			this.oMenu.Separator = string.Empty;
			this.oMenu.Direction = View.Web.Controls.Menu.Menu.LayoutDirection.Horizontal;
			this.oMenu.Style.PaddingTop = 7;
			this.oMenu.ItemStyle.VerticalAlignment = Ophelia.Web.View.VerticalAlignment.Middle;
			this.oMenu.ItemStyle.SetFont(-1, "#FFFFFF", Ophelia.Web.View.Forms.FontWeight.Bold);
			this.oMenu.ItemStyle.SetPadding(8, 4, 40, 4);
			this.oMenu.ItemStyle.Height = 24;
			this.oMenu.SelectedItemStyle.Borders.Width = -1;
			this.oMenu.SelectedItemStyle.Height = 27;
		}
		protected virtual void ConfigureSubMenu()
		{
			this.oSubMenu.Separator = string.Empty;
			this.oSubMenu.Direction = View.Web.Controls.Menu.Menu.LayoutDirection.Horizontal;
			this.oSubMenu.Style.PaddingTop = 7;
			this.oSubMenu.ItemStyle.VerticalAlignment = Ophelia.Web.View.VerticalAlignment.Middle;
			this.oSubMenu.ItemStyle.SetFont(8, "#FFFFFF", Ophelia.Web.View.Forms.FontWeight.Bold);
			this.oSubMenu.ItemStyle.SetPadding(8, 4, 40, 4);
			this.oSubMenu.ItemStyle.Height = 18;
			this.oSubMenu.SelectedItemStyle.Borders.Width = -1;
			this.oSubMenu.SelectedItemStyle.Height = 21;
		}
		public Menu.MenuItem AddMenuItem(string ID, string Url, string BackgroundColor = "gray", string BorderColor = "black", byte UrlInNewWindow = false)
		{
			Menu.MenuItem MenuItem = this.Menu.MenuItems.Add(ID, this.GetUrl(Url), UrlInNewWindow);
			MenuItem.Style.BackgroundColor = BackgroundColor;
			MenuItem.Style.Borders.Bottom.Color = BorderColor;
			MenuItem.Style.Borders.Bottom.Width = 3;
			MenuItem.SelectionStyle.BackgroundColor = BorderColor;
			if (this.RawUrl.Contains(Url))
				MenuItem.Select();
			return MenuItem;
		}
		public Menu.MenuItem AddSubMenuItem(string Name, string Url, string BackgroundColor = "gray", string BorderColor = "black", byte UrlInNewWindow = false)
		{
			Menu.MenuItem MenuItem = this.SubMenu.MenuItems.Add(Name, this.GetUrl(Url), UrlInNewWindow);
			MenuItem.Style.BackgroundColor = BackgroundColor;
			MenuItem.Style.Borders.Bottom.Color = BorderColor;
			MenuItem.Style.Borders.Bottom.Width = 3;
			MenuItem.SelectionStyle.BackgroundColor = BorderColor;
			if (this.RawUrl.Contains(Url))
				MenuItem.Select();
			return MenuItem;
		}
		protected Structure Layout {
			get { return this.oLayout; }
		}
		public Cell HeaderControl {
			get { return this.oHeader; }
		}
		public Structure HeaderLayout {
			get { return this.oHeaderLayout; }
		}
		public Structure FooterControl {
			get { return this.oFooter; }
		}
		public Cell MenuRegion {
			get { return this.ContentRegionLayout(0, 0); }
		}
		public Cell ContentRegion {
			get { return this.ContentRegionLayout(1, 0); }
		}
		protected abstract Controls.DataControl GetLogo();
		protected abstract int GetHeaderHeight();
		protected abstract int GetFooterHeight();
		protected abstract int GetLayoutWidth();
		protected abstract string GetConceptColor();

		protected virtual void OnBeforeLoad()
		{
		}
		protected override sealed void OnBeforeDraw()
		{
			base.OnBeforeDraw();
			Style Style = new Style();
			Style.Font.Family = this.Layout.Style.Font.Family;
			Style.Font.Size = this.Layout.Style.Font.Size;
			Style.Font.Unit = this.Layout.Style.Font.Unit;
			this.StyleSheet.AddCustomRule("TD,INPUT,TEXTAREA,SELECT", Style);
			this.StyleSheet.AddCustomRule("A", "text-decoration:none;");
			this.StyleSheet.AddCustomRule("A:hover", "text-decoration:underline;");
			if (this.Menu.MenuItems.Count == 0) {
				this.MenuRegion.Controls.Remove(this.Menu);
			}
			if (this.SubMenu.MenuItems.Count == 0) {
				this.MenuRegion.Controls.Remove(this.SubMenu);
			}
		}
		protected override sealed void OnLoadingStarted()
		{
			base.OnLoadingStarted();
			this.OnBeforeLoad();
			this.oLayout = new Ophelia.Web.View.Controls.Structure.Structure("Layout", 2, 2);
			this.Controls.Add(this.oLayout);
			this.Layout.Technique = LayoutTechnique.Css;
			this.Layout.StyleSheet.AddCustomRule("Html,Body", "height:100%;margin:0;padding:0;");

			this.Layout.Style.Font.Family = "Arial";
			this.Layout.Style.Font.Size = 10;


			Layout.Style.WidthInPercent = 100;
			Layout.Style.HeightInPercent = 100;
			Layout.Style.Margin = 0;
			Layout.Style.Padding = 0;
			Layout.Rows(0).Style.MinHeightInPercent = 100;
			Layout.Rows(0).Style.Float = FloatType.None;
			Layout(0, 0).ColumnSpan = 2;
			Layout(1, 0).ColumnSpan = 2;
			Layout(0, 0).Style.PaddingBottom = this.GetFooterHeight();
			Layout(0, 0).Style.MarginInAuto = 0;
			Layout(0, 0).Style.Width = this.GetLayoutWidth();

			this.oHeaderLayout = new Ophelia.Web.View.Controls.Structure.Structure("Header", 2, 2);
			this.oHeaderLayout(0, 0).ColumnSpan = 2;
			this.oHeaderLayout(0, 0).RowSpan = 2;
			this.oHeaderLayout.Technique = LayoutTechnique.Css;
			this.oHeaderLayout.Style.WidthInPercent = 100;
			this.oHeaderLayout.Style.Height = this.GetHeaderHeight();
			this.oHeaderLayout.Style.PositionStyle = Position.Absolute;
			this.oHeaderLayout.Style.PositionLeft = 0;
			this.HeaderLayout.Style.PositionTop = 0;
			this.oHeaderLayout.Style.BackgroundColor = this.GetConceptColor();
			//Me.oHeaderLayout.Rows(0).Style.BackgroundColor = 
			this.oHeaderLayout.Rows(0).Style.PositionStyle = Position.Relative;
			this.oHeaderLayout.Rows(0).Style.Width = this.GetLayoutWidth();
			this.oHeaderLayout.Rows(0).Style.Height = this.oHeaderLayout.Style.Height;
			this.oHeaderLayout.Rows(0).Style.MarginInAuto = 0;
			this.oHeader = this.oHeaderLayout(0, 0);
			Link MainPageLink = new Link("", this.GetUrl(""), this.GetLogo().Draw());
			this.HeaderControl.Controls.Add(MainPageLink);
			this.StyleSheet.AddCustomRule("#" + this.HeaderControl.ID, "margin:auto;width:" + this.GetLayoutWidth() + "px");
			this.Layout(0, 0).Controls.Add(this.HeaderLayout);

			this.ContentRegionLayout = Layout(0, 0).Controls.AddStructure("Content", 3, 1);
			this.ContentRegionLayout.Technique = LayoutTechnique.Css;
			this.ContentRegionLayout.Rows(0).Style.PaddingTop = this.GetHeaderHeight();
			this.ContentRegionLayout(0, 0).Style.WidthInPercent = 100;

			this.ContentRegion.Style.Top = 10;

			this.ContentRegionLayout(1, 0).Style.WidthInPercent = 100;


			this.ContentRegionLayout.Rows(2).Style.Clear = ClearStyle.Both;
			this.ContentRegionLayout.Rows(2).Style.MarginBottom = 10;


			this.oFooter = new Structure("Footer");
			this.oFooter.Style.WidthInPercent = 100;
			this.Layout.Rows.LastRow.Cells.FirstCell.Controls.Add(this.oFooter);
			this.Layout.Rows.LastRow.Cells.FirstCell.Style.HorizontalAlignment = HorizontalAlignment.Center;

			this.StyleSheet.AddCustomRule("#" + this.FooterControl.ID, "margin:auto;width:" + this.GetLayoutWidth() + "px");
			this.oFooter.Technique = LayoutTechnique.Css;

			this.Layout.Rows.LastRow.Style.Clear = ClearStyle.Both;
			this.Layout.Rows.LastRow.Style.Height = this.GetFooterHeight();
			this.Layout.Rows.LastRow.Style.Top = -this.Layout.Rows.LastRow.Style.Height;
			this.Layout.Rows.LastRow.Style.BackgroundColor = this.GetConceptColor();

		}

	}
}

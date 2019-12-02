using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Menu
{
	public class MenuItem
	{
		private Menu oParentMenu;
		private Menu oSubMenu;
		private string sText = "";
		private string sUrl = "";
		private int nIndex;
		private Style oStyle;
		private Style oSelectionStyle;
		private Style oHoverStyle;
		private string sSelectionImageUrl;
		private string sImageUrl;
		private byte bUrlInNewWindow;
		public byte UrlInNewWindow {
			get { return this.bUrlInNewWindow; }
			set { this.bUrlInNewWindow = value; }
		}
		public Style SelectionStyle {
			get {
				if (this.oSelectionStyle == null) {
					this.oSelectionStyle = new Style();
				}
				return this.oSelectionStyle;
			}
		}
		public Style HoverStyle {
			get {
				if (this.oHoverStyle == null) {
					this.oHoverStyle = new Style();
				}
				return this.oHoverStyle;
			}
		}
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = new Style();
				}
				return this.oStyle;
			}
		}
		public bool IsSelected {
			get { return this.ParentMenu != null && object.ReferenceEquals(this.ParentMenu.SelectedItem, this); }
		}
		public void Select()
		{
			if (this.ParentMenu != null) {
				this.ParentMenu.Select(this);
			}
		}
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public string Url {
			get { return this.sUrl; }
			set { this.sUrl = value; }
		}
		public string ImageUrl {
			get { return this.sImageUrl; }
			set { this.sImageUrl = value; }
		}
		public string SelectionImageUrl {
			get { return this.sSelectionImageUrl; }
			set { this.sSelectionImageUrl = value; }
		}
		public int Index {
			get { return this.nIndex; }
			set { this.nIndex = value; }
		}
		public string Name {
			get { return this.ParentMenu.Name + "_" + this.Index; }
		}
		public Menu ParentMenu {
			get { return this.oParentMenu; }
		}
		public Menu SubMenu {
			get { return this.oSubMenu; }
		}
		public MenuItemCollection MenuItems {
			get { return this.SubMenu.MenuItems; }
		}
		private MenuItem()
		{
			this.oSubMenu = new Menu(this.Text);
			this.oSubMenu.MenuItem = this;
		}
		public MenuItem(Menu Menu) : this()
		{
			this.oParentMenu = Menu;
		}
	}
}

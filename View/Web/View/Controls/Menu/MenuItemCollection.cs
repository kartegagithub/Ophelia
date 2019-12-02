using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Menu
{
	public class MenuItemCollection : System.Collections.CollectionBase
	{
		private Menu oMenu;
		private Style oSelectedItemStyle;
		private Style oItemHoverStyle;
		private MenuItemStyle oStyle = new MenuItemStyle();
		private MenuItem oSelectedItem;
		public MenuItemStyle ItemStyle {
			get { return this.oStyle; }
		}
		public Style SelectedItemStyle {
			get {
				if (this.oSelectedItemStyle == null) {
					this.oSelectedItemStyle = new Style();
				}
				return this.oSelectedItemStyle;
			}
		}
		public Style ItemHoverStyle {
			get {
				if (this.oItemHoverStyle == null) {
					this.oItemHoverStyle = new Style();
				}
				return this.oItemHoverStyle;
			}
		}
		public MenuItem SelectedItem {
			get { return this.oSelectedItem; }
		}
		public MenuItem FirstMenuItem {
			get {
				if (this.Count > 0) {
					return this[0];
				}
				return null;
			}
		}
		public MenuItem LastMenuItem {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1];
				}
				return null;
			}
		}
		public Menu Menu {
			get { return this.oMenu; }
		}
		public MenuItem this[int Index] {
			get {
				if (Index > -1 && Index < this.List.Count) {
					return this.List[Index];
				}
				return null;
			}
		}
		public MenuItem Add(MenuItem MenuItem)
		{
			this.List.Add(MenuItem);
			MenuItem.Index = this.List.Count - 1;
			return MenuItem;
		}
		public MenuItem AddImage(string Url, string Source, string SelectedSource = "", byte UrlInNewWindow = false)
		{
			MenuItem MenuItem = default(MenuItem);
			MenuItem = new MenuItem(this.Menu);
			MenuItem.ImageUrl = Source;
			if (string.IsNullOrEmpty(SelectedSource)) {
				MenuItem.SelectionImageUrl = Source;
			} else {
				MenuItem.SelectionImageUrl = SelectedSource;
			}
			MenuItem.Url = Url;
			MenuItem.UrlInNewWindow = UrlInNewWindow;
			return this.Add(MenuItem);
		}
		public MenuItem Add(string Text, string Url, byte UrlInNewWindow = false)
		{
			MenuItem MenuItem = default(MenuItem);
			MenuItem = new MenuItem(this.Menu);
			MenuItem.Text = Text;
			MenuItem.Url = Url;
			MenuItem.UrlInNewWindow = UrlInNewWindow;
			return this.Add(MenuItem);
		}
		public MenuItemCollection(Menu Menu)
		{
			this.oMenu = Menu;
		}

	}
}

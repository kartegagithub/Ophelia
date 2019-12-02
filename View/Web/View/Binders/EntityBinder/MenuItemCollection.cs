using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class MenuItemCollection : CollectionBase
	{
		private Menu oMenu = null;
		public new MenuItem this[int Index] {
			get {
				if (Index > -1 && Index < this.List.Count) {
					return this.List[Index];
				}
				return null;
			}
			set {
				if (Index > -1 && Index < this.List.Count) {
					this.List[Index] = value;
				}
			}
		}
		public new MenuItem this[string Name] {
			get {
				for (int i = 0; i <= this.List.Count - 1; i++) {
					if (this.List[i] != null && this.List[i].ID == Name) {
						return this.List[i];
					}
				}
				return null;
			}
			set {
				for (int i = 0; i <= this.List.Count - 1; i++) {
					if (this.List[i] != null && this.List[i].ID == Name) {
						this.List[i] = value;
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
		}
		public Menu Menu {
			get { return this.oMenu; }
		}
		public MenuItem AddMenuItem(string ID)
		{
			MenuItem MenuItem = new MenuItem(this, ID, this.Menu.EntityBinder.Client.Dictionary.GetWord("Concept." + ID));
			this.List.Add(MenuItem);
			return MenuItem;
		}
		public MenuItemCollection(Menu Menu)
		{
			this.oMenu = Menu;
		}
	}
}

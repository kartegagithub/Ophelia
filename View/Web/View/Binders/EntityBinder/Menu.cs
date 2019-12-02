using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class Menu : Controls.ComplexWebControl
	{
		private MenuItemCollection oMenuItems = null;
		private EntityBinder oEntityBinder = null;
		private bool bUseDefaultStyle = true;
		public bool UseDefaultStyle {
			get { return this.bUseDefaultStyle; }
			set { this.bUseDefaultStyle = value; }
		}
		public MenuItemCollection MenuItems {
			get { return this.oMenuItems; }
		}
		public EntityBinder EntityBinder {
			get { return this.oEntityBinder; }
		}
		private void SetDefaultParameters()
		{
			if (this.UseDefaultStyle) {
				this.Page.Header.StyleSheet.AddCustomRule("#EntityBinderStructure .EntityBinderMenuItem", "height: 22px; padding: 8px 13px 0px;font-weight: bold;color:#666666;border:1px solid #BDBDBD;border-top:none; background-color:#DADADA;cursor:pointer;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityBinderStructure .EntityBinderMenuItem:hover", "background-color:#FFF; color:#666666;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityBinderStructure .active", "background-color:#FFF;color:#3C5766;");
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.SetDefaultParameters();
			Ophelia.Web.View.Controls.Structure.Structure EntityBinderMenuStructure = new Ophelia.Web.View.Controls.Structure.Structure(this.EntityBinder.ID + "_MenuStructure", 1, 2);
			EntityBinderMenuStructure.Style.VerticalAlignment = VerticalAlignment.Top;
			EntityBinderMenuStructure(0, 0).Style.Borders.Set(1, Forms.BorderStyle.Solid, "#D9D9D9");
			EntityBinderMenuStructure(0, 0).Style.Width = 175;
			EntityBinderMenuStructure(0, 0).Style.Height = 500;
			EntityBinderMenuStructure(0, 0).Style.Left = 10;
			EntityBinderMenuStructure(0, 0).Style.BackgroundColor = "#E6E6E6";
			EntityBinderMenuStructure(0, 1).Style.Height = 500;
			Controls.Panel Panel = new Controls.Panel(this.ID + "_Container");
			Panel.SetStyle(this.Style);
			Panel.CloneEventsFrom(this);
			for (int i = 0; i <= this.MenuItems.Count - 1; i++) {
				if (i == 0)
					this.MenuItems(i).Style.Class += " active";
				EntityBinderMenuStructure(0, 0).Content.Add(this.MenuItems(i).Draw());
				EntityBinderMenuStructure(0, 1).Content.Add(this.MenuItems(i).DrawGroups(i == 0));
			}
			Panel.Controls.Add(EntityBinderMenuStructure);
			Content.Add(Panel.Draw);
		}
		public Menu(EntityBinder EntityBinder)
		{
			this.ID = EntityBinder.ID + "_EntityBinderMenu";
			this.oEntityBinder = EntityBinder;
			this.oMenuItems = new MenuItemCollection(this);
			this.Style.BackgroundColor = "#F2F2F2";
		}
	}
}

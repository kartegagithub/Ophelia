using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class MenuItem : Controls.ComplexWebControl
	{
		private GroupCollection oGroups = null;
		private string sText = "";
		private MenuItemCollection oMenuItemCollection = null;
		public MenuItemCollection MenuItemCollection {
			get { return this.oMenuItemCollection; }
		}
		public GroupCollection Groups {
			get { return this.oGroups; }
		}
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public string DrawGroups(bool Enabled)
		{
			this.Page.StyleSheet.AddClassBasedRule(".MenuItemGroup div", "white-space:nowrap");
			System.Text.StringBuilder returnString = new System.Text.StringBuilder();
			Controls.Panel ContainerPanel = new Controls.Panel(this.ID + "_GroupContainer");
			ContainerPanel.Style.Class = this.MenuItemCollection.Menu.EntityBinder.ID + "_EntityBinderGroup";
			if (!Enabled) {
				ContainerPanel.Style.Display = DisplayMethod.Hidden;
			}
			for (int i = 0; i <= this.Groups.Count - 1; i++) {
				if (this.Groups.Count > 1) {
					this.Groups(i).Fields.Form.Style.Borders.Set(1, Forms.BorderStyle.Solid, "#BABABD");
					this.Groups(i).Style.PaddingTop = 20;
				}
				this.Groups(i).Style.PaddingLeft = 20;
				this.Groups(i).Fields.Form.Style.Class = "MenuItemGroup";
				this.Groups(i).Style.Float = FloatType.Left;
				if (this.Groups(i).Fields.Count == 1 && this.Groups(i).Fields.FirstField != null) {
					this.Groups(i).Fields.FirstField.ShowHeader = false;
				}
				ContainerPanel.Controls.Add(this.Groups(i));
				if (((i + 1) % this.MenuItemCollection.Menu.EntityBinder.GroupColumnCount) == 0) {
					this.Groups(i).Fields.Form.Style.PaddingTop = 20;
					Controls.Label WrapDiv = new Controls.Label("", "");
					WrapDiv.Style.Clear = ClearStyle.Both;
					ContainerPanel.Controls.Add(WrapDiv);
				} else {
					this.Groups(i).Fields.Form.Style.PaddingTop = 5;
				}
			}
			return returnString.AppendLine(ContainerPanel.Draw()).ToString();
		}
		public override void OnBeforeDraw(Content Content)
		{
			Controls.Panel Panel = new Controls.Panel(this.MenuItemCollection.Menu.EntityBinder.ID + "_" + this.ID + "_MenuItem");
			Panel.SetStyle(this.Style);
			Panel.CloneEventsFrom(this);
			Panel.Content.Add(this.Text);
			Content.Add(Panel.Draw);
		}
		private string GetOnClickEvent()
		{
			string returnString = "";
			returnString += " $('." + this.MenuItemCollection.Menu.EntityBinder.ID + "_EntityBinderGroup').hide(); ";
			returnString += " $('#" + this.MenuItemCollection.Menu.EntityBinder.ID + "_MenuStructure #" + this.ID + "_GroupContainer').show(); ";
			returnString += " $('.EntityBinderMenuItem').removeClass('active'); ";
			returnString += " $(this).addClass('active'); ";
			return returnString;
		}
		public MenuItem(MenuItemCollection MenuItemCollection, string ID, string Text)
		{
			this.ID = ID;
			this.sText = Text;
			this.oMenuItemCollection = MenuItemCollection;
			this.oGroups = new GroupCollection(MenuItemCollection.Menu.EntityBinder);
			this.Style.Class = "EntityBinderMenuItem";
			this.OnClickEvent = GetOnClickEvent();
		}
	}
}

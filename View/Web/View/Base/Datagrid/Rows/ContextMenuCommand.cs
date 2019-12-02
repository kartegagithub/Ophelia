using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ContextMenuCommand : WebControl
	{
		private ContextMenu oContextMenu;
		private string sTitle = "";
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public ContextMenu ContextMenu {
			get { return this.oContextMenu; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			Panel command = new Panel(this.ContextMenu.ID + "_" + this.ID);
			command.SetStyle(this.Style);
			command.CloneEventsFrom(this);
			if (string.IsNullOrEmpty(this.Title)) {
				command.Content.Add(this.ID);
			} else {
				command.Content.Add(this.Title);
			}
			for (int i = 0; i <= this.Attributes.Count - 1; i++) {
				command.Attributes.Add(this.Attributes.Keys(i), this.Attributes.Values(i));
			}
			Content.Add(command.Draw);
		}
		public ContextMenuCommand(string Title, string ID, string OnClickEvent, ContextMenu ContextMenu)
		{
			this.ID = ID;
			this.sTitle = Title;
			this.OnClickEvent = OnClickEvent;
			this.oContextMenu = ContextMenu;
			this.Style.Class = "ContextMenuCommand";
			this.Page.StyleSheet.AddCustomRule(".ContextMenuCommand", "background-color:#FFF;box-shadow:3px 5px 3px gray; text-align:left; color:gray; cursor:pointer; padding:4px 25px 2px 5px;font-weight:normal;white-space:nowrap;");
			this.Page.StyleSheet.AddCustomRule(".ContextMenuCommand:hover", "background-color:gray; color:white");
		}
	}
}

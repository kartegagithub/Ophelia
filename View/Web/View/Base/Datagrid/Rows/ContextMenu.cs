using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ContextMenu : WebControl
	{
		private ContextMenuCommandCollection oCommands;
		private Row oRow;
		public Row Row {
			get { return this.oRow; }
		}
		public ContextMenuCommandCollection Commands {
			get {
				if (this.oCommands == null) {
					this.oCommands = new ContextMenuCommandCollection(this);
				}
				return this.oCommands;
			}
		}
		public ContextMenuCommand AddCommand(string Title, string ID, string OnClickEvent)
		{
			ContextMenuCommand Command = new ContextMenuCommand(Title, ID, OnClickEvent, this);
			this.Commands.Add(Command);
			return Command;
		}
		public override void OnBeforeDraw(Content Content)
		{
			Panel contextMenuContainer = new Panel(this.Row.ID + "_ContextMenu");
			this.Page.StyleSheet.AddClassBasedRule("RowContextMenu", "width:100%;height:100%;padding:0;marging:0;display:none;");
			contextMenuContainer.Style.Class = "RowContextMenu";

			Image contextMenuImage = new Image("", this.Page.Client.ApplicationBase + "?DisplayImage=ResourceName,,bulletdown2.png$$$Namespace,,Ophelia");
			this.Page.StyleSheet.AddCustomRule("#CB_" + Row.DataGrid.ID + " .RowIDClassName div img", "width:100%;height:100%;display:block;padding:0;margin:0");
			if (this.Commands.Count == 0) {
				contextMenuContainer.Style.Opacity = 0;
				Content.Add(contextMenuContainer.Draw());
				//Görsel problem olmaması için çizilsin.
				return;
			}
			contextMenuImage.OnClickEvent = "ToggleContextMenu('" + this.Row.ID + "_ContextMenuCommandContainer','contextMenuCommandContainer');RowIDHoverFunction();";
			Panel contextMenuCommandContainer = new Panel(this.Row.ID + "_ContextMenuCommandContainer");
			this.Page.StyleSheet.AddClassBasedRule("contextMenuCommandContainer", "position:absolute;z-index:999;background: url(\"" + this.Page.Client.ApplicationBase + "?DisplayImage=ResourceName,,transparentImage.png$$$Namespace,,Ophelia\") repeat scroll 0 0 transparent; margin-top:-22px; padding-top:20px;padding-bottom:15px;padding-right:15px; display:none;");
			contextMenuCommandContainer.Style.Class = "contextMenuCommandContainer";
			contextMenuCommandContainer.OnClickEvent = "$('.contextMenuCommandContainer').hide('fast');";

			this.Page.StyleSheet.AddClassBasedRule("CommandGroup", "display:table-cell");
			Panel CommandGroup = new Panel("CommandGroup_0");
			CommandGroup.Style.Class = "CommandGroup";
			for (int i = 0; i <= this.Commands.Count - 1; i++) {
				if (i >= 3 && (i % 3) == 0) {
					CommandGroup = new Panel("CommandGroup_" + i.ToString());
					CommandGroup.Style.Borders.Left.SetInput("#CCC", 1, Forms.BorderStyle.Solid);
					CommandGroup.Style.Class = "CommandGroup";
				}
				CommandGroup.Controls.Add(this.Commands(i));
				if (contextMenuCommandContainer.Controls("CommandGroup_" + i.ToString()) == null) {
					contextMenuCommandContainer.Controls.Add(CommandGroup);
				}
			}

			contextMenuContainer.Controls.Add(contextMenuImage);
			Content.Add(contextMenuContainer.Draw());
			Content.Add(contextMenuCommandContainer.Draw());


			if (this.Script.Function("ToggleContextMenu") == null) {
				this.Page.ScriptManager.FirstScript.AppendLine("var ToogleMenuCanBeClosed = false;");
				this.Page.ScriptManager.FirstScript.AppendLine("$(document).ready(function() { RowIDHoverFunction(); }); ");


				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function RowIDHoverFunction = this.Script.AddFunction("RowIDHoverFunction", "", "");
				RowIDHoverFunction.AppendLine("$('.RowIDClassName').hover(function() { }, function() { if (ToogleMenuCanBeClosed) { $('.contextMenuCommandContainer').hide('fast');} });");

				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function RowFunction = this.Script.AddFunction("ToggleContextMenu", "", "VisibleMenuID,MenuClassName");
				RowFunction.AppendLine(" ToogleMenuCanBeClosed = false;");
				RowFunction.AppendLine("var CurrentMenuIsVisible = true;");
				RowFunction.AppendLine(" if ($('#' + VisibleMenuID).css('display') == 'none')");
				RowFunction.AppendLine("       CurrentMenuIsVisible = false;");
				RowFunction.AppendLine("  $('.' + MenuClassName).hide('fast');");
				RowFunction.AppendLine(" if (CurrentMenuIsVisible == false) {");
				RowFunction.AppendLine("  $('#' + VisibleMenuID).show('fast',function() { ToogleMenuCanBeClosed = true; });}");
			}
		}
		public ContextMenu(Row Row)
		{
			this.oRow = Row;
			this.ID = Row.ID + "_ContextMenu";
		}
	}
}

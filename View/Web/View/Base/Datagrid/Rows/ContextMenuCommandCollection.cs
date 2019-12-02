using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ContextMenuCommandCollection : Ophelia.Application.Base.CollectionBase
	{
		private ContextMenu oContextMenu;
		public ContextMenu ContextMenu {
			get { return this.oContextMenu; }
		}
		public new ContextMenuCommand this[int Index] {
			get { return base.Item(Index); }
			set { base.Item(Index) = value; }
		}
		public ContextMenuCommand FirstContextMenuCommand {
			get {
				if (this.Count > 0) {
					return this[0];
				}
				return null;
			}
		}
		public ContextMenuCommand LastContextMenuCommand {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1];
				}
				return null;
			}
		}
		public virtual ContextMenuCommand Add(ContextMenuCommand ContextMenuCommand)
		{
			this.List.Add(ContextMenuCommand);
			return ContextMenuCommand;
		}
		public virtual bool Remove(ContextMenuCommand ContextMenuCommand)
		{
			if (this.List.Contains(ContextMenuCommand))
				this.List.Remove(ContextMenuCommand);
			return true;
		}
		public ContextMenuCommandCollection(ContextMenu ContextMenu)
		{
			this.oContextMenu = ContextMenu;
		}
	}
}

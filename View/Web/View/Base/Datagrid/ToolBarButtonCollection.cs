using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ToolBarButtonCollection : Ophelia.Application.Base.CollectionBase
	{
		private ToolBar oToolbar = null;
		public ToolBar Toolbar {
			get { return this.oToolbar; }
		}
		public new ToolBarButton this[int Index] {
			get {
				if (Index > -1 && Index < this.List.Count) {
					return this.List(Index);
				}
				return null;
			}
			set {
				if (Index > -1 && Index < this.List.Count) {
					this.List(Index) = value;
				}
			}
		}
		public void SetButtonStyleClass(string ClassName)
		{
			for (int i = 0; i <= this.List.Count - 1; i++) {
				this[i].Style.Class = ClassName;
			}
		}
		public void SortButtonsInOrder()
		{
			int index = -1;
			while (!(index == this.List.Count - 1)) {
				index += 1;
				if (index + 1 <= this.List.Count - 1) {
					if (this[index + 1].ViewOrder < this[index].ViewOrder) {
						this.MoveItemDown(this[index + 1]);
						index = -1;
					}
				}
			}
		}
		public ToolBarButton AddButton(string ID, string ToolTip, string URL, string DefaultImageSource)
		{
			ToolBarButton oButton = new ToolBarButton(this);
			oButton.ID = ID;
			oButton.Url = URL;
			oButton.DefaultImageSource = DefaultImageSource;
			if (!string.IsNullOrEmpty(ToolTip))
				oButton.ToolTipText = this.Toolbar.DataGrid.Page.Client.Dictionary.GetWord("Concept." + ToolTip);
			this.List.Add(oButton);
			return oButton;
		}
		public ToolBarButtonCollection(ToolBar Toolbar)
		{
			this.oToolbar = Toolbar;
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ToolBarButton : WebControl
	{
		private ToolBarButtonCollection oToolBarButtonCollection = null;
		private string sToolTipText = "";
		private string sUrl = "";
		private string sDefaultImageSource = "";
		private bool bVisible = false;
		private int iViewOrder = 0;
		public ToolBarButtonCollection Collection {
			get { return this.oToolBarButtonCollection; }
		}
		public int ViewOrder {
			get { return this.iViewOrder; }
			set { this.iViewOrder = value; }
		}
		public string ToolTipText {
			get { return this.sToolTipText; }
			set { this.sToolTipText = value; }
		}
		public string Url {
			get { return this.sUrl; }
			set {
				this.sUrl = value;
				if (!string.IsNullOrEmpty(value))
					this.Visible = true;
			}
		}
		public string DefaultImageSource {
			get { return this.sDefaultImageSource; }
			set { this.sDefaultImageSource = value; }
		}
		public bool Visible {
			get { return this.bVisible; }
			set {
				this.bVisible = value;
				if (value) {
					this.oToolBarButtonCollection.Toolbar.Visible = true;
				}
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			if (this.Visible) {
				Link NewLink = new Link(this.ID);
				NewLink.CloneEventsFrom(this);
				NewLink.SetStyle(this.Style);
				if (!this.Style.IsCustomized)
					NewLink.Style.Float = FloatType.Right;
				NewLink.ParentControl = this.oToolBarButtonCollection.Toolbar.DataGrid.ParentControl;
				NewLink.Container = this.oToolBarButtonCollection.Toolbar.DataGrid.Container;
				if (string.IsNullOrEmpty(this.Style.BackgroundImage) && this.Style.BackgroundImageLeft == int.MinValue) {
					Image NewLinkImage = new Image("", this.DefaultImageSource);
					NewLink.Value = NewLinkImage.Draw;
				}
				if (!string.IsNullOrEmpty(this.Url)) {
					NewLink.Url = this.Url;
				}
				if (!string.IsNullOrEmpty(this.ToolTipText)) {
					NewLink.Title = this.ToolTipText;
				}
				Content.Add(NewLink.Draw());
			}
		}
		public ToolBarButton(ToolBarButtonCollection ToolBarButtonCollection, bool Visibility = false)
		{
			this.oToolBarButtonCollection = ToolBarButtonCollection;
			this.bVisible = Visibility;
		}
	}
}

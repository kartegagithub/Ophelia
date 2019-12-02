using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders
{
	public class BinderGroup : Controls.ComplexWebControl
	{
		private BinderCollection oBinders = null;
		private ArrayList oWebControls = null;
		private BinderGroupCollection oCollection = null;
		private BinderManager oBinderManager = null;
		private string sTitle = "";
		private bool bIsDefault = false;
		public void SetAsDefault()
		{
			for (int i = 0; i <= this.Collection.Count - 1; i++) {
				this.Collection(i).bIsDefault = false;
			}
			this.bIsDefault = true;
		}
		public bool IsDefault {
			//Set which bindergroup will be drawn while EntityForm is requested from client
			get { return this.bIsDefault; }
		}
		public BinderGroupCollection Collection {
			get { return this.oCollection; }
		}
		public BinderManager BinderManager {
			get { return this.oBinderManager; }
		}
		public BinderCollection Binders {
			get {
				if (this.oBinders == null) {
					this.oBinders = new BinderCollection();
				}
				return this.oBinders;
			}
		}
		public ArrayList WebControls {
			get {
				if (this.oWebControls == null) {
					this.oWebControls = new ArrayList();
				}
				return this.oWebControls;
			}
		}
		public string Title {
			get { return sTitle; }
			set { sTitle = value; }
		}
		public Ophelia.Web.View.Controls.WebControl AddCustomControl(ref Ophelia.Web.View.Controls.WebControl WebControl)
		{
			this.WebControls.Add(WebControl);
			return WebControl;
		}
		public EntityBinder.EntityBinder AddBinder(ref EntityBinder.EntityBinder Binder)
		{
			this.Binders.Add(Binder);
			return Binder;
		}
		public CollectionBinder AddCollectionBinder(ref CollectionBinder CollectionBinder)
		{
			this.Binders.Add(CollectionBinder);
			return CollectionBinder;
		}
		public override void OnBeforeDraw(Content Content)
		{
			Controls.Panel Panel = new Controls.Panel(this.ID + "_Sheet");
			this.Style.Class = "sheetContainer";
			Panel.SetStyle(this.Style);
			Panel.Style.Clear = ClearStyle.Both;
			Panel.CloneEventsFrom(this);
			for (int i = 0; i <= this.Binders.Count - 1; i++) {
				if (this.Binders(i).GetType.BaseType.Name == "CollectionBinder" && ((CollectionBinder)this.Binders(i)).Configuration.AllowNew) {
					((CollectionBinder)this.Binders(i)).ConfigureToolbar();
					((CollectionBinder)this.Binders(i)).Configuration.AllowRefresh = false;
					//HACK Şimdilik sadece yeni kayıt akışı sağlanacak.
					((CollectionBinder)this.Binders(i)).Configuration.AllowConfiguration = false;
					((CollectionBinder)this.Binders(i)).Configuration.AllowCreateExcelDocument = false;
					((CollectionBinder)this.Binders(i)).Configuration.AllowPrint = false;
					((CollectionBinder)this.Binders(i)).Configuration.AllowReport = false;
					((CollectionBinder)this.Binders(i)).Configuration.AllowHelp = false;
					((CollectionBinder)this.Binders(i)).ToolBar.Visible = true;

					Ophelia.Web.View.Controls.Panel ToolbarContainerPanel = new Ophelia.Web.View.Controls.Panel("ToolbarContainer");
					this.Page.StyleSheet.AddIDBasedRule("CB_" + this.Binders(i).ID + "_TOOLBAR", "background-color:#848484 !important;");
					ToolbarContainerPanel.Style.WidthInPercent = 100;
					ToolbarContainerPanel.Style.BackgroundColor = "#848484";
					ToolbarContainerPanel.Style.Display = DisplayMethod.InlineBlock;
					ToolbarContainerPanel.Content.Add(((CollectionBinder)this.Binders(i)).ToolBar.Draw);
					Panel.Controls.Add(ToolbarContainerPanel);
					((CollectionBinder)this.Binders(i)).Configuration.AllowNew = false;
					((CollectionBinder)this.Binders(i)).ToolBar.Visible = false;
				}
				Panel.Controls.Add(this.Binders(i));
			}
			for (int i = 0; i <= this.WebControls.Count - 1; i++) {
				Panel.Controls.Add(this.WebControls[i]);
			}
			Content.Add(Panel.Draw);
		}
		public BinderGroup(BinderGroupCollection BinderGroupCollection)
		{
			this.oCollection = BinderGroupCollection;
		}
	}
}

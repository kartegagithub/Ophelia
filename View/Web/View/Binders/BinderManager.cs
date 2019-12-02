using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders
{
	public class BinderManager
	{
		private BinderGroupCollection oBinderGroups;
		private Forms.EntityForm oEntityForm = null;
		protected Client Client {
			get { return this.oEntityForm.Client; }
		}
		public BinderGroupCollection BinderGroups {
			get { return this.oBinderGroups; }
		}
		public Ophelia.Web.View.Controls.WebControl AddCustomControl(string Title, ref Ophelia.Web.View.Controls.WebControl WebControl, bool DrawAsDefault = false)
		{
			BinderGroup BinderGroup = this.oBinderGroups.AddBinderGroup(WebControl.ID);
			if (DrawAsDefault)
				BinderGroup.SetAsDefault();
			BinderGroup.AddCustomControl(WebControl);
			BinderGroup.Title = Title;
			return WebControl;
		}
		public EntityBinder.EntityBinder AddBinder(string Title, ref EntityBinder.EntityBinder Binder, bool DrawAsDefault = false)
		{
			BinderGroup BinderGroup = this.oBinderGroups.AddBinderGroup(Binder.ID);
			if (DrawAsDefault)
				BinderGroup.SetAsDefault();
			BinderGroup.AddBinder(Binder);
			BinderGroup.Title = Title;
			return Binder;
		}
		public CollectionBinder AddCollectionBinder(string Title, ref CollectionBinder CollectionBinder, bool DrawAsDefault = false)
		{
			BinderGroup BinderGroup = this.oBinderGroups.AddBinderGroup(CollectionBinder.ID);
			if (DrawAsDefault)
				BinderGroup.SetAsDefault();
			CollectionBinder.EntityForm = this.oEntityForm;
			BinderGroup.AddCollectionBinder(CollectionBinder);
			BinderGroup.Title = Title;
			return CollectionBinder;
		}
		public BinderManager(Forms.EntityForm EntityForm)
		{
			this.oBinderGroups = new BinderGroupCollection(this);
			this.oEntityForm = EntityForm;
		}
	}
}

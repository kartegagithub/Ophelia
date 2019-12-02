using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
using Ophelia.Web.View;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class EntityBinderAjaxDelegate : AjaxControl
	{
		private EntityBinder oBinder = null;
		public EntityBinder Binder {
			get { return this.oBinder; }
			set { this.oBinder = value; }
		}

		public virtual void CreateAjaxFunctions()
		{
		}
		protected override void OnLoad()
		{
			if (this.Binder == null) {
				throw new Exception("BinderPropertyMustBeSet");
			}
			this.CreateAjaxFunctions();
		}
		public EntityBinderAjaxDelegate()
		{
		}
	}
}

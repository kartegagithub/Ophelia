using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class EntityBinderField
	{
		private Web.Binders.Field oControlField = null;
		private EntityBinder oEntityBinder = null;
		private string sMemberName = "";
		private string sMenuName = "";
		private string sGroupName = "";
		public string MemberName {
			get { return this.sMemberName; }
			set { this.sMemberName = value; }
		}
		public string GroupName {
			get { return this.sGroupName; }
			set { this.sGroupName = value; }
		}
		public string MenuName {
			get { return this.sMenuName; }
			set { this.sMenuName = value; }
		}
		public Binders.Field ControlField {
			get { return this.oControlField; }
			set { this.oControlField = value; }
		}
		public EntityBinder EntityBinder {
			get { return this.oEntityBinder; }
			set { this.oEntityBinder = value; }
		}
		public EntityBinderField(EntityBinder EntityBinder, string MemberName, string MenuName, string GroupName)
		{
			if (!string.IsNullOrEmpty(MenuName) || GroupName == "BaseGroup") {
				this.sMemberName = MemberName;
				this.sGroupName = GroupName;
				this.sMenuName = MenuName;
				this.oEntityBinder = EntityBinder;
				switch (new System.Diagnostics.StackFrame(1).GetMethod().Name) {
					case "AddLabel":
						this.ControlField = this.oEntityBinder.FieldsForm.Fields.AddLabel(MemberName);
						break;
					case "AddCheckBox":
						this.ControlField = this.oEntityBinder.FieldsForm.Fields.AddCheckBox(MemberName);
						break;
					case "AddDateTimePicker":
						this.ControlField = this.oEntityBinder.FieldsForm.Fields.AddDateTimePicker(MemberName);
						this.ControlField.Control.ReadOnly = true;
						break;
					case "AddGrid":
						this.ControlField = this.oEntityBinder.FieldsForm.Fields.AddGrid(MemberName, null);
						break;
				}
			} else {
				throw new Exception("MenuName Is Required.");
			}
		}
	}
}

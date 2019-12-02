using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders
{
	public class BinderGroupCollection : Ophelia.Application.Base.CollectionBase
	{
		private BinderManager oBinderManager = null;
		public BinderManager BinderManager {
			get { return this.oBinderManager; }
		}
		public new BinderGroup this[int Index] {
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
		internal IList Items {
			get { return this.List; }
		}
		public BinderGroup FirstBinderGroup {
			get { return this[0]; }
		}
		public BinderGroup LastBinderGroup {
			get {
				if (this.Count == 0) {
					return null;
				} else {
					return this[this.Count - 1];
				}
			}
		}
		public BinderGroup AddBinderGroup(string ID)
		{
			BinderGroup BinderGroup = new BinderGroup(this);
			BinderGroup.ID = ID;
			if (this.List.Count == 0)
				BinderGroup.SetAsDefault();
			this.List.Add(BinderGroup);
			return BinderGroup;
		}
		public BinderGroupCollection(BinderManager BinderManager)
		{
			this.oBinderManager = BinderManager;
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders
{
	public class BinderCollection : Ophelia.Application.Base.CollectionBase
	{
		public new Base.Binders.Binder this[int Index] {
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
		public new Base.Binders.Binder this[string MemberName] {
			get {
				Base.Binders.Binder Binder = null;
				foreach ( Binder in this) {
					if (Binder.Binding.MemberName == MemberName) {
						return Binder;
					}
				}
				return null;
			}
		}
		internal IList Items {
			get { return this.List; }
		}
		public Base.Binders.Binder FirstBinder {
			get { return this[0]; }
		}
		public Base.Binders.Binder LastBinder {
			get {
				if (this.Count == 0) {
					return null;
				} else {
					return this[this.Count - 1];
				}
			}
		}
		public EntityBinder.EntityBinder Add(ref EntityBinder.EntityBinder Binder)
		{
			this.List.Add(Binder);
			return Binder;
		}
		public CollectionBinder Add(ref CollectionBinder CollectionBinder, BinderGroup BinderGroup = null)
		{
			this.List.Add(CollectionBinder);
			return CollectionBinder;
		}
	}
}

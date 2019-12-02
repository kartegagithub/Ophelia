using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
using Ophelia.View.Base.Binders;
using Ophelia.View;
namespace Ophelia.Web.View.Base.Binders
{
	public abstract class Binder : Controls.ComplexWebControl, IBinder
	{
		private BinderState eBindState = BinderState.Pending;
		private Ophelia.View.Base.Binders.Binding withEventsField_oBinding;
		private Ophelia.View.Base.Binders.Binding oBinding {
			get { return withEventsField_oBinding; }
			set {
				if (withEventsField_oBinding != null) {
					withEventsField_oBinding.ItemInitiliazed -= Binding_ItemInitiliazed;
					withEventsField_oBinding.ValueInitiliazed -= Binding_ValueInitiliazed;
				}
				withEventsField_oBinding = value;
				if (withEventsField_oBinding != null) {
					withEventsField_oBinding.ItemInitiliazed += Binding_ItemInitiliazed;
					withEventsField_oBinding.ValueInitiliazed += Binding_ValueInitiliazed;
				}
			}
		}
		public event RefreshStartedEventHandler RefreshStarted;
		public delegate void RefreshStartedEventHandler(object Sender, ref BindingEventArgs e);
		public event RefreshCompletedEventHandler RefreshCompleted;
		public delegate void RefreshCompletedEventHandler(object Sender, ref BindingEventArgs e);
		public event RebindStartedEventHandler RebindStarted;
		public delegate void RebindStartedEventHandler(object Sender, ref BindingEventArgs e);
		public event RebindCompletedEventHandler RebindCompleted;
		public delegate void RebindCompletedEventHandler(object Sender, ref BindingEventArgs e);
		public event BindingStartedEventHandler BindingStarted;
		public delegate void BindingStartedEventHandler(object Sender, ref BindingEventArgs e);
		public event BindingCompletedEventHandler BindingCompleted;
		public delegate void BindingCompletedEventHandler(object Sender, ref BindingEventArgs e);
		private bool bReadonly = true;
		public bool Readonly {
			get { return this.bReadonly; }
			set { this.bReadonly = value; }
		}
		public Binding Binding {
			get {
				if (this.oBinding == null) {
					this.oBinding = this.CreateBinding();
				}
				return this.oBinding;
			}
		}
		public BinderState BindState {
			get { return this.eBindState; }
			set { this.eBindState = value; }
		}
		public abstract void Bind();
		public void Bind(object Data)
		{
			if (!string.IsNullOrEmpty(this.Binding.MemberName)) {
				this.BindState = BinderState.Pending;
				this.Binding.Item = Data;
			} else {
				this.BindState = BinderState.Pending;
				this.Binding.Value = Data;
			}
			this.Bind();
		}
		public void Bind(object Data, string MemberName)
		{
			this.Binding.Item = Data;
			this.Binding.MemberName = MemberName;
			this.Bind();
		}
		public virtual void Rebind()
		{
			this.OnRebindStarted(new BindingEventArgs(this.Binding));
			this.BindState = BinderState.Pending;
			this.Bind();
			this.OnRebindCompleted(new BindingEventArgs(this.Binding));
		}
		public virtual void RefreshData()
		{
			this.OnRefreshStarted(new BindingEventArgs(this.Binding));
			this.BindState = BinderState.Pending;
			this.Binding.Refresh();
			this.Bind();
			this.OnRefreshCompleted(new BindingEventArgs(this.Binding));
		}
		void IBinder.Refresh()
		{
			RefreshData();
		}
		public virtual void Reset()
		{
			this.BindState = BinderState.Pending;
			this.Binding.Refresh();
		}
		public virtual void Destroy()
		{
			this.oBinding = null;
		}
		protected virtual Binding CreateBinding()
		{
			return new Binding();
		}
		internal virtual void OnRefreshStarted(BindingEventArgs e)
		{
			if (RefreshStarted != null) {
				RefreshStarted(this, e);
			}
		}
		internal virtual void OnRefreshCompleted(BindingEventArgs e)
		{
			if (RefreshCompleted != null) {
				RefreshCompleted(this, e);
			}
		}
		internal virtual void OnRebindStarted(BindingEventArgs e)
		{
			if (RebindStarted != null) {
				RebindStarted(this, e);
			}
		}
		internal virtual void OnRebindCompleted(BindingEventArgs e)
		{
			if (RebindCompleted != null) {
				RebindCompleted(this, e);
			}
		}
		internal virtual void OnBindingStarted(BindingEventArgs e)
		{
			if (BindingStarted != null) {
				BindingStarted(this, e);
			}
		}
		internal virtual void OnBindingCompleted(BindingEventArgs e)
		{
			if (BindingCompleted != null) {
				BindingCompleted(this, e);
			}
		}

		public virtual void Binding_ItemInitiliazed(object Item)
		{
		}

		public virtual void Binding_ValueInitiliazed(ref object Value)
		{
		}
	}
}

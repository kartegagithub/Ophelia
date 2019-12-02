using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public abstract class InputDataControl : DataControl, IInputDataControl
	{
		private string sOnChangeEvent;
		private string sOnBlurEvent;
		private string sOnFocusEvent = "";
		private string sName = "";
		private bool bDisabled = false;
		private int nTabIndex = -2;
		public int TabIndex {
			get { return this.nTabIndex; }
			set { this.nTabIndex = value; }
		}
		public virtual string OnChangeEvent {
			get { return this.sOnChangeEvent; }
			set { this.sOnChangeEvent = value; }
		}
		public bool Disabled {
			get { return this.bDisabled; }
			set { this.bDisabled = value; }
		}
		bool IInputDataControl.Disable {
			get { return Disabled; }
			set { Disabled = value; }
		}
		public string Name {
			get {
				if (string.IsNullOrEmpty(this.sName))
					return this.ID;
				return this.sName;
			}
			set { this.sName = value; }
		}
		public virtual string OnBlurEvent {
			get { return this.sOnBlurEvent; }
			set { this.sOnBlurEvent = value; }
		}
		public string OnFocusEvent {
			get { return this.sOnFocusEvent; }
			set { this.sOnFocusEvent = value; }
		}
		protected override void DrawEvents(Content Content)
		{
			base.DrawEvents(Content);
			this.CheckEvent(this.OnBlurEvent);
			this.CheckEvent(this.OnChangeEvent);
			this.CheckEvent(this.OnFocusEvent);

			if (!string.IsNullOrEmpty(this.OnChangeEvent))
				Content.Add(" onchange=\"" + this.OnChangeEvent + "\"");
			if (!string.IsNullOrEmpty(this.OnFocusEvent))
				Content.Add(" onfocus=\"" + this.OnFocusEvent + "\"");
			if (!string.IsNullOrEmpty(this.OnBlurEvent))
				Content.Add(" onblur=\"" + this.OnBlurEvent + "\"");
			if (this.TabIndex > -2)
				Content.Add(" tabindex=\"" + this.TabIndex + "\"");

		}
		public override void CloneEventsFrom(WebControl WebControl)
		{
			base.CloneEventsFrom(WebControl);
			if (WebControl.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Controls.InputDataControl"))) {
				InputDataControl InputDataControl = WebControl;
				this.OnChangeEvent = InputDataControl.OnChangeEvent;
				this.OnFocusEvent = InputDataControl.OnFocusEvent;
				this.OnBlurEvent = InputDataControl.OnBlurEvent;
				this.TabIndex = InputDataControl.TabIndex;
			}
		}
	}
}

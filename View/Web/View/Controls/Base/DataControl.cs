using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Ophelia.Web.View.Controls
{
	public abstract class DataControl : WebControl, IDataControl
	{
		private string sValue = "";
		private bool bValueInitilize = false;
		private bool bReadOnly = false;
		private Validator.ValidatorCollection oValidators;
		private Style oHoverStyle;
		private string sTitle = "";
		private bool bNeedToValidate = true;
		public bool NeedToValidate {
			get { return this.bNeedToValidate; }
			set { this.bNeedToValidate = value; }
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public Validator.ValidatorCollection Validators {
			get { return this.oValidators; }
		}
		public Style HoverStyle {
			get {
				if (this.oHoverStyle == null) {
					this.oHoverStyle = new Style();
				}
				return this.oHoverStyle;
			}
		}
		protected virtual string InitilizeValue()
		{
			return this.Page.QueryString(this.ID);
		}
		public virtual string Value {
			get {
				if (!this.bValueInitilize && string.IsNullOrEmpty(this.sValue) && (this.Request != null)) {
					this.sValue = this.InitilizeValue();
				}
				return this.sValue;
			}
			set {
				this.bValueInitilize = true;
				this.sValue = value;
				this.OnControlValueChanged();
			}
		}
		public bool ReadOnly {
			get { return this.bReadOnly; }
			set { this.bReadOnly = value; }
		}

		protected virtual void OnControlValueChanged()
		{
		}
		protected override void DrawEvents(Content Content)
		{
			this.Validators.DrawValidatorString();
			this.StyleSheet.AddCustomRule("#" + this.ID + ":hover", this.HoverStyle.Draw);
			base.DrawEvents(Content);
		}
		private void CreateValidators()
		{
			this.oValidators = new Validator.ValidatorCollection(this);
		}
		public DataControl()
		{
			this.CreateValidators();
		}
	}
}

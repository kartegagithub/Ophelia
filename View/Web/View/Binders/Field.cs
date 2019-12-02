using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Binders
{
	public class Field : Container
	{
		private FieldCollection oCollection;
		private Label oLabel;
		private string sMemberName = "";
		protected WebControl oControl;
		private Ophelia.Web.View.Content oContent;
		private bool bIsRequired = false;
		private bool bBoldText = false;
		private Style oStyle;
		private Structure.Cells.Cell oSection;
		private WebControlCollection oControls;
		private bool bShowHeader = true;
		private Label oDescriptionLabel;
		private string sDescription = "";
		private int sGroupIndex = int.MinValue;
		private View.Base.Binders.Binding oBinding;
		public View.Base.Binders.Binding Binding {
			get { return this.oBinding; }
		}

		public virtual void Bind()
		{
		}

		public virtual void BindReverse(string Value)
		{
		}
		public int GroupIndex {
			get { return this.sGroupIndex; }
			set { this.sGroupIndex = value; }
		}
		public Structure.Cells.Cell Section {
			get { return this.oSection; }
			set { this.oSection = value; }
		}
		public virtual DataControl Control {
			get { return this.oControl; }
		}
		public FieldCollection Collection {
			get { return this.oCollection; }
		}
		public string MemberName {
			get { return this.sMemberName; }
			set { this.sMemberName = value; }
		}
		public string Description {
			get { return this.sDescription; }
			set { this.sDescription = value; }
		}
		public Label DescriptionLabel {
			get {
				if (!string.IsNullOrEmpty(this.Description) && this.oDescriptionLabel == null) {
					this.oDescriptionLabel = new Label(this.Control.ID + "Description");
					this.oDescriptionLabel.Value = this.Description;
					this.oDescriptionLabel.Style.Class = "formdescription";
				}
				return this.oDescriptionLabel;
			}
		}
		public bool ShowHeader {
			get { return this.bShowHeader; }
			set { this.bShowHeader = value; }
		}
		public Label Header {
			get { return this.oLabel; }
		}
		public bool IsRequired {
			get {
				if (this.bIsRequired) {
					this.Description = "(*)";
					this.DescriptionLabel.Style.Font.Color = "red";
				}
				return this.bIsRequired;
			}
		}
		public void SetAsRequired(string ValidationMessage = "", int MinLength = 1)
		{
			this.bIsRequired = true;
			if (string.IsNullOrEmpty(ValidationMessage))
				ValidationMessage = this.Header.Value + "IsRequired";
			this.Control.Validators.AddTextValidator(ValidationMessage, MinLength);
		}

		internal virtual new void OnBeforeDraw()
		{
		}

		protected virtual void CreateControls()
		{
		}
		public Field(ref FieldCollection Collection, string Header, string MemberName)
		{
			this.MemberName = MemberName;
			this.oCollection = Collection;
			this.oBinding = new View.Base.Binders.Binding();
			this.Binding.MemberName = MemberName;
			this.CreateControls();
			this.OnAfterCreateControls();
			this.Header.Value = Header;
		}
		private void OnAfterCreateControls()
		{
			this.oLabel = new Label(this.MemberName + "_Header");
			if (this.Collection != null) {
				this.oLabel.Style.Class = this.Collection.Form.ID + "_HeaderClassName";
				if (!string.IsNullOrEmpty(this.Collection.Form.FieldHeadersStyle.Class))
					this.oLabel.Style.Class += " " + this.Collection.Form.FieldHeadersStyle.Class;
			}
			this.Controls.Add(this.Control);
		}
	}
	public enum FieldDirection : int
	{
		Vertical = 1,
		Horizontal = 2
	}
}

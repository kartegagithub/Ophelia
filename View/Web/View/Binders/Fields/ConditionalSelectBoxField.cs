using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class ConditionalSelectBoxField : Field
	{
		private HttpRequest oRequest;
		private Web.Controls.Link oNewLink;
		private Web.Controls.Link oEditLink;
		private bool bEditInNewWindow = false;
		public new View.Web.Controls.ConditionalSelectBox Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			this.Control.SelectedItem = this.Binding.Value;
		}
		public string NewLinkUrl {
			get { return this.NewLink.Url; }
			set { this.NewLink.Url = value; }
		}
		public Web.Controls.Link NewLink {
			get {
				if (this.oNewLink == null) {
					this.oNewLink = new Controls.Link("NewLink");
					this.oNewLink.ID = this.Control.ID + "new";
					Controls.Label Label = new Controls.Label("NewLinkLabel");
					Label.Style.BackgroundImage = Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("WebNew2");
					Label.Style.Width = 15;
					Label.Style.Height = 15;
					Label.Style.Left = 5;
					this.oNewLink.Value = Label.Draw;
					this.oNewLink.Url = this.NewLinkUrl;
					this.oNewLink.Title = this.Page.Client.Dictionary.GetWord("Concept.New");
				}
				return this.oNewLink;
			}
			set { this.oNewLink = value; }
		}
		public string EditLinkUrl {
			get { return this.EditLink.Url; }
			set { this.EditLink.Url = value; }
		}
		public Web.Controls.Link EditLink {
			get {
				if (this.oEditLink == null) {
					this.oEditLink = new Controls.Link("EditLink");
					this.oEditLink.ID = this.Control.ID + "edit";
					Controls.Label Label = new Controls.Label("EditLinkLabel");
					Label.Style.BackgroundImage = Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("WebEdit");
					Label.Style.Width = 15;
					Label.Style.Height = 15;
					Label.Style.Left = 5;
					this.oEditLink.Value = Label.Draw;
					this.oEditLink.Title = this.Page.Client.Dictionary.GetWord("Concept.Edit");
				}
				return this.oEditLink;
			}
			set { this.oEditLink = value; }
		}
		private bool EditInNewWindow {
			get { return this.bEditInNewWindow; }
		}
		public void SetEditLinkUrl(string EditLinkUrl, bool EditInNewWindow)
		{
			this.EditLinkUrl = EditLinkUrl;
			this.bEditInNewWindow = EditInNewWindow;
		}
		internal override void OnBeforeDraw()
		{
			base.OnBeforeDraw();
			if (!string.IsNullOrEmpty(this.NewLinkUrl) || !string.IsNullOrEmpty(this.EditLinkUrl)) {
				this.Controls.Remove(this.Control);
				Controls.Structure.Structure Layout = this.Controls.AddStructure("Structure", 1, 2);
				if (!string.IsNullOrEmpty(this.NewLinkUrl)) {
					Layout(0, 1).Style.VerticalAlignment = VerticalAlignment.Middle;
					Layout(0, 1).Controls.Add(this.NewLink);
				}
				string ControlToString = "";
				if (!string.IsNullOrEmpty(this.EditLinkUrl)) {
					if (this.Control.SelectedValue == null)
						this.EditLink.Style.Display = DisplayMethod.Hidden;
					string Url = "";
					if (this.EditLinkUrl.IndexOf("?") > -1) {
						Url = this.EditLinkUrl.Insert(this.EditLinkUrl.IndexOf("?"), "$$");
					} else {
						Url = this.EditLinkUrl + "$$";
					}
					this.Control.OnChangeEvent = "SBValueChangedEditLinkUrl(this,'" + Url + "');" + this.Control.OnChangeEvent;
					this.OnControlValueChangedFunction();
					ControlToString = Control.Draw;
					if ((this.Control.Options.SelectedOption != null)) {
						this.EditLink.Url = Url.Replace("$$", this.Control.Options.SelectedOption.Value);
					} else {
						this.EditLink.Url = Url.Replace("$$", "0");
					}
					this.EditLink.NewWindow = this.EditInNewWindow;
					if (!string.IsNullOrEmpty(this.NewLinkUrl)) {
						Layout.Columns.Add();
						Layout(0, 2).Style.VerticalAlignment = VerticalAlignment.Middle;
						Layout(0, 2).Controls.Add(this.EditLink);
					} else {
						Layout(0, 1).Style.VerticalAlignment = VerticalAlignment.Middle;
						Layout(0, 1).Controls.Add(this.EditLink);
					}
				}
				if (string.IsNullOrEmpty(ControlToString))
					ControlToString = Control.Draw;
				Layout(0, 0).Content.Add(ControlToString);
			}
		}
		private void OnControlValueChangedFunction()
		{
			if (this.Script.Function("SBValueChangedEditLinkUrl") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function ChangeSelectedRow = this.Script.AddFunction("SBValueChangedEditLinkUrl", "", "element,url");
				ChangeSelectedRow.AppendLine("var elementid = element.id + 'edit'; ");
				ChangeSelectedRow.AppendLine("var editelement = document.getElementById(elementid);");
				ChangeSelectedRow.AppendLine("if (element.value == undefined ||element.value == ''|| element.value == '0'){");
				ChangeSelectedRow.AppendLine("    editelement.href = url.replace('$$',0);");
				ChangeSelectedRow.AppendLine("    editelement.style.display = 'none';");
				ChangeSelectedRow.AppendLine("}");
				ChangeSelectedRow.AppendLine("else {");
				ChangeSelectedRow.AppendLine("    editelement.href = url.replace('$$',element.value);");
				ChangeSelectedRow.AppendLine("    editelement.style.display = 'block';");
				ChangeSelectedRow.AppendLine("}");
			}
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.ConditionalSelectBox(this.MemberName);
		}
		public ConditionalSelectBoxField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}

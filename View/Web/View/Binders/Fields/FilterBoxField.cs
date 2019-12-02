using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.Fields
{
	public class FilterBoxField : Field
	{
		private string sNewLinkUrl = "";
		private string sEditLinkUrl = "";
		private bool bEditInNewWindow = false;
		public new View.Web.Controls.FilterBox Control {
			get { return base.Control; }
		}
		public override void Bind()
		{
			base.Bind();
			if (this.Binding.ValueItem != null) {
				this.Control.Value = this.Binding.ValueItem.Name;
			}
		}
		public string NewLinkUrl {
			get { return this.sNewLinkUrl; }
			set { this.sNewLinkUrl = value; }
		}
		public string EditLinkUrl {
			get { return this.sEditLinkUrl; }
			set { this.sEditLinkUrl = value; }
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
					Controls.Link NewLink = new Controls.Link("NewLink");
					NewLink.ID = this.Control.ID + "new";
					Controls.Label Label = new Controls.Label("NewLinkLabel");
					Label.Style.BackgroundImage = Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("WebNew2");
					Label.Style.Width = 15;
					Label.Style.Height = 15;
					Label.Style.Left = 5;
					NewLink.Value = Label.Draw;
					NewLink.Url = this.NewLinkUrl;
					Layout(0, 1).Style.VerticalAlignment = VerticalAlignment.Middle;
					Layout(0, 1).Controls.Add(NewLink);
				}
				if (!string.IsNullOrEmpty(this.EditLinkUrl)) {
					Controls.Link EditLink = new Controls.Link("EditLink");
					EditLink.ID = this.Control.ID + "edit";
					Controls.Label Label = new Controls.Label("EditLinkLabel");
					Label.Style.BackgroundImage = Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("WebEdit");
					Label.Style.Width = 15;
					Label.Style.Height = 15;
					Label.Style.Left = 5;
					EditLink.Value = Label.Draw;

					string Url = "";
					if (this.EditLinkUrl.IndexOf("?") > -1) {
						Url = this.EditLinkUrl.Insert(this.EditLinkUrl.IndexOf("?"), "$$");
					} else {
						Url = this.EditLinkUrl + "$$";
					}
					this.Control.OnChangeEvent = "SBValueChangedEditLinkUrl(this,'" + Url + "');";
					this.OnControlValueChangedFunction();

					int SelectedID = this.Control.SelectedEntity != null ? this.Control.SelectedEntity.ID : 0;

					EditLink.Url = Url.Replace("$$", SelectedID);

					EditLink.NewWindow = this.EditInNewWindow;
					if (!string.IsNullOrEmpty(this.NewLinkUrl)) {
						Layout.Columns.Add();
						Layout(0, 2).Style.VerticalAlignment = VerticalAlignment.Middle;
						Layout(0, 2).Controls.Add(EditLink);
					} else {
						Layout(0, 1).Style.VerticalAlignment = VerticalAlignment.Middle;
						Layout(0, 1).Controls.Add(EditLink);
					}
				}
				Layout(0, 0).Content.Add(Control.Draw);
			}
		}
		private void OnControlValueChangedFunction()
		{
			if (this.Script.Function("SBValueChangedEditLinkUrl") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function ChangeSelectedRow = this.Script.AddFunction("SBValueChangedEditLinkUrl", "", "element,url");
				ChangeSelectedRow.AppendLine("var elementid = element.id + 'edit'; ");
				ChangeSelectedRow.AppendLine("var editelement = document.getElementById(elementid + 'SelectedID');");
				ChangeSelectedRow.AppendLine("editelement.href = url.replace('$$',element.value);");
			}
		}
		protected override void CreateControls()
		{
			base.CreateControls();
			this.oControl = new View.Web.Controls.FilterBox(this.MemberName);
		}
		public FilterBoxField(ref FieldCollection Collection, string Header, string MemberName) : base(Collection, Header, MemberName)
		{
		}
	}
}


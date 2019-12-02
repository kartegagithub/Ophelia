using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class Group : Controls.ComplexWebControl
	{
		private GroupCollection oGroupCollection = null;
		private string sTitle = "";
		private string sGroupContent = "";
		private Ophelia.Web.View.Controls.Form.Form oFieldsForm = null;
		private ArrayList oSubGroups = null;
		public Ophelia.Web.View.Controls.Form.Form FieldsForm {
			get { return this.oFieldsForm; }
		}
		public GroupCollection GroupCollection {
			get { return this.oGroupCollection; }
		}
		public string Title {
			get { return sTitle; }
			set { this.sTitle = value; }
		}
		public FieldCollection Fields {
			get { return this.oFieldsForm.Fields; }
		}
		public string GroupContent {
			get { return sGroupContent; }
			set { this.sGroupContent = value; }
		}
		public ArrayList SubGroups {
			get {
				if (oSubGroups == null)
					oSubGroups = new ArrayList();
				return this.oSubGroups;
			}
		}
		private void BindFields()
		{
			for (int i = 0; i <= this.Fields.Count - 1; i++) {
				this.Fields(i).Binding.Item = this.GroupCollection.EntityBinder.Entity;
				this.Fields(i).Bind();
			}
		}
		private string DrawFieldsContent(bool Enabled = false)
		{
			this.BindFields();
			System.Text.StringBuilder returnString = new System.Text.StringBuilder();

			int CurrentIndex = int.MinValue;
			Ophelia.Web.View.Controls.Panel GroupDiv = new Ophelia.Web.View.Controls.Panel("");
			Ophelia.Web.View.Controls.Form.Form CurrentFieldsForm = null;
			bool GroupFound = false;
			Ophelia.Web.View.Controls.Form.Form UngroupedFieldsForm = new Ophelia.Web.View.Controls.Form.Form("UngroupedFields");
			UngroupedFieldsForm.FieldCellStyle.Font.Weight = Forms.FontWeight.Bold;
			for (int i = 0; i <= this.FieldsForm.Fields.Count - 1; i++) {
				if (this.FieldsForm.Fields(i).GroupIndex != int.MinValue) {
					if (CurrentIndex != this.FieldsForm.Fields(i).GroupIndex) {
						CurrentIndex = this.FieldsForm.Fields(i).GroupIndex;
						GroupFound = false;
						for (int j = 0; j <= this.SubGroups.Count - 1; j++) {
							if (this.SubGroups[j] != null && this.SubGroups[j].ID == this.ID + "_Group_" + CurrentIndex) {
								GroupDiv = this.SubGroups[j];
								CurrentFieldsForm = GroupDiv.Controls.LastControl;
								GroupFound = true;
							}
						}
						if (!GroupFound) {
							GroupDiv = new Ophelia.Web.View.Controls.Panel(this.ID + "_Group_" + CurrentIndex);
							GroupDiv.Style.Margin = 15;
							GroupDiv.Style.Float = FloatType.Left;
							GroupDiv.Style.Display = DisplayMethod.Table;
							GroupDiv.Style.BackgroundColor = "#FFF";
							GroupDiv.Style.Borders.Color = "#D1D3D5";
							GroupDiv.Style.Borders.Width = 1;
							GroupDiv.Style.Borders.Style = Forms.BorderStyle.Solid;
							this.SubGroups.Add(GroupDiv);
							CurrentFieldsForm = new Ophelia.Web.View.Controls.Form.Form(this.ID + "_Group_" + CurrentIndex + "_FieldsForm");
							CurrentFieldsForm.FieldCellStyle.Font.Weight = Forms.FontWeight.Bold;
							GroupDiv.Controls.Add(CurrentFieldsForm);
						}
					}
				//Ungrouped Field
				} else {
					UngroupedFieldsForm.Fields.Add(this.FieldsForm.Fields(i));
					continue;
				}
				CurrentFieldsForm.Fields.Add(this.FieldsForm.Fields(i));
			}
			if (UngroupedFieldsForm.Fields.Count > 0) {
				returnString.AppendLine(UngroupedFieldsForm.Draw());
			}
			for (int i = 0; i <= this.SubGroups.Count - 1; i++) {
				returnString.AppendLine(this.SubGroups[i].Draw());
			}
			return returnString.ToString();
		}
		public override void OnBeforeDraw(Content Content)
		{
			Controls.Panel Panel = new Controls.Panel(this.GroupCollection.EntityBinder.ID + "_" + this.ID + "_Group");
			Panel.OnClickEvent = "";
			Panel.SetStyle(this.Style);
			Panel.CloneEventsFrom(this);
			if (string.IsNullOrEmpty(this.Title.Trim())) {
				this.FieldsForm.Header.Style.Display = DisplayMethod.None;
			} else {
				this.FieldsForm.Header.Title = this.Title;
			}
			Panel.Controls.Add(this.FieldsForm);
			Content.Add(Panel.Draw);
		}
		private void LoadFormHeaderStyle()
		{
			this.Page.StyleSheet.AddClassBasedRule("EntityBinderHeaderStyle", "height: 22px; background-color: #DADADA;border: 1px solid #CCCDCE; color: #3C5766; margin-left: 15px; margin-top: -33px; padding: 0; position: absolute;");
		}
		public Group(GroupCollection GroupCollection, string ID, string Title)
		{
			this.ID = ID;
			this.Title = Title;
			this.oGroupCollection = GroupCollection;
			this.oFieldsForm = new Ophelia.Web.View.Controls.Form.Form(this.ID + "_FieldsForm", "", Controls.Form.Form.FormMethod.Post, Ophelia.Web.View.Controls.Form.Form.FormEncodeType.MultipartFormData, Ophelia.Web.View.Controls.Form.Form.FormMethod.Post, Ophelia.Web.View.Controls.Form.Form.FormEncodeType.MultipartFormData);
			//Me.oFieldsForm.Style.BackgroundColor = "#FFF"
			this.oFieldsForm.FieldCellStyle.Font.Color = "#3D5A66";
			this.oFieldsForm.FieldCellStyle.Font.Weight = Forms.FontWeight.Bold;
			this.oFieldsForm.Header.Control.Style.Font.Color = "#3C5766";
			this.oFieldsForm.Header.Style.Class = "EntityBinderHeaderStyle";
			this.oFieldsForm.Header.Style.Padding = 0;
			this.LoadFormHeaderStyle();
		}
	}
}

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Binders;
namespace Ophelia.Web.View.Controls.V2.Form
{
	public class FormV2 : Ophelia.Web.View.Controls.Form.Form
	{
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			Hashtable SectionsFields = new Hashtable();
			string SectionID = "";
			Layout.Technique = Ophelia.Web.View.LayoutTechnique.Css;
			if (this.Fields(this.ID + "IsSubmitted") == null) {
				this.Fields.AddHiddenBox(this.ID + "IsSubmitted");
			}
			if (this.Commands.Count > 0) {
				this.Script.AddFunction("SetAction_" + this.ID, "document." + this.ID + "." + this.ID + "Action.value = ActionName; ", "ActionName");
				if (this.Fields(ID + "Action") == null) {
					this.Fields.AddHiddenBox(this.ID + "Action");
				}
			}
			//GroupFieldsForSectionAndDecideColumnCount
			this.StyleSheet.AddClassBasedRule(this.ID + "_HeaderClassName", this.FieldHeadersStyle);
			int ColumnCount = 2;
			string AllFieldsInputMemberNames = "";
			for (int i = 0; i <= this.Fields.Count - 1; i++) {
				if (Fields(i).GetType.Name == "HiddenField") {
					SectionID = "Hidden";
				} else {
					SectionID = this.Fields(i).Section.Row.Index + "_" + this.Fields(i).Section.Column.Index;
				}
				if (!string.IsNullOrEmpty(Fields(i).Description)) {
					ColumnCount = 3;
				}
				if (SectionsFields[SectionID] == null) {
					SectionsFields[SectionID] = new ArrayList();
				}
				((ArrayList)SectionsFields[SectionID]).Add(Fields(i));
				for (int j = 0; j <= this.Fields(i).Controls.Count - 1; j++) {
					if (i > 0)
						AllFieldsInputMemberNames += ",";
					AllFieldsInputMemberNames += this.Fields(i).Controls(j).ID;
				}
			}
			//ArrangeLayout
			this.StyleSheet.AddCustomRule("." + this.ID + "FieldCellStyle", FieldCellStyle.Draw(true));
			ArrayList FieldsArray = null;
			for (int i = 0; i <= this.Layout.Rows.Count - 1; i++) {
				for (int j = 0; j <= this.Layout.Columns.Count - 1; j++) {
					Structure.Cells.Cell Cell = this.Layout.Rows(i).Cells(j, true);
					if (Cell.DependentCell == null) {
						FieldsArray = SectionsFields[i + "_" + j];
						if (FieldsArray != null) {
							Structure.Structure Structure = new Structure.Structure(Cell.ID + "FieldsStructure", 1, ColumnCount);
							Structure.Technique = Ophelia.Web.View.LayoutTechnique.Css;
							for (int k = 0; k <= FieldsArray.Count - 1; k++) {
								if (k > 0)
									Structure.Rows.Add();
								Field Field = FieldsArray[k];
								Field.OnBeforeDraw();
								if (Field.Style.IsCustomized) {
									Structure.Rows(k).SetStyle(Field.Style);
								}
								for (int n = 0; n <= Field.Controls.Count - 1; n++) {
									if (Field.Controls(n).GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Controls.DataControl"))) {
										((Ophelia.Web.View.Controls.DataControl)Field.Controls(n)).Validators.ValidationScript = this.Script;
									}
								}
								if (Field.ShowHeader) {
									Structure.Rows(k)(0).Controls.Add(Field.Header);
									Structure.Rows(k)(0).Style.Class = this.ID + "FieldCellStyle";
									Field.Control.Validators.ValidationScript = this.Script;
									for (int n = 0; n <= Field.Controls.Count - 1; n++) {
										Structure.Rows(k).Cell(1).Controls.Add(Field.Controls(n));
									}
									if (!string.IsNullOrEmpty(Field.Description)) {
										Structure.Rows(k).Cell(2).Controls.Add(Field.DescriptionLabel);
									}
								} else {
									for (int n = 0; n <= Field.Controls.Count - 1; n++) {
										Structure.Rows(k).Cell(0).ColumnSpan = 2;
										Structure.Rows(k).Cell(0).Controls.Add(Field.Controls(n));
									}
									if (!string.IsNullOrEmpty(Field.Description)) {
										Structure.Rows(k).Cell(2).Controls.Add(Field.DescriptionLabel);
									}
								}

							}
							Cell.Controls.Insert(0, Structure);
						}
					}
				}
			}

			if (base.ValidationActive) {
				base.Validate();
			}

			Content.Add("<div id=\"Container" + this.ID + "" + "\">");
			Content.Add("<form name=\"" + this.ID + "\" ");
			Content.Add("id=\"" + this.ID + "\" ");
			switch (this.Method) {
				case FormMethod.Get:
				case FormMethod.Ajax:
					Content.Add("method=\"get\" ");
					break;
				case FormMethod.Post:
					Content.Add("method=\"post\" ");
					break;
			}
			if (!this.AutoComplete) {
				Content.Add("autocomplete=\"off\" ");
			}
			if (this.TargetType == FormTarget.Custom) {
				if (!string.IsNullOrEmpty(this.Target)) {
					Content.Add("target=\"" + this.Target + "\" ");
				}
			} else if (this.TargetType == FormTarget.Blank) {
				Content.Add("target=\"_blank\" ");
			} else if (this.TargetType == FormTarget.Parent) {
				Content.Add("target=\"_parent\" ");
			} else if (this.TargetType == FormTarget.Self) {
				Content.Add("target=\"_self\" ");
			} else if (this.TargetType == FormTarget.Top) {
				Content.Add("target=\"_top\" ");
			}
			if (this.Method == FormMethod.Ajax) {
				this.Script.AddAjaxEvent(this.Action, this.Page.GetType.FullName, this.Action, AllFieldsInputMemberNames, true);
				Content.Add("onsubmit=\"if (" + this.ID + "_OnSubmit" + "()) {" + this.Action + "();return false;}else{return false;}\" ");
			} else {
				Content.Add("action=\"" + this.Action + "\" ");
				Content.Add("onsubmit=\"return " + this.ID + "_OnSubmit" + "();\" ");
			}
			switch (this.EncodeType) {
				case FormEncodeType.UrlEncoded:
					Content.Add("enctype=\"application/x-www-form-urlencoded\" ");
					break;
				case FormEncodeType.MultipartFormData:
					Content.Add("enctype=\"multipart/form-data\" ");
					break;
			}
			Content.Add(">");
			FieldsArray = SectionsFields["Hidden"];
			if (FieldsArray != null && FieldsArray.Count > 0) {
				for (int i = 0; i <= FieldsArray.Count - 1; i++) {
					this.ConfigureSubControls(((Field)FieldsArray[i]).Control);
					Content.Add(((Field)FieldsArray[i]).Control.Draw());
				}
			}
			this.ConfigureSubControls(this.Layout);
			Content.Add(this.Layout.Draw());
			if (this.Commands.HasAutoDrawCommand) {
				Content.Add("<div id=\"" + this.ID + "_SubmitContent\">");
				for (int i = 0; i <= this.Commands.Count - 1; i++) {
					this.ConfigureSubControls(this.Commands(i).Button);
					Content.Add(this.Commands(i).Draw);
				}
				Content.Add("</div>");
			}
			Content.Add("</form>");
			Content.Add("</div>");
		}
		public FormV2(string ID) : base(ID, "", FormMethod.Post, FormEncodeType.MultipartFormData)
		{
		}
		public FormV2(string ID, FormEncodeType EncodeType) : base(ID, "", FormMethod.Post, EncodeType)
		{
		}
		public FormV2(string ID, string Action, FormMethod Method, FormEncodeType EncodeType) : base(ID, Action, Method, EncodeType, 1, 1)
		{
		}
		public FormV2(string ID, string Action, FormMethod Method, FormEncodeType EncodeType, int RowNumber, int ColumnNumber) : base(ID, Action, Method, EncodeType, RowNumber, ColumnNumber)
		{
		}
	}
}

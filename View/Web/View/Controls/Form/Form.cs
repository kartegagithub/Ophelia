using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
using Ophelia.Web.View.Binders;
namespace Ophelia.Web.View.Controls.Form
{
	public class Form : ComplexWebControl
	{
		private Web.Binders.FieldCollection oFields;
		private string sAction = "";
		private FormMethod eMethod;
		private FormEncodeType eEncodeType = FormEncodeType.Plain;
		private string sOnSubmit = "";
		private Function oValidationFunction;
		private System.Web.HttpRequest oRequest;
		private Structure.Structure oLayout;
		private Style oFieldHeadersStyle;
		private Style oFieldControlStyle;
		private int nCellSpacing = 2;
		private int nCellPadding = 2;
		private string sConfirmMessage = "";
		private CommandCollection oCommands;
		private Header oHeader;
		private bool bValidationActive = true;
		private Style oFieldCellStyle;
		private string sTarget = "";
		private Form.FormTarget sTargetType = FormTarget.None;
		private bool sAutoComplete = true;
		private bool bUseDictionary = true;
		private bool bShowToolTip = false;
		public bool ShowToolTip {
			get { return this.bShowToolTip; }
			set { this.bShowToolTip = value; }
		}
		public bool UseDictionary {
			get { return this.bUseDictionary; }
			set { this.bUseDictionary = value; }
		}
		public bool AutoComplete {
			get { return this.sAutoComplete; }
			set { this.sAutoComplete = value; }
		}
		public string Target {
			get { return this.sTarget; }
			set {
				this.sTarget = value;
				if (!string.IsNullOrEmpty(this.sTarget)) {
					this.TargetType = FormTarget.Custom;
				}
			}
		}
		public Form.FormTarget TargetType {
			get { return this.sTargetType; }
			set { this.sTargetType = value; }
		}
		public FormMethod Method {
			get { return this.eMethod; }
			set { this.eMethod = value; }
		}
		public string Action {
			get { return this.sAction; }
			set { this.sAction = value; }
		}
		public FormEncodeType EncodeType {
			get { return this.eEncodeType; }
			set { this.eEncodeType = value; }
		}
		public string OnSubmit {
			get { return this.sOnSubmit; }
			set { this.sOnSubmit = value; }
		}
		public Style FieldCellStyle {
			get {
				if (this.oFieldCellStyle == null) {
					this.oFieldCellStyle = new Style();
					this.oFieldCellStyle.VerticalAlignment = VerticalAlignment.Top;
					this.oFieldCellStyle.PaddingTop = 3;
					this.oFieldCellStyle.PaddingRight = 5;
				}
				return this.oFieldCellStyle;
			}
		}
		public Header Header {
			get {
				if (this.oHeader == null)
					this.oHeader = new Header(this);
				return this.oHeader;
			}
		}
		public CommandCollection Commands {
			get {
				if (this.oCommands == null) {
					this.oCommands = new CommandCollection(this);
				}
				return this.oCommands;
			}
		}
		public bool ValidationActive {
			get { return this.bValidationActive; }
			set { this.bValidationActive = value; }
		}
		public string ConfirmMessage {
			get { return this.sConfirmMessage; }
			set { this.sConfirmMessage = value; }
		}
		public int CellSpacing {
			get { return this.nCellSpacing; }
			set { this.nCellSpacing = value; }
		}
		public int CellPadding {
			get { return this.nCellPadding; }
			set { this.nCellPadding = value; }
		}
		public Style FieldHeadersStyle {
			get {
				if (this.oFieldHeadersStyle == null) {
					this.oFieldHeadersStyle = new Style();
				}
				return this.oFieldHeadersStyle;
			}
		}
		public Style FieldControlStyle {
			get {
				if (this.oFieldControlStyle == null) {
					this.oFieldControlStyle = new Style();
				}
				return this.oFieldControlStyle;
			}
		}
		public Structure.Cells.Cell Sections {
			get { return this.Layout.Cell(RowNumber, ColumnName); }
		}
		public Structure.Structure Layout {
			get { return this.oLayout; }
		}
		public string ImageBase {
			get { return this.Client.ApplicationBase; }
		}
		public FieldCollection Fields {
			get { return this.oFields; }
		}
		public Client Client {
			get { return this.Page.Client; }
		}
		public HttpRequest HttpRequest {
			get { return this.oRequest; }
		}
		public Application.Framework.Dictionary Dictionary {
			get { return this.Client.Dictionary; }
		}
		public Function ValidationFunction {
			get {
				if (this.oValidationFunction == null) {
					this.oValidationFunction = this.Script.AddFunction(this.ID + "_OnSubmit", this.OnSubmit);
				}
				return this.oValidationFunction;
			}
		}
		public static bool IsSubmitted(string FormID)
		{
			HttpRequest Request = UI.PageContext.Current.Request;
			if (Request(FormID + "IsSubmitted") != null && Request(FormID + "IsSubmitted") == 1) {
				return true;
			}
			return false;
		}
		public static string GetSubmitAction(string FormID)
		{
			HttpRequest Request = UI.PageContext.Current.Request;
			if (Request(FormID + "Action") != null) {
				return Request(FormID + "Action");
			}
			return "";
		}
		protected override void CustomizeScript(Script Script)
		{
			base.CustomizeScript(Script);

		}
		public override void OnBeforeDraw(Content Content)
		{
			Hashtable SectionsFields = new Hashtable();
			string SectionID = "";
			if (this.Fields(this.ID + "IsSubmitted") == null) {
				this.Fields.AddHiddenBox(this.ID + "IsSubmitted");
			}

			if (this.Commands.Count > 0) {
				this.Script.AddFunction("SetAction_" + this.ID, "document." + this.ID + "." + this.ID + "Action.value = ActionName; ", "ActionName");
				if (this.Fields(ID + "Action") == null) {
					this.Fields.AddHiddenBox(this.ID + "Action");
				}
			}
			///GroupFieldsForSectionAndDecideColumnCount
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
							Structure.CellPadding = this.CellPadding;
							Structure.CellSpacing = this.CellSpacing;
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

			if (this.ValidationActive) {
				this.Validate();
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

			Content.Add("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" " + this.Style.Draw + ">");
			if ((this.oHeader != null) && (!string.IsNullOrEmpty(this.Header.Title) || !string.IsNullOrEmpty(this.Header.FormMessage))) {
				Content.Add("<tr>");
				Content.Add("<td " + Header.Style.Draw + ">");
				Content.Add(Header.Draw);
				Content.Add("</td>");
				Content.Add("</tr>");
			}
			Content.Add("<tr>");
			FieldsArray = SectionsFields["Hidden"];
			if (FieldsArray != null && FieldsArray.Count > 0) {
				Content.Add("<td>");
				for (int i = 0; i <= FieldsArray.Count - 1; i++) {
					this.ConfigureSubControls(((Field)FieldsArray[i]).Control);
					Content.Add(((Field)FieldsArray[i]).Control.Draw());
				}
				Content.Add("</td>");
				Content.Add("</tr>");
				Content.Add("<tr>");
			}
			Content.Add("<td>");
			this.ConfigureSubControls(this.Layout);
			Content.Add(this.Layout.Draw());
			Content.Add("</td>");
			if (this.Commands.HasAutoDrawCommand) {
				Content.Add("</tr>");
				Content.Add("<tr id=\"" + this.ID + "_SubmitContent\">");
				Content.Add("<td " + this.Commands.Style.Draw + ">");
				for (int i = 0; i <= this.Commands.Count - 1; i++) {
					this.ConfigureSubControls(this.Commands(i).Button);
					Content.Add(this.Commands(i).Draw);
				}
				Content.Add("</td>");
			}
			Content.Add("</tr>");
			Content.Add("</table>");

			Content.Add("</form>");
			Content.Add("</div>");

		}
		protected virtual void Validate()
		{
			bool DrawScript = false;
			this.oValidationFunction = this.Script.AddFunction(this.ID + "_OnSubmit");
			if (ShowToolTip)
				this.ValidationFunction.AppendLine("var Form_Validate=1;var FocusId='';");
			for (int i = 0; i <= this.Fields.Count - 1; i++) {
				for (int j = 0; j <= Fields(i).Controls.Count - 1; j++) {
					string ValidatorString = "";
					if (Fields(i).Controls(j).GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Controls.DataControl"))) {
						ValidatorString = ((Ophelia.Web.View.Controls.DataControl)Fields(i).Controls(j)).Validators.DrawValidatorString(this.ShowToolTip);
					}
					ValidatorString = Strings.Replace(ValidatorString, ";", "");
					if (!string.IsNullOrEmpty(ValidatorString)) {
						DrawScript = true;
						this.ValidationFunction.AppendLine("if(" + ValidatorString + "==false){");
						if (this.Commands.Count > 0) {
							this.ValidationFunction.AppendLine("document." + this.ID + "." + this.ID + "Action.value = '';");
						}
						if (ShowToolTip) {
							string InputId = ValidatorString.Substring(0, ValidatorString.IndexOf("_"));
							this.ValidationFunction.AppendLine("if(Form_Validate==1){FocusId='#'+'" + InputId + "';}$('#'+'" + InputId + "').css({'border':'2px solid #B94A48','background-color':'#FEE'});Form_Validate=0;}");
							this.ValidationFunction.AppendLine("else{$('#'+'" + InputId + "').removeAttr('style').removeAttr('bt-xtitle');}");
						} else {
							this.ValidationFunction.AppendLine("return false;}");
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(this.ConfirmMessage)) {
				this.ValidationFunction.AddConfirmMessage(this.ConfirmMessage, "document." + this.ID + "." + this.ID + "Action.value = '';");
			}

			if (DrawScript) {
				if (ShowToolTip) {
					this.ValidationFunction.AppendLine("if(Form_Validate==0){setTimeout(function () {$(FocusId).focus();}, 100); return false;}else{return true;}");
				} else {
					this.ValidationFunction.AppendLine("else {");
					this.ValidationFunction.AppendLine(this.OnSubmit);
					this.ValidationFunction.AppendLine("return true;");
					this.ValidationFunction.AppendLine("}");
				}
			} else {
				this.ValidationFunction.AppendLine(this.OnSubmit);
				this.ValidationFunction.AppendLine("return true;");
			}
		}
		public Form(string ID) : this(ID, "", FormMethod.Post, FormEncodeType.Plain)
		{
		}
		public Form(string ID, FormEncodeType EncodeType) : this(ID, "", FormMethod.Post, EncodeType)
		{
		}
		public Form(string ID, string Action, FormMethod Method, FormEncodeType EncodeType) : this(ID, Action, Method, EncodeType, 1, 1)
		{
		}
		public Form(string ID, string Action, FormMethod Method, FormEncodeType EncodeType, int RowNumber, int ColumnNumber)
		{
			this.oFields = new FieldCollection(this);
			this.ID = ID;
			if (string.IsNullOrEmpty(Action))
				Action = Request.RawUrl;
			this.Action = Action;
			this.Method = Method;
			this.EncodeType = EncodeType;
			if (RowNumber > 0 && ColumnNumber > 0) {
				this.oLayout = new Structure.Structure(this.ID + "_Layout", RowNumber, ColumnNumber);
			}
		}
		public enum FormEncodeType
		{
			Plain = 1,
			MultipartFormData = 2,
			UrlEncoded = 3
		}
		public enum FormMethod
		{
			Get = 1,
			Post = 2,
			Ajax = 3
		}
		public enum FormTarget
		{
			None = 0,
			Blank = 1,
			Self = 2,
			Parent = 3,
			Top = 4,
			Custom = 5
		}
	}
}


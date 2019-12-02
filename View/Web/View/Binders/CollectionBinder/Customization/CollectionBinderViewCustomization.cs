using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinderViewCustomization : AjaxControl
	{
		private CollectionBinder oCollectionBinder = null;
		private PopupControl oPopUp = null;
		private Style oHeaderStyle;
		public Style HeaderStyle {
			get {
				if (this.oHeaderStyle == null) {
					this.oHeaderStyle = new Style();
				}
				return this.oHeaderStyle;
			}
		}
		public PopupControl PopUp {
			get {
				if (this.oPopUp == null) {
					this.oPopUp = new PopupControl("ViewCustomizationPopup");
					this.oPopUp.Style.MinWidth = 260;
					this.oPopUp.Style.ZIndex = 999;
					this.oPopUp.Style.Padding = 0;
					this.oPopUp.OverlayPanelHasCloseEvent = false;
				}
				return this.oPopUp;
			}
		}
		public string ShowEvent {
			get { return "DrawCBCustomizationForm('" + this.Binder.ID + "', document.getElementById('" + this.Binder.ID + "_CBDef').innerHTML);"; }
		}
		public string HideEvent {
			get { return this.PopUp.HideEvent(); }
		}
		public CollectionBinder Binder {
			get { return this.oCollectionBinder; }
		}
		protected override void OnLoad()
		{
			this.AddAjax("DrawCBCustomizationForm", "", "BinderID,CBDefinition");
			this.AddAjax("SaveCustomization", "SelectedVisibleColumnsInJSON", "BinderID,CBDef");
		}
		public void SaveCustomization()
		{
			this.oCollectionBinder = Functions.GetCollectionBinderFromDefinition(this.QueryString("CBDef"));
			System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			Array Definition = null;
			string SDef = this.QueryString("SelectedVisibleColumnsInJSON", "", true);
			try {
				Definition = Serializer.DeserializeObject(SDef);
			} catch (Exception ex) {
				throw new Exception("Definition Cannot Be Read");
			}

			CollectionBinderColumnCustomization ColumnCustomization = null;
			for (int i = 0; i <= this.Binder.Columns.Count - 1; i++) {
				if (string.IsNullOrEmpty(this.Binder.Columns(i).MemberName)) {
					ColumnCustomization = this.Binder.Customization.ColumnCustomizations(this.Binder.Columns(i).MemberNameToBeDrawn);
				} else {
					ColumnCustomization = this.Binder.Customization.ColumnCustomizations(this.Binder.Columns(i).MemberName);
				}
				if (ColumnCustomization == null) {
					ColumnCustomization = this.Binder.Customization.ColumnCustomizations.Add();
					ColumnCustomization.MemberName = this.Binder.Columns(i).MemberName;
					ColumnCustomization.Indis = this.Binder.Columns.Count + 1;
					ColumnCustomization.CollectionBinderCustomization = this.Binder.Customization;
				}
				ColumnCustomization.Visible = 0;
				ColumnCustomization.Save();
			}
			int Indis = -1;
			// kolonlar gizlenip saklanırken aynı indisli visible kolonlar oluşabiliyor, burada onun kontrolü yapılıyor.
			for (int i = 0; i <= Definition.Length - 1; i++) {
				ColumnCustomization = this.Binder.Customization.ColumnCustomizations(Definition(i)("columnid"));
				if (ColumnCustomization != null) {
					if (Indis > -1 && Indis == Definition(i)("indis")) {
						Indis += 1;
					} else {
						Indis = Definition(i)("indis");
					}
					ColumnCustomization.Indis = Indis;
					ColumnCustomization.Visible = 1;
					ColumnCustomization.Save();
				}
			}
			this.Binder.Customization.IsDefaultCustomization = 0;
			this.Binder.Customization.Save(false, false);

			this.Script.AppendLine(this.PopUp.HideEvent());
			this.Script.AppendLine(this.oCollectionBinder.AjaxDelegate.RedrawEvent);
		}
		private string VisibleColumnClickedEvent()
		{
			System.Text.StringBuilder returnString = new System.Text.StringBuilder();
			returnString.AppendLine("$('#MoveUp').attr('disabled','disabled');");
			returnString.AppendLine("$('#MoveDown').attr('disabled','disabled');");
			returnString.AppendLine("if ($('#VisibleColumns option').length > 1) {");
			returnString.AppendLine("      if ($('#VisibleColumns option:selected').prev().length > 0){");
			returnString.AppendLine("         $('#MoveUp').removeAttr('disabled');}");
			returnString.AppendLine("      if ($('#VisibleColumns option:selected').next().length > 0){");
			returnString.AppendLine("         $('#MoveDown').removeAttr('disabled');}");
			returnString.AppendLine(" }");
			returnString.AppendLine(" else{");
			returnString.AppendLine(" $('#MoveDown').attr('disabled','disabled'); $('#MoveUp').attr('disabled','disabled');}");
			returnString.AppendLine("$('#MoveToInVisibleColumns').removeAttr('disabled');");
			returnString.AppendLine("$('#InVisibleColumns option:selected').removeAttr('selected');");
			returnString.AppendLine("$('#MoveToVisibleColumns').attr('disabled','disabled');");
			return returnString.ToString();
		}
		private string InVisibleColumnClickedEvent()
		{
			System.Text.StringBuilder returnString = new System.Text.StringBuilder();
			returnString.AppendLine("$('#MoveDown').attr('disabled','disabled')");
			returnString.AppendLine("$('#MoveUp').attr('disabled','disabled');");
			returnString.AppendLine("$('#MoveToInVisibleColumns').attr('disabled','disabled');");
			returnString.AppendLine("$('#MoveToVisibleColumns').removeAttr('disabled');");
			returnString.AppendLine("$('#VisibleColumns option:selected').removeAttr('selected');");
			return returnString.ToString();
		}
		public void DrawCBCustomizationForm()
		{
			SelectBox VisibleColumns = new SelectBox("VisibleColumns");
			VisibleColumns.OnClickEvent = VisibleColumnClickedEvent();
			VisibleColumns.Style.Width = 210;
			VisibleColumns.Style.Height = 250;
			VisibleColumns.Multiple = true;
			VisibleColumns.CreateBlankOption = false;

			SelectBox InVisibleColumns = new SelectBox("InVisibleColumns");
			InVisibleColumns.OnClickEvent = InVisibleColumnClickedEvent();
			InVisibleColumns.Style.Width = 210;
			InVisibleColumns.Style.Height = 250;
			InVisibleColumns.Multiple = true;
			InVisibleColumns.CreateBlankOption = false;

			System.Collections.Generic.Dictionary<string, object> Configurations = this.GetCollectionBinderConfigurationsFromDefinition(this.QueryString("CBDefinition"));

			string memberName = "";
			string indis = "";
			string visible = "";
			for (int i = 0; i <= Configurations["binder"](2)("configuration")("configurations").Length - 1; i++) {
				memberName = Configurations["binder"](2)("configuration")("configurations")(i)("membername");
				indis = Configurations["binder"](2)("configuration")("configurations")(i)("indis");
				// Bu ne?
				visible = Configurations["binder"](2)("configuration")("configurations")(i)("visible");
				if (visible == "1") {
					VisibleColumns.Options.Add(memberName, this.GetColumnTitle(memberName));
					VisibleColumns.Options.LastOption.Attributes.Add("indis", indis);
				} else {
					InVisibleColumns.Options.Add(memberName, this.GetColumnTitle(memberName));
					InVisibleColumns.Options.LastOption.Attributes.Add("indis", indis);
				}
			}

			Panel HeaderPanel = new Panel("CBCustomizationFormHeader");
			HeaderPanel.Style.Class = Configurations["binder"](2)("configuration")("headerstyleclass");
			HeaderPanel.Content.Add(Ophelia.Web.View.UI.Current.Client.Dictionary.GetWord("Concept.CollectionBinderCustomization"));

			Structure.Structure CustomizationStructure = new Structure.Structure(this.QueryString("BinderID") + "_CustomizationForm", 3, 4);
			CustomizationStructure(0, 0).ColumnSpan = 4;
			CustomizationStructure(0, 0).Controls.Add(HeaderPanel);

			CustomizationStructure(1, 0).Controls.AddLabel("", this.Page.Client.Dictionary.GetWord("Concept.SelectableColumns"));
			CustomizationStructure(1, 0).Controls.Add(InVisibleColumns);
			CustomizationStructure(1, 0).Style.Padding = 10;

			CustomizationStructure(1, 1).Controls.AddButton("MoveToVisibleColumns", Ophelia.Web.View.UI.Current.Client.Dictionary.GetWord("Command.MoveToVisibleColumns")).OnClickEvent = this.GetMoveToVisibleColumnsScript();
			((Button)CustomizationStructure(1, 1).Controls.LastControl).ReadOnly = true;
			CustomizationStructure(1, 1).Controls.LastControl.Style.Display = DisplayMethod.Block;
			CustomizationStructure(1, 1).Style.Margin = 5;

			CustomizationStructure(1, 1).Controls.AddButton("MoveToInVisibleColumns", Ophelia.Web.View.UI.Current.Client.Dictionary.GetWord("Command.MoveToInVisibleColumns")).OnClickEvent = this.GetMoveToInVisibleColumnsScript();
			((Button)CustomizationStructure(1, 1).Controls.LastControl).ReadOnly = true;
			CustomizationStructure(1, 1).Controls.LastControl.Style.Display = DisplayMethod.Block;
			CustomizationStructure(1, 1).Style.Margin = 5;

			CustomizationStructure(1, 2).Controls.AddLabel("", this.Page.Client.Dictionary.GetWord("Concept.VisibleColumns"));
			CustomizationStructure(1, 2).Controls.Add(VisibleColumns);
			CustomizationStructure(1, 2).Controls.AddHiddenBox("SelectedVisibleColumnsInJSON", "");
			CustomizationStructure(1, 2).Style.Padding = 10;

			CustomizationStructure(2, 0).Controls.AddButton(this.QueryString("BinderID") + "_SaveButton", Ophelia.Web.View.UI.Current.Client.Dictionary.GetWord("Command.Save")).OnClickEvent = this.GetSaveCustomizationScript();
			CustomizationStructure(2, 0).Controls.AddButton(this.QueryString("BinderID") + "_Cancel", Ophelia.Web.View.UI.Current.Client.Dictionary.GetWord("Command.Cancel")).OnClickEvent = this.HideEvent();
			CustomizationStructure(2, 0).Style.Padding = 10;
			CustomizationStructure(2, 0).Style.PaddingRight = 100;
			CustomizationStructure(2, 0).ColumnSpan = 4;
			CustomizationStructure(2, 0).Style.HorizontalAlignment = HorizontalAlignment.Right;

			CustomizationStructure(1, 3).Controls.AddButton("MoveUp", Ophelia.Web.View.UI.Current.Client.Dictionary.GetWord("Command.MoveUp")).OnClickEvent = "MoveColumn(1);";
			((Button)CustomizationStructure(1, 3).Controls.LastControl).ReadOnly = true;
			CustomizationStructure(1, 3).Controls.LastControl.Style.WidthInPercent = 100;
			CustomizationStructure(1, 3).Controls.LastControl.Style.Display = DisplayMethod.Block;
			CustomizationStructure(1, 3).Controls.AddButton("MoveDown", Ophelia.Web.View.UI.Current.Client.Dictionary.GetWord("Command.MoveDown")).OnClickEvent = "MoveColumn(0);";
			((Button)CustomizationStructure(1, 3).Controls.LastControl).ReadOnly = true;
			CustomizationStructure(1, 3).Controls.LastControl.Style.WidthInPercent = 100;
			CustomizationStructure(1, 3).Style.Padding = 5;
			CustomizationStructure(1, 3).Style.PaddingTop = 25;
			CustomizationStructure(1, 3).Style.VerticalAlignment = VerticalAlignment.Top;
			this.DrawMoveColumnScript();

			this.PopUp.ContentRegion.Controls.Add(CustomizationStructure);
			if (this.Request("AjaxRequestWithApplication") != null) {
				this.Script.AppendLine(this.PopUp.ShowEvent);
			} else {
				this.Script.AddFunctionToOnload(this.PopUp.ShowEvent);
			}
			this.Controls.Add(this.PopUp);
		}
		private string GetColumnTitle(string MemberName)
		{
			string Word = null;
			if (MemberName.IndexOf(".") > -1) {
				string[] Words = MemberName.Split(".");
				if (Words[Words.Length - 1] == "Name" || Words[Words.Length - 1] == "Count") {
					if (Words.Length > 2) {
						Word = Words[Words.Length - 3] + Words[Words.Length - 2];
					} else {
						Word = Words[Words.Length - 2];
					}
				} else {
					Word = Words[Words.Length - 1];
				}
			} else {
				Word = MemberName;
			}
			return Strings.Replace(this.Page.Client.Dictionary.GetWord("Concept." + Word), "Concept.", "");
		}
		private string DrawMoveColumnScript()
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function MoveColumnFunction = this.Script.AddFunction("MoveColumn", "", "direction");
			//1=UP , 0=DOWN
			MoveColumnFunction.AppendLine("$('#VisibleColumns option:selected').each(function(){");
			MoveColumnFunction.AppendLine("  if (direction==1){");
			MoveColumnFunction.AppendLine("    if ($(this).prev().length > 0){ ");
			MoveColumnFunction.AppendLine("        var tempIndis =$(this).attr('indis');  ");
			MoveColumnFunction.AppendLine("        $(this).attr('indis', $(this).prev().attr('indis'));");
			MoveColumnFunction.AppendLine("        $(this).prev().attr('indis',tempIndis);");
			MoveColumnFunction.AppendLine("    }; ");
			MoveColumnFunction.AppendLine("   $(this).insertBefore($(this).prev());");
			MoveColumnFunction.AppendLine("   if ($(this).prev().length == 0) {$('#MoveUp').attr('disabled','disabled');} ");
			MoveColumnFunction.AppendLine("   $('#MoveDown').removeAttr('disabled');");
			MoveColumnFunction.AppendLine("  }else{ ");
			MoveColumnFunction.AppendLine("    if ($(this).next().length > 0){ ");
			MoveColumnFunction.AppendLine("        var tempIndis =$(this).attr('indis');  ");
			MoveColumnFunction.AppendLine("        $(this).attr('indis', $(this).next().attr('indis'));");
			MoveColumnFunction.AppendLine("        $(this).next().attr('indis',tempIndis);");
			MoveColumnFunction.AppendLine("    }; ");
			MoveColumnFunction.AppendLine("    $(this).insertAfter($(this).next());");
			MoveColumnFunction.AppendLine("    if ($(this).next().length == 0) {$('#MoveDown').attr('disabled','disabled');} ");
			MoveColumnFunction.AppendLine("   $('#MoveUp').removeAttr('disabled');");
			MoveColumnFunction.AppendLine("  };");
			MoveColumnFunction.AppendLine("});   ");
			return "";
		}
		private string GetSaveCustomizationScript()
		{
			string returnString = "";
			returnString += " var columns = new Array();";
			returnString += " var column = null;  ";
			returnString += " $('#VisibleColumns option').each(function() {    ";
			returnString += "  column = new Object();      ";
			returnString += "  column.columnid = this.value; ";
			returnString += "  column.indis = $(this).attr('indis');";
			returnString += "  columns.push(column);}); ";
			returnString += "  $('#SelectedVisibleColumnsInJSON').val(JSON.stringify(columns));";
			//returnString &= " "
			//returnString &= " "
			returnString += " SaveCustomization('" + this.QueryString("BinderID") + "',document.getElementById('" + this.QueryString("BinderID") + "_CBDef').innerHTML); ";
			//returnString &= "$('#SelectedVisibleColumnsInJSON').val("
			//returnString &= " $.map($('#VisibleColumns option'), "
			//returnString &= "   function(element) {"
			//returnString &= "           return element.value;"
			//returnString &= "   }).join() "
			//returnString &= "); "
			//returnString &= " SaveCustomization('" & Me.QueryString("BinderID") & "',document.getElementById('" & Me.QueryString("BinderID") & "_CBDef').innerHTML); "
			return returnString;
		}
		private string GetMoveToVisibleColumnsScript()
		{
			string returnString = "";
			returnString += "$('#InVisibleColumns option:selected').each(function(){";
			returnString += "$('#VisibleColumns').append(this.outerHTML); ";
			//returnString &= "$('#VisibleColumns').append('<option value=' + this.value + '>' + this.label + '</option>'); "
			returnString += "$('#InVisibleColumns option:selected').removeAttr('selected');";
			returnString += "$('#VisibleColumns option:selected').removeAttr('selected');";
			returnString += "if ($(this).next().length > 0){";
			returnString += "    $(this).next().attr('selected','selected');}";
			returnString += "else if ($(this).prev().length > 0) {";
			returnString += "    $(this).prev().attr('selected','selected');}";
			returnString += "else {";
			returnString += "    $('#MoveToVisibleColumns').attr('disabled','disabled'); } ";
			//returnString &= "$(this).next().attr('selected','selected');"
			returnString += "$(this).remove();}) ;";
			//returnString &= Me.GetButtonVisibilityScript()
			return returnString;
		}
		//Private Function GetButtonVisibilityScript() As String
		//Dim returnString As String = ""
		//returnString &= "document.getElementById('MoveToVisibleColumns').disabled= ($('#InVisibleColumns option').length == 0); "
		//returnString &= "document.getElementById('MoveToInVisibleColumns').disabled= ($('#VisibleColumns option').length == 0);"
		//Return returnString
		//End Function
		private string GetMoveToInVisibleColumnsScript()
		{
			string returnString = "";
			returnString += "$('#VisibleColumns option:selected').each(function(){";
			returnString += "$('#InVisibleColumns').append(this.outerHTML); ";
			//returnString &= "$('#InVisibleColumns').append('<option value=' + this.value + '>' + this.label + '</option>'); "
			returnString += "$('#VisibleColumns option:selected').removeAttr('selected');";
			returnString += "$('#InVisibleColumns option:selected').removeAttr('selected');";
			returnString += "if ($(this).next().length > 0){";
			returnString += "    $(this).next().attr('selected','selected');}";
			returnString += "else if ($(this).prev().length > 0) {";
			returnString += "    $(this).prev().attr('selected','selected');}";
			returnString += "else {";
			returnString += "    $('#MoveToInVisibleColumns').attr('disabled','disabled'); } ";
			returnString += "$(this).remove();}) ;";
			returnString += "if ($('#VisibleColumns option').length < 2) { $('#MoveUp').attr('disabled','disabled');$('#MoveDown').attr('disabled','disabled');}";
			//returnString &= Me.GetButtonVisibilityScript()
			return returnString;
		}
		public System.Collections.Generic.Dictionary<string, object> GetCollectionBinderConfigurationsFromDefinition(string DefinitionString)
		{
			System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			string CollectionDef = Functions.DecryptWithEncoding(System.Text.Encoding.UTF8, DefinitionString, "OpheliaDefinitionToCollectionConverter");
			System.Collections.Generic.Dictionary<string, object> Definition = null;
			try {
				Definition = Serializer.DeserializeObject(CollectionDef);
			} catch (Exception ex) {
				throw new Exception("DefinitionCannotBeRead");
			}
			return Definition;
		}
		public CollectionBinderViewCustomization(CollectionBinder CollectionBinder)
		{
			this.oCollectionBinder = CollectionBinder;
		}

		public CollectionBinderViewCustomization()
		{
		}
	}
}

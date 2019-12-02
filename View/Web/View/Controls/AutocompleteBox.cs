using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class AutocompleteBox : InputDataControl
	{

		#region "Private Members"

		private string _SearchFor;
		private bool _AllowMultiSelect;

		private string _AjaxRequestUrl = "ContentManager/BlogPosts/BlogPost";
		#endregion

		#region "Public Members"


		public string SearchFor {
			get { return _SearchFor; }
			set { _SearchFor = value; }
		}

		public bool AllowMultiSelect {
			get { return _AllowMultiSelect; }
			set { _AllowMultiSelect = value; }
		}

		public string AjaxRequestUrl {
			get { return _AjaxRequestUrl; }
			set { _AjaxRequestUrl = value; }
		}

		#endregion

		#region "Procedures"

		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);

			if (Script.Function("TagSearchChanged") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function TagSearchChanged = Script.AddFunction("TagSearchChanged", "", "element");
				TagSearchChanged.AppendLine("var tagSearchBox = document.getElementById(element.id);");
				TagSearchChanged.AppendLine("alert(tagSearchBox.value);");
			}

			if (Script.Function("SearchTag") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function SearchTag = Script.AddFunction("SearchTag", "", "element");
				SearchTag.AppendLine("alert('searching ' + element.value)");
			}


			if (Script.Function("AddTag") == null) {
				ServerSide.ScriptManager.Function AddTagFunction = Script.AddAjaxFunction("AddTag", string.Empty, string.Empty);
			}


			if (Script.Function("PerformAutocompleteBoxSearch") == null) {
				ServerSide.ScriptManager.AjaxFunction AutocompleteBoxSearch = Script.AddAjaxFunction("PerformAutocompleteBoxSearch", string.Empty, "searchValue, searchFor");
				AutocompleteBoxSearch.AppendLine("var result = " + AutocompleteBoxSearch.HttpObject + ".responseText;");

				AutocompleteBoxSearch.AppendLine("if(result) {" + Constants.vbCrLf + "   var jsonArray = JSON.parse(result);" + Constants.vbCrLf + "   if(jsonArray.length > 0) {" + Constants.vbCrLf + "      autocompleteBoxObject.showResults(jsonArray);" + Constants.vbCrLf + "   }" + Constants.vbCrLf + "}" + Constants.vbCrLf + "else {" + Constants.vbCrLf + "   autocompleteBoxObject.showResults('');" + Constants.vbCrLf + "}");

				AutocompleteBoxSearch.AjaxRequestParameter.Add("SearchFor", this.SearchFor);
				AutocompleteBoxSearch.AjaxRequestParameter.Add("SearchValue", "'+searchValue+'");
				AutocompleteBoxSearch.ShowOverlay = false;


			}

		}


		public void HobareyFunction()
		{
		}

		public override void OnBeforeDraw(Content Content)
		{
			this.Page.Header.Links.Add("/assets/content-manager/autocomplete-box/style.css", Ophelia.Web.View.UI.HeadLink.ReleationShipType.StyleSheet);
			this.Page.Header.ScriptManager.Add("autocomplete-box-js", "/assets/content-manager/autocomplete-box/script.js");

			//Me.OnChangeEvent = "TagSearchChanged(this);"
			//Me.OnKeyUpEvent = "AutocompleteBoxSearch(this, " & Me.SearchFor & ");"

			Content.Add("<div id=\"" + (string.IsNullOrEmpty(this.ID) ? "AutocompleteBox" : this.ID) + "_Container\" class=\"autocomplete-control\" " + this.Style.Draw() + " >");
			Content.Add("<input class=\"search-box\"");

			if (!string.IsNullOrEmpty(this.ID))
				Content.Add(" id=" + this.ID + "_SearchBox");
			if (!string.IsNullOrEmpty(this.Name))
				Content.Add(" name=" + this.Name + "");
			//If Not String.IsNullOrEmpty(Me.Value) Then Content.Add(" value=" & Me.Value & "")

			Content.Add(" autocomplete=\"off\"");
			Content.Add(" data-allow-multi-select=\"" + this.AllowMultiSelect.ToString().ToLower() + "\"");
			Content.Add(" data-search-for=\"" + this.SearchFor + "\"");
			this.DrawEvents(Content);
			Content.Add(" type=\"text\" ");


			Content.Add(" />");


			Content.Add("<div class=\"search-results-panel-container\">");
			Content.Add("<ul class=\"search-results-panel\"></ul>");
			Content.Add("</div>");
			Content.Add("<div class=\"selected-items\"></div>");
			Content.Add("</div>");
		}

		protected override void DrawEvents(Content Content)
		{
			base.DrawEvents(Content);
			this.CheckEvent(this.OnBlurEvent);
			this.CheckEvent(this.OnChangeEvent);
			this.CheckEvent(this.OnFocusEvent);

			if (!string.IsNullOrEmpty(this.OnKeyPressEvent))
				Content.Add(" onkeypress=\"" + this.OnKeyPressEvent + "\"");
			if (!string.IsNullOrEmpty(this.OnKeyDownEvent))
				Content.Add(" onkeydown=\"" + this.OnKeyDownEvent + "\"");
			if (!string.IsNullOrEmpty(this.OnKeyUpEvent))
				Content.Add(" onkeyup=\"" + this.OnKeyUpEvent + "\"");
			if (!string.IsNullOrEmpty(this.OnChangeEvent))
				Content.Add(" onchange=\"" + this.OnChangeEvent + "\"");
			if (!string.IsNullOrEmpty(this.OnFocusEvent))
				Content.Add(" onfocus=\"" + this.OnFocusEvent + "\"");
			if (!string.IsNullOrEmpty(this.OnBlurEvent))
				Content.Add(" onblur=\"" + this.OnBlurEvent + "\"");
			if (this.TabIndex > -2)
				Content.Add(" tabindex=\"" + this.TabIndex + "\"");

		}

		#endregion

		#region "Constructors"

		public AutocompleteBox(string ID) : this(ID: ID, SearchFor: string.Empty, AllowMultiSelect: false)
		{
		}
		public AutocompleteBox(string ID, string SearchFor) : this(ID: ID, SearchFor: SearchFor, AllowMultiSelect: false)
		{
		}
		public AutocompleteBox(string ID, string SearchFor, bool AllowMultiSelect)
		{
			this.ID = ID;
			this.SearchFor = SearchFor;
			this.AllowMultiSelect = AllowMultiSelect;
		}

		public AutocompleteBox()
		{
		}

		#endregion

	}
}

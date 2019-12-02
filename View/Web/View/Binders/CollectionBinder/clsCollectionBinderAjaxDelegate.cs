using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
using Ophelia.Web.View;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinderAjaxDelegate : AjaxControl
	{
		private CollectionBinder oBinder = null;
		public CollectionBinder Binder {
			get { return this.oBinder; }
			set { this.oBinder = value; }
		}
		public virtual EntityCollection DataSource {
			get { return this.oBinder.Binding.Value; }
			set { this.oBinder.Binding.Value = value; }
		}
		private string sOnAfterRedrawScript = "";
		public virtual string OnAfterRedrawScript {
			get { return this.sOnAfterRedrawScript; }
			set { this.sOnAfterRedrawScript = value; }
		}
		#region "EntityCollectionToJSONFunctions"
		private JSON.JSONEntity EntityCollectionToJSON(EntityCollectionDefinition Definition)
		{
			JSON.JSONEntity JSEntity = new JSON.JSONEntity("collection");
			if (DataSource == null)
				return JSEntity;
			JSEntity.SetData("EntityType", Definition.EntityDefinition.EntityType.FullName);
			JSEntity.SetData("PageSize", Definition.PageSize);
			JSEntity.SetData("Distinct", Definition.Distinct);
			JSON.JSONEntityCollection FiltersJSCollection = new JSON.JSONEntityCollection("filters");
			JSON.JSONEntity FilterJS = default(JSON.JSONEntity);
			FilterCollection Filters = Definition.Filters;
			for (int i = 0; i <= Filters.Count - 1; i++) {
				FilterJS = FiltersJSCollection.Add();
				FilterJS.SetData("Name", Filters(i).Name);
				FilterJS.SetData("AsIs", Filters(i).AsIs);
				FilterJS.SetData("Comparison", Filters(i).Comparison);
				FilterJS.SetData("Constraint", Filters(i).Constraint);
				FilterJS.SetData("Excludes", Filters(i).Excludes);
				FilterJS.SetData("Prefix", Filters(i).Prefix);
				FilterJS.SetData("Suffix", Filters(i).Suffix);
				JSON.JSONEntityCollection FiltersParametersJSCollection = new JSON.JSONEntityCollection("parameters");
				JSON.JSONEntity FiltersParameterJS = default(JSON.JSONEntity);
				for (int j = 0; j <= Filters(i).Parameters.Count - 1; j++) {
					FiltersParameterJS = FiltersParametersJSCollection.Add;
					if (Filters(i).Parameters(j).Value.GetType.FullName == "Ophelia.Application.Base.EntityCollectionDefinition") {
						JSON.JSONEntityCollection JSCollection = new JSON.JSONEntityCollection();
						JSCollection.Add(this.EntityCollectionToJSON(Filters(i).Parameters(j).Value));
						FiltersParameterJS.SetData("value", JSCollection);
					} else {
						FiltersParameterJS.SetData("Value", Filters(i).Parameters(j).Value);
					}
				}
				FilterJS.SetData("parameters", FiltersParametersJSCollection);
			}
			JSEntity.SetData("filters", FiltersJSCollection);

			JSON.JSONEntityCollection SortersJSCollection = new JSON.JSONEntityCollection("sorters");
			JSON.JSONEntity SorterJS = default(JSON.JSONEntity);
			SorterCollection Sorters = Definition.Sorters;
			for (int i = 0; i <= Sorters.Count - 1; i++) {
				SorterJS = SortersJSCollection.Add;
				SorterJS.SetData("Direction", Sorters(i).Direction);
				SorterJS.SetData("Name", Sorters(i).Name);
				SorterJS.SetData("NullsOrder", Sorters(i).NullsOrder);
			}
			JSEntity.SetData("sorters", SortersJSCollection);

			JSON.JSONEntityCollection GroupersJSCollection = new JSON.JSONEntityCollection("groupers");
			JSON.JSONEntity GrouperJS = default(JSON.JSONEntity);
			GrouperCollection Groupers = Definition.Groupers;
			for (int i = 0; i <= Groupers.Count - 1; i++) {
				GrouperJS = GroupersJSCollection.Add;
				GrouperJS.SetData("Name", Groupers(i).Name);
			}
			JSEntity.SetData("groupers", GroupersJSCollection);

			JSON.JSONEntityCollection RequestedPropertiesJSCollection = new JSON.JSONEntityCollection("requestedproperties");
			JSON.JSONEntity RequestedPropertyJS = default(JSON.JSONEntity);
			Application.Base.PropertyCollection RequestedProperties = Definition.RequestedProperties;
			for (int i = 0; i <= RequestedProperties.Count - 1; i++) {
				RequestedPropertyJS = RequestedPropertiesJSCollection.Add;
				RequestedPropertyJS.SetData("AccessType", RequestedProperties(i).AccessType);
				RequestedPropertyJS.SetData("Alias", RequestedProperties(i).Alias);
				RequestedPropertyJS.SetData("Name", RequestedProperties(i).Name);
			}
			JSEntity.SetData("requestedproperties", RequestedPropertiesJSCollection);

			JSON.JSONEntityCollection PropertiesRestrictedToJSCollection = new JSON.JSONEntityCollection("propertiesrestrictedto");
			JSON.JSONEntity PropertyRestrictedToJS = default(JSON.JSONEntity);
			Application.Base.PropertyCollection PropertiesRestrictedTo = Definition.PropertiesRestrictedTo;
			for (int i = 0; i <= PropertiesRestrictedTo.Count - 1; i++) {
				PropertyRestrictedToJS = PropertiesRestrictedToJSCollection.Add;
				PropertyRestrictedToJS.SetData("AccessType", PropertiesRestrictedTo(i).AccessType);
				PropertyRestrictedToJS.SetData("Alias", PropertiesRestrictedTo(i).Alias);
				PropertyRestrictedToJS.SetData("Name", PropertiesRestrictedTo(i).Name);
			}
			JSEntity.SetData("propertiesrestrictedto", PropertiesRestrictedToJSCollection);

			JSON.JSONEntityCollection RelatedPropertiesJSCollection = new JSON.JSONEntityCollection("relatedproperties");
			JSON.JSONEntity RelatedPropertyJS = default(JSON.JSONEntity);
			Application.Base.PropertyCollection RelatedProperties = Definition.RelatedProperties;
			for (int i = 0; i <= RelatedProperties.Count - 1; i++) {
				RelatedPropertyJS = RelatedPropertiesJSCollection.Add;
				RelatedPropertyJS.SetData("AccessType", RelatedProperties(i).AccessType);
				RelatedPropertyJS.SetData("Alias", RelatedProperties(i).Alias);
				RelatedPropertyJS.SetData("Name", RelatedProperties(i).Name);
				RelatedPropertyJS.SetData("RelatedPropertyInfoName", RelatedProperties(i).RelatedPropertyInfo.Name);
				RelatedPropertyJS.SetData("RelatedPropertyPropertyTypeName", RelatedProperties(i).RelatedPropertyInfo.ReflectedType.FullName);
			}
			JSEntity.SetData("relatedproperties", RelatedPropertiesJSCollection);
			return JSEntity;
		}
		#endregion
		#region "JSONToEntityCollectionFunctions"
		public void DrawCollectionBinderByPage()
		{
			CollectionBinder oCollectionBinder = Ophelia.Application.Base.Functions.GetCollectionBinderFromDefinition(this.QueryString("BinderDef", "", true));
			this.Content.Clear();
			this.Content.Add(oCollectionBinder.GetRowsInString());

			this.Content.Add(";;;;" + oCollectionBinder.ID + ";;;;");

			if (oCollectionBinder.Collection.Pages.Count == oCollectionBinder.Pager.CurrentPage) {
				this.Content.Add("collectionLoaded;;;;");
			}

			//Me.Page.Response.ContentType = "text/html"
			//Me.Page.Response.Write(Me.Content.Value)
			//Me.Page.Response.End()
		}
		public void RedrawCollectionBinder()
		{
			CollectionBinder oCollectionBinder = Ophelia.Application.Base.Functions.GetCollectionBinderFromDefinition(this.QueryString("BinderDef", "", true));
			this.Controls.Add(oCollectionBinder);
			if (!string.IsNullOrEmpty(oCollectionBinder.AjaxDelegate.OnAfterRedrawScript)) {
				this.Script.Append("setTimeout(function(){ " + oCollectionBinder.AjaxDelegate.OnAfterRedrawScript + "},0);");
			}
		}
		public void ExportToExcel()
		{
			string sDefinitionString = "";
			sDefinitionString = System.Web.HttpContext.Current.Server.UrlDecode(this.QueryString(this.QueryString("BinderID") + "_CBDef", "", true));
			CollectionBinder oCollectionBinder = Ophelia.Application.Base.Functions.GetCollectionBinderFromDefinition(sDefinitionString);
			oCollectionBinder.PageSize = int.MaxValue;
			oCollectionBinder.Binding.Value.Definition.PageSize = int.MaxValue;
			oCollectionBinder.ExportToExcel();
		}
		public void PrintCollectionBinder()
		{
			string sDefinitionString = "";
			sDefinitionString = System.Web.HttpContext.Current.Server.UrlDecode(this.QueryString(this.QueryString("BinderID") + "_CBDef", "", true));
			CollectionBinder oCollectionBinder = Ophelia.Application.Base.Functions.GetCollectionBinderFromDefinition(sDefinitionString);
			oCollectionBinder.PageSize = int.MaxValue;
			oCollectionBinder.Binding.Value.Definition.PageSize = int.MaxValue;
			oCollectionBinder.CheckBoxes = false;
			this.Controls.Add(oCollectionBinder);
			this.Script.AppendLine("window.print();");
		}
		#endregion
		public string DrawByPageEvent {
			get { return "DrawCollectionBinderByPage('" + this.Binder.ID + "',document.getElementById('" + this.Binder.ID + "_NextPageIndex').value,document.getElementById('" + this.Binder.ID + "_CBDef').innerHTML);"; }
		}
		public string RedrawEvent {
			get { return "RedrawCollectionBinder('" + this.Binder.ID + "', document.getElementById('" + this.Binder.ID + "_CBDef').innerHTML, document.getElementById('" + this.Binder.ID + "_SelectedRowItemIDs').innerHTML);"; }
		}
		public string ExportToExcelEvent {
			get { return "ExportCollectionBinderToExcel('" + this.Binder.ID + "','" + this.GetType().FullName + "');"; }
		}
		public string PrintEvent {
			get { return "PrintCollectionBinder('" + this.Binder.ID + "','" + this.GetType().FullName + "');"; }
		}

		protected virtual void AddCustomFunctionalities()
		{
		}
		protected void AddFunctionalities()
		{
			if (this.Binder.Configuration.AllowRefresh || this.Binder.Configuration.AllowConfiguration) {
				this.AddAjax("RedrawCollectionBinder", "", "BinderID,BinderDef,BinderSelectedRowItemIDs");
			}
			this.AddDrawCollectionBinderByPageScript();
			if (this.Binder.Configuration.AllowCreateExcelDocument) {
				this.AddExportToExcelScript();
			}
			if (this.Binder.Configuration.AllowPrint) {
				this.AddPrintScript();
			}
			this.AddCustomFunctionalities();
		}
		private void AddDrawCollectionBinderByPageScript()
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.AjaxFunction DrawByPageFunction = this.Script.AddAjaxFunction("DrawCollectionBinderByPage", "", "", "BinderID,Page,BinderDef", false, true);
			DrawByPageFunction.AjaxRequestParameter.Add("BinderID", "'+" + "BinderID" + "+'");
			DrawByPageFunction.AjaxRequestParameter.Add("" + "BinderDef" + "", "'+" + "BinderDef" + "+'");
			DrawByPageFunction.AjaxRequestParameter.Add("" + "Page" + "", "'+" + "Page" + "+'");
			DrawByPageFunction.AjaxRequestParameter.Add("" + "AjaxControlType" + "", "'+'" + this.GetType().FullName + "'+'");
			DrawByPageFunction.AjaxRequestParameter.Add("" + "AjaxControlID" + "", "'+'" + this.ID + "'+'");
			DrawByPageFunction.AjaxRequestParameter.Add("" + "CallBackFunctionName" + "", "'+'" + "DrawCollectionBinderByPage" + "'+'");
			this.CustomizeAjaxFunctionProperties(DrawByPageFunction);

			//DrawByPageFunction.OverlayElementID = "CB_" & "'+ BinderID + '"
			DrawByPageFunction.AppendLine(" var ResponseText = new String(" + DrawByPageFunction.HttpObject + ".responseText); ");
			DrawByPageFunction.AppendLine(" var ResponseList = ResponseText.split(';;;;'); ");
			DrawByPageFunction.AppendLine(" var CBRowsContent=ResponseList[0];");
			DrawByPageFunction.AppendLine(" var BinderID = ResponseList[1];");
			//DrawByPageFunction.AppendLine("alert(BinderID)")
			DrawByPageFunction.AppendLine(" var element = document.getElementById('CB_" + "'+ BinderID + '" + "_Footer');");
			DrawByPageFunction.AppendLine(" var tableElement = document.getElementById('CB_" + "'+ BinderID" + ").getElementsByTagName('tbody')[0];");
			DrawByPageFunction.AppendLine("    if (element != null){    ");
			DrawByPageFunction.AppendLine("         tableElement.removeChild(tableElement.lastChild);} ");
			//DrawByPageFunction.AppendLine("        tableElement.innerHTML = tableElement.innerHTML + CBRowsContent; ") 'IE'de çalışmıyor. O yüzden jQuery html() ile problem giderildi.
			DrawByPageFunction.AppendLine("    $(tableElement).html(tableElement.innerHTML + CBRowsContent) ");
			DrawByPageFunction.AppendLine("    if (!(ResponseList[2] == 'collectionLoaded')) {");
			//Tüm sayfalar çizildiyse, moreButton çizilmesin.
			DrawByPageFunction.AppendLine("        tableElement.appendChild(element);");
			DrawByPageFunction.AppendLine("        document.getElementById('" + "'+ BinderID + '" + "_NextPageIndex').value = parseInt(document.getElementById(" + " BinderID + '" + "_NextPageIndex').value) +1 ; }");
			DrawByPageFunction.AppendLine("CallbackAjaxFunction( " + DrawByPageFunction.HttpObjectResult + ");");
			this.Script.Manager.AddGeneralJQueryLibrary();
		}
		private void AddPrintScript()
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function PrintFunction = this.Script.AddFunction("PrintCollectionBinder", "", "BinderID,DelegaterName");
			PrintFunction.AppendLine("var newform = document.getElementById('PrintCollectionBinder')");
			PrintFunction.AppendLine("if (!(newform == undefined || newform == null))");
			PrintFunction.AppendLine("    document.body.removeChild(newform);");
			PrintFunction.AppendLine("newform = document.createElement('form'); newform.id = 'PrintCollectionBinderElement'; newform.style.visibility='hidden'");
			PrintFunction.AppendLine("var body = document.getElementsByTagName('body')[0]; ");
			PrintFunction.AppendLine("body.insertBefore(newform, body.childNodes[0]); ");
			PrintFunction.AppendLine("var newinput = document.createElement('input');");
			PrintFunction.AppendLine("newinput.type = 'text'; newinput.name = 'AjaxRequestWithApplication';");
			PrintFunction.AppendLine("newinput.value = 'PrintCollectionBinder,,CallBackFunctionName,,PrintCollectionBinder$$$' + BinderID + '_CBDef,,' + document.getElementById(BinderID + '_CBDef').innerHTML + '$$$BinderID,,' + BinderID + '$$$AjaxControlType,,' + DelegaterName;");
			PrintFunction.AppendLine("newform.appendChild(newinput);newform.method = 'POST';");
			PrintFunction.AppendLine("newform.target = '_blank'; newform.action = '/Ophelia/CollectionBinder/?OP=PrintCollectionBinder';");
			PrintFunction.AppendLine("document.body.appendChild(newform)");
			PrintFunction.AppendLine("newform.submit();");
		}
		private void AddExportToExcelScript()
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function ExportFunction = this.Script.AddFunction("ExportCollectionBinderToExcel", "", "BinderID,DelegaterName");
			ExportFunction.AppendLine("var newform = document.getElementById('ExportCollectionBinderToExcelElement')");
			ExportFunction.AppendLine("if (!(newform == undefined || newform == null))");
			ExportFunction.AppendLine("    document.body.removeChild(newform);");
			ExportFunction.AppendLine("newform = document.createElement('form'); newform.id = 'ExportCollectionBinderToExcelElement'");
			ExportFunction.AppendLine("var body = document.getElementsByTagName('body')[0]; ");
			ExportFunction.AppendLine("body.insertBefore(newform, body.childNodes[0]); ");
			ExportFunction.AppendLine("var newinput = document.createElement('input');");
			ExportFunction.AppendLine("newinput.type = 'text'; newinput.name = 'AjaxRequestWithApplication';");
			ExportFunction.AppendLine("newinput.value = 'ExportCollectionBinderToExcel,,CallBackFunctionName,,ExportToExcel$$$' + BinderID + '_CBDef,,' + document.getElementById(BinderID + '_CBDef').innerHTML + '$$$BinderID,,' + BinderID + '$$$AjaxControlType,,' + DelegaterName;");
			ExportFunction.AppendLine("newform.appendChild(newinput);newform.method = 'POST';");
			ExportFunction.AppendLine("newform.target = '_blank'; newform.action = '/Ophelia/CollectionBinder/?OP=CreateExcelDocument';");
			ExportFunction.AppendLine("document.body.appendChild(newform)");
			ExportFunction.AppendLine("newform.submit();");
		}
		protected override void OnLoad()
		{
			if (this.Binder == null) {
				throw new Exception("BinderPropertyMustBeSet");
			}
			this.Controls.AddLabel(this.Binder.ID + "_CBDef", Functions.GetCollectionBinderDefinition(this.Binder, true)).Style.Display = DisplayMethod.Hidden;
			this.Controls.AddLabel(this.Binder.ID + "_SelectedRowItemIDs", "").Style.Display = DisplayMethod.Hidden;
			this.AddFunctionalities();
		}
		public CollectionBinderAjaxDelegate()
		{
		}
	}
}

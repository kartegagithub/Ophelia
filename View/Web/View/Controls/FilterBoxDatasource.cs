using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.Script.Serialization;
namespace Ophelia.Web.View.Controls
{
	public class FilterBoxDatasource : AjaxControl
	{
		protected override sealed void OnLoad()
		{
			Ophelia.Web.View.Controls.ServerSide.ScriptManager.AjaxFunction AjaxFunction = this.Script.AddAjaxEvent("ReloadFilterBox", this.GetType().FullName, "ReloadData", "", true);
			AjaxFunction.Parameters.Add("ID");
			AjaxFunction.Parameters.Add("ECDef");
			AjaxFunction.Parameters.Add("CCMNStr");
			AjaxFunction.AjaxRequestParameter.Add("FID", "'+ID+'");
			AjaxFunction.AjaxRequestParameter.Add("FECDef", " '+ECDef+'");
			AjaxFunction.AjaxRequestParameter.Add("FCCMNStr", "'+CCMNStr+'");
			AjaxFunction.AjaxRequestParameter.Add("FBFV", "'+  FBTempValue.split('&').join('****') +'");
			AjaxFunction.ShowOverlay = false;
			AjaxFunction.OnBeforeRequestStarted = "document.getElementById(ID + '_Info').innerHTML = '" + "<img style=\"float:right;height:13px;width:13px;\" src=\"" + Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("AjaxLoader", , , "gif") + "\"/>" + "';";
			this.Script.AppendLine("var image= document.createElement('img');image.src=\"" + Ophelia.Web.View.Controls.ServerSide.ImageDrawer.GetImageUrl("AjaxLoader", , , "gif") + "\"");
		}
		protected string FilterParameter {
			get { return this.QueryString("FBFV"); }
		}
		protected string GridID {
			get { return this.QueryString("FID"); }
		}
		protected internal override sealed bool Authorize()
		{
			return base.Authorize();
		}
		public override sealed void CloneEventsFrom(WebControl WebControl)
		{
			base.CloneEventsFrom(WebControl);
		}
		protected override sealed void DrawEvents(Content Content)
		{
			base.DrawEvents(Content);
		}
		public override sealed void OnBeforeDraw(Content Content)
		{
			base.OnBeforeDraw(Content);
		}
		public override sealed Style CreateStyle()
		{
			return base.CreateStyle();
		}
		protected override sealed void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
		}
		private EntityCollection GetCollectionFromJson(System.Collections.Generic.Dictionary<string, object> Definition)
		{
			EntityCollection Collection = null;

			Collection = this.Page.Client.Application.GetCollection(Definition["entitytype"]);

			if (Information.IsNumeric(Definition["pagesize"]) && 0 < Convert.ToInt32(Definition["pagesize"]) && Convert.ToInt32(Definition["pagesize"]) < 31) {
				Collection.Definition.PageSize = Definition["pagesize"];
			} else {
				Collection.Definition.PageSize = 10;
			}
			if (Definition["distinct"] == "true") {
				Collection.Definition.Distinct = true;
			}

			System.Array Filters = Definition["filters"];
			System.Collections.Generic.Dictionary<string, object> FilterElements = null;
			for (int j = 0; j <= Filters.Length - 1; j++) {
				FilterElements = Filters(j);
				Filter Filter = Collection.Definition.Filters.Add(FilterElements["name"]);
				Filter.AsIs = FilterElements["asis"];
				Filter.Comparison = FilterElements["comparison"];
				Filter.Constraint = FilterElements["constraint"];
				Filter.Excludes = FilterElements["excludes"];
				Filter.Prefix = FilterElements["prefix"];
				Filter.Suffix = FilterElements["suffix"];
				for (int m = 0; m <= FilterElements["parameters"].Length - 1; m++) {
					if (FilterElements["parameters"](m)("value").ToString == "System.Object[]") {
						Filter.Parameters.Add(this.GetCollectionFromJson(FilterElements["parameters"](m)("value")(0)("collection")).Definition);
						Filter.Property = Collection.Definition.EntityDefinition.Properties(FilterElements["name"]);
					} else {
						Filter.Parameters.Add(FilterElements["parameters"](m)("value"));
					}
				}
			}

			System.Array Sorters = Definition["sorters"];
			System.Collections.Generic.Dictionary<string, object> Elements = null;
			for (int j = 0; j <= Sorters.Length - 1; j++) {
				Elements = Sorters(j);
				Sorter Sorter = Collection.Definition.Sorters.Add(Elements["name"]);
				Sorter.Direction = Elements["direction"];
				Sorter.NullsOrder = Elements["nullsorder"];
			}

			System.Array Groupers = Definition["groupers"];
			for (int j = 0; j <= Groupers.Length - 1; j++) {
				Elements = Groupers(j);
				Grouper Grouper = new Grouper();
				Grouper.Name = Elements["name"];
				Collection.Definition.Groupers.Add(Grouper);
			}

			System.Array RequestedProperties = Definition["requestedproperties"];
			for (int j = 0; j <= RequestedProperties.Length - 1; j++) {
				Elements = RequestedProperties(j);
				Property Property = Collection.Definition.RequestProperty(Elements["name"], Elements["accesstype"]);
				Property.Alias = Elements["alias"];
			}

			System.Array PropertiesRestrictedTo = Definition["propertiesrestrictedto"];
			for (int j = 0; j <= PropertiesRestrictedTo.Length - 1; j++) {
				Elements = PropertiesRestrictedTo(j);
				Property Property = Collection.Definition.PropertiesRestrictedTo.Add(Elements["name"]);
				Property.Alias = Elements["alias"];
				Property.AccessType = Elements["accesstype"];
			}

			System.Array RelatedProperties = Definition["relatedproperties"];
			for (int j = 0; j <= RelatedProperties.Length - 1; j++) {
				Elements = RelatedProperties(j);
				Property Property = Collection.Definition.RelatedProperties.Add(Elements["name"]);
				Property.Alias = Elements["alias"];
				Property.AccessType = Elements["accesstype"];
				Property.PropertyInfo = GetPropertyInfo(Collection.Definition.EntityDefinition.EntityType, Elements["name"]);
				Property.RelatedPropertyInfo = GetPropertyInfo(this.Page.Client.Application.GetType(Elements["relatedpropertypropertytypename"]), Elements["relatedpropertyinfoname"]);
			}
			return Collection;
		}
		protected virtual EntityCollection SearchCollection(EntityCollection Collection, string SearchProperty, string SearchValue, FilterComparison FilterComparison)
		{
			Collection = Collection.FilterBy(SearchProperty, SearchValue, FilterComparison.StartsWith);
			return Collection;
		}
		public void ReloadData()
		{
			WebControl ReturnControl = this.OnReloadData(this.QueryString("FID"), this.QueryString("FECDef", "", true), this.QueryString("FBFV"));
			if (ReturnControl.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Controls.DataGrid.DataGrid")) || ReturnControl.GetType.FullName == "Ophelia.Web.View.Controls.DataGrid.DataGrid") {
				Controls.DataGrid.DataGrid DataGrid = ReturnControl;
				if (DataGrid.Columns.Count == 1) {
					DataGrid.HideColumnsHeader = true;
				}
				DataGrid.OnRowClick = "SelectFBRow(this);";
				DataGrid.ContainerStyle.PositionStyle = Position.Absolute;
				DataGrid.ContainerStyle.PositionLeft = 0;
				DataGrid.ContainerStyle.ZIndex = 9999;
				this.StyleSheet.AddCustomRule(".SelectedFBRow", "background-color:" + this.QueryString("SelectedBackgroundColor", "black") + " !important;color:" + this.QueryString("SelectedTextColor", "white") + " !important;");
				this.StyleSheet.AddCustomRule("." + this.QueryString("FID") + "RowStyle:hover", "background-color:" + this.QueryString("SelectedBackgroundColor", "black") + ";color:" + this.QueryString("SelectedTextColor", "white") + ";");
				this.Script.AppendLine("document.getElementById('" + this.QueryString("FID") + "_Info').innerHTML = '...';");
				this.Script.AppendLine("document.getElementById('" + this.QueryString("FID") + "Container').style.top = document.getElementById('" + this.QueryString("FID") + "').offsetHeight;");
			}
			this.Content.Add(ReturnControl.Draw);
		}
		protected virtual WebControl OnReloadData(string WebControlID, string CollectionDefinition, string SearchCriteria)
		{
			EntityCollection Collection = null;

			DataGrid.DataGrid DataGrid = null;


			JavaScriptSerializer Serializer = new JavaScriptSerializer();
			System.Collections.Generic.Dictionary<string, object> Definition = null;

			string CollectionDef = Functions.DecryptWithEncoding(System.Text.Encoding.UTF8, CollectionDefinition, "FilterBox");
			try {
				Definition = Serializer.DeserializeObject(CollectionDef);
			} catch (Exception ex) {
				int a = 0;
			}

			if (Definition["binder"](0)("datasourceisentitycollection") == "True") {
				Binders.CollectionBinder CollectionBinder = new Binders.CollectionBinder(WebControlID);
				CollectionBinder.AlwaysHidePager = true;
				Collection = this.GetCollectionFromJson(Definition["binder"](2)("collection"));
				CollectionBinder.PageSize = Collection.Definition.PageSize;
				Collection = SearchCollection(Collection, Definition["binder"](0)("searchfilter"), SearchCriteria.Replace("****", "&"), FilterComparison.StartsWith);
				DataGrid = CollectionBinder;
				DataGrid.Binding.Value = Collection;
			} else {
				DataGrid = new DataGrid.DataGrid(WebControlID);
				if ((UI.Current.GetType.GetInterface("Ophelia.Web.View.Controls.IFilterBoxDataSource") != null)) {
					DataGrid.Binding.Value = ((IFilterBoxDataSource)UI.Current).GetCollection(Definition["binder"](0)("searchfilter"), SearchCriteria.Replace("****", "&"));
				}
			}

			DataGrid.HasForm = false;

			DataGrid.NoRowMessage = Definition["binder"](0)("norowmessage");
			System.Array Columns = Definition["binder"](1)("columns");
			Base.DataGrid.Column Column = null;
			for (int i = 0; i <= Columns.Length - 1; i++) {
				switch (Columns(i)("type")) {
					case "Ophelia.Web.View.Base.DataGrid.CheckBoxColumn":
						Column = DataGrid.Columns.AddCheckBoxColumn(Columns(i)("name"), Columns(i)("membername"), Columns(i)("width"));
						break;
					case "Ophelia.Web.View.Base.DataGrid.DateTimeColumn":
						Column = DataGrid.Columns.AddDateTimeColumn(Columns(i)("name"), Columns(i)("membername"), Columns(i)("width"));
						break;
					case "Ophelia.Web.View.Base.DataGrid.InfinityNumberBoxColumn":
						Column = DataGrid.Columns.AddInfinityNumberBoxColumn(Columns(i)("name"), Columns(i)("membername"), Convert.ToInt32(Columns(i)("width")), "");
						((Base.DataGrid.NumberBoxColumn)Column).DecimalDigits = Columns(i)("decimaldigits");
						break;
					case "Ophelia.Web.View.Base.DataGrid.NumberBoxColumn":
					case "Ophelia.Web.View.Base.DataGrid.NumberBoxTextColumn":
						Column = DataGrid.Columns.AddNumberBoxColumn(Columns(i)("name"), Columns(i)("membername"), Columns(i)("width"));
						((Base.DataGrid.NumberBoxColumn)Column).DecimalDigits = Columns(i)("decimaldigits");
						break;
					default:
						Column = DataGrid.Columns.AddTextBoxColumn(Columns(i)("name"), Columns(i)("membername"), Convert.ToInt32(Columns(i)("width")));
						break;
				}
				Column.Suffix = Columns(i)("suffix");
				Column.Suffix = Columns(i)("membernametobedrawn");
				Column.ReadOnly = true;
			}
			return DataGrid;
		}
	}
}

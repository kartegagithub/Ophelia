﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ophelia.Web.UI.Controls;
using Ophelia.Reflection;
using Ophelia.Web.View.Mvc.Models;
using System.Reflection;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using Ophelia.Web.Extensions;
using System.Transactions;
using Ophelia.Data;
using Ophelia.Data.Querying.Query;
using Ophelia;
using Ophelia.Web.View.Mvc.Controls.Binders.CollectionBinder.Columns;
using System.Collections;

namespace Ophelia.Web.View.Mvc.Controls.Binders.CollectionBinder
{
    public class CollectionBinder<TModel, T> : Panel, IDisposable where T : class where TModel : ListModel<T>
    {
        private UrlHelper _Url;
        public UrlHelper Url
        {
            get
            {
                if (this._Url == null)
                {
                    if (this.viewContext == null)
                        throw new Exception("Please override onViewContextSet function. ViewContext is not ready yet.");
                    this._Url = new UrlHelper(this.viewContext.RequestContext);
                }
                return this._Url;
            }
        }
        public bool IsAjaxRequest
        {
            get
            {
                return this.Request["IsAjaxRequest"] == "1";
            }
        }
        public HttpRequest Request { get { return this.Client.Request; } }
        public HttpResponse Response { get { return this.Client.Response; } }
        public TModel DataSource { get; private set; }
        public Controllers.Base.Controller Controller { get; set; }
        public Client Client { get; private set; }
        public string Title { get; set; }
        private readonly ViewContext viewContext;
        protected IQueryable<IGrouping<object, T>> GroupedData { get; set; }
        public Configuration Configuration { get; private set; }
        public FilterPanel<TModel, T> FilterPanel { get; private set; }
        public GrouperList<T> Groupers { get; private set; }
        public List Breadcrumb { get; private set; }
        public List ActionButtons { get; private set; }
        public List<Columns.BaseColumn<TModel, T>> Columns { get; private set; }
        public bool ParentDrawsLayout { get; set; }
        protected ContentRenderMode ContentRenderMode { get; set; }
        protected virtual bool CanExport
        {
            get
            {
                return false;
            }
        }
        protected virtual string ExportOutputFileFormat
        {
            get
            {
                return "xls";
            }
        }
        protected virtual string ExportDocumentName
        {
            get
            {
                return "";
            }
        }
        public virtual string[] DefaultEntityProperties
        {
            get
            {
                return null;
            }
        }
        public CollectionBinder(Client client, TModel dataSource, string title)
        {
            this.ContentRenderMode = ContentRenderMode.Normal;
            this.Client = client;
            this.Configuration = new Configuration();
            this.DataSource = dataSource;
            this.Columns = new List<Binders.CollectionBinder.Columns.BaseColumn<TModel, T>>();
            this.Breadcrumb = new List();
            this.Breadcrumb.CssClass = "breadcrumb";
            this.ActionButtons = new List();
            this.ActionButtons.CssClass = "breadcrumb-elements";
            this.Groupers = new GrouperList<T>();

            this.ID = title;
            this.Title = this.Client.TranslateText(title);
            this.FilterPanel = new FilterPanel<TModel, T>(this);
            this.Configure();
            this.ProcessQuery();
            this.CheckAjaxFunctions();
            this.Export();
        }
        protected virtual void CheckAjaxFunctions()
        {
            if (this.Request["IsAjaxRequest"] == "1" && !string.IsNullOrEmpty(this.Request["CollectionBinderTriggerFunction"]))
            {
                this.Visible = false;
                this.CanRender = false;
                this.Response.Clear();
                this.Response.ClearContent();
                this.Response.ClearHeaders();
                this.Response.Buffer = false;
                this.Response.ContentType = "application/json";
                object result = new { success = 0, message = "" };
                switch (this.Request["CollectionBinderTriggerFunction"])
                {
                    case "OnColumnsResized":
                        if (!string.IsNullOrEmpty(this.Request["Columns"]))
                        {
                            result = this.OnColumnsResized(this.Request["Columns"].Split(';').ToList());
                        }
                        break;
                    case "OnColumnOrderChange":
                        if (!string.IsNullOrEmpty(this.Request["Columns"]))
                        {
                            result = this.OnColumnOrderChanged(this.Request["Columns"].Split(',').ToList());
                        }
                        break;
                    case "OnCellValueChange":
                        if (this.Configuration.RowUpdateType == RowUpdateType.CellValueChange && this.DataSource.Items != null && this.DataSource.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(this.Request["Column"]) && this.Request["ClassName"] == GetClassName() && !string.IsNullOrEmpty(this.Request["Identifier"]))
                            {
                                var column = this.Columns.Where(op => op.FormatName() == this.Request["Column"]).FirstOrDefault();
                                if (column != null && column.IdentifierExpression != null)
                                {
                                    T item = null;
                                    item = this.DataSource.Items.Where(op => Convert.ToString(column.IdentifierExpression.GetValue(op)) == this.Request["Identifier"]).FirstOrDefault();
                                    if (item != null)
                                    {
                                        var oldValue = column.GetValue(item);
                                        if (Convert.ToString(oldValue) != this.Request["Value"])
                                        {
                                            result = this.OnCellValueChanged(item, column, this.Request["Value"]);
                                        }
                                        else
                                            result = new { success = 0, message = this.Client.TranslateText("ValueIsNotChanged") };
                                    }
                                    else
                                        result = new { success = 0, message = this.Client.TranslateText("InvalidIdentifier") };
                                }
                                else
                                    result = new { success = 0, message = this.Client.TranslateText("InvalidColumnOrConfiguration") };
                            }
                            else
                                result = new { success = 0, message = this.Client.TranslateText("InvalidParameters") };
                        }
                        break;
                }
                this.Response.Write(result.ToJson());
                this.Response.End();
            }
        }
        protected virtual object OnColumnsResized(List<string> Columns)
        {
            return new { success = 0, message = "" };
        }
        protected virtual object OnColumnOrderChanged(List<string> Columns)
        {
            return new { success = 0, message = "" };
        }
        protected virtual object OnCellValueChanged(T item, Columns.BaseColumn<TModel, T> Column, string Value)
        {
            return new { success = 0, message = "" };
        }
        protected virtual void ProcessQuery()
        {
            if (this.Configuration.AllowGrouping && this.Configuration.EnableGroupingByDragDrop)
            {
                foreach (var column in this.Columns)
                {
                    if (column.EnableGrouping && column.Expression != null && this.EnableGrouping(column))
                    {
                        this.Groupers.Add(column.Expression);
                        this.Groupers.LastOrDefault().IsSelected = this.IsDefaultSelected(this.Groupers.LastOrDefault());
                    }
                    else
                        column.EnableGrouping = false;
                }
            }
            var additionalParams = new Dictionary<string, object>();
            var isQueryableDataSet = false;
            if (this.DataSource.Query != null)
                isQueryableDataSet = this.DataSource.Query.GetType().IsQueryableDataSet();

            if (this.DataSource.RemoteDataSource != null && this.DataSource.Query == null && !this.DataSource.DataImportPreview && !this.ParentDrawsLayout)
            {
                this.DataSource.Query = this.DataSource.Items.AsQueryable();
            }
            if ((this.DataSource.RemoteDataSource != null || this.DataSource.Items == null || this.DataSource.Items.Count == 0) && this.DataSource.Query != null && !this.DataSource.DataImportPreview)
            {
                var defaultModel = Activator.CreateInstance(typeof(TModel));

                foreach (Ophelia.Web.View.Mvc.Controls.Binders.Fields.BaseField<TModel> item in this.FilterPanel.Controls)
                {
                    try
                    {
                        var path = "";

                        if (item.Expression != null)
                            path = item.Expression.ParsePath();
                        else
                        {
                            if (!string.IsNullOrEmpty(item.DataControl.ID))
                            {
                                path = item.DataControl.ID.Replace("_", ".");
                                if (item.DataControl.ID.IndexOf("Filters") == -1)
                                    path = "Filters." + path;
                            }
                            else if (item is Fields.DateField<TModel> dateFieldItem)
                            {
                                if (dateFieldItem.LowExpression != null)
                                    path = dateFieldItem.LowExpression.ParsePath().Replace("Low", "");
                                else if (dateFieldItem.HighExpression != null)
                                    path = dateFieldItem.HighExpression.ParsePath().Replace("High", "");
                            }
                            else if (!string.IsNullOrEmpty(item.Text))
                            {
                                path = item.Text.Replace("_", ".");
                                if (item.Text.IndexOf("Filters") == -1)
                                    path = "Filters." + path;
                            }
                        }
                        if (string.IsNullOrEmpty(path))
                            continue;

                        var entityProp = path.Replace("Filters.", "");
                        string value = "";
                        string lowValue = "";
                        string highValue = "";

                        var doubleSelection = false;
                        if (item is Fields.NumberboxField<TModel> && (item as Fields.NumberboxField<TModel>).Mode == Fields.NumberboxFieldMode.DoubleSelection)
                        {
                            doubleSelection = true;

                            var tmpPath = (item as Fields.NumberboxField<TModel>).LowPropertyName.Replace("_", ".");
                            if ((item as Fields.NumberboxField<TModel>).LowExpression == null || !string.IsNullOrEmpty(this.Request[tmpPath]))
                                lowValue = this.Request[tmpPath];
                            else
                                lowValue = Convert.ToString(item.GetExpressionValue((item as Fields.NumberboxField<TModel>).LowExpression));

                            tmpPath = (item as Fields.NumberboxField<TModel>).HighPropertyName.Replace("_", ".");
                            if ((item as Fields.NumberboxField<TModel>).HighExpression == null || !string.IsNullOrEmpty(this.Request[tmpPath]))
                                highValue = this.Request[tmpPath];
                            else
                                highValue = Convert.ToString(item.GetExpressionValue((item as Fields.NumberboxField<TModel>).HighExpression));
                        }
                        else if (item is Fields.DateField<TModel> && (item as Fields.DateField<TModel>).Mode == Fields.DateFieldMode.DoubleSelection)
                        {
                            doubleSelection = true;

                            var propertyName = (item as Fields.DateField<TModel>).LowPropertyName;
                            if (!string.IsNullOrEmpty(propertyName))
                                propertyName = propertyName.Replace("_", ".");
                            if ((item as Fields.DateField<TModel>).LowExpression == null || !string.IsNullOrEmpty(this.Request[propertyName]))
                                lowValue = this.Request[propertyName];
                            else
                                lowValue = Convert.ToString(item.GetExpressionValue((item as Fields.DateField<TModel>).LowExpression));

                            propertyName = (item as Fields.DateField<TModel>).HighPropertyName;
                            if (!string.IsNullOrEmpty(propertyName))
                                propertyName = propertyName.Replace("_", ".");

                            if ((item as Fields.DateField<TModel>).HighExpression == null || !string.IsNullOrEmpty(this.Request[propertyName]))
                                highValue = this.Request[propertyName];
                            else
                                highValue = Convert.ToString(item.GetExpressionValue((item as Fields.DateField<TModel>).HighExpression));
                        }
                        else
                        {
                            if (item.Expression == null || !string.IsNullOrEmpty(this.Request[path]))
                                value = this.Request[path];
                            else
                                value = Convert.ToString(item.GetExpressionValue());
                        }

                        object formattedValue = null;
                        var defaultValue = Convert.ToString(defaultModel.GetPropertyValue(path));
                        if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(defaultValue) && defaultValue.IsDate())
                        {
                            try
                            {
                                if (DateTime.Parse(defaultValue) == DateTime.MinValue)
                                    defaultValue = "";
                            }
                            catch (Exception)
                            {
                                defaultValue = "";
                            }
                        }
                        if (!string.IsNullOrEmpty(lowValue) && lowValue.IsDate())
                        {
                            try
                            {
                                if (DateTime.Parse(lowValue) == DateTime.MinValue)
                                    lowValue = "";
                            }
                            catch (Exception)
                            {
                                lowValue = "";
                            }
                        }
                        if (!string.IsNullOrEmpty(highValue) && highValue.IsDate())
                        {
                            try
                            {
                                if (DateTime.Parse(highValue) == DateTime.MinValue)
                                    highValue = "";
                            }
                            catch (Exception)
                            {
                                highValue = "";
                            }
                        }
                        if ((doubleSelection && (!string.IsNullOrEmpty(lowValue) || !string.IsNullOrEmpty(highValue))) || (!string.IsNullOrEmpty(this.Request[path]) && defaultValue != value && value != null))
                        {
                            var propTree = typeof(T).GetPropertyInfoTree(entityProp);
                            var propInfo = propTree.LastOrDefault();
                            var propType = propInfo?.PropertyType;
                            if (propType != null)
                            {
                                if (propType.IsGenericType && propType.Name.StartsWith("Null"))
                                    propType = propType.GenericTypeArguments[0];

                                if (propType.Name == "Boolean")
                                {
                                    if (value == "-1")
                                        continue;
                                    else
                                    {
                                        value = value == "1" ? "true" : "false";
                                        formattedValue = Convert.ChangeType(value, propType);
                                        if (isQueryableDataSet)
                                            this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue);
                                        else
                                            this.DataSource.Query = this.DataSource.Query.Where(entityProp + " = @0", formattedValue);
                                    }
                                }
                                else if (propType.Name.Contains("String"))
                                    this.ApplyFilter(entityProp, value, propType, isQueryableDataSet, propTree);
                                else if (propType.IsNumeric())
                                {
                                    if (propType.Name == "Decimal")
                                    {
                                        var customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                                        if (customCulture.NumberFormat.NumberDecimalSeparator == ",")
                                        {
                                            if (!string.IsNullOrEmpty(value))
                                                value = value.Replace(".", ",");
                                            if (!string.IsNullOrEmpty(lowValue))
                                                lowValue = lowValue.Replace(".", ",");
                                            if (!string.IsNullOrEmpty(highValue))
                                                highValue = highValue.Replace(".", ",");
                                        }
                                        else if (customCulture.NumberFormat.NumberDecimalSeparator == ".")
                                        {
                                            if (!string.IsNullOrEmpty(value))
                                                value = value.Replace(",", ".");
                                            if (!string.IsNullOrEmpty(lowValue))
                                                lowValue = lowValue.Replace(",", ".");
                                            if (!string.IsNullOrEmpty(highValue))
                                                highValue = highValue.Replace(",", ".");
                                        }

                                        if (doubleSelection)
                                        {
                                            if (!string.IsNullOrEmpty(lowValue))
                                            {
                                                formattedValue = Convert.ChangeType(lowValue, propType);
                                                if (isQueryableDataSet)
                                                    this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue, Comparison.GreaterAndEqual);
                                                else
                                                    this.DataSource.Query = this.DataSource.Query.Where(entityProp + " >= @0", formattedValue);
                                            }
                                            if (!string.IsNullOrEmpty(highValue))
                                            {
                                                if (isQueryableDataSet)
                                                    this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue, Comparison.LessAndEqual);
                                                else
                                                    this.DataSource.Query = this.DataSource.Query.Where(entityProp + " <= @0", formattedValue);
                                            }
                                        }
                                        else
                                            this.ApplyFilter(entityProp, value, propType, isQueryableDataSet, propTree);
                                    }
                                    else
                                    {
                                        if (value.IndexOf(",") > -1)
                                        {
                                            var orParams = "";
                                            var parameters = new List<object>();
                                            var values = value.Split(',');
                                            var counter = 0;
                                            foreach (var val in values)
                                            {
                                                try
                                                {
                                                    parameters.Add(Convert.ChangeType(val, propType));
                                                    if (!string.IsNullOrEmpty(orParams))
                                                        orParams += " || ";
                                                    orParams += entityProp + " = @" + counter;
                                                }
                                                catch (Exception)
                                                {
                                                    counter++;
                                                    continue;
                                                }
                                                counter++;
                                            }
                                            if (parameters.Count > 0)
                                            {
                                                if (isQueryableDataSet)
                                                    this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, parameters.ToArray(), Comparison.In);
                                                else
                                                    this.DataSource.Query = this.DataSource.Query.Where(orParams, parameters.ToArray());
                                            }
                                        }
                                        else
                                        {
                                            if (doubleSelection)
                                            {
                                                if (!string.IsNullOrEmpty(lowValue))
                                                {
                                                    formattedValue = Convert.ChangeType(lowValue, propType);
                                                    if (isQueryableDataSet)
                                                        this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue, Comparison.GreaterAndEqual);
                                                    else
                                                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " >= @0", formattedValue);
                                                }
                                                if (!string.IsNullOrEmpty(highValue))
                                                {
                                                    formattedValue = Convert.ChangeType(highValue, propType);
                                                    if (isQueryableDataSet)
                                                        this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue, Comparison.LessAndEqual);
                                                    else
                                                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " <= @0", formattedValue);
                                                }
                                            }
                                            else
                                                this.ApplyFilter(entityProp, value, propType, isQueryableDataSet, propTree);
                                        }
                                    }
                                }
                                else
                                {
                                    if (doubleSelection)
                                    {
                                        if (!string.IsNullOrEmpty(lowValue))
                                        {
                                            formattedValue = Convert.ChangeType(lowValue, propType);
                                            if (formattedValue is DateTime dateData)
                                            {
                                                if (dateData > DateTime.MinValue)
                                                    formattedValue = dateData.StartOfDay();
                                            }
                                            if (isQueryableDataSet)
                                                this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue, Comparison.GreaterAndEqual);
                                            else
                                                this.DataSource.Query = this.DataSource.Query.Where(entityProp + " >= @0", formattedValue);
                                        }
                                        if (!string.IsNullOrEmpty(highValue))
                                        {
                                            formattedValue = Convert.ChangeType(highValue, propType);
                                            if (formattedValue is DateTime dateData)
                                            {
                                                if (dateData > DateTime.MinValue)
                                                    formattedValue = dateData.EndOfDay();
                                            }
                                            if (isQueryableDataSet)
                                                this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue, Comparison.LessAndEqual);
                                            else
                                                this.DataSource.Query = this.DataSource.Query.Where(entityProp + " <= @0", formattedValue);
                                        }
                                    }
                                    else
                                        this.ApplyFilter(entityProp, value, propType, isQueryableDataSet, propTree);
                                }
                            }
                            else
                            {
                                additionalParams.Add(entityProp, value);
                            }

                            var modelProperty = typeof(TModel).GetPropertyInfo(path.Remove(path.Length - 2, 2));
                            if (path.EndsWith("ID") && modelProperty != null && (modelProperty.PropertyType.IsDataEntity() || modelProperty.PropertyType.IsPOCOEntity()))
                            {
                                var accessor = new Accessor();
                                accessor.Item = this.DataSource;
                                accessor.MemberName = path.Remove(path.Length - 2, 2);
                                accessor.Value = this.GetReferencedEntity(modelProperty.PropertyType, (formattedValue != null ? formattedValue : value));
                                accessor = null;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                defaultModel = null;
                if (!string.IsNullOrEmpty(this.Request["OrderBy"]))
                {
                    var direction = this.Request["OrderByDirection"]?.ToLower();
                    if (direction != "desc" && direction != "asc")
                        direction = "asc";

                    foreach (var column in this.Columns)
                    {
                        if (column.IsSortable && column.Expression?.ParsePath() == this.Request["OrderBy"])
                        {
                            var propInfo = typeof(T).GetPropertyInfoTree(this.Request["OrderBy"]);
                            var ordering = "";
                            if (propInfo != null && propInfo.Length > 0)
                            {
                                if (this.Request["OrderBy"].EndsWith("ID"))
                                {
                                    try
                                    {
                                        PropertyInfo entityPropInfo = null;
                                        if (propInfo.Length > 1)
                                        {
                                            for (int i = 0; i < propInfo.Length - 1; i++)
                                            {
                                                entityPropInfo = propInfo[i];
                                                if (!string.IsNullOrEmpty(ordering))
                                                    ordering = entityPropInfo.Name;
                                                else
                                                    ordering += "." + entityPropInfo.Name;
                                            }
                                        }
                                        entityPropInfo = propInfo.LastOrDefault();
                                        if (entityPropInfo.PropertyType.IsNullable() || entityPropInfo.PropertyType.IsPrimitiveType())
                                            ordering += "." + entityPropInfo.Name;
                                        else if (entityPropInfo.PropertyType.GetProperty("Name") != null)
                                            ordering += ".Name";
                                        else if (entityPropInfo.PropertyType.GetProperty("Title") != null)
                                            ordering += ".Title";
                                        else
                                            ordering += "." + entityPropInfo.Name;

                                        ordering = ordering.Trim('.');
                                    }
                                    catch (Exception)
                                    {
                                        ordering = this.Request["OrderBy"];
                                    }
                                }
                                else
                                {
                                    ordering = this.Request["OrderBy"];
                                }
                                if (ordering.Contains("()"))
                                {
                                    ordering = string.Join(".", ordering.Split('.').Take(ordering.Split('.').Length - 1));
                                    ordering += "." + typeof(T).GetDisplayTextProperty(ordering).Name;
                                }
                                if (isQueryableDataSet)
                                    this.DataSource.Query = Ophelia.Data.QueryableDataSetExtensions.OrderBy(this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>, ordering + " " + direction);
                                else
                                    this.DataSource.Query = this.DataSource.Query.OrderBy(ordering + " " + direction);
                            }
                        }
                    }
                }
                this.DataSource.Query = this.OnAfterProcessQuery();

                if (this.Request.QueryString.Count > 0 || this.Request.Form.Count > 0)
                {
                    foreach (var grouper in this.Groupers)
                    {
                        if (!grouper.IsSelected)
                            grouper.IsSelected = this.Request[grouper.FormatRequestName()] == "on";

                        if (string.IsNullOrEmpty(this.Request["CollectionBinderTriggerFunction"]))
                            this.SaveGrouping(grouper);
                    }
                }
                var selectedGroupers = this.Groupers.Where(op => op.IsSelected).Select(op => op.Expression).ToList();
                if (selectedGroupers.Count > 0)
                {
                    if (!string.IsNullOrEmpty(this.Request[this.DataSource.GroupPagination.PageKey]) && this.Request[this.DataSource.GroupPagination.PageKey].IsNumeric())
                        this.DataSource.GroupPagination.PageNumber = Convert.ToInt32(this.Request[this.DataSource.GroupPagination.PageKey]);

                    this.ContentRenderMode = ContentRenderMode.Group;

                    //TODO: https://stackoverflow.com/a/12829015/1766100
                    var groupedData = (IQueryable<IGrouping<object, T>>)this.DataSource.Query.GroupBy(selectedGroupers.ToArray());
                    if (this.DataSource.RemoteDataSource != null)
                    {
                        using (var queryData = new QueryData())
                        {
                            using (var visitor = new SQLPreparationVisitor(queryData))
                            {
                                if (this.DataSource.OnBeforeQueryExecuted != null)
                                    this.DataSource.Query = this.DataSource.OnBeforeQueryExecuted(this.DataSource.Query);

                                this.OnBeforeQueryExecuted();
                                visitor.Visit(groupedData.Expression);
                                queryData.GroupPageSize = this.DataSource.Pagination.PageSize;
                                var qs = new Ophelia.Web.Application.Client.QueryString(this.Request);
                                foreach (string item in qs.KeyList)
                                {
                                    if (item.StartsWith("grouppage"))
                                    {
                                        var index = item.Replace("grouppage", "").ToInt32();
                                        queryData.GroupPagination[index] = this.Request[item].ToInt32();
                                    }
                                }
                                var request = new Service.WebApiCollectionRequest<T>() { Page = this.CanExport ? 1 : this.DataSource.Pagination.PageNumber, PageSize = this.CanExport ? int.MaxValue : this.DataSource.Pagination.PageSize, QueryData = queryData.Serialize(), Parameters = additionalParams, TypeName = typeof(T).FullName, Data = this.FiltersToEntity() };
                                if (this.DataSource.OnBeforeRemoteDataSourceCall != null)
                                    request = this.DataSource.OnBeforeRemoteDataSourceCall(request);

                                var response = this.DataSource.RemoteDataSource("Get" + typeof(T).Name.Pluralize(), request);
                                if (response.RawData != null)
                                {
                                    var groupers = new List<Data.Querying.Query.Helpers.Grouper>();
                                    foreach (var item in queryData.Groupers)
                                    {
                                        groupers.AddRange(item.Serialize());
                                    }
                                    var dynamicObjectFields = (from grouper in groupers where !string.IsNullOrEmpty(grouper.Name) && !string.IsNullOrEmpty(grouper.TypeName) select new Ophelia.Reflection.ObjectField() { FieldName = grouper.Name, FieldType = Type.GetType(grouper.TypeName) }).ToList();
                                    var dynamicObject = Ophelia.Reflection.ObjectBuilder.CreateNewObject(dynamicObjectFields);
                                    var groupingType = typeof(Data.Model.OGrouping<,>).MakeGenericType(dynamicObject.GetType(), typeof(T));
                                    var listType = typeof(List<>).MakeGenericType(groupingType);
                                    this.GroupedData = (IQueryable<IGrouping<object, T>>)((response.RawData as Newtonsoft.Json.Linq.JArray).ToObject(listType) as IList).AsQueryable();
                                }
                                else
                                    this.DataSource.Items = (List<T>)response.GetPropertyValue("Data");

                                this.DataSource.GroupPagination.ItemCount = response.TotalDataCount;

                                this.OnAfterQueryExecuted();
                            }
                        }
                    }
                    else
                    {
                        this.DataSource.GroupPagination.ItemCount = groupedData.Count();
                        this.GroupedData = groupedData.Paginate(this.CanExport ? 1 : this.DataSource.GroupPagination.PageNumber, this.CanExport ? int.MaxValue : this.DataSource.GroupPagination.PageSize);
                    }
                    groupedData = null;
                    this.DataSource.Query = null;
                }
                else
                {
                    if (this.DataSource.RemoteDataSource != null && !this.DataSource.DataImportPreview && !this.DataSource.ParentDrawsLayout)
                    {
                        using (var queryData = new QueryData())
                        {
                            using (var visitor = new SQLPreparationVisitor(queryData))
                            {
                                if (this.DataSource.OnBeforeQueryExecuted != null)
                                    this.DataSource.Query = this.DataSource.OnBeforeQueryExecuted(this.DataSource.Query);

                                this.OnBeforeQueryExecuted();
                                visitor.Visit(this.DataSource.Query.Expression);
                                var request = new Service.WebApiCollectionRequest<T>() { Page = this.CanExport ? 1 : this.DataSource.Pagination.PageNumber, PageSize = this.CanExport ? int.MaxValue : this.DataSource.Pagination.PageSize, QueryData = queryData.Serialize(), Parameters = additionalParams, TypeName = typeof(T).FullName, Data = this.FiltersToEntity() };
                                if (this.DataSource.OnBeforeRemoteDataSourceCall != null)
                                    request = this.DataSource.OnBeforeRemoteDataSourceCall(request);

                                var response = this.DataSource.RemoteDataSource("Get" + typeof(T).Name.Pluralize(), request);
                                if (response.RawData != null)
                                    this.DataSource.Items = (List<T>)response.RawData;
                                else
                                    this.DataSource.Items = (List<T>)response.GetPropertyValue("Data");

                                this.DataSource.Pagination.ItemCount = response.TotalDataCount;

                                this.OnAfterQueryExecuted();
                            }
                        }
                    }
                    else
                    {
                        if (this.DataSource.OnBeforeQueryExecuted != null)
                            this.DataSource.Query = this.DataSource.OnBeforeQueryExecuted(this.DataSource.Query);

                        this.OnBeforeQueryExecuted();
                        if (this.DataSource.Query.GetType().IsQueryableDataSet())
                        {
                            this.DataSource.Items = Ophelia.Data.QueryableDataSetExtensions.Paginate(this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>, this.CanExport ? 1 : this.DataSource.Pagination.PageNumber, this.CanExport ? this.GetMaxExportSize() : this.DataSource.Pagination.PageSize).ToList();
                        }
                        else
                        {
                            this.DataSource.Items = this.DataSource.Query.Paginate(this.CanExport ? 1 : this.DataSource.Pagination.PageNumber, this.CanExport ? int.MaxValue : this.DataSource.Pagination.PageSize).ToList();
                        }
                        this.DataSource.Pagination.ItemCount = this.DataSource.Query.Count();

                        this.OnAfterQueryExecuted();
                    }
                }
            }
        }
        protected virtual int GetMaxExportSize()
        {
            return int.MaxValue;
        }
        protected virtual bool EnableGrouping(Columns.BaseColumn<TModel, T> column)
        {
            return true;
        }
        private void ApplyFilter(string entityProp, string value, Type propType, bool isQueryableDataSet, PropertyInfo[] propTree)
        {
            var comparison = Comparison.Contains;
            if (!string.IsNullOrEmpty(this.Request[entityProp + "-Comparison"]))
                comparison = (Comparison)this.Request[entityProp + "-Comparison"].ToInt32();

            value = Convert.ToString(value).Trim();
            var formattedValue = Convert.ChangeType(value, propType);

            if (isQueryableDataSet)
                this.DataSource.Query = (this.DataSource.Query as Ophelia.Data.Model.QueryableDataSet<T>).Where(propTree, formattedValue, comparison);
            else
            {
                switch (comparison)
                {
                    case Comparison.Equal:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " = @0", formattedValue);
                        break;
                    case Comparison.Different:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " != @0", formattedValue);
                        break;
                    case Comparison.StartsWith:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + ".StartsWith(@0)", formattedValue);
                        break;
                    case Comparison.EndsWith:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + ".EndsWith(@0)", formattedValue);
                        break;
                    case Comparison.Contains:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + ".Contains(@0)", formattedValue);
                        break;
                    case Comparison.Less:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " < @0", formattedValue);
                        break;
                    case Comparison.LessAndEqual:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " <= @0", formattedValue);
                        break;
                    case Comparison.Greater:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " > @0", formattedValue);
                        break;
                    case Comparison.GreaterAndEqual:
                        this.DataSource.Query = this.DataSource.Query.Where(entityProp + " >= @0", formattedValue);
                        break;
                }
            }
        }
        protected virtual bool IsDefaultSelected(Grouper<T> grouper)
        {
            return false;
        }
        protected virtual void SaveGrouping(Grouper<T> grouper)
        {

        }
        protected virtual IQueryable<T> OnAfterProcessQuery()
        {
            return this.DataSource.Query;
        }
        protected virtual void OnAfterQueryExecuted()
        {

        }
        protected virtual void OnBeforeQueryExecuted()
        {

        }
        protected virtual T FiltersToEntity()
        {
            try
            {
                var entity = (T)Activator.CreateInstance(typeof(T));
                var filters = this.DataSource.GetPropertyValue("Filters");
                var props = filters.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var entityProp = entity.GetType().GetProperty(prop.Name);
                    try
                    {
                        if (entityProp == null)
                            continue;

                        entityProp.SetValue(entity, prop.GetValue(filters));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
        protected virtual string GetSortingFieldName(PropertyInfo info)
        {
            return info.Name + "ID";
        }
        public CollectionBinder(ViewContext viewContext, TModel dataSource, string title) : this(((Controllers.Base.Controller)viewContext.Controller).Client, dataSource, title)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");

            this.viewContext = viewContext;
            this.Output = this.viewContext.Writer;
            if (this.IsAjaxRequest && this.Response != null && !this.CanExport)
            {
                this.Response.Clear();
            }
            this.Controller = (Controllers.Base.Controller)this.viewContext.Controller;
            this.onViewContextSet();
        }
        protected virtual object GetReferencedEntity(Type entityType, object value)
        {
            return null;
        }
        public virtual void RenderHeader()
        {
            this.RenderBreadcrumb();
            this.Output.Write("<div class='table-container'>");
            this.RenderFilterPanel();
        }

        protected virtual void RenderFilterPanel()
        {

        }

        protected virtual void RenderPagination(bool forGroupItems = false)
        {
            if (!forGroupItems && this.ContentRenderMode == ContentRenderMode.Group)
            {
                var paginator = new Paginator(Convert.ToInt32(this.DataSource.GroupPagination.ItemCount), this.DataSource.GroupPagination.PageSize, this.DataSource.GroupPagination.PageNumber, this.DataSource.GroupPagination.LinkedPageCount, this.DataSource.GroupPagination.PageKey);
                paginator.ExcludedKeys = new string[] { "IsAjaxRequest", "AjaxEntityBinder", "isajaxrequest", "ajaxentitybinder" };
                paginator.StartTitle = this.Client.TranslateText("FirstPage");
                paginator.EndTitle = this.Client.TranslateText("LastPage");
                paginator.NextPageTitle = this.Client.TranslateText("NextPage");
                paginator.PreviousPageTitle = this.Client.TranslateText("PreviousPage");
                paginator.CssClass += " dataTables_paginate";
                paginator.RenderControlAsText(this.Output);
            }
            else if (forGroupItems || this.ContentRenderMode == ContentRenderMode.Normal)
            {
                var paginator = new Paginator(Convert.ToInt32(this.DataSource.Pagination.ItemCount), this.DataSource.Pagination.PageSize, this.DataSource.Pagination.PageNumber, this.DataSource.Pagination.LinkedPageCount, this.DataSource.Pagination.PageKey);
                paginator.ExcludedKeys = new string[] { "IsAjaxRequest", "AjaxEntityBinder", "isajaxrequest", "ajaxentitybinder" };
                paginator.StartTitle = this.Client.TranslateText("FirstPage");
                paginator.EndTitle = this.Client.TranslateText("LastPage");
                paginator.NextPageTitle = this.Client.TranslateText("NextPage");
                paginator.PreviousPageTitle = this.Client.TranslateText("PreviousPage");
                paginator.CssClass += " dataTables_paginate";
                paginator.RenderControlAsText(this.Output);
            }
        }
        protected string GetClassName()
        {
            return typeof(T).GetNamespace() + "." + typeof(T).Name;
        }
        public virtual void RenderContent()
        {
            //if (!this.ParentDrawsLayout)
            //{
            //    if (this.Messages != null && this.Messages.Count > 0)
            //    {
            //        var messageType = !this.Messages.Where(op => op.IsSuccess == true).Any() ? "warning" : "success";
            //        this.Output.Write("<div class=\"alert alert-" + messageType + " alert-styled-left\">");
            //        this.Output.Write("<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span>×</span><span class=\"sr-only\">" + this.Client.TranslateText("Close") + "</span></button>");
            //        this.Output.Write("<ul>");
            //        foreach (var message in this.Messages)
            //        {
            //            this.Output.Write("<li>");
            //            this.Output.Write(this.Client.TranslateText(message.Description));
            //            this.Output.Write("</li>");
            //        }
            //        this.Output.Write("</ul>");
            //        this.Output.Write("</div>"); /* alert */
            //    }
            //}
            if (this.Configuration.EnableGroupingByDragDrop)
            {
                this.DrawGroupingArea();
            }
            this.ReorderColumns();

            if (string.IsNullOrEmpty(this.Configuration.ContentTableCSSClass))
                this.Configuration.ContentTableCSSClass = "table table-striped datatable-responsive-row-control" + (this.Configuration.AllowServerSideOrdering ? " disable-client-side-sorting" : "");
            else
                this.Configuration.ContentTableCSSClass += (this.Configuration.AllowServerSideOrdering ? " disable-client-side-sorting" : "");

            this.Output.Write("<table class=\"" + this.Configuration.ContentTableCSSClass + (this.Configuration.EnableColumnFiltering ? " filterable" : "") + "\" column-filtering-type='" + this.Configuration.ColumnFilteringType.ToString() + "' data-class='" + GetClassName() + "' data-rowupdate='" + this.Configuration.RowUpdateType.ToString() + "' " + (this.Configuration.SaveChangesOnUIInteraction ? "data-savechanges='true'" : "") + ">");

            this.DrawColumnHeaders();

            if (this.GroupedData == null)
            {
                this.DrawItems();
            }
            else if (this.DataSource != null && this.GroupedData != null && this.ContentRenderMode == ContentRenderMode.Group)
            {
                var index = 0;
                var list = new List<IGrouping<object, T>>();
                foreach (var group in this.GroupedData)
                {
                    list.Add(group);
                }
                foreach (var group in list)
                {
                    this.DataSource.Pagination.PageKey = "grouppage" + index;
                    this.DataSource.Pagination.PageNumber = this.Request[this.DataSource.Pagination.PageKey].ToInt32() > 0 ? this.Request[this.DataSource.Pagination.PageKey].ToInt32() : 1;

                    var count = 0;
                    if (group.GetType().Name.IndexOf("OGrouping") > -1)
                        count = (Int32)group.GetPropertyValue("Count");
                    else
                        count = group.Count();

                    this.DataSource.Pagination.ItemCount = count;

                    if (this.DataSource.RemoteDataSource == null)
                        this.DataSource.Items = group.Paginate(this.DataSource.Pagination.PageNumber, this.DataSource.Pagination.PageSize).ToList();
                    else
                        this.DataSource.Items = group.ToList();

                    this.Output.Write("<tbody class='group-header' id='group-header-" + index + "' data-toggle='collapse' href='#group-data-" + index + "'>");
                    this.Output.Write("<tr>");
                    this.Output.Write("<td colspan='" + this.Columns.Count + "'>");

                    var selectedGroupers = this.Groupers.Where(op => op.IsSelected).ToList();
                    var counter = 0;
                    foreach (var grouper in selectedGroupers)
                    {
                        var name = grouper.Expression.ParsePath();
                        if (grouper.Expression.Body.Type.IsClass && !grouper.Expression.Body.Type.FullName.Contains("System."))
                            name += "ID";

                        if (name.IndexOf(".") > -1)
                            name = name.Replace(".", "");

                        var text = Convert.ToString(group.Key.GetPropertyValue(name));

                        if (grouper.Type != null && grouper.Type.IsEnum)
                            text = grouper.Type.GetEnumDisplayName(text, this.Client);
                        else if (grouper.Expression.Body.Type.IsClass && !grouper.Expression.Body.Type.FullName.Contains("System.") && text.IsNumeric() && grouper.DisplayMemberExpression != null)
                        {
                            var refEntity = this.GetReferencedEntity(grouper.Expression.Body.Type, Convert.ToInt64(text));
                            if (refEntity != null)
                            {
                                this.DataSource.Items.ForEach(op => op.SetPropertyValue(grouper.Expression.ParsePath(), refEntity));
                                if (grouper.DisplayMemberExpression != null)
                                    text = Convert.ToString(grouper.DisplayMemberExpression.Execute(this.DataSource.Items[0]));
                            }
                        }
                        else if (text.IsNumeric() && grouper.FormatName().EndsWith("ID"))
                        {
                            var prop = typeof(T).GetProperty(grouper.FormatName().Remove(grouper.FormatName().LastIndexOf("ID"), 2));
                            if (prop != null)
                            {
                                var refEntity = this.GetReferencedEntity(prop.PropertyType, Convert.ToInt64(text));
                                if (refEntity != null)
                                {
                                    this.DataSource.Items.ForEach(op => prop.SetValue(op, refEntity));
                                    if (grouper.DisplayMemberExpression != null)
                                        text = Convert.ToString(grouper.DisplayMemberExpression.Execute(this.DataSource.Items[0]));
                                    else
                                        text = this.GetDisplayName(refEntity);
                                }
                            }
                        }
                        this.Output.Write("<label class='grouper-title' title='" + grouper.FormatText(this.Client) + "'>" + text + "</label>");
                        counter++;
                        if (counter < selectedGroupers.Count)
                            this.Output.Write(", ");
                    }

                    this.Output.Write("<label class='group-count'>" + this.Client.TranslateText("Count") + ": " + count + "</label></td>");
                    this.Output.Write("</tr>");
                    this.Output.Write("</tbody>");
                    this.DrawItems("group-data collapse", "group-data-" + index);
                    index++;
                }
                list = null;
            }
            else
            {
                this.Output.Write("<tr><td colspan='" + this.Columns.Count + "'><div class=\"alert alert-info alert-styled-left alert-bordered empty-table-warning\">" + this.Client.TranslateText("ThereIsNoRecordToDisplay") + "</div></td></tr>");
            }
            this.Output.Write("</table>");
        }
        protected virtual void DrawItems(string className = "", string tbodyId = "")
        {
            if (this.DataSource == null || this.DataSource.Items == null && this.DataSource.Items.Count == 0)
                return;

            if (string.IsNullOrEmpty(className))
                className = "table-body";
            if (string.IsNullOrEmpty(tbodyId))
                tbodyId = Guid.NewGuid().ToString();
            this.Output.Write("<tbody class='" + className + "' id='" + tbodyId + "'>");

            if (!this.Configuration.ColumnFiltersInHead && !this.ParentDrawsLayout && this.GroupedData == null)
                this.RenderColumnFilters();

            T blankItem = null;
            if (this.Configuration.AllowNewRow && this.GroupedData == null)
            {
                blankItem = this.CreateNewItem();
                if (blankItem != null)
                    this.DataSource.Items.Insert(0, blankItem);
            }
            var counter = -1;
            var allIDs = "";
            if (this.Configuration.AppendListOfIDOnItemLink)
            {
                foreach (T item in this.DataSource.Items)
                {
                    if (!string.IsNullOrEmpty(allIDs))
                        allIDs += ",";
                    allIDs += item.GetPropertyValue("ID");
                }
                allIDs = "IDList=" + allIDs;
            }
            foreach (T item in this.DataSource.Items)
            {
                counter++;
                var link = new Link();
                try
                {
                    link.URL = this.GetItemLink(item);
                }
                catch (Exception)
                {
                    link.URL = "javascript:void(0);";
                }
                if (link.URL.IndexOf("?") == -1)
                    link.URL += "?";

                link.URL = link.URL.Trim('&') + "&" + allIDs;

                this.Output.Write("<tr ");
                this.Output.Write("data-name=\"" + this.GetDisplayName(item) + "\" ");
                this.RenderRowProperties(item, counter);
                this.Output.Write(">");
                if (this.Configuration.AddBlankColumnToStart)
                    this.Output.Write("<td></td>");
                this.RenderOnBeforeDrawLine(item);
                if (this.Configuration.Checkboxes && !string.IsNullOrEmpty(this.Configuration.CheckboxProperty) && item.GetType().GetProperties().Where(op => op.Name == this.Configuration.CheckboxProperty).Any())
                {
                    this.Output.Write("<td>");
                    if (this.OnDrawCheckbox(item))
                    {
                        var id = this.Configuration.CheckboxProperty;
                        var identifier = "";
                        if (!string.IsNullOrEmpty(this.Configuration.CheckboxIdentifierProperty))
                        {
                            identifier = Convert.ToString(item.GetPropertyValue(this.Configuration.CheckboxIdentifierProperty));
                            id += "_" + identifier;
                        }
                        var val = Convert.ToBoolean(item.GetPropertyValue(this.Configuration.CheckboxProperty));

                        this.Output.Write("<input type='checkbox' class='binder-checkbox' name='" + this.Configuration.CheckboxProperty + "' id='" + id + "'" + (val ? " checked" : "") + " value='" + identifier + "' " + this.GetBinderCheckboxProperties(item) + ">");
                    }
                    this.Output.Write("</td>");
                }
                foreach (var column in this.Columns)
                {
                    if (column.Visible)
                    {
                        if (this.Groupers.Where(op => op.IsSelected && (op.FormatName() == column.FormatName())).Any())
                            continue;

                        this.OnBeforeGetCellValue(item, column);
                        var value = column.GetValue(item);
                        link.Text = Convert.ToString(value).RemoveHTML();
                        link.Title = link.Text;
                        if (column.MaxTextLength > 0)
                        {
                            if (!string.IsNullOrEmpty(link.Text) && link.Text.Length > column.MaxTextLength)
                            {
                                link.Text = link.Text.Left(column.MaxTextLength) + "...";
                            }
                        }
                        try
                        {
                            link.ID = column.FormatName() + "_" + item.GetPropertyValue("ID");
                            link.Name = column.FormatName() + "_" + item.GetPropertyValue("ID");
                        }
                        catch { }
                        var tdClassName = "";
                        this.Output.Write("<td ");
                        if (!string.IsNullOrEmpty(column.Width))
                            this.Output.Write("style='width:" + (column.Width.IndexOf("%") == -1 ? column.Width + "px" : column.Width) + "'");
                        if (column.Alignment == System.Web.UI.WebControls.HorizontalAlign.Right)
                            tdClassName += " text-right";
                        if (!string.IsNullOrEmpty(column.Width))
                            tdClassName += " has-width";
                        this.Output.Write("class='" + tdClassName + "'");
                        this.RenderCellProperties(item, column, link);
                        this.Output.Write(">");
                        this.OnBeforeRenderCell(item, column);
                        if (column.AllowEdit)
                        {
                            using (var editControl = column.GetEditableControl(item, value, this.Request))
                            {
                                editControl.AddAttribute("data-filters", this.Request.QueryString.ToString().Replace("IsAjaxRequest=1", "").Replace("ajaxentitybinder=1", "").Trim('&'));
                                this.Output.Write(this.DrawCellEditableControl(column, editControl, item));
                            }
                        }
                        else
                            this.Output.Write(link.Draw());
                        this.OnAfterRenderCell(item, column);
                        this.Output.Write("</td>");
                    }
                }
                this.RenderOnAfterDrawLine(item);
                this.Output.Write("</tr>");
            }
            if (this.GroupedData != null && this.DataSource.Pagination.ItemCount > this.DataSource.Pagination.PageSize)
            {
                this.Output.Write("<tr>");
                this.Output.Write("<td colspan='" + this.Columns.Count + "'>");
                this.RenderPagination(true);
                this.Output.Write("</tr>");
            }
            this.Output.Write("</tbody>");
        }
        protected virtual void DrawColumnHeaders()
        {
            this.Output.Write("<thead>");
            this.Output.Write("<tr>");
            if (this.Configuration.AddBlankColumnToStart)
                this.Output.Write("<th class='no-sort'></th>");
            if (this.Configuration.Checkboxes && this.Configuration.ShowCheckAll)
            {
                this.Output.Write("<th class='no-sort'><input type='checkbox' id='CheckAll' class='binder-check-all' name='CheckAll'> " + this.Client.TranslateText("All") + "</th>");
            }
            else if (this.Configuration.Checkboxes)
            {
                this.Output.Write("<th class='no-sort'></th>");
            }

            this.RenderOnBeforeDrawLine(null);
            var qs = new Ophelia.Web.Application.Client.QueryString(this.Request);
            foreach (var column in this.Columns)
            {
                if (column.Visible)
                {
                    if (this.Groupers.Where(op => op.IsSelected && (op.FormatName() == column.FormatName())).Any())
                        continue;

                    this.Output.Write("<th");
                    var className = "";
                    if (column.Alignment == System.Web.UI.WebControls.HorizontalAlign.Right)
                        className += " text-right";

                    if (column.EnableGrouping && this.Configuration.AllowGrouping && this.Configuration.EnableGroupingByDragDrop)
                        className += " groupable";

                    var link = "";
                    if (!this.ParentDrawsLayout && this.Configuration.AllowServerSideOrdering && column.IsSortable && this.GroupedData == null)
                    {
                        if (string.IsNullOrEmpty(column.Name))
                            column.Name = column.FormatName();

                        qs.Update("OrderBy", column.Expression.ParsePath());
                        if (this.Request["OrderBy"] == column.Expression.ParsePath())
                        {
                            if (this.Request["OrderByDirection"] != null && this.Request["OrderByDirection"].Equals("ASC", StringComparison.InvariantCultureIgnoreCase))
                            {
                                qs.Update("OrderByDirection", "DESC");
                                this.Output.Write(" aria-sort='ascending' data-class='sorting_asc'");
                                className += " sorting_asc";
                            }
                            else
                            {
                                qs.Update("OrderByDirection", "ASC");
                                this.Output.Write(" aria-sort='descending' data-class='sorting_desc'");
                                className += " sorting_desc";
                            }
                        }
                        else
                        {
                            qs.Update("OrderByDirection", "ASC");
                            this.Output.Write(" aria-sort='ascending' data-class='sorting'");
                            className += " sorting";
                        }
                        className += " no-sort";
                        link = qs.Value;
                        if (!this.Configuration.ColumnSortingByLink && this.Configuration.ColumnSortingByClick)
                            this.Output.Write(" onclick=\"document.location.href='" + link + "'\"");
                    }
                    if (!string.IsNullOrEmpty(className))
                        this.Output.Write(" class='" + className + "'");

                    if (!string.IsNullOrEmpty(column.Width))
                        this.Output.Write(" data-width='" + column.Width + "'");

                    if (column.Expression != null)
                    {
                        var uExp = (column.Expression.Body as UnaryExpression);
                        if (uExp != null)
                        {
                            this.Output.Write(" data-datatype='" + uExp.Operand.Type.Name + "'");

                            if (uExp.Operand.Type.IsNumeric() && column is NumericColumn<TModel, T>)
                                this.Output.Write(" data-template='number'");
                            else if (uExp.Operand.Type.Name.IndexOf("bool", StringComparison.InvariantCultureIgnoreCase) > -1)
                                this.Output.Write(" data-template='boolean'");
                            else if (uExp.Operand.Type.Name.IndexOf("date", StringComparison.InvariantCultureIgnoreCase) > -1)
                                this.Output.Write(" data-template='date'");
                            else
                                this.Output.Write(" data-template='string'");
                        }
                        else
                            this.Output.Write(" data-template='string'");
                    }
                    else
                        this.Output.Write(" data-template='string'");

                    this.Output.Write(" data-align='" + column.Alignment.ToString() + "'");

                    this.Output.Write(" data-name='" + column.FormatColumnName() + "'");
                    this.OnDrawingColumnHeader(column, true);
                    this.Output.Write(">");
                    if (!column.HideColumnTitle)
                    {
                        this.DrawColumnText(column, column.FormatText(), link);
                        this.OnDrawingColumnHeader(column);
                    }
                    this.Output.Write("</th>");
                }
            }
            this.RenderOnAfterDrawLine(null);
            this.Output.Write("</tr>");
            if (this.Configuration.ColumnFiltersInHead && !this.ParentDrawsLayout && this.GroupedData == null)
                this.RenderColumnFilters();
            this.Output.Write("</thead>");
        }
        protected virtual void OnDrawingColumnHeader(Columns.BaseColumn<TModel, T> column, bool drawingTag = false)
        {

        }
        private bool GroupingAreaDrawn = false;
        protected virtual void DrawGroupingArea()
        {
            if (this.GroupingAreaDrawn)
                return;

            this.GroupingAreaDrawn = true;
            this.Output.Write("<div class='collection-binder-grouping-area'>");
            var counter = 0;
            foreach (var item in this.Groupers)
            {
                if (item.IsSelected)
                {
                    var name = item.FormatName();
                    this.Output.Write("<label id='Grouper-" + name + "-label' data-name='" + name + "' class='grouper'>" + item.FormatText(this.Client) + " <a onclick='$.Ophelia.View.Grouper.Remove(this)'><i class='icon-cross3'></i></a> <input type='hidden' name='Grouper-" + name + "' value='on' id='Grouper-" + name + "'/></label>");
                    counter++;
                }
            }
            if (counter == 0)
            {
                this.Output.Write("<span>" + this.Client.TranslateText("DragColumnsToGroup") + "</span>");
            }
            this.Output.Write("</div>");
        }
        protected virtual string GetItemLink(T item)
        {
            if (this.Configuration.DrawViewLinkInsteadOfEdit)
                return this.Configuration.ViewURL.TrimEnd('/') + "/" + item.GetPropertyValue("ID");
            else
            {
                var StartIndex = this.Configuration.EditURL.TrimEnd('/').IndexOf('?');
                if (StartIndex > -1)
                    return (this.Configuration.EditURL.TrimEnd('/')).ToString().Insert(StartIndex, item.GetPropertyValue("ID").ToString());
                else
                    return this.Configuration.EditURL.TrimEnd('/') + "/" + item.GetPropertyValue("ID").ToString();
            }
        }
        protected virtual string GetDisplayName(object entity)
        {
            var name = entity.GetPropertyValue("Name");
            if (name == null)
                name = entity.GetPropertyValue("Title");
            if (name != null)
                return name.ToString();
            return "";
        }
        protected virtual string GetBinderCheckboxProperties(T entity)
        {
            return "";
        }
        protected virtual T CreateNewItem()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
        protected virtual bool OnDrawCheckbox(T item)
        {
            return true;
        }
        protected virtual void RenderColumnFilters()
        {
            if (this.Configuration.EnableColumnFiltering && this.GroupedData == null)
            {
                var tag = "th";
                if (!this.Configuration.ColumnFiltersInHead)
                    tag = "td";
                this.Output.Write("<tr class='filters'>");
                if (this.Configuration.AddBlankColumnToStart)
                    this.Output.Write("<" + tag + " class='no-sort'></" + tag + ">");

                if (this.Configuration.Checkboxes)
                    this.Output.Write("<" + tag + " class='no-sort'></" + tag + ">");

                foreach (var column in this.Columns)
                {
                    if (column.Visible)
                    {
                        if (this.Groupers.Where(op => op.IsSelected && (op.FormatName() == column.FormatName())).Any())
                            continue;

                        this.Output.Write("<" + tag + ">");
                        if (column.EnableColumnFiltering)
                        {
                            var value = "";
                            if (this.Request["Filters." + column.FormatName()] != null && this.Request["Filters." + column.FormatName()] != "")
                            {
                                value = this.Request["Filters." + column.FormatName()];
                            }
                            this.Output.Write(this.GetEditableControl(column, value).Draw());
                        }
                        this.Output.Write("</" + tag + ">");
                    }
                }
                this.Output.Write("</tr>");
            }
        }
        protected WebControl GetEditableControl(Columns.BaseColumn<TModel, T> column, object value)
        {
            if (column is Columns.DateColumn<TModel, T>)
            {
                (column as Columns.DateColumn<TModel, T>).Mode = Fields.DateFieldMode.DoubleSelection;
            }
            if (column is Columns.BoolColumn<TModel, T>)
            {
                return (column as Columns.BoolColumn<TModel, T>).GetEditableControlAsSelect(null, value, this.Request);
            }
            return column.GetEditableControl(null, value, this.Request);
        }
        protected virtual void Export()
        {
            if (this.CanExport)
            {
                this.ReorderColumns();
                this.ContentRenderMode = ContentRenderMode.Export;
                var fileName = "";
                if (!string.IsNullOrEmpty(ExportDocumentName))
                    fileName = ExportDocumentName;
                else if (typeof(T).Name.EndsWith("List"))
                    fileName = "ListOf" + typeof(T).Name;
                else
                    fileName = typeof(T).Name + "List";

                var grid = new Ophelia.Data.Exporter.Controls.Grid();
                grid.Text = this.Client.TranslateText(fileName);
                foreach (var column in this.Columns)
                {
                    if (column.Visible)
                    {
                        if (string.IsNullOrEmpty(column.Name))
                            column.Name = column.FormatName();

                        grid.Columns.Add(new Data.Exporter.Controls.Column(grid) { Text = column.FormatText(), ID = column.Name, IsNumeric = column.Alignment == System.Web.UI.WebControls.HorizontalAlign.Right });
                    }
                }
                foreach (T item in this.DataSource.Items)
                {
                    var gridRow = new Ophelia.Data.Exporter.Controls.Row(grid);
                    Ophelia.Data.Exporter.Controls.Column gridColumn = null;
                    grid.Rows.Add(gridRow);
                    foreach (var column in this.Columns)
                    {
                        if (!column.Visible)
                            continue;

                        gridColumn = grid.Columns.Where(op => op.ID == column.Name).FirstOrDefault();

                        var cell = new Ophelia.Data.Exporter.Controls.Cell(grid, gridColumn, gridRow);
                        gridRow.Cells.Add(cell);

                        this.OnBeforeGetCellValue(item, column);
                        var value = column.GetValue(item);
                        value = this.FindCustomCellValue(item, column, value);
                        if (this.CanExportCellValue(column, value))
                        {
                            cell.Value = this.FormatCellValueForExport(column, value);
                            var link = new Ophelia.Web.UI.Controls.Link() { Text = Convert.ToString(cell.Value) };
                            this.RenderCellProperties(item, column, link);
                            cell.Value = link.Text;
                        }

                        value = null;
                    }
                }

                byte[] fileContent = null;
                var fileExtension = "xls";
                switch (this.ExportOutputFileFormat)
                {
                    case "xls":
                        fileExtension = "xlsx";
                        fileContent = new Ophelia.Data.Exporter.ExcelExporter().Export(grid);
                        break;
                    case "pdf":
                        fileExtension = "pdf";
                        fileContent = new Ophelia.Data.Exporter.PDFExporter().Export(grid);
                        break;
                    case "csv":
                        fileExtension = "csv";
                        fileContent = new Ophelia.Data.Exporter.CSVExporter().Export(grid);
                        break;
                    case "xml":
                        fileExtension = "xml";
                        fileContent = new Ophelia.Data.Exporter.XMLExporter().Export(grid);
                        break;
                    case "tdf":
                        fileExtension = "txt";
                        fileContent = new Ophelia.Data.Exporter.TDFExporter().Export(grid);
                        break;
                }
                grid.Dispose();
                grid = null;
                this.DataSource.Dispose();

                if (fileContent != null)
                {
                    this.Response.Clear();
                    this.Response.ClearContent();
                    this.Response.ClearHeaders();
                    this.Response.Buffer = false;

                    this.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1254");
                    this.Response.Charset = "windows-1254";

                    if (fileExtension.Equals("xml"))
                        this.Response.ContentType = "text/xml";
                    else if (fileExtension.Equals("txt"))
                        this.Response.ContentType = "text/plain";
                    else if (fileExtension.Equals("pdf"))
                        this.Response.ContentType = "application/pdf";
                    else if (fileExtension.Equals("xlsx") || fileExtension.Equals("csv"))
                        this.Response.ContentType = "application/vnd.ms-excel";
                    else
                        this.Response.ContentType = "application/octet-stream";

                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + this.Client.TranslateText(fileName) + "." + fileExtension);
                    if (fileContent != null)
                        this.Response.OutputStream.Write(fileContent, 0, fileContent.Length);
                }
                this.Response.Flush();
                this.Response.End();
            }
        }
        protected virtual bool CanExportCellValue(Columns.BaseColumn<TModel, T> column, object value)
        {
            return true;
        }
        protected virtual object FormatCellValueForExport(Columns.BaseColumn<TModel, T> column, object value)
        {
            return value;
        }
        protected virtual void OnBeforeGetCellValue(T item, Columns.BaseColumn<TModel, T> column)
        {

        }
        protected virtual object FindCustomCellValue(T item, Columns.BaseColumn<TModel, T> column, object value)
        {
            return value;
        }
        protected virtual void RenderOnBeforeDrawLine(T item)
        {

        }
        protected virtual void RenderOnAfterDrawLine(T item)
        {

        }
        protected virtual void RenderRowProperties(T item, int index)
        {
            this.Output.Write("class='" + ((index + 1) % 2 == 0 ? "even" : "odd") + "' ");
        }
        protected virtual void RenderCellProperties(T item, Columns.BaseColumn<TModel, T> column, Link link)
        {

        }
        protected virtual void OnBeforeRenderCell(T item, Columns.BaseColumn<TModel, T> column)
        {

        }
        protected virtual void OnAfterRenderCell(T item, Columns.BaseColumn<TModel, T> column)
        {

        }
        protected virtual string DrawCellEditableControl(Columns.BaseColumn<TModel, T> column, WebControl control, T item)
        {
            return control.Draw();
        }
        protected virtual void DrawColumnText(Columns.BaseColumn<TModel, T> column, string text, string sortingLink)
        {
            if (this.Configuration.ColumnSortingByLink)
            {
                this.Output.Write("<a class='column-header-link' href='" + sortingLink + "'>");
                this.Output.Write(text);
                this.Output.Write("</a>");
            }
            else
            {
                this.Output.Write(text);
            }
        }
        protected virtual string RenderColumnFilter(Columns.BaseColumn<TModel, T> column, WebControl control)
        {
            return control.Draw() + this.DrawFilterComparison(column);
        }
        protected string DrawFilterComparison(Columns.BaseColumn<TModel, T> column)
        {
            var comparisons = new List<Comparison>();
            if (column is NumericColumn<TModel, T>)
            {
                comparisons.Add(Comparison.Equal);
                comparisons.Add(Comparison.Different);
                comparisons.Add(Comparison.Greater);
                comparisons.Add(Comparison.GreaterAndEqual);
                comparisons.Add(Comparison.Less);
                comparisons.Add(Comparison.LessAndEqual);
            }
            else if (column is FilterboxColumn<TModel, T>)
            {
                comparisons.Add(Comparison.Equal);
                comparisons.Add(Comparison.Different);
            }
            else if (column is TextColumn<TModel, T>)
            {
                comparisons.Add(Comparison.Contains);
                comparisons.Add(Comparison.Equal);
                comparisons.Add(Comparison.Different);
                comparisons.Add(Comparison.StartsWith);
                comparisons.Add(Comparison.EndsWith);
            }
            var sb = new System.Text.StringBuilder();
            if (comparisons.Count > 0)
            {
                var selectName = column.FormatName() + "-Comparison";
                sb.Append("<select id='" + selectName + "' name='" + selectName + "' class='comparison form-control'>");
                foreach (var item in comparisons)
                {
                    var isSelected = (!string.IsNullOrEmpty(this.Request[selectName]) && this.Request[selectName] == ((int)item).ToString()) || (string.IsNullOrEmpty(this.Request[selectName]) && item == comparisons.FirstOrDefault());
                    var sign = "";
                    switch (item)
                    {
                        case Comparison.Equal:
                            sign = " = ";
                            break;
                        case Comparison.Different:
                            sign = " !=";
                            break;
                        case Comparison.Greater:
                            sign = " > ";
                            break;
                        case Comparison.Less:
                            sign = " < ";
                            break;
                        case Comparison.GreaterAndEqual:
                            sign = " >=";
                            break;
                        case Comparison.LessAndEqual:
                            sign = " <=";
                            break;
                        case Comparison.StartsWith:
                            sign = "*..";
                            break;
                        case Comparison.EndsWith:
                            sign = "..*";
                            break;
                        case Comparison.Contains:
                            sign = "*.*";
                            break;
                    }
                    sb.Append("<option " + (isSelected ? "selected" : "") + " value='" + ((int)item) + "'>" + sign.Replace(" ", "&nbsp;") + "</option>");
                }
                sb.Append("</select>");
            }
            return sb.ToString();
        }
        public virtual void RenderFooter()
        {
            this.Output.Write("</div>");//Table container
            this.Output.Write("<div class='datatable-footer collection-binder-footer'>");
            this.RenderPagination();
            this.Output.Write("</div>");
        }
        public virtual void RenderBreadcrumb()
        {

        }

        protected virtual void Configure()
        {

        }
        protected virtual void onViewContextSet()
        {

        }
        protected virtual void ReorderColumns()
        {
            this.Columns = this.Columns.OrderBy(op => op.SortOrder).ToList();
        }
        public virtual bool CanAddColumn(Columns.BaseColumn<TModel, T> column)
        {
            return true;
        }
        protected virtual void FinishRequest()
        {
            if (this.Output != null)
            {
                this.Response.Write(this.Output);
            }
            this.Response.Flush();
            this.Response.End();
        }
        public override void Dispose()
        {
            if (this.IsAjaxRequest && this.Response != null && !this.CanExport)
                this.FinishRequest();

            this.FilterPanel = null;
            this.DataSource = null;
            this.Output = null;
            this.Client = null;
            this.Configuration = null;
            this.Breadcrumb = null;
            this.ActionButtons = null;
            this.Title = "";
            this.Columns.Clear();
            this.Columns = null;
            this.Groupers.Clear();
            this.Groupers = null;
            this.GroupedData = null;
            base.Dispose();
        }
    }
    public enum ContentRenderMode
    {
        Group,
        Normal,
        Export
    }
}

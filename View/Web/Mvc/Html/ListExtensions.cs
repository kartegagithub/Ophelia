using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Ophelia.Web.View.Mvc.Models;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class ListExtensions
    {
        public static MvcHtmlString TrackBarFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string controlName, string trueText, string falseText, string value, bool IsReadOnly = false)
        {

            var selectList = new[] { new SelectListItem { Value = "0", Text = falseText, Selected = value.Equals("0") }, 
                                     new SelectListItem { Value = "1", Text = trueText, Selected = value.Equals("1") } };

            object attributes = new { data_role = "slider" };
            if (IsReadOnly) { attributes = new { data_role = "slider", disabled = "disabled" }; }
            return htmlHelper.DropDownList(controlName, selectList, attributes);
        }

        public static MvcHtmlString RoyalSliderFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression, object listAttributes = null, object listItemAttributes = null)
        {
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(listAttributes);
            var listItemHtmlAttibutes = HtmlHelper.AnonymousObjectToHtmlAttributes(listItemAttributes);
            return htmlHelper.ListForInternal(expression, null, listHtmlAttributes, listItemHtmlAttibutes, -1, 0, ItemDrawingMode.Grid);
        }
        public static MvcHtmlString SliderFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
            string templateName = null, ItemDrawingMode drawingMode = ItemDrawingMode.List, object listAttributes = null, object listItemAttributes = null, int page = -1, int pageSize = 0)
        {
            //if (listAttributes == null) listAttributes = new { @class = "thumbnails" };
            //if (listItemAttributes == null) listItemAttributes = new { @class = "thumbnail-item" };
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(listAttributes);
            var listItemHtmlAttibutes = HtmlHelper.AnonymousObjectToHtmlAttributes(listItemAttributes);
            return htmlHelper.ListForInternal(expression, templateName, listHtmlAttributes, listItemHtmlAttibutes, page, pageSize);
        }
        //public static MvcHtmlString ThumbnailsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression)
        //{
        //    return htmlHelper.ThumbnailsFor<TModel, TItem>(expression, null);
        //}
        //public static MvcHtmlString ThumbnailsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression, string templateName)
        //{
        //    return htmlHelper.ThumbnailsFor<TModel, TItem>(expression, templateName, null);
        //}
        //public static MvcHtmlString ThumbnailsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
        //    string templateName, object listAttributes)
        //{
        //    return htmlHelper.ThumbnailsFor<TModel, TItem>(expression, templateName, listAttributes, null);
        //}
        //public static MvcHtmlString ThumbnailsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
        //    string templateName, object listAttributes, object listItemAttributes)
        //{
        //    return htmlHelper.ThumbnailsFor<TModel, TItem>(expression, templateName, listAttributes, listItemAttributes, -1, 0);
        //}
        //public static MvcHtmlString ThumbnailsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
        //    string templateName, object listItemAttributes)
        //{
        //    var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "span" });
        //    var listItemHtmlAttibutes = HtmlHelper.AnonymousObjectToHtmlAttributes(listItemAttributes);
        //    return htmlHelper.ListForInternal_Normal(expression, templateName, listHtmlAttributes, listItemHtmlAttibutes, -1, 0);
        //}

        //public static MvcHtmlString ThumbnailsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
        //    string templateName, object listAttributes, object listItemAttributes, int page, int pageSize)
        //{
        //    return htmlHelper.ThumbnailsFor(expression, templateName, null, listItemAttributes, page, pageSize);
        //}

        public static MvcHtmlString ThumbnailsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
            string templateName = null, ItemDrawingMode drawingMode = ItemDrawingMode.List, object listAttributes = null, object listItemAttributes = null, int page = -1, int pageSize = 0)
        {
            //if (listAttributes == null) listAttributes = new { @class = "thumbnails" };
            //if (listItemAttributes == null) listItemAttributes = new { @class = "thumbnail-item" };
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(listAttributes);
            var listItemHtmlAttibutes = HtmlHelper.AnonymousObjectToHtmlAttributes(listItemAttributes);
            return htmlHelper.ListForInternal(expression, templateName, listHtmlAttributes, listItemHtmlAttibutes, page, pageSize);
        }

        public static MvcHtmlString NavsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression)
        {
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "nav" });
            return htmlHelper.ListForInternal(expression, null, listHtmlAttributes, null, -1, 0);
        }

        public static MvcHtmlString NavPillsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression)
        {
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "nav nav-pills" });
            return htmlHelper.ListForInternal(expression, null, listHtmlAttributes, null, -1, 0);
        }

        public static MvcHtmlString NavsFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
            string templateName = null, ItemDrawingMode drawingMode = ItemDrawingMode.List, object listAttributes = null, object listItemAttributes = null, int page = -1, int pageSize = 0)
        {
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(listAttributes);
            var listItemHtmlAttibutes = HtmlHelper.AnonymousObjectToHtmlAttributes(listItemAttributes);
            return htmlHelper.ListForInternal(expression, templateName, listHtmlAttributes, listItemHtmlAttibutes, page, pageSize);
        }
        //public static MvcHtmlString BreadcrumbsFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<BreadcrumbsItemModel>>> expression)
        //{
        //    var listItems = expression.Compile().Invoke(htmlHelper.ViewData.Model) as IEnumerable<BreadcrumbsItemModel>;
        //    listItems.Last().IsActive = true;
        //    var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "breadcrumb" });
        //    var listItemHtmlAttibutes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "active" });
        //    return htmlHelper.ListForInternal(expression, null, listHtmlAttributes, listItemHtmlAttibutes, -1, 0);
        //}

        public static MvcHtmlString ListFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression, object htmlAttributes)
        {
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return htmlHelper.ListForInternal(expression, null, listHtmlAttributes, null, -1, 0);
        }

        public static MvcHtmlString ListFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression, object htmlAttributes, object itemHtmlAttributes)
        {
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var listItemHtmlAttibutes = HtmlHelper.AnonymousObjectToHtmlAttributes(itemHtmlAttributes);
            return htmlHelper.ListForInternal(expression, null, listHtmlAttributes, listItemHtmlAttibutes, -1, 0);
        }

        public static MvcHtmlString ListFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression, string templateName)
        {
            return htmlHelper.ListForInternal(expression, templateName, null, null, -1, 0);
        }

        public static MvcHtmlString ListFor<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression)
        {
            return htmlHelper.ListForInternal(expression, null, null, null, -1, 0);
        }

        internal static MvcHtmlString ListForInternal<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
            string templateName, IDictionary<string, object> htmlAttributes, IDictionary<string, object> itemHtmlAttributes, int page, int pageSize, ItemDrawingMode drawingMode = ItemDrawingMode.List)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var listItems = expression.Compile().Invoke(htmlHelper.ViewData.Model) as IEnumerable<TItem>;

            // Paging items.
            if (page > -1 && pageSize > 0)
                listItems = listItems.Paginate(page, pageSize);


            // Get view name of template.
            templateName = templateName ?? metaData.TemplateHint;
            var templatePath = "ListItemTemplates/" + templateName;
            if (string.IsNullOrEmpty(templateName))
                templatePath = string.Format("ListItemTemplates/{0}View", metaData.PropertyName);

            var listItemBuilder = new StringBuilder();
            TagBuilder ulBuilder, liBuilder;
            if (drawingMode == ItemDrawingMode.List)
            {
                foreach (var item in listItems)
                {
                    var innerHtml = htmlHelper.Partial(templatePath, item);
                    if (innerHtml.ToString().Length > 0)
                    {
                        liBuilder = new TagBuilder("li") { InnerHtml = innerHtml.ToHtmlString() };
                        liBuilder.MergeAttributes(itemHtmlAttributes);
                        listItemBuilder.AppendLine(liBuilder.ToString(TagRenderMode.Normal));
                    }
                }

                ulBuilder = new TagBuilder("ul") { InnerHtml = listItemBuilder.ToString() };
                ulBuilder.MergeAttribute("name", metaData.PropertyName);
                ulBuilder.MergeAttribute("id", metaData.PropertyName);
                ulBuilder.MergeAttributes(htmlAttributes);
            }
            else
            {
                foreach (var item in listItems)
                {
                    var innerHtml = htmlHelper.Partial(templatePath, item);
                    if (innerHtml.ToString().Length > 0)
                    {
                        liBuilder = new TagBuilder("div") { InnerHtml = innerHtml.ToHtmlString() };
                        liBuilder.MergeAttributes(itemHtmlAttributes);
                        listItemBuilder.AppendLine(liBuilder.ToString(TagRenderMode.Normal));
                    }
                }

                ulBuilder = new TagBuilder("div") { InnerHtml = listItemBuilder.ToString() };
                ulBuilder.MergeAttribute("name", metaData.PropertyName);
                ulBuilder.MergeAttribute("id", metaData.PropertyName);
                ulBuilder.MergeAttributes(htmlAttributes);
            }

            return MvcHtmlString.Create(ulBuilder.ToString(TagRenderMode.Normal));
        }

        internal static MvcHtmlString TableInternal<TModel, TItem>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TItem>>> expression,
            string templateName, IDictionary<string, object> htmlAttributes, IDictionary<string, object> itemHtmlAttributes, int page, int pageSize,
            int itemPerRow)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var items = expression.Compile().Invoke(htmlHelper.ViewData.Model) as IEnumerable<TItem>;
            var itemList = items.ToList();

            // Paging items.
            if (page > -1 && pageSize > 0)
                items = items.Paginate(page, pageSize);


            // Get view name of template.
            templateName = templateName ?? metaData.TemplateHint;
            var templatePath = "ListItemTemplates/" + templateName;
            if (string.IsNullOrEmpty(templateName))
                templatePath = string.Format("ListItemTemplates/{0}View", metaData.PropertyName);

            // Build items.
            var htmlBuilder = new StringBuilder();

            int rowCount = itemList.Count / itemPerRow + (itemList.Count % itemPerRow != 0 ? 1 : 0);
            int cnt = 0;
            for (int i = 0; i < rowCount; i++)
            {
                var rowBuilder = new StringBuilder();

                for (int j = 0; j < itemPerRow && cnt < itemList.Count; j++)
                {
                    var innerHtml = htmlHelper.Partial(templatePath, itemList[cnt++]);
                    var columnBuilder = new TagBuilder("td") { InnerHtml = innerHtml.ToHtmlString() };
                    columnBuilder.MergeAttributes(itemHtmlAttributes);
                    rowBuilder.Append(columnBuilder.ToString(TagRenderMode.Normal));
                }

                var rowTag = new TagBuilder("tr") { InnerHtml = rowBuilder.ToString() };

                htmlBuilder.AppendLine(rowTag.ToString(TagRenderMode.Normal));
            }

            // Build ul element.
            var ulBuilder = new TagBuilder("table") { InnerHtml = htmlBuilder.ToString() };
            ulBuilder.MergeAttribute("name", metaData.PropertyName);
            ulBuilder.MergeAttribute("id", metaData.PropertyName);
            ulBuilder.MergeAttributes(htmlAttributes);

            return MvcHtmlString.Create(ulBuilder.ToString(TagRenderMode.Normal));
        }
    }
}

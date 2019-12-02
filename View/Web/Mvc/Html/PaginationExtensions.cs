using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Ophelia.Web.View.Mvc.Models;
using Ophelia.Web.Application.Client;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class PaginationExtensions
    {
        public static MvcHtmlString PaginationFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PaginationModel>> expression)
        {
            return htmlHelper.PaginationFor(expression, PaginationSize.Default, PaginationAlignment.Centered);
        }

        public static MvcHtmlString PaginationFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PaginationModel>> expression, PaginationSize size, PaginationAlignment align)
        {
            var className = GenerateClassName(size, align);
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = className });
            return htmlHelper.PaginationInternal(expression, listHtmlAttributes);
        }

        internal static MvcHtmlString PaginationInternal<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PaginationModel>> expression, IDictionary<string, object> htmlAttributes)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            if (model == null)
                return MvcHtmlString.Empty;

            var routeValues = htmlHelper.ViewContext.RouteData.Values;
            model.EnsurePageCount();

            if (model.PageCount > 1)
            {
                var listBuilder = new TagBuilder("ul");
                if (model.PageNumber > 1)
                {
                    listBuilder.InnerHtml += PageLinkInternal(htmlHelper, routeValues, model.PageNumber, -1, model.PageNumber <= 1, false, model);
                }
                for (int index = model.FirstPage; index < model.LastPage; index++)
                {
                    listBuilder.InnerHtml += PageLinkInternal(htmlHelper, routeValues, index, 0, false, index == model.PageNumber, model);
                }
                if (model.PageNumber < model.PageCount)
                {
                    listBuilder.InnerHtml += PageLinkInternal(htmlHelper, routeValues, model.PageNumber, 1, model.PageNumber >= model.PageCount, false, model);
                }

                var containerBuilder = new TagBuilder("div") { InnerHtml = listBuilder.ToString() };
                containerBuilder.MergeAttribute("name", metaData.PropertyName);
                containerBuilder.MergeAttribute("id", metaData.PropertyName);
                containerBuilder.MergeAttributes(htmlAttributes);
                return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
            }
            return MvcHtmlString.Create(string.Empty);
        }

        internal static string PageLinkInternal(HtmlHelper htmlHelper, RouteValueDictionary routeValues, int pageNumber, int move, bool disabled, bool active, PaginationModel PaginationModel = null)
        {
            var model = htmlHelper.ViewData.Model;
            var cssClass = disabled ? "disabled" : string.Empty;
            cssClass = active ? "active" : cssClass;
            var pageLinkBuilder = new TagBuilder("li");
            var linkText = pageNumber.ToString();
            if (move != 0)
            {
                pageNumber += move;
                linkText = (move == -1) ? "«" : "»";
            }

            var routeData = new RouteValueDictionary(routeValues);
            routeData["page"] = pageNumber;
            var queryString = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.Keys)
            {
                if (!routeData.ContainsKey(key))
                    routeData.Add(key, queryString[key]);
            }

            string LinkUrl = "javascript:void(0);";
            if (!active)
            {

                LinkUrl = "/" + routeData["Controller"] + "/" + routeData["Action"] + "/" + routeData["page"];
                if (!string.IsNullOrEmpty(queryString.ToString()))
                {
                    LinkUrl += "?" + queryString;
                }
            }

            if (PaginationModel != null && PaginationModel.DrawMode == 1)
            {
                string IsSelected = active ? "a" : "c";
                return DrawPageNumberForJQueryMobile(linkText, LinkUrl, IsSelected);
            }
            else
            {
                pageLinkBuilder.InnerHtml = "<a href='" + LinkUrl + "' class='" + cssClass + "'>" + linkText + "</a>";
            }
            return pageLinkBuilder.ToString(TagRenderMode.Normal);
        }

        private static string DrawPageNumberForJQueryMobile(string Value, string Url, string IsSelected = "c")
        {
            StringBuilder PageNumberHtml = new StringBuilder();
            PageNumberHtml.AppendLine("<a data-inline='true' data-theme='" + IsSelected + "' data-role='button' href='" + Url + "' data-corners='true' data-shadow='true' data-iconshadow='true'");
            PageNumberHtml.AppendLine(" data-wrapperels='span' class='ui-btn ui-shadow ui-btn-corner-all ui-btn-inline ui-btn-up-" + IsSelected + "'>");
            PageNumberHtml.AppendLine("<span class='ui-btn-inner'><span class='ui-btn-text'>" + Value + "</span></span></a>");
            return PageNumberHtml.ToString();
        }

        private static string GenerateClassName(PaginationSize size, PaginationAlignment align)
        {
            var className = "pagination";
            switch (size)
            {
                case PaginationSize.Large:
                    className += " pagination-large";
                    break;
                case PaginationSize.Small:
                    className += " pagination-small";
                    break;
                case PaginationSize.Mini:
                    className += " pagination-mini";
                    break;
            }

            switch (align)
            {
                case PaginationAlignment.Centered:
                    className += " pagination-centered";
                    break;
                case PaginationAlignment.Right:
                    className += " pagination-right";
                    break;
            }

            return className;
        }
    }

    public static class CustomPaginationExtensions
    {
        public static MvcHtmlString CustomPaginationFor<TModel>(this HtmlHelper<TModel> htmlHelper, long ItemCount, int pageNumber, int pageSize, List<string> excludedKeys = null)
        {
            var model = new PaginationModel()
            {
                ItemCount = ItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var className = CustomGenerateClassName(PaginationSize.Default, PaginationAlignment.Centered);
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = className });
            return htmlHelper.CustomPaginationInternal(model, listHtmlAttributes, excludedKeys);
        }

        public static MvcHtmlString CustomPaginationFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PaginationModel>> expression, List<string> excludedKeys = null)
        {
            return htmlHelper.CustomPaginationFor(expression, PaginationSize.Default, PaginationAlignment.Centered, excludedKeys);
        }

        public static MvcHtmlString CustomPaginationFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PaginationModel>> expression, PaginationSize size, PaginationAlignment align, List<string> excludedKeys = null)
        {
            var className = CustomGenerateClassName(size, align);
            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = className });
            return htmlHelper.CustomPaginationInternal(expression, listHtmlAttributes, excludedKeys);
        }

        internal static MvcHtmlString CustomPaginationInternal<TModel>(this HtmlHelper<TModel> htmlHelper, PaginationModel model, IDictionary<string, object> htmlAttributes, List<string> excludedKeys = null, string ID = "Pagination")
        {
            if (model == null)
                return MvcHtmlString.Empty;

            var routeValues = htmlHelper.ViewContext.RouteData.Values;
            model.EnsurePageCount();

            var QueryString = new QueryString(htmlHelper.ViewContext.RequestContext.HttpContext.Request);
            if (excludedKeys != null)
            {
                foreach (var key in excludedKeys)
                {
                    QueryString.Remove(key);
                }
            }
            if (model.PageCount > 1)
            {
                var listBuilder = new TagBuilder("ul");

                if (model.FirstPage > 1)
                {
                    //  if (model.FirstPage > 2)
                    listBuilder.InnerHtml += CustomPageLinkInternal(htmlHelper, QueryString, 1, Button.First, false, false, model);
                    listBuilder.InnerHtml += CustomPageLinkInternal(htmlHelper, QueryString, model.PageNumber - 1, Button.Previous, false, false, model);
                }

                for (int index = model.FirstPage; index <= model.LastPage; index++)
                {
                    listBuilder.InnerHtml += CustomPageLinkInternal(htmlHelper, QueryString, index, Button.Number, false, index == model.PageNumber, model);
                }

                if (model.LastPage < model.PageCount)
                {

                    listBuilder.InnerHtml += CustomPageLinkInternal(htmlHelper, QueryString, model.PageNumber + 1, Button.Next, false, false, model);
                    // if (model.LastPage + 1 < model.PageCount)
                    listBuilder.InnerHtml += CustomPageLinkInternal(htmlHelper, QueryString, Convert.ToInt32(model.PageCount), Button.Last, false, false, model);
                }
                var containerBuilder = new TagBuilder("div") { InnerHtml = listBuilder.ToString() };
                containerBuilder.MergeAttribute("name", ID);
                containerBuilder.MergeAttribute("id", ID);
                containerBuilder.MergeAttributes(htmlAttributes);
                return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
            }
            return MvcHtmlString.Create(string.Empty);
        }
        internal static MvcHtmlString CustomPaginationInternal<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PaginationModel>> expression, IDictionary<string, object> htmlAttributes, List<string> excludedKeys = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            return CustomPaginationInternal(htmlHelper, model, htmlAttributes, excludedKeys, metaData.PropertyName);
        }

        internal static string CustomPageLinkInternal(HtmlHelper htmlHelper, QueryString QS, int pageNumber, Button button, bool disabled, bool active, PaginationModel paginationModel = null)
        {
            var model = htmlHelper.ViewData.Model;
            var cssClass = disabled ? "disabled" : string.Empty;
            cssClass = active ? "active" : cssClass;
            var pageLinkBuilder = new TagBuilder("li");
            var linkText = pageNumber.ToString();
            if (button != Button.Number)
            {

                switch (button)
                {
                    case Button.First: linkText = "«"; break;
                    case Button.Previous: linkText = "‹"; break;
                    case Button.Next: linkText = "›"; break;
                    case Button.Last: linkText = "»"; break;
                }
            }

            string LinkUrl = "javascript:void(0);";
            if (!active)
            {
                QS.Update("page", pageNumber.ToString());
                LinkUrl = QS.Value;
            }

            if (paginationModel != null && paginationModel.DrawMode == 1)
            {
                string IsSelected = active ? "a" : "c";
                return CustomDrawPageNumberForJQueryMobile(linkText, LinkUrl, IsSelected);
            }
            else
            {
                pageLinkBuilder.InnerHtml = "<a href='" + LinkUrl + "' class='" + cssClass + "'>" + linkText + "</a>";
            }
            return pageLinkBuilder.ToString(TagRenderMode.Normal);
        }

        private static string CustomDrawPageNumberForJQueryMobile(string Value, string Url, string IsSelected = "c")
        {
            StringBuilder PageNumberHtml = new StringBuilder();
            PageNumberHtml.AppendLine("<a data-inline='true' data-theme='" + IsSelected + "' data-role='button' href='" + Url + "' data-corners='true' data-shadow='true' data-iconshadow='true'");
            PageNumberHtml.AppendLine(" data-wrapperels='span' class='ui-btn ui-shadow ui-btn-corner-all ui-btn-inline ui-btn-up-" + IsSelected + "'>");
            PageNumberHtml.AppendLine("<span class='ui-btn-inner'><span class='ui-btn-text'>" + Value + "</span></span></a>");
            return PageNumberHtml.ToString();
        }

        private static string CustomGenerateClassName(PaginationSize size, PaginationAlignment align)
        {
            var className = "pagination";
            switch (size)
            {
                case PaginationSize.Large:
                    className += " pagination-large";
                    break;
                case PaginationSize.Small:
                    className += " pagination-small";
                    break;
                case PaginationSize.Mini:
                    className += " pagination-mini";
                    break;
            }

            switch (align)
            {
                case PaginationAlignment.Centered:
                    className += " pagination-centered";
                    break;
                case PaginationAlignment.Right:
                    className += " pagination-right";
                    break;
            }

            return className;
        }
    }
}

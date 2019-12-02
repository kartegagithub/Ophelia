using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class ButtonExtensions
    {
        public static MvcHtmlString LinkButton(this HtmlHelper htmlHelper,
            string text,
            string action,
            ButtonStyles style = ButtonStyles.Default,
            ButtonSize size = ButtonSize.Default,
            bool block = false,
            bool disabled = false,
            Nullable<Glyphicons> icon = null,
            bool inverted = false)
        {
            return LinkButton(htmlHelper, text, action, null, null, null, style, size, block, disabled, icon, inverted);
        }
        public static MvcHtmlString LinkButton(this HtmlHelper htmlHelper,
            string text,
            string action,
            object htmlAttributes,
            ButtonStyles style = ButtonStyles.Default,
            ButtonSize size = ButtonSize.Default,
            bool block = false,
            bool disabled = false,
            Nullable<Glyphicons> icon = null,
            bool inverted = false)
        {
            return LinkButton(htmlHelper, text, action, null, null, htmlAttributes, style, size, block, disabled, icon, inverted);
        }
        public static MvcHtmlString LinkButton(this HtmlHelper htmlHelper,
            string text,
            string action,
            string controller,
            object routeValues,
            object htmlAttributes,
            ButtonStyles style = ButtonStyles.Default,
            ButtonSize size = ButtonSize.Default,
            bool block = false,
            bool disabled = false,
            Nullable<Glyphicons> icon = null,
            bool inverted = false)
        {
            var values = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            attributes = attributes ?? new RouteValueDictionary();
            string actionUrl = htmlHelper.GenerateUrl(action, controller, values);
            attributes.Add("href", actionUrl);
            return ButtonInternal(htmlHelper, "a", text, attributes, style, size, block, disabled, icon, inverted);
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string text,
            IDictionary<string, object> htmlAttributes,
            ButtonStyles style = ButtonStyles.Default,
            ButtonSize size = ButtonSize.Default,
            bool block = false,
            bool disabled = false,
            Nullable<Glyphicons> icon = null,
            bool inverted = false)
        {
            return ButtonInternal(htmlHelper, "button", text, htmlAttributes, style, size, block, disabled, icon, inverted);
        }

        internal static MvcHtmlString ButtonInternal(this HtmlHelper helper, string tagName, string text,
            IDictionary<string, object> htmlAttributes,
            ButtonStyles style = ButtonStyles.Default,
            ButtonSize size = ButtonSize.Default,
            bool block = false,
            bool disabled = false,
            Nullable<Glyphicons> icon = null,
            bool inverted = false)
        {
            string className = "btn button ";
            if (style != ButtonStyles.Default)
                className += style.ToClassName("btn-");

            if (size != ButtonSize.Default)
                className += size.ToClassName("button-");

            if (block)
                className += " btn-block";

            if (disabled)
            {
                htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
                var key = "disabled";
                className += " " + key;
                if (htmlAttributes.ContainsKey(key))
                    htmlAttributes[key] = key;
                else
                    htmlAttributes.Add(key, key);
            }
            var innerText = new StringBuilder();
            innerText.Append("<span>").Append(text);

            if (icon.HasValue)
                innerText.Append(icon.Value.InternalIcon(null, inverted));

            innerText.Append("</span>");

            var buttonBuilder = new TagBuilder(tagName) { InnerHtml = innerText.ToString() };
            buttonBuilder.MergeAttributes(htmlAttributes);
            buttonBuilder.AddCssClass(className);
            return MvcHtmlString.Create(buttonBuilder.ToString(TagRenderMode.Normal));
        }

        public static string GenerateUrl(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            controllerName = controllerName ?? htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            actionName = actionName ?? htmlHelper.ViewContext.RouteData.Values["action"] as string;
            return UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues,
                htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class TextBoxExtensions
    {
        public static MvcHtmlString TextBoxForMobile(this HtmlHelper htmlHelper,
            string value,
            bool disabled,
            object htmlAttributes = null
            )
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attributes = attributes ?? new RouteValueDictionary();

            var key =string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
               key = "value";
                if (attributes.ContainsKey(key))
                    attributes[key] = value;
                else
                    attributes.Add(key, value);
            }

            key = "class";
            var defaultValue = "ui-input-text ui-body-c";

            if (attributes.ContainsKey(key))
                attributes[key] = defaultValue + " " + attributes[key];
            else
                attributes.Add(key, defaultValue);

            if (disabled)
            {
                key = "disabled";
                if (attributes.ContainsKey(key))
                    attributes[key] = key;
                else
                    attributes.Add(key, key);

                key = "aria-disabled";
                if (attributes.ContainsKey(key))
                    attributes[key] = "true";
                else
                    attributes.Add(key, "true");
            }

            attributes.Add("type", "text");
            if (attributes.ContainsKey("id")) {
                attributes.Add("name", attributes["id"]);
            }
            var textboxBuilder = new TagBuilder("input") { };
            textboxBuilder.MergeAttributes(attributes);
            return MvcHtmlString.Create(textboxBuilder.ToString(TagRenderMode.Normal));
        }
    }
}

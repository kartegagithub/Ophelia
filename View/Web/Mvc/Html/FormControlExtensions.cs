using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class FormControlExtensions
    {
        public static MvcInputGroup Append(this HtmlHelper htmlHelper)
        {
            return htmlHelper.BeginGroup(InputGroupOption.Append);
        }

        public static MvcInputGroup Prepend(this HtmlHelper htmlHelper)
        {
            return htmlHelper.BeginGroup(InputGroupOption.Prepend);
        }

        public static MvcInputGroup BeginGroup(this HtmlHelper htmlHelper, InputGroupOption option)
        {
            var bothClasses = new[] { "input-prepend", "input-append" };
            var className = bothClasses.First();
            switch (option)
            {
                case InputGroupOption.Append:
                    className = bothClasses.Last();
                    break;
                case InputGroupOption.Both:
                    className += " " + bothClasses.Last();
                    break;
            }

            var listHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = className });
            return htmlHelper.BeginGroupInternal(listHtmlAttributes);
        }

        internal static MvcInputGroup BeginGroupInternal(this HtmlHelper htmlHelper, IDictionary<string, object> listHtmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(listHtmlAttributes);

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcInputGroup(htmlHelper.ViewContext.Writer);
        }
    }
}

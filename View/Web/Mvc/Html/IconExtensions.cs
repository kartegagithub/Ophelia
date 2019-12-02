using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class IconExtentions
    {
        internal static string InternalIcon(this Glyphicons icon, IDictionary<string, object> htmlAttributes, bool inverse = false)
        {
            string className = string.Concat(icon.ToClassName("icon-"), inverse ? " icon-white" : string.Empty);

            var iconBuilder = new TagBuilder("i");
            iconBuilder.MergeAttributes(htmlAttributes);
            iconBuilder.AddCssClass(className);
            return iconBuilder.ToString(TagRenderMode.Normal);
        }
    }
}

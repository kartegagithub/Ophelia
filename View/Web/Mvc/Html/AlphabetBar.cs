using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class AlphabetBarControl
    {
        public static MvcHtmlString AlphabetBar(this HtmlHelper htmlHtlper)
        {
            var alphabet = "ABCÇDEFGHIİJKLMNOÖPRSŞTUÜVWXYZ";
            var builder = new StringBuilder();
            builder.Append(@"<div class='alphabet-widget'><div class='alphabet-left'>
                            <div class='current-alphabet'></div>
                            <i class='fa fa-caret-right'></i>
                            </div><div class='alphabet-right'><ul>");
            foreach (var str in alphabet)
                builder.Append("<li><a href='javascript:;'>").Append(str).Append("</a></li>");
            builder.Append("<li class='selected'><a href='javascript:;'>Tümü</a></li>");
            builder.Append("</ul></div></div>");
            return new MvcHtmlString(builder.ToString());
        }
    }
}

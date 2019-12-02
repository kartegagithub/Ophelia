using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class HtmlEditor
    {
        public static MvcHtmlString HtmlEditorFor(this HtmlHelper htmlHelper, string name)
        {
            TagBuilder builder = new TagBuilder("script");
            builder.MergeAttribute("type", "text/javascript");
            builder.InnerHtml = "KindEditor.ready(function(editor) {editor.create('#" + name + "',{langType : 'en'}); });";
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
    }
}

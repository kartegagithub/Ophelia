using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class WebControlExtensions
    {
        public static MvcHtmlString Render(this Ophelia.Web.UI.Controls.WebControl control)
        {
            return new MvcHtmlString(control.Draw());
        }
    }
}

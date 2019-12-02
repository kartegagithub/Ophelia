using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ophelia.Web.View.Mvc.Controls
{
    public class HtmlHelper : System.Web.Mvc.HtmlHelper
    {
        public virtual string Translate(string Key)
        {
            return Key;
        }

        public HtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) : base(viewContext, viewDataContainer)
        {

        }

        public HtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection) : base(viewContext, viewDataContainer, routeCollection)
        {

        }
    }
}

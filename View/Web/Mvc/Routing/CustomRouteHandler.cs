using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.Routing;

namespace Ophelia.Web.View.Mvc.Routing
{
    public abstract class CustomRouteHandler
    {
        public RouteHandler RouteHandler { get; set; }
        public abstract RouteItem Handle(System.Web.Routing.RequestContext context, string friendlyURL, out bool handled);
    }
}

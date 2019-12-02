using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ophelia.Web.View.Mvc
{
    public class HttpApplication : Ophelia.Web.HttpApplication
    {
        protected override void Application_BeginRequest(object sender, EventArgs e)
        {
            base.Application_BeginRequest(sender, e);
        }
        protected virtual void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}

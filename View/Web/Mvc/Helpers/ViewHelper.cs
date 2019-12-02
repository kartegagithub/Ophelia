using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ophelia.Web.View.Mvc
{
    public static class ViewHelper
    {
        public static string RenderView(object model, string filePath)
        {
            using (var oWriter = new StringWriter())
            {
                var context = new HttpContextWrapper(HttpContext.Current);
                var routeData = new RouteData();
                var controllerContext = new ControllerContext(new RequestContext(context, routeData), new EmptyController());
                var razor = new RazorView(controllerContext, filePath, null, false, null);
                razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), oWriter), oWriter);

                return oWriter.GetStringBuilder().ToString();
            }
        }
    }
    public class EmptyController : Controller
    {
    }
}

using Ophelia.Reflection;
using Ophelia.Web.View.Mvc.Models;
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
    public static class ControllerExtensions
    {
        public static string RenderPartialView(this Controller controller, string viewName, object model)
        {
            using (var writer = new StringWriter())
            {
                var viewData = new ViewDataDictionary(model);
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, controller.GetViewName(viewName));
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, viewData, controller.TempData, writer);
                viewResult.View.Render(viewContext, writer);

                return writer.GetStringBuilder().ToString();
            }
        }

        public static string RenderViewToString(this Controller controller, string viewName, Dictionary<string, string> viewParameters)
        {
            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, controller.GetViewName(viewName));
                if (viewParameters != null)
                {
                    foreach (string key in viewParameters.Keys)
                        controller.ViewData[key] = viewParameters[key];
                }

                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }

        public static string RenderView(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        public static string GetViewName(this Controller controller, string viewName)
        {
            return !string.IsNullOrEmpty(viewName)
                ? viewName
                : controller.RouteData.GetRequiredString("action");
        }

        public static HtmlHelper GetHtmlHelper(this Controller controller)
        {
            var viewContext = new ViewContext(controller.ControllerContext, new EmptyView(), controller.ViewData, controller.TempData, TextWriter.Null);
            return new HtmlHelper(viewContext, new ViewPage());
        }

        public static string GetUserHostAddress(this Controller controller)
        {
            string Ip = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (!string.IsNullOrEmpty(Ip))
            {
                if (Ip.IndexOf(",") > -1)
                {
                    string[] ArrayAddress = Ip.Trim(',').Split(',');
                    if (ArrayAddress.Length > 1)
                        Ip = ArrayAddress[0];
                }
            }
            return Ip;
        }
    }
}

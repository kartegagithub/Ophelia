using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Ophelia.Web.View.Mvc.Attributes;

namespace Ophelia.Web.View.Mvc.Controllers
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return SessionStateBehavior.Default;
            }

            var actionName = requestContext.RouteData.Values["action"].ToString();
            MethodInfo actionMethodInfo;

            try
            {
                actionMethodInfo = controllerType.GetMethod(actionName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
            catch
            {
                var httpRequestTypeAttr =
                    requestContext.HttpContext.Request.RequestType.Equals("POST")
                        ? typeof(HttpPostAttribute)
                        : typeof(HttpGetAttribute);

                actionMethodInfo =
                    controllerType.GetMethods().FirstOrDefault(
                        mi =>
                        mi.Name.Equals(actionName, StringComparison.CurrentCultureIgnoreCase) && mi.GetCustomAttributes(httpRequestTypeAttr, false).Length > 0);
            }


            if (actionMethodInfo != null)
            {
                var actionSessionStateAttr = actionMethodInfo.GetCustomAttributes(typeof(ActionSessionStateAttribute), false)
                                 .OfType<ActionSessionStateAttribute>()
                                 .FirstOrDefault();

                if (actionSessionStateAttr != null)
                {
                    return actionSessionStateAttr.Behavior;
                }
            }

            return base.GetControllerSessionBehavior(requestContext, controllerType);

        }
    }
}
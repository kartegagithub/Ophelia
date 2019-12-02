using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Attributes
{
    public class PermanentRedirectAttribute : ActionFilterAttribute
    {
        private StringBuilder NewURL { get; set; }
        public PermanentRedirectAttribute(string NewURL)
        {
            if (NewURL.Substring(0, 1) != "/")
            {
                NewURL = "/" + NewURL;
            }
            this.NewURL = new StringBuilder(NewURL);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string RequestedURL = HttpContext.Current.Request.Url.AbsolutePath;
            if (this.NewURL.ToString().IndexOf(RequestedURL, StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString.ToString()))
                {
                    this.NewURL.Append("?" + filterContext.HttpContext.Request.QueryString.ToString());
                }
                filterContext.HttpContext.Response.StatusCode = 301;
                filterContext.HttpContext.Response.Status = "301 Moved Permanently";
                filterContext.HttpContext.Response.StatusDescription = "301 Moved Permanently";
                filterContext.Result = new RedirectResult(this.NewURL.ToString(), true);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Ophelia.Web.View.Mvc.Attributes
{
    public class SslFilterAttribute : ActionFilterAttribute
    {
        public bool SslRequired { get; set; }
        public bool NoRedirection { get; set; }
        public SslFilterAttribute(bool sslRequired)
            : this(sslRequired, false)
        {
        }

        public SslFilterAttribute(bool sslRequired, bool noRedirection)
        {
            this.SslRequired = sslRequired;
            this.NoRedirection = noRedirection;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            if (filterContext.HttpContext == null || request.IsLocal || ConfigurationManager.IgnoreSecurity)
                return;
            bool s = !filterContext.IsChildAction;
            string actionResultType = filterContext.Result != null ? filterContext.Result.ToString() : string.Empty;
            if (actionResultType.Equals("System.Web.Mvc.ViewResult", StringComparison.InvariantCultureIgnoreCase) && !this.NoRedirection)
            {
                var uriBuilder = new UriBuilder(request.Url);
                if (this.SslRequired && !request.IsSecureConnection)
                {
                    uriBuilder.Scheme = Uri.UriSchemeHttps;
                    uriBuilder.Port = 443;
                    filterContext.Result = new RedirectResult(uriBuilder.Uri.ToString());
                }
                else if (!this.SslRequired && request.IsSecureConnection)
                {
                    uriBuilder.Scheme = Uri.UriSchemeHttp;
                    uriBuilder.Port = 80;
                    filterContext.Result = new RedirectResult(uriBuilder.Uri.ToString());
                }
            }
        }
    }
}

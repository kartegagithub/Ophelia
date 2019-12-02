using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ophelia.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetUserHostAddress(this HttpRequest request)
        {
            string userHostAddress = string.Empty;
            if (HttpContext.Current != null)
            {
                userHostAddress = !string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? request.ServerVariables["HTTP_X_FORWARDED_FOR"] : request.ServerVariables["REMOTE_ADDR"];
                string[] ArrayAddress = userHostAddress.Split(',');
                if (ArrayAddress.Length > 1)
                    userHostAddress = ArrayAddress[0];
            }
            return userHostAddress;
        }

        public static HttpRequestBase ToRequestBase(this HttpRequest request)
        {
            return new HttpRequestWrapper(request);
        }
    }
}

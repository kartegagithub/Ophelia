using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ophelia.Web.Application.Client
{
    public static class CookieManager
    {
        /// <summary>
        /// Verilen isimdeki cookie bilgisini döner.
        /// </summary>
        public static HttpCookie Get(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName];
        }

        /// <summary>
        /// Verilen isimdeki cookie bilgisini temizler.
        /// </summary>
        public static void ClearByName(string cookieName)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    cookie.Value = null;
                    HttpContext.Current.Request.Cookies.Add(cookie);
                }
                cookie = HttpContext.Current.Response.Cookies[cookieName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    cookie.Value = null;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ophelia.Web.Application.Server
{
    public static class SessionManager
    {
        public static object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }
        public static void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
        public static void Clear(string key)
        {
            HttpContext.Current.Session[key] = null;
        }
        public static void ClearAll()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
        public static string GetSessionID()
        {
            return HttpContext.Current.Session.SessionID;
        }
    }
}

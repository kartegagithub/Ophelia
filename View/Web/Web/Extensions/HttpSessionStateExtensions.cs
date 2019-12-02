using System;
using System.Web;
using System.Web.SessionState;

namespace Ophelia.Web.Extensions
{
    public static class HttpSessionStateExtensions
    {
        public static string GetSessionID(this HttpSessionState session)
        {
            string sessionId = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                sessionId = HttpContext.Current.Session.SessionID;
            else
                sessionId = Ophelia.Utility.GenerateRandomPassword(10);
            return sessionId;
        }
    }
}

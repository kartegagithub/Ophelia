using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using Ophelia.Web.Extensions;
namespace Ophelia.Web
{
    public class Client : IClient, IDisposable
    {
        private string sSessionID;
        private string sUserHostAddress = string.Empty;
        protected int nCurrentLanguageID = 0;
        public Dictionary<string, object> SharedData { get; set; }
        public decimal InstanceID { get; set; }
        public string ApplicationName { get; set; }
        protected HttpContext Context
        {
            get { return HttpContext.Current; }
        }
        public HttpApplicationState Application
        {
            get { return this.Context.Application; }
        }
        public HttpSessionState Session
        {
            get { return this.Context.Session; }
        }
        public HttpResponse Response
        {
            get { return this.Context.Response; }
        }
        public HttpRequest Request
        {
            get { return this.Context.Request; }
        }
        public string ComputerName
        {
            get { return System.Net.Dns.GetHostName(); }
        }
        public string UserHostAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this.sUserHostAddress)) this.sUserHostAddress = this.Request.GetUserHostAddress();
                return this.sUserHostAddress;
            }
        }
        public string UserAgent
        {
            get
            {
                return this.Context.Request.UserAgent;
            }
        }
        public string SessionID
        {
            get
            {
                if (string.IsNullOrEmpty(this.sSessionID)) this.sSessionID = this.Session.GetSessionID();
                return this.sSessionID;
            }
        }

        public long LoggedInUserCount
        {
            get
            {
                if (this.Application != null)
                {
                    if (this.Application["LoggedInUserCount"] == null)
                        this.Application["LoggedInUserCount"] = 0;
                    return Convert.ToInt64(this.Application["LoggedInUserCount"]);
                }
                return 0;
            }
            set
            {
                if (this.Application != null)
                    this.Application["LoggedInUserCount"] = value;
            }
        }
        public virtual int CurrentLanguageID
        {
            get
            {
                if(this.nCurrentLanguageID == 0)
                {
                    if (this.Session != null)
                    {
                        if (this.Session["CurrentLanguageID"] != null)
                        {
                            this.nCurrentLanguageID = Convert.ToInt32(this.Session["CurrentLanguageID"]);
                        }
                        else if(this.nCurrentLanguageID > 0)
                        {
                            this.Session["CurrentLanguageID"] = this.nCurrentLanguageID;
                        }
                    }
                    if (this.nCurrentLanguageID == 0)
                    {
                        this.nCurrentLanguageID = this.GetCurrentLanguageCookie();
                    }
                }
                else
                {
                    if (this.Session != null && this.Session["CurrentLanguageID"] == null)
                        this.Session["CurrentLanguageID"] = this.nCurrentLanguageID;
                }
                return this.nCurrentLanguageID;
            }
            set
            {
                this.nCurrentLanguageID = value;
                this.SetCurrentLanguageCookie();
                if (this.Session != null)
                {
                    this.Session["CurrentLanguageID"] = value;
                }
            }
        }

        protected void SetCurrentLanguageCookie()
        {
            Ophelia.Web.Application.Client.CookieManager.ClearByName("Language");
            HttpCookie languageCookies = new HttpCookie("Language");
            languageCookies.Value = this.nCurrentLanguageID.ToString();
            languageCookies.Expires = DateTime.Now.AddDays(365);
            HttpContext.Current.Response.Cookies.Add(languageCookies);
        }

        protected int GetCurrentLanguageCookie()
        {
            HttpCookie languageCookies = Ophelia.Web.Application.Client.CookieManager.Get("Language");
            int ID = 0;
            if (languageCookies != null && !string.IsNullOrEmpty(languageCookies.Value))
            {
                if(int.TryParse(languageCookies.Value, out ID))
                {
                    return ID;
                }
            }
            return 0;
        }

        public virtual string TranslateText(string Text)
        {
            return Text;
        }
        public virtual string GetImagePath(string path, bool forListing = true)
        {
            return path;
        }
        public virtual void Disconnect()
        {
            this.SharedData = null;
        }
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.Disconnect();
        }
        public Client()
        {
            var rnd = new Random();
            this.InstanceID = rnd.Next(int.MaxValue);
            this.SharedData = new Dictionary<string, object>();
            rnd = null;
        }
    }
}

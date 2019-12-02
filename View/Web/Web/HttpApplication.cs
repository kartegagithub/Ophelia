using Ophelia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web
{
    public class HttpApplication : System.Web.HttpApplication
    {
        protected DateTime beginRequestTime, endRequestTime;

        [ThreadStatic]
        private Client oClient;

        public virtual Client Client
        {
            get
            {
                if (oClient == null)
                    oClient = this.CreateClient();
                return oClient;
            }
        }
        protected virtual Client CreateClient()
        {
            return new Client();
        }
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            this.beginRequestTime = DateTime.Now;
        }
        protected virtual void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            
        }
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
            this.endRequestTime = DateTime.Now;
            if (oClient != null) {
                oClient.Disconnect();
                oClient = null;
            }
        }
    }
}

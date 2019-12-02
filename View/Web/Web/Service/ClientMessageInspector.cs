using Ophelia.Web.Extensions;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace Ophelia.Web.Service
{
    public class ClientMessageInspector : IClientMessageInspector
    {
        protected HttpContext Context
        {
            get { return HttpContext.Current; }
        }
        private string _ApplicationName = string.Empty;
        private bool _CachingEnabled = true;
        public ClientMessageInspector(string applicationName, bool caching)
        {
            this._ApplicationName = applicationName;
            this._CachingEnabled = caching;
        }
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            this.GetRequestInfo();
            var typedHeader = new MessageHeader<RequestInfo>(HeaderInformation.Current);
            var untypedHeader = typedHeader.GetUntypedHeader("request-info", "Ophelia.Web.Service");

            request.Headers.Add(untypedHeader);
            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
        protected void GetRequestInfo()
        {
            HeaderInformation.Current.Application = this._ApplicationName;
            HeaderInformation.Current.CachingEnabled = this._CachingEnabled;
            HeaderInformation.Current.MachineName = System.Net.Dns.GetHostName();
            HeaderInformation.Current.RequestStartTime = DateTime.Now;
            if (this.Context != null)
            {
                if (this.Context.Session != null)
                    HeaderInformation.Current.SessionID = this.Context.Session.GetSessionID();
                else
                    HeaderInformation.Current.SessionID = string.Empty;
                if (this.Context.Request != null)
                {
                    HeaderInformation.Current.UserAgent = this.Context.Request.UserAgent;
                    HeaderInformation.Current.IPAddress = this.Context.Request.GetUserHostAddress();
                }
                else
                {
                    HeaderInformation.Current.UserAgent = string.Empty;
                    HeaderInformation.Current.IPAddress = string.Empty;
                }
            }
            else
            {
                HeaderInformation.Current.SessionID = string.Empty;
                HeaderInformation.Current.UserAgent = string.Empty;
                HeaderInformation.Current.IPAddress = string.Empty;
            }
        }
    }
}

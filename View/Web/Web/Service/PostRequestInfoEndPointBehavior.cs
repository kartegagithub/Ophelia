using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace Ophelia.Web.Service
{
    public class PostRequestInfoEndPointBehavior : IEndpointBehavior
    {
        private string _ApplicationName = string.Empty;
        private bool _CachingEnabled = true;
        public PostRequestInfoEndPointBehavior(string applicationName, bool caching)
        {
            this._ApplicationName = applicationName;
            this._CachingEnabled = caching;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new ClientMessageInspector(this._ApplicationName, this._CachingEnabled);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var channelDispatcher = endpointDispatcher.ChannelDispatcher;
            if (channelDispatcher == null) return;
            foreach (var ep in channelDispatcher.Endpoints)
            {
                var inspector = new DispatchMessageInspector();
                ep.DispatchRuntime.MessageInspectors.Add(inspector);
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }
}
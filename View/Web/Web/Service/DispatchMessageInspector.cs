using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Xml;

namespace Ophelia.Web.Service
{
    public class DispatchMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var header = request.Headers.GetHeader<RequestInfo>("request-info", "Ophelia.Web.Service");
            if (header != null)
            {
                OperationContext.Current.IncomingMessageProperties.Add("request-info", header);
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var header = reply.Headers.GetHeader<RequestInfo>("request-info", "Ophelia.Web.Service");
            if (header != null)
            {
                header.RequestEndTime = DateTime.Now;
                OperationContext.Current.IncomingMessageProperties.Add("request-info", header);
            }
        }
    }
}
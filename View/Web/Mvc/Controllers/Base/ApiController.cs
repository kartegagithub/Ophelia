using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.ServiceModel.Channels;

namespace Ophelia.Web.View.Mvc.Controllers.Base
{
    public abstract class ApiController : System.Web.Http.ApiController
    {
        private Client oClient;
        public ApiController()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(ConfigurationManager.Culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(ConfigurationManager.Culture);
        }
        public string UserHostAddress
        {
            get
            {
                if (this.Request.Properties.ContainsKey("MS_HttpContext"))
                {
                    return ((HttpContextWrapper)this.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                }
                else if (this.Request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                {
                    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                    return prop.Address;
                }
                else if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    return null;
                }
            }
        }
        public HttpApplication Application { get; set; }
        public virtual Client Client
        {
            get
            {
                if (this.oClient == null)
                    this.oClient = this.CreateClient();
                return this.oClient;
            }
        }
        protected abstract Client CreateClient();

        [HttpGet]
        public virtual HttpResponseMessage WADL()
        {
            var basePath = this.Request.RequestUri.Scheme + "://" + this.Request.RequestUri.Authority + "/";

            var doc = new System.Xml.XmlDocument();
            var appNode = doc.AppendChild(doc.CreateElement("application"));
            appNode.Attributes.Append(doc.CreateAttribute("xmlns:xsi")).InnerText = "http://www.w3.org/2001/XMLSchema-instance";
            appNode.Attributes.Append(doc.CreateAttribute("xsi:schemaLocation")).InnerText = "http://wadl.dev.java.net/2009/02 wadl.xsd";
            appNode.Attributes.Append(doc.CreateAttribute("xmlns:xsd")).InnerText = "http://www.w3.org/2001/XMLSchema";

            var resources = appNode.AppendChild(doc.CreateElement("resources"));
            resources.Attributes.Append(doc.CreateAttribute("base")).InnerText = basePath;

            var baseResource = resources.AppendChild(doc.CreateElement("resource"));
            baseResource.Attributes.Append(doc.CreateAttribute("path")).InnerText = this.GetType().Name.Replace("Controller", "").ToLower().Replace("ı", "i");

            var list = new List<dynamic>();
            var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.ReturnType != null && method.ReturnType.Name.IndexOf("Result") > -1 && method.Name != "WADL" && method.Name != "NavigateToLoginPage")
                {
                    var resourceNode = baseResource.AppendChild(doc.CreateElement("resource"));
                    resourceNode.Attributes.Append(doc.CreateAttribute("path")).InnerText = method.Name;

                    var methodNode = resourceNode.AppendChild(doc.CreateElement("method"));
                    methodNode.Attributes.Append(doc.CreateAttribute("name")).InnerText = "GET";
                    methodNode.Attributes.Append(doc.CreateAttribute("id")).InnerText = method.Name;

                    var requestNode = methodNode.AppendChild(doc.CreateElement("request"));

                    foreach (var param in method.GetParameters())
                    {
                        var parameterNode = requestNode.AppendChild(doc.CreateElement("param"));
                        parameterNode.Attributes.Append(doc.CreateAttribute("name")).InnerText = param.Name;
                        parameterNode.Attributes.Append(doc.CreateAttribute("type")).InnerText = "xsd:" + param.ParameterType.Name.ToLower().Replace("ı", "i").Replace("16", "").Replace("32", "").Replace("64", "");
                        parameterNode.Attributes.Append(doc.CreateAttribute("required")).InnerText = (!param.IsOptional).ToString().ToLower();
                        if(param.DefaultValue != null)
                            parameterNode.Attributes.Append(doc.CreateAttribute("default")).InnerText = param.DefaultValue.ToString();
                    }

                    var responseNode = methodNode.AppendChild(doc.CreateElement("response"));
                    var representationNode = responseNode.AppendChild(doc.CreateElement("representation"));
                    if (method.ReturnType.Name.IndexOf("json", StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        representationNode.Attributes.Append(doc.CreateAttribute("mediaType")).InnerText = "application/json";
                    }
                    else
                    {
                        representationNode.Attributes.Append(doc.CreateAttribute("mediaType")).InnerText = "text/html";
                    }
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, doc.OuterXml, "application/xml");
        }
    }
}

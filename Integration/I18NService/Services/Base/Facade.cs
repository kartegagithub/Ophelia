using Ophelia.Web.Service;
using Ophelia.Web;
using Ophelia.Web.Application.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Ophelia.Web.Extensions;

namespace Ophelia.Integration.I18NService.Services.Base
{
    public class Facade: IDisposable
    {

        public I18NIntegratorClient API { get; private set; }

        protected virtual string Schema { get; }

        public virtual TResult PostObject<T, TResult>(string URL, T entity, dynamic parameters, long languageID = 0)
        {
            return this.ProcessResult(Ophelia.Web.Extensions.URLExtensions.PostObject<T, TResult>(this.API.ServiceURL + "/" + this.Schema + "/" + URL, entity, parameters, this.GetHeaders(URL), true, languageID));
        }
        public virtual ServiceObjectResult<T> GetObject<T>(string URL, T entity)
        {
            return (ServiceObjectResult<T>)this.ProcessResult(Ophelia.Web.Extensions.URLExtensions.PostObject<T, ServiceObjectResult<T>>(this.API.ServiceURL + "/" + this.Schema + "/" + URL, entity, null, this.GetHeaders(URL), false, 0));
        }
        public virtual ServiceCollectionResult<T> GetCollection<T>(string URL, int page, int pageSize, T filterEntity, dynamic parameters = null)
        {
            return this.ProcessResult(Ophelia.Web.Extensions.URLExtensions.GetCollection<T>(this.API.ServiceURL + "/" + this.Schema + "/" + URL, page, pageSize, filterEntity, parameters, this.GetHeaders(URL)));
        }
        private object ProcessResult(object obj)
        {
            return obj;
        }

        private WebHeaderCollection GetHeaders(string URL)
        {
            var headers = new WebHeaderCollection();
            headers.Add("AppKey", this.API.AppKey);
            headers.Add("AppCode", this.API.AppCode);
            headers.Add("AppName", this.API.AppName);
            headers.Add("ProjectCode", this.API.ProjectCode);
            headers.Add("ProjectName", this.API.ProjectName);
            return headers;
        }

        public void Dispose()
        {
            
        }

        public Facade(I18NIntegratorClient API)
        {
            this.API = API;
        }
    }
}
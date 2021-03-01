using Newtonsoft.Json;
using Ophelia.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Extensions
{
    public static class URLExtensions
    {
        public static ServiceObjectResult<T> GetObject<T>(this string URL, long ID, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            var request = new WebApiObjectRequest<T>() { ID = ID };
            return URL.GetObject(request, headers, PreAuthenticate);
        }
        public static ServiceObjectResult<T> GetObject<T>(this string URL, dynamic parameters, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            var request = new WebApiObjectRequest<T>();
            SetParameters(request, parameters);
            return URL.GetObject(request, headers, PreAuthenticate);
        }
        public static ServiceObjectResult<T> GetObject<T>(this string URL, WebApiObjectRequest<T> request, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            return JsonConvert.DeserializeObject<ServiceObjectResult<T>>(URL.PostURL(request.ToJson(), "application/json", headers, PreAuthenticate));
        }
        public static ServiceCollectionResult<T> GetCollection<T>(this string URL, int page, int pageSize, T filterEntity, dynamic parameters = null, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            var request = new WebApiCollectionRequest<T>() { Page = page, PageSize = pageSize, Data = filterEntity };
            SetParameters(request, parameters);
            return URL.GetCollection(request, headers, PreAuthenticate);
        }
        public static ServiceCollectionResult<T> GetCollection<T>(this string URL, int page, int pageSize, dynamic parameters = null, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            var request = new WebApiCollectionRequest<T>() { Page = page, PageSize = pageSize };
            SetParameters(request, parameters);
            return URL.GetCollection(request, headers, PreAuthenticate);
        }
        public static ServiceCollectionResult<T> GetCollection<T>(this string URL, WebApiCollectionRequest<T> request, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            return JsonConvert.DeserializeObject<ServiceCollectionResult<T>>(URL.PostURL(request.ToJson(), "application/json", headers, PreAuthenticate));
        }
        public static TResult PostObject<T, TResult>(this string URL, T entity, dynamic parameters, WebHeaderCollection headers = null, bool PreAuthenticate = false, long languageID = 0)
        {
            var request = new WebApiObjectRequest<T>() { Data = entity };
            SetParameters(request, parameters);
            return URL.GetObject<T, TResult>(request, headers, PreAuthenticate);
        }
        public static TResult GetObject<T, TResult>(this string URL, WebApiObjectRequest<T> request, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            var response = "";
            try
            {
                response = URL.PostURL(request.ToJson(), "application/json", headers, PreAuthenticate);
                return JsonConvert.DeserializeObject<TResult>(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not convert data: " + response, ex);
            }
        }
        public static T PostURL<T, TEntity>(this string URL, WebApiObjectRequest<TEntity> request, WebHeaderCollection headers = null, bool PreAuthenticate = false)
        {
            return JsonConvert.DeserializeObject<T>(URL.PostURL(request.ToJson(), "application/json", headers, PreAuthenticate));
        }
        private static void SetParameters<T>(WebApiObjectRequest<T> request, dynamic parameters)
        {
            if (parameters != null)
            {
                var jsonParams = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(Newtonsoft.Json.JsonConvert.SerializeObject(parameters));
                foreach (var item in jsonParams.Keys)
                {
                    request.Parameters[item] = Convert.ToString(jsonParams[item]);
                }
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ophelia
{
    public static class URLExtensions
    {
        public static T PostURL<T>(this string URL, dynamic parameters, WebHeaderCollection headers = null, bool PreAuthenticate = false, string contentType = "application/x-www-form-urlencoded", NetworkCredential credential = null)
        {
            var sParams = "";
            if (parameters != null)
            {
                var jsonParams = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(Newtonsoft.Json.JsonConvert.SerializeObject(parameters));
                foreach (var item in jsonParams.Keys)
                {
                    if (!string.IsNullOrEmpty(sParams))
                        sParams += "&";

                    sParams += item + "=" + JsonConvert.SerializeObject(jsonParams[item]);
                }
            }
            var result = URL.PostURL(sParams, contentType, headers, PreAuthenticate, credential);
            if (!string.IsNullOrEmpty(result))
            {
                if (result.StartsWith("<"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    result = JsonConvert.SerializeXmlNode(doc);
                }
            }
            return JsonConvert.DeserializeObject<T>(result);
        }
        public static T PostURL<T>(this string URL, string parameters, string contentType = "application/x-www-form-urlencoded", WebHeaderCollection headers = null, bool PreAuthenticate = false, NetworkCredential credential = null)
        {
            var result = URL.DownloadURL("POST", parameters, contentType, headers, PreAuthenticate, 120000, credential);
            if (!string.IsNullOrEmpty(result))
            {
                if (result.StartsWith("<"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    result = JsonConvert.SerializeXmlNode(doc);
                }
            }
            return JsonConvert.DeserializeObject<T>(result);
        }
        public static string PostURL(this string URL, string parameters, string contentType = "application/x-www-form-urlencoded", WebHeaderCollection headers = null, bool PreAuthenticate = false, NetworkCredential credential = null)
        {
            return URL.DownloadURL("POST", parameters, contentType, headers, PreAuthenticate, 120000, credential);
        }
        public static string DownloadURL(this string URL, string method = "GET", string parameters = "", string ContentType = "application/x-www-form-urlencoded", WebHeaderCollection headers = null, bool PreAuthenticate = false, int Timeout = 120000, NetworkCredential credential = null)
        {
            byte[] postData = null;
            if (!string.IsNullOrEmpty(parameters))
            {
                if (method == "GET")
                {
                    if (URL.IndexOf("?") == -1)
                        URL += "?";
                    URL += parameters;
                }
                else
                {
                    postData = Encoding.UTF8.GetBytes(parameters);
                }
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Timeout = Timeout;
            request.PreAuthenticate = PreAuthenticate;
            if (credential != null)
                request.Credentials = credential;
            if (headers != null)
                request.Headers.Add(headers);
            request.Method = method;
            if (!URL.StartsWith("ftp://", StringComparison.InvariantCultureIgnoreCase))
            {
                request.ContentType = ContentType;
                if ((method == "POST" || method == "PUT") && postData != null)
                {
                    request.ContentLength = postData.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(postData, 0, postData.Length);
                    }
                }
                else
                    request.ContentLength = 0;
            }
            return request.GetResponseWithoutException().Read();
        }
    }
}

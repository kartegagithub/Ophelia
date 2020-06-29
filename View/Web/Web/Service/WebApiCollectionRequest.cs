using Ophelia.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service
{
    public class WebApiCollectionRequest<T> : WebApiObjectRequest<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Data.Querying.Query.QueryData QueryData { get; set; }
        public WebApiCollectionRequest()
        {
            this.Parameters = new Dictionary<string, object>();
        }
    }
}

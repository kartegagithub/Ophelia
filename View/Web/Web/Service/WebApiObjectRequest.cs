using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service
{
    public class WebApiObjectRequest<T>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public T Data { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public WebApiObjectRequest()
        {
            this.Parameters = new Dictionary<string, object>();
        }
    }
}

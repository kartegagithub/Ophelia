using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Routing
{
    public class PermanentRouteData : RouteData
    {
        public bool PermanentRedirect { get; set; }
        public PermanentRouteData()
        {
            this.PermanentRedirect = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service
{
    public class HeaderInformation
    {
        [ThreadStatic]
        private static RequestInfo _Current;

        public static RequestInfo Current
        {
            get
            {
                _Current = _Current ?? new RequestInfo();
                return _Current;
            }
        }

        public static void Dispose()
        {
            _Current = null;
        }
    }
}

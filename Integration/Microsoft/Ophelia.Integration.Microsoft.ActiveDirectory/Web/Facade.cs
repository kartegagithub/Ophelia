using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Ophelia.Integration.Microsoft.ActiveDirectory.Web
{
    public class Facade : ADFacade
    {
        public static IPrincipal GetIdentity()
        {
            return System.Web.HttpContext.Current.User;
        }
    }
}

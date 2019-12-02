using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Ophelia.Integration.Microsoft.ActiveDirectory.Windows
{
    public class Facade:ADFacade
    {
        public static WindowsIdentity GetIdentity()
        {
            return WindowsIdentity.GetCurrent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.View.Mvc.Models
{
    public interface ISensible
    {
        AlertMessageModel AlertMessage { get; set; }
    }
}

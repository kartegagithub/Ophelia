using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ophelia.Data.EntityFramework
{
    public interface IDataDependency
    {
        Action OnInserted { get; set; }
    }
}

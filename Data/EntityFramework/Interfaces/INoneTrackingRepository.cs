using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ophelia.Data.EntityFramework
{
    public interface INoneTrackingRepository
    {
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
    }
}

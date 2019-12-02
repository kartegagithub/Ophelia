using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ophelia.Data.EntityFramework
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepositoryFromEntity<TEntity>() where TEntity : class;
        TRepository GetRepository<TRepository>();
    }
}

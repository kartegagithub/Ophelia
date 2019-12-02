using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Ophelia.Data.EntityFramework
{
    public interface ICommonRepository
    {
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        TEntity Create<TEntity>() where TEntity : class;
        TEntity Add<TEntity>(TEntity entity) where TEntity : class;
        bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;
    }
}

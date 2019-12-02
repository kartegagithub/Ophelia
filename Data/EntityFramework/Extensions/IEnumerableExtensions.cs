using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Data.EntityFramework
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TEntity> Bind<TEntity, TBase>(this IEnumerable<TEntity> source, ObjectSet<TBase> objectSet)
            where TEntity : class, TBase
            where TBase : class
        {
            Guard.ArgumentNullException(source, "source");
            Guard.ArgumentNullException(objectSet, "objectSet");

            return source.Select(e => objectSet.FindOrAttach(e));
        }

        public static IEnumerable<TEntity> Bind<TEntity>(this IEnumerable<TEntity> source, ObjectContext context)
            where TEntity : class
        {
            Guard.ArgumentNullException(source, "source");
            Guard.ArgumentNullException(context, "context");

            return source.Bind(context.CreateObjectSet<TEntity>());
        }
    }
}

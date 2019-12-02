using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Ophelia.Data.EntityFramework
{
    public interface ILookupProvider
    {
        IEnumerable<KeyValuePair<string, string>> GetLookup<TEntity>(
            Func<TEntity, string> keySelector,
            Func<TEntity, string> valueSelector,
            Func<TEntity, bool> predicate) where TEntity : class;

        IEnumerable<KeyValuePair<string, string>> GetLookup<TEntity>(
            Func<TEntity, string> keySelector,
            Func<TEntity, string> valueSelector) where TEntity : class;

        TResult GetValue<TEntity, TResult>(
            Func<TEntity, TResult> selector, 
            Func<TEntity, bool> predicate) where TEntity : class;

        bool HasValue<TEntity>(
            Func<TEntity, bool> predicate) where TEntity : class;
    }
}

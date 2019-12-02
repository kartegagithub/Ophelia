using System;
using System.Linq;
using System.Data.Objects;
using System.ComponentModel;
using System.Globalization;

namespace Ophelia.Data.EntityFramework
{
    public static class IQueryableExtensions
    {
        public static ObjectQuery<T> AsObjectQuery<T>(this IQueryable<T> source)
        {
            return source as ObjectQuery<T>;
        }

        public static string ToTraceString<T>(this IQueryable<T> source)
        {
            return source.ToObjectQuery("source").ToTraceString();
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, params string[] includes)
        {
            IQueryable<T> result = source;
            foreach (var included in includes)
                result = result.Include(included);
            return result;
        }

        public static IQueryable<T> SetMergeOption<T>(this IQueryable<T> source, MergeOption mergeOption)
        {
            ObjectQuery<T> result = source.ToObjectQuery("source");
            result.MergeOption = mergeOption;
            return result;
        }

        public static IBindingList ToBindingList<T>(this IQueryable<T> source)
        {
            Guard.ArgumentNullException(source, "source");
            IListSource listSource = source as IListSource;
            if (null == listSource)
                Guard.ArgumentNullException(listSource, "source");
            IBindingList bindingList = listSource.GetList() as IBindingList;
            if (null == bindingList)
                Guard.ArgumentNullException(bindingList, "source");
            return bindingList;
        }

        private static ObjectQuery<T> ToObjectQuery<T>(this IQueryable<T> source, string argumentName)
        {
            Guard.ArgumentNullException(source, "source");
            ObjectQuery<T> result = source as ObjectQuery<T>;
            if (null == result)
            {
                throw new ArgumentException();
            }
            return result;
        }
    }
}

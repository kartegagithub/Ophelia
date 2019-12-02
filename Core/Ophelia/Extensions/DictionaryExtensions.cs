﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace Ophelia
{
    public static class DictionaryExtensions
    {
        public static string ToQueryString<TKey, TValue>(this IDictionary<TKey, TValue> target)
        {
            var s = new StringBuilder();
            foreach (var item in target)
            {
                s.Append(Convert.ToString(item.Key));
                s.Append("=");
                s.Append(HttpUtility.UrlEncode(Convert.ToString(item.Value)));
                s.Append("&");
            }
            return s.ToString();
        }

        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
        {
            Guard.ArgumentNullException(source, "source");
            Guard.ArgumentNullException(target, "target");

            foreach (var item in source)
                if (!target.ContainsKey(item.Key))
                    target.Add(item.Key, item.Value);
        }

        public static Nullable<DateTime> GetDateFromString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                if (value is string)
                {
                    DateTime date;
                    if (DateTime.TryParseExact(value as string, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        return date;
                }
            }
            return null;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType()
                .GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }
    }
}

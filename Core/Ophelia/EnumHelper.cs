using System;
using System.Collections.Generic;

namespace Ophelia
{
    public static class EnumHelper
    {
        public static bool Equals<TEnum>(object x, object y) where TEnum : struct
        {
            TEnum xValue;
            if (!Enum.TryParse<TEnum>(x.ToString(), out xValue))
                throw new InvalidCastException();

            TEnum yValue;
            if (!Enum.TryParse<TEnum>(y.ToString(), out yValue))
                throw new InvalidCastException();

            return Enum.Equals(xValue, yValue);
        }

        public static List<T> GetValueList<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).ToList<T>(value => { return (T)Enum.Parse(typeof(T), value.ToString()); });
        }
    }
}

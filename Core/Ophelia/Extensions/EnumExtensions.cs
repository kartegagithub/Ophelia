﻿using System;
using System.Linq;
using System.Reflection;
using Ophelia.Attributes;
using System.Collections.Generic;

namespace Ophelia
{
    public static class EnumExtensions
    {
        public static string ToStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            StringValueAttribute[] attributes = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attributes.Length > 0 ? attributes[0].StringValue : value.ToString();
        }

        public static Int32 ToInt32(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static byte ToByte(this Enum value)
        {
            return Convert.ToByte(value);
        }

        public static string GetValue<T>(this Enum value)
        {
            return (Convert.ChangeType(value, typeof(T))).ToString();
        }
        public static List<TEnum> ToList<TEnum>()
        {
            Type type = typeof(TEnum);

            if (type.BaseType == typeof(Enum))
                throw new ArgumentException("T must be type of System.Enum");

            Array values = Enum.GetValues(type);
            if (values.Length > 0)
                return values.Cast<TEnum>().ToList();
            return null;
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia
{
    public static class TypeExtensions
    {
        public static bool IsPrimitiveType(this Type type)
        {
            if (type == typeof(String)) return true;
            return (type.IsValueType || type.IsPrimitive);
        }
        public static MethodInfo GetRuntimeMethod(
            this Type type, string name, Func<MethodInfo, bool> predicate, params Type[][] parameterTypes)
        {
            return parameterTypes
                .Select(t => type.GetRuntimeMethod(name, predicate, t))
                .FirstOrDefault(m => m != null);
        }
        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }
        private static MethodInfo GetRuntimeMethod(
            this Type type, string name, Func<MethodInfo, bool> predicate, Type[] parameterTypes)
        {
            var methods = type.GetRuntimeMethods().Where(
                m => name == m.Name
                     && predicate(m)
                     && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes)).ToArray();

            if (methods.Length == 1)
            {
                return methods[0];
            }

            return methods.SingleOrDefault(
                m => !methods.Any(m2 => m2.DeclaringType.IsSubclassOf(m.DeclaringType)));
        }
        public static bool IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)
        {
            Guard.ArgumentNullException(toType, "toType");
            Guard.ArgumentNullException(fromType, "fromType");

            if (!toType.IsGenericTypeDefinition ||
                fromType.IsGenericTypeDefinition)
            {
                genericArguments = null;
                return false;
            }

            if (toType.IsInterface)
            {
                foreach (Type interfaceCandidate in fromType.GetInterfaces())
                {
                    if (interfaceCandidate.IsGenericType && interfaceCandidate.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = interfaceCandidate.GetGenericArguments();
                        return true;
                    }
                }
            }
            else
            {
                while (fromType != null)
                {
                    if (fromType.IsGenericType && fromType.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = fromType.GetGenericArguments();
                        return true;
                    }
                    fromType = fromType.BaseType;
                }
            }
            genericArguments = null;
            return false;
        }
        public static Type GetMemberType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }
}

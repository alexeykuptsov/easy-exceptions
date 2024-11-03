using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyExceptions.Yaml
{
    internal static class ReflectionExtensions
    {
        public static Type? BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        private static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static TypeCode GetTypeCode(this Type type)
        {
            var isEnum = type.IsEnum();
            if (isEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }

            if (type == typeof(bool))
            {
                return TypeCode.Boolean;
            }
            else if (type == typeof(char))
            {
                return TypeCode.Char;
            }
            else if (type == typeof(sbyte))
            {
                return TypeCode.SByte;
            }
            else if (type == typeof(byte))
            {
                return TypeCode.Byte;
            }
            else if (type == typeof(short))
            {
                return TypeCode.Int16;
            }
            else if (type == typeof(ushort))
            {
                return TypeCode.UInt16;
            }
            else if (type == typeof(int))
            {
                return TypeCode.Int32;
            }
            else if (type == typeof(uint))
            {
                return TypeCode.UInt32;
            }
            else if (type == typeof(long))
            {
                return TypeCode.Int64;
            }
            else if (type == typeof(ulong))
            {
                return TypeCode.UInt64;
            }
            else if (type == typeof(float))
            {
                return TypeCode.Single;
            }
            else if (type == typeof(double))
            {
                return TypeCode.Double;
            }
            else if (type == typeof(decimal))
            {
                return TypeCode.Decimal;
            }
            else if (type == typeof(DateTime))
            {
                return TypeCode.DateTime;
            }
            else if (type == typeof(string))
            {
                return TypeCode.String;
            }
            else
            {
                return TypeCode.Object;
            }
        }

        public static bool IsDbNull(this object value)
        {
            return value.GetType().FullName == "System.DBNull";
        }

        private static readonly Func<PropertyInfo, bool> IsInstance = property => !(property.GetMethod ?? property.SetMethod).IsStatic;
        private static readonly Func<PropertyInfo, bool> IsInstancePublic = property => IsInstance(property) && (property.GetMethod ?? property.SetMethod).IsPublic;

        public static IEnumerable<PropertyInfo> GetProperties(this Type type, bool includeNonPublic)
        {
            var predicate = includeNonPublic ? IsInstance : IsInstancePublic;

            return type.IsInterface()
                ? new[] { type }
                    .Concat(type.GetInterfaces())
                    .SelectMany(i => i.GetRuntimeProperties().Where(predicate))
                : type.GetRuntimeProperties().Where(predicate);
        }

        public static IEnumerable<FieldInfo> GetPublicFields(this Type type)
        {
            return type.GetRuntimeFields().Where(f => !f.IsStatic && f.IsPublic);
        }

        public static Attribute[] GetAllCustomAttributes<TAttribute>(this PropertyInfo member)
        {
            // IMemberInfo.GetCustomAttributes ignores it's "inherit" parameter for properties,
            // and the suggested replacement (Attribute.GetCustomAttributes) is not available
            // on netstandard1.3
            var result = new List<Attribute>();
            var type = member.DeclaringType;

            while (type != null)
            {
                result.AddRange(member.GetCustomAttributes(typeof(TAttribute)));

                type = type.BaseType();
            }

            return result.ToArray();
        }
    }
}

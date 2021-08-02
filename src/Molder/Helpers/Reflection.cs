using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Molder.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class Reflection
    {
        public static IEnumerable<ParameterInfo?> GetMethodParameters(this MethodInfo method)
        {
            return method.GetParameters();
        }

        public static Type? GetObjectType(this object obj, bool checkElementType = false)
        {
            Type? t;
            switch(obj)
            {
                case null:
                    return null;
                case Type type:
                    t = type;
                    break;
                case ParameterInfo info:
                    t = info.ParameterType;
                    break;
                case PropertyInfo info:
                    t = info.PropertyType;
                    break;
                case FieldInfo info:
                    t = info.FieldType;
                    break;
                default:
                    t = obj.GetType();
                    break;
            }

            if(checkElementType && t.HasElementType)
            {
                t = t.GetElementType();
            }

            t = Nullable.GetUnderlyingType(t) ?? t;

            return t;
        }

        public static object CreateArray(this Type type, int length)
        {
            return Array.CreateInstance(type, length);
        }

        public static object? GetDefault(this Type type)
        {
            if(type == null || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return null;
            }

            Func<object?> f = GetDefault<object>;
            return f.Method.GetGenericMethodDefinition().MakeGenericMethod(type).Invoke(null, null);
        }

        public static T? GetDefault<T>()
        {
            return default;
        }

        public static object? ConvertObject(object obj, Type type)
        {
            var t = GetObjectType(type);

            if(obj == null)
            {
                return GetDefault(t);
            }

            if(t.IsEnum)
            {
                obj = Enum.Parse(t, obj is string value ? value : obj.ToString(), false);
            }

            if(t == typeof(string))
            {
                if(obj is string convertObject)
                {
                    return convertObject;
                }

                var mi = obj.GetType().GetMethods().SingleOrDefault(m => m.Name == "ToString" && !m.GetMethodParameters().Any());
                return mi?.Invoke(obj, Array.Empty<object>());
            }

            if((obj is string s) && t == typeof(char[]))
            {
                return s.Split();
            }

            if(t.IsArray)
            {
                if(obj is Array arrSrc)
                {
                    var arrDest = (Array)CreateArray(t.GetElementType(), arrSrc.Length);
                    Array.Copy(arrSrc, arrDest, arrSrc.Length);
                    return arrDest;
                }
            }

            if(t == typeof(object))
            {
                return obj;
            }

            if(obj is not string o)
            {
                return Convert.ChangeType(obj, t);
            }

            if(t == typeof(bool))
            {
                if(short.TryParse(o, out var i))
                {
                    return i != 0;
                }

                return bool.Parse(o);
            }

            if(t == typeof(decimal) || t == typeof(float))
            {
                var types = new[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider), t.MakeByRefType() };
                var args = new[] { o, NumberStyles.Any, new NumberFormatInfo { NumberDecimalSeparator = "," }, GetDefault(t) };

                if((bool)t.GetMethod("TryParse", types)?.Invoke(null, args)!)
                {
                    return args[3];
                }

                types = new[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider) };
                args = new object[] { o, NumberStyles.Any, new NumberFormatInfo { NumberDecimalSeparator = "." } };
                return t.GetMethod("Parse", types)?.Invoke(null, args);
            }

            if(t == typeof(long)
                || t == typeof(ulong)
                || t == typeof(int)
                || t == typeof(uint)
                || t == typeof(short)
                || t == typeof(ushort)
                || t == typeof(byte)
                || t == typeof(sbyte)
                || t == typeof(char))
            {
                return t.GetMethod("Parse", new[] { typeof(string) })?.Invoke(null, new object[] {o});
            }

            if(t == typeof(DateTime))
            {
                return DateTime.TryParse(o, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out var dt) ? dt : DateTime.Parse(o, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AssumeLocal);
            }

            return Convert.ChangeType(o, t);
        }

        public static T ConvertObject<T>(object obj)
        {
            return (T)ConvertObject(obj, typeof(T))!;
        }

        public static T[] CreateArray<T>(int length)
        {
            return new T[length];
        }

        public static object? TryConvertObject(this object obj, Type type)
        {
            try
            {
                return ConvertObject(obj, type);
            }
            catch
            {
                return GetDefault(type);
            }
        }

        public static T TryConvertObject<T>(object obj)
        {
            return (T)TryConvertObject(obj, typeof(T))!;
        }
    }
}
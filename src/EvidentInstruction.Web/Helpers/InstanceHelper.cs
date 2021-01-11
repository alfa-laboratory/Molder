using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace EvidentInstruction.Web.Helpers
{
    public class InstanceCreator<T>
    {
        public static readonly Func<T> Instance = Creator();

        static Func<T> Creator()
        {
            Type t = typeof(T);
            if (t == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (t.HasDefaultConstructor())
                return Expression.Lambda<Func<T>>(Expression.New(t)).Compile();

            return () => (T)FormatterServices.GetUninitializedObject(t);
        }
    }

    public static class DefaultConstruction
    {
        public static bool HasDefaultConstructor(this Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }
    }

    public class Creat<T>
    {
        public static Func<T> Creator<T>(T obj)
        {
            Type t = typeof(T);
            if (t == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (t.HasDefaultConstructor())
                return Expression.Lambda<Func<T>>(Expression.New(t)).Compile();

            return () => (T)FormatterServices.GetUninitializedObject(t);
        }
    }




}

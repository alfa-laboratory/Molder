using System;
using System.Reflection;

namespace Molder.Web.Extensions
{
    public static class FieldExtension
    {
        public static bool CheckAttribute(this FieldInfo fieldInfo, Type type)
        {
            return fieldInfo.GetCustomAttribute(type) == null ? false : true;
        }
    }
}

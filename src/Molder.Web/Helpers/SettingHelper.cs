using Molder.Web.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Molder.Web.Helpers
{
    public static class SettingHelper
    {
        public static IEnumerable<FieldInfo> GetAllFields(Type type)
        {
            return type.GetNestedTypes().SelectMany(GetAllFields)
                       .Concat(type.GetFields());
        }

        public static string GetValue(this Enum value)
        {
            var customEnum = value;
            var type = customEnum.GetType();
            var field = type.GetField(customEnum.ToString());
            if (!(field.GetCustomAttributes(typeof(EnumValue),
                                       false) is EnumValue[] attrs))
            {
                return null;
            }

            return attrs.Length > 0 ? attrs[0].Value : null;
        }
    }
}
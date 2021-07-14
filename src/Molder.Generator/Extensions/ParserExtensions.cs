using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using System.Linq;
using Molder.Controllers;
using Molder.Extensions;
using System.ComponentModel;

namespace Molder.Generator.Extensions
{
    public static class ParserExtensions
    {
        public static IEnumerable<object> ToEnumerable(this Table table, VariableController variableController) {
            var enumerable = new List<object>();
            foreach (var value in table.Header.ToList()) 
            {
                enumerable.Add(variableController.ReplaceVariables(value) ?? value);
            }
            return enumerable;
        }

        public static Dictionary<string, object> ToDictionary(this Table table, VariableController variableController)
        {
            var enumerable = new Dictionary<string, object>();
            var keys = table.Header.ToList();
            var values = table.Rows.ToList()[0];
            for  (int i=0;i<keys.Count;i++)
            {
                enumerable.Add(variableController.ReplaceVariables(keys[i]) ?? keys[i], variableController.ReplaceVariables(values[i]) ?? values[i]);
            }
            return enumerable;
        }

        public static IEnumerable<T> TryParse<T>(this IEnumerable<object> enumerable)
        {
            List<T> tmpList = new List<T>();
            var tmpType = TypeDescriptor.GetConverter(typeof(T));
            foreach (object value in enumerable)
            {
                try
                {
                    tmpList.Add((T)Convert.ChangeType(value, typeof(T)));
                }
                catch (InvalidCastException)
                {

                }
            }
            return tmpList;
        }
    }
}

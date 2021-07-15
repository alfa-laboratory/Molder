using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using System.Linq;
using Molder.Controllers;
using Molder.Extensions;
using FluentAssertions;
using FluentAssertions.Numeric;

namespace Molder.Generator.Extensions
{
    public static class ParserExtensions
    {
        public static IEnumerable<object> ToEnumerable(this Table table, VariableController variableController) {
            var enumerable = new List<object>();
            table.Rows.ToList().Count.Should().BeLessThan(1, "Table must have only 1 row: Values");
            foreach (var value in table.Header.ToList()) 
            {
                enumerable.Add(variableController.ReplaceVariables(value) ?? value);
            }
            return enumerable;
        }

        public static Dictionary<string, object> ToDictionary(this Table table, VariableController variableController)
        {
            var enumerable = new Dictionary<string, object>();
            table.Header.ToList().Count.Should().BeInRange(1, 1, "Table must have only 2 rows: Keys and Values");
            var keys = table.Header.ToList();
            table.Rows.ToList().Count.Should().BeInRange(1,1, "Table must have only 2 rows: Keys and Values");
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
            foreach (object value in enumerable)
            {
                try
                {
                    tmpList.Add((T)Convert.ChangeType(value, typeof(T)));
                }
                catch (InvalidCastException){}
            }
            return tmpList;
        }
    }
}

using Molder.Database.Infrastructures;
using Molder.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Molder.Database.Extensions
{
    public static class ParserExtensions
    {
        /// <summary>
        /// Создание Sql INSERT запроса
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>        
        public static string ToSqlQuery(this IEnumerable<Dictionary<string, object>> tableParameters, string tableName) 
        {
            var value = string.Empty;
            var header = string.Empty;

            var strBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                Log.Logger().LogWarning("Table name is Empty.");
                throw new ArgumentNullException("Table name is Empty.");
            }

            var parameters = tableParameters as Dictionary<string, object>[] ?? tableParameters.ToArray();
            if (parameters.Any())
            {
                parameters.ToList().ForEach(row =>
                {
                    if (!header.Any())
                    {
                        header = string.Join(",", row.Keys);
                    }

                    strBuilder.Append($"({string.Join(",", row.Values)}),");
                });

                value = strBuilder.ToString().TrimEnd(',');
            }
            else
            {
                Log.Logger().LogWarning("List with table parameters is Empty.");
                throw new ArgumentNullException("List with table parameters is Empty.");
            }
            
            return $"{QueryType.INSERT} INTO {tableName} ({header}) VALUES {value}";
        }
        public static IEnumerable<Dictionary<string, object>> ToDictionary(this Table table)
        {
            var tableParameters = new List<Dictionary<string, object>>();
            var insertTable = table.CreateDynamicSet();
            var list = insertTable.ToList();

            if (!list.Any())
            {
                Log.Logger().LogWarning("List with table patameters is Empty.");
                throw new ArgumentNullException("List with table patameters is Empty.");
            }

            foreach (var element in list)
            {
                tableParameters
                    .Add(((IDictionary<string, object>)element)
                    .ToDictionary(e => e.Key, e =>
                    {
                        var (_, value) = e;
                        if (string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            return "NULL";
                        }

                        if (DateTime.TryParse(value.ToString(), out DateTime date))
                        {
                            var result = date.ToString("yyyy-M-dd");
                            return $"'{result}'";
                        }

                        if (value.ToString()?.ToUpper() == "TRUE" || value.ToString()?.ToUpper() == "FALSE")
                        {
                            return value;
                        }

                        if (value.ToString()!.Any(char.IsLetter) & value.ToString()?.ToUpper() != "NULL")
                        {
                            return $"'{value}'";
                        }

                        return value;
                    }));
            }
            return tableParameters;
        }
    }
}
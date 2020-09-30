using EvidentInstruction.Database.Infrastructures;
using EvidentInstruction.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvidentInstruction.Database.Helpers
{
    public static class ParserToString
    {
        /// <summary>
        /// Создание Sql INSERT запроса
        /// </summary>        
        public static string ToSqlQuery(this IEnumerable<Dictionary<string, object>> tableParameters, string tableName) 
        {
            var value = string.Empty;
            var header = string.Empty;

            var strBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                Log.Logger.Warning("Table name is Empty.");
                throw new ArgumentNullException("Table name is Empty.");
            }

            if (tableParameters.Any())
            {
                tableParameters.ToList().ForEach(row =>
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
                Log.Logger.Warning("List with table parameters is Empty.");
                throw new ArgumentNullException("List with table parameters is Empty.");
            }
            
            return $"{QueryType.INSERT} INTO {tableName} ({header}) VALUES {value}";
        }
    }
}

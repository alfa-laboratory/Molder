using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Molder.Extensions;
using Molder.Infrastructures;

namespace Molder.Helpers
{
    public static class Message
    {
        public static string? CreateMessage(IEnumerable<string> list)
        {
            try
            {
                var enumerable = list as string[] ?? list.ToArray();
                if (enumerable.Any())
                {
                    var message = string.Empty;
                    message = enumerable.Aggregate((i, j) => i + Environment.NewLine + j);
                    return message;
                }

                Log.Logger().LogWarning("The IEnumerable<string> array contains no elements");
                return null;
            }
            catch(ArgumentNullException)
            {
                Log.Logger().LogWarning("No array was passed to convert to string (null)");
                return null;
            }
            
        }

        public static string? CreateMessage(ICollection<ValidationResult> results)
        {
            try
            {
                if (results.Any())
                {
                    var message = string.Empty;
                    results.ToList().ForEach(res => message += res.ErrorMessage + Environment.NewLine);
                    return message;
                }

                Log.Logger().LogWarning("The ICollection<ValidationResult> array contains no elements");
                return null;
            }
            catch (ArgumentNullException)
            {
                Log.Logger().LogWarning("No array was passed to convert to string (null)");
                return null;
            }
        }

        public static string CreateMessage(this DataTable dataTable)
        {
            var (str, isMoreMaxRows) = dataTable.ConvertToString();
            if (isMoreMaxRows)
            {
                str += $"...{Environment.NewLine}Table contains is more {Constants.MAX_ROWS} rows";
            }
            return str;
        }

        public static string CreateMessage(this DataRow dataRow)
        {
            return dataRow.ConvertToString();
        }
    }
}
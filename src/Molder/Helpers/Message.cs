using Microsoft.Extensions.Logging;
using Molder.Extensions;
using Molder.Infrastructures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Molder.Helpers
{
    public static class Message
    {
        public static string CreateMessage(IEnumerable<string> list)
        {
            try
            {
                if (list.Any())
                {
                    string message = string.Empty;
                    message = list.Aggregate((i, j) => i + System.Environment.NewLine + j);
                    return message;
                }
                else
                {
                    Log.Logger().LogWarning("The IEnumerable<string> array contains no elements");
                    return null;
                }
            }
            catch(ArgumentNullException)
            {
                Log.Logger().LogWarning("No array was passed to convert to string (null)");
                return null;
            }
            
        }

        public static string CreateMessage(ICollection<System.ComponentModel.DataAnnotations.ValidationResult> results)
        {
            try
            {
                if (results.Any())
                {
                    string message = string.Empty;
                    results.ToList().ForEach(res => message += res.ErrorMessage + Environment.NewLine);
                    return message;
                }
                else
                {
                    Log.Logger().LogWarning("The ICollection<ValidationResult> array contains no elements.");
                    return null;
                }
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
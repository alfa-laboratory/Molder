using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EvidentInstruction.Helpers
{
    public static class Message
    {
        public static string CreateMessage(IEnumerable<string> list)
        {
            string message = string.Empty;
            try
            {
                message = list.Aggregate((i, j) => i + System.Environment.NewLine + j);
            }
            catch(ArgumentNullException)
            {
                Log.Logger.Warning("Массив для преобразования в строку не передан (null).");
                return null;
            }
            catch(InvalidOperationException)
            {
                Log.Logger.Warning("Массив для преобразования в строку пустой.");
                return null;
            }
            return message;
        }

        public static string CreateMessage(ICollection<System.ComponentModel.DataAnnotations.ValidationResult> results)
        {
            string message = string.Empty;
            try
            {
                results.ToList().ForEach(res => message += res.ErrorMessage + System.Environment.NewLine);
            }
            catch (ArgumentNullException)
            {
                Log.Logger.Warning("Массив для преобразования в строку не передан (null).");
                return null;
            }
            catch (InvalidOperationException)
            {
                Log.Logger.Warning("Массив для преобразования в строку пустой.");
                return null;
            }
            return message;
        }

        public static string CreateMessage(this DataTable dataTable)
        {
            string message = new string('-', 75);
            var colHeaders = dataTable.Columns.Cast<DataColumn>().Select(arg => arg.ColumnName);
            foreach (var s in colHeaders)
            {
                message += ("| {0,-20}", s);
            }
            message += Environment.NewLine;
            message += new string('-', 75);
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var o in row.ItemArray)
                {
                    message += ("| {0,-20}", o.ToString());
                }
                message += Environment.NewLine;
            }

            message += new string('-', 75);
            return message;
        }
    }
}
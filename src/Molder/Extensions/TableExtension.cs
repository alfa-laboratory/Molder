using Molder.Infrastructures;
using System;
using System.Data;
using System.Text;

namespace Molder.Extensions
{
    public static class TableExtensions
    {
        public static (string str, bool isMoreMaxRows) ConvertToString(this DataTable dataTable)
        {
            if (dataTable is null)
            {
                throw new ArgumentNullException(nameof(dataTable), "The table to convert to string is null");
            }
            var isMoreMaxRows = dataTable.Rows.Count > Constants.MAX_ROWS ? true : false;
            var rowCount = isMoreMaxRows ? Constants.MAX_ROWS : dataTable.Rows.Count;

            var output = new StringBuilder();
            var columnsWidths = dataTable.GetColumnsSize();

            // Write Column titles
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var text = dataTable.Columns[i].ColumnName.Shorten();
                output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
            }
            output.Append($"|{Environment.NewLine}{new string('=', output.Length)}{Environment.NewLine}");

            // Write Rows
            int currentRow = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var text = row[i].ToString().Shorten();
                    output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
                }
                output.Append($"|{Environment.NewLine}");
                if (currentRow < rowCount)
                    currentRow++;
                else break;
            }
            return (output.ToString(), isMoreMaxRows);
        }

        public static string ConvertToString(this DataRow dataRow)
        {
            var output = new StringBuilder();
            var columnsWidths = GetColumnsSize(dataRow.Table.Columns, dataRow);

            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                var text = dataRow.Table.Columns[i].ColumnName.Shorten();
                output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
            }
            output.Append($"|{Environment.NewLine}{new string('=', output.Length)}{Environment.NewLine}");

            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                var text = dataRow[i].ToString().Shorten();
                output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
            }
            output.Append($"|{Environment.NewLine}");

            return output.ToString();
        }

        private static int[] GetColumnsSize(this DataTable dataTable)
        {
            var columnsWidths = new int[dataTable.Columns.Count];

            // Get Column Titles
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var length = dataTable.Columns[i].ColumnName.Length;
                if (columnsWidths[i] < length)
                {
                    columnsWidths[i] = length;
                }
            }

            // Get column widths
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var length = row[i].ToString().Length;
                    if (columnsWidths[i] < length)
                        columnsWidths[i] = length;
                }
            }
            return columnsWidths;
        }

        private static int[] GetColumnsSize(DataColumnCollection columns, DataRow row)
        {
            var columnsWidths = new int[columns.Count];

            // Get Column Titles
            for (int i = 0; i < columns.Count; i++)
            {
                var length = columns[i].ColumnName.Length;
                if (columnsWidths[i] < length)
                {
                    columnsWidths[i] = length;
                }
            }

            // Get column widths

            for (int i = 0; i < columns.Count; i++)
            {
                var length = row[i].ToString().Length;
                if (columnsWidths[i] < length)
                    columnsWidths[i] = length;
            }
            return columnsWidths;
        }

        private static string PadCenter(string text, int maxLength)
        {
            int diff = maxLength - text.Length;
            return new string(' ', diff / 2) + text + new string(' ', (int)(diff / 2.0 + 0.5));
        }

        private static string Shorten(this string str)
        {
            return
                str.Length > Constants.MAX_LENGTH ?
                str.Substring(0, str.Length - 3) + "..." : str;
        }
    }
}
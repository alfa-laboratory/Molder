using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public static class CreateObject
    {
        public static DataTable CreateDataTable(List<string> columns, List<string> rows)
        {
            var dt = new DataTable();
            foreach (var column in columns)
            {
                dt.Columns.Add(column);
            }

            foreach (var row in rows)
            {
                var elements = row.Split(";");
                dt.Rows.Add(elements);
            }
            return dt;
        }
    }
}

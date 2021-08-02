using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Molder.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public static class CreateObject
    {
        public static DataTable CreateDataTable(IEnumerable<string> columns, IEnumerable<string> rows)
        {
            var dt = new DataTable();
            foreach (var column in columns)
            {
                dt.Columns.Add(column);
            }

            foreach (object?[] elements in rows.Select(row => row.Split(";")))
            {
                dt.Rows.Add(elements);
            }
            return dt;
        }
    }
}

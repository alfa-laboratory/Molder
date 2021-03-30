using Molder.Controllers;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Molder.Database.Extensions
{
    public static class ReplaceExtensions
    {
        public static Table ReplaceWith(this Table table, VariableController variableController)
        {
            var dt = new Table(table.Header.ToArray());
            table.Rows.ToList().ForEach(row =>
            {
                var tr = new List<string>();
                row.Values.ToList().ForEach(elem =>
                {
                    tr.Add(variableController.ReplaceVariables(elem));
                });
                dt.AddRow(tr.ToArray());
                tr.Clear();
            });
            return dt;
        }
    }
}

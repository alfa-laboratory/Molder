using System.Collections.Generic;
using System.Linq;
using Molder.Controllers;
using Molder.Extensions;
using TechTalk.SpecFlow;

namespace Molder.SpecFlow.Extensions
{
    public static class Extensions
    {
        public static Table ReplaceWith(this Table table, VariableController variableController)
        {
            if (table is null) return null;
            
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AlfaBank.AFT.Core.Helpers
{
    public static class DataCompare
    {
        public static (bool status, string errors) AreTablesTheSame(this DataTable table1, DataTable table2)
        {
            var errors = new List<string>();
            if (table1.Rows.Count != table2.Rows.Count || table1.Columns.Count != table2.Columns.Count)
            {
                throw new ArgumentOutOfRangeException($"Размеры первой таблицы ({table1.Rows.Count};{table1.Columns.Count}) " +
                        $"не совпадают с размерами второй таблицы ({table2.Rows.Count};{table2.Columns.Count})");
            }

            for (int i = 0; i < table1.Rows.Count; i++)
            {
                for (int c = 0; c < table1.Columns.Count; c++)
                {
                    if (!Equals(table1.Rows[i][c], table2.Rows[i][c]))
                    {
                        errors.Add($"Элементы таблиц в позиции ({i};{c}) не совпадают => \"{table1.Rows[i][c]}\" не равен \"{table2.Rows[i][c]}\"");
                    }
                }
            }

            return errors.Any() ? (false, string.Join(". /n ", errors)) : (true, null);
        }
    }
}
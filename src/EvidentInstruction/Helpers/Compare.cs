using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EvidentInstruction.Helpers
{
    public static class Compare
    {
        public static bool AreTablesTheSame(this DataTable fTable, DataTable sTable)
        {
            var errors = new List<string>();
            if (fTable.Rows.Count != sTable.Rows.Count || fTable.Columns.Count != sTable.Columns.Count)
            {
                Log.Logger.Error($"Размеры первой таблицы ({fTable.Rows.Count};{fTable.Columns.Count}) " +
                        $"не совпадают с размерами второй таблицы ({sTable.Rows.Count};{sTable.Columns.Count})");
                return false;
            }

            for (int i = 0; i < fTable.Rows.Count; i++)
            {
                for (int c = 0; c < fTable.Columns.Count; c++)
                {
                    if (!Equals(fTable.Rows[i][c], sTable.Rows[i][c]))
                    {
                        errors.Add($"Элементы таблиц в позиции ({i};{c}) не совпадают => \"{fTable.Rows[i][c]}\" не равен \"{sTable.Rows[i][c]}\"");
                    }
                }
            }
            if(errors.Any())
            {
                Log.Logger.Error(Message.CreateMessage(errors));
                return false;
            }

            return true;
        }
    }
}
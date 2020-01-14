using System.Data;

namespace AlfaBank.AFT.Core.Helpers
{
    public static class DataCompare
    {
        public static bool AreTablesTheSame(this DataTable table1, DataTable table2)
        {

            if (table1.Rows.Count != table2.Rows.Count || table1.Columns.Count != table2.Columns.Count)
                return false;


            for (int i = 0; i < table1.Rows.Count; i++)
            {
                for (int c = 0; c < table1.Columns.Count; c++)
                {
                    if (!Equals(table1.Rows[i][c], table2.Rows[i][c]))
                        return false;
                }
            }
            return true;
        }
    }
}

using AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper;
using AlfaBank.AFT.Core.Helpers;
using System.Collections.Generic;

namespace AlfaBank.AFT.Core.Models.Context
{
    public class DatabaseContext
    {
        public int Timeout { get; set; }
        public Dictionary<string, (DbConnectionWrapper connection, int? timeout)> DbConnections { get; set; } =
            new Dictionary<string, (DbConnectionWrapper, int?)>();

        public ICollection<string> IsSqlQueryValid(string sql)
        {
            return SqlValidator.IsSqlQueryValid(sql);
        }
    }
}

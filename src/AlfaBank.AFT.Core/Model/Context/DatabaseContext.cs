using AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper;
using AlfaBank.AFT.Core.Helpers;
using System.Collections.Generic;

namespace AlfaBank.AFT.Core.Model.Context
{
    public class DatabaseContext
    {
        public Dictionary<string, DbConnectionWrapper> DbConnections { get; set; } =
            new Dictionary<string, DbConnectionWrapper>();

        public ICollection<string> IsSqlQueryValid(string sql)
        {
            return SqlValidator.IsSqlQueryValid(sql);
        }
    }
}

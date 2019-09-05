using AlfaBank.AFT.Core.Exceptions;
using System.Collections.Generic;
using System.Data;

namespace AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper
{
    public interface IDbConnectionWrapper
    {
        (DataTable, int, IEnumerable<Error>) ExecuteQuery(string query, int? timeout = null, ICollection<DbCommandParameter.DbCommandParameter> parameters = null);
        (int, IEnumerable<Error>) ExecuteNonQuery(string query, ICollection<DbCommandParameter.DbCommandParameter> parameters = null, int? timeout = null);
        (object, IEnumerable<Error>) ExecuteScalar(string query, ICollection<DbCommandParameter.DbCommandParameter> parameters = null, int? timeout = null);
    }
}

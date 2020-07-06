using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace EvidentInstruction.Database.Models.Interfaces
{
    public interface IConnectionWrapper : IDisposable
    {
        DbConnection GetDb();

        (DataTable, int) ExecuteQuery(string query, ICollection<DbCommandParameter> dbCommandParams = null, int? timeout = null);
        int ExecuteNonQuery(string query, ICollection<DbCommandParameter> dbCommandParams = null, int? timeout = null);
        object ExecuteScalar(string query, ICollection<DbCommandParameter> dbCommandParams = null, int? timeout = null);

        (object, int) SelectQuery(string query, string tableName = null, int? timeout = null);
        (object, int) InsertRows(string tableName, object records, int? timeout = null);
        int UpdateRows(string query, string tableName = null, int? timeout = null);
        int DeleteRows(string query, string tableName = null, int? timeout = null);
    }
}

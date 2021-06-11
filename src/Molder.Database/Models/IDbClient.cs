using System;
using System.Data;
using System.Data.Common;

namespace Molder.Database.Models
{
    public interface IDbClient : IDisposable
    {
        IDbConnection Get();

        bool Create(DbConnectionStringBuilder parameters);
        bool IsConnectAlive();
        (object, int) ExecuteQuery(string query, int? timeout = null);
        string ExecuteStringQuery(string query, int? timeout = null);
        int ExecuteNonQuery(string query, int? timeout = null);
        object ExecuteScalar(string query, int? timeout = null);
    }
}

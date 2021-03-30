using Molder.Database.Models.Parameters;
using System;

namespace Molder.Database.Models
{
    public interface IDbClient : IDisposable
    {
        bool Create(DbConnectionParams parameters);
        bool IsConnectAlive();
        (object, int) ExecuteQuery(string query, int? timeout = null);
        int ExecuteNonQuery(string query, int? timeout = null);
        object ExecuteScalar(string query, int? timeout = null);
    }
}

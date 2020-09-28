using System;

namespace EvidentInstruction.Database.Models.Interfaces
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

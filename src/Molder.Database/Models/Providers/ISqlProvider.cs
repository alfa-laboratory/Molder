using System;
using System.Data.SqlClient;

namespace Molder.Database.Models.Providers
{
    public interface ISqlProvider
    {
        bool Create(string connectionString);
        bool IsConnectAlive();
        void UsingTransaction(Action<SqlTransaction> onExecute, Action<Exception> onError, Action onSuccess = null);
        SqlCommand SetupCommand(string query, int? timeout = null);
        bool Disconnect();
    }
}

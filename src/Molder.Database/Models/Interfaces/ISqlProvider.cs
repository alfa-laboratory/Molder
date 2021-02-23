using System;
using System.Data.Common;

namespace Molder.Database.Models
{
    public interface ISqlProvider
    {
        bool Create(string connectionString);
        DbConnection Open(string connectionString);
        bool IsConnectAlive();
        void UsingTransaction(Action<DbTransaction> onExecute, Action<System.Exception> onError, Action onSuccess = null);
        DbCommand SetupCommand(string query, int? timeout = null);
        bool Disconnect();
    }
}

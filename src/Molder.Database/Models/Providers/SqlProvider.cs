using Molder.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Molder.Database.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class SqlProvider : ISqlProvider
    {
        private AsyncLocal<SqlConnection> _connection = new AsyncLocal<SqlConnection>() { Value = null };
        private SqlConnection connection
        {
            get => _connection.Value;
            set
            {
                _connection.Value = value;
            }
        }

        public bool Create(string connectionString)
        {
            try
            {
                if (connection is null)
                {
                    connection = Open(connectionString);
                    return true;
                }
                else
                {
                    if (connection.ConnectionString.Equals(connectionString))
                    {
                        Log.Logger().LogWarning($"Connection with parameters: {Helpers.Message.CreateMessage(connectionString)} is already create");
                        return false;
                    }
                    else
                    {
                        Log.Logger().LogWarning($"Connection parameters are different: {Helpers.Message.CreateMessage(connectionString)}");
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Logger().LogError($"Connection with parameters: {Helpers.Message.CreateMessage(connectionString)} failed.{Environment.NewLine} {ex.Message}");
                throw new ConnectSqlException($"Connection with parameters: {Helpers.Message.CreateMessage(connectionString)} failed.{Environment.NewLine} {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Log.Logger().LogError($"Connection string is empty: {Helpers.Message.CreateMessage(connectionString)} {ex.Message}");
                throw new InvalidOperationException($"Connection string is empty: {Helpers.Message.CreateMessage(connectionString)} {ex.Message}");
            }
        }

        public void UsingTransaction(Action<SqlTransaction> onExecute, Action<Exception> onError, Action onSuccess = null)
        {
            var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                onExecute(transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Transaction failed: {transaction} {Environment.NewLine} {ex.Message}");
                transaction.Rollback();
                onError(ex.GetBaseException());
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public SqlCommand SetupCommand(string query, int? timeout = null)
        {
            var command = connection.CreateCommand();
            command.CommandTimeout = Math.Min(300, Math.Max(0, timeout ?? 0));
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            return command;
        }

        public bool IsConnectAlive()
        {
            if (connection is null)
            {
                return false;
            }
            return connection.State.HasFlag(ConnectionState.Open);
        }

        public bool Disconnect()
        {
            if (connection == null)
            {
                return true;
            }

            try
            {
                connection.Dispose();
                Log.Logger().LogInformation($"Connection is closed and disposed.");
                connection = null;
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Connection not disposed {ex.Message}");
                connection = null;
                return false;
            }
        }

        protected SqlConnection Open(string connectionString)
        {
            var _connection = new SqlConnection(connectionString);
            _connection.Open();
            return _connection;
        }
    }
}

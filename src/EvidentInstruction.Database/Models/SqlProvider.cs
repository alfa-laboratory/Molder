using EvidentInstruction.Database.Exceptions;
using EvidentInstruction.Database.Models.Interfaces;
using EvidentInstruction.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Database.Models
{
    [ExcludeFromCodeCoverage]
    public class SqlProvider : ISqlProvider
    {
        [ThreadStatic]
        private DbConnection connection = null;

        public bool Create(string connectionString)
        {
            try
            {
                if (connection is null)
                {
                    connection = Open(connectionString);
                    Log.Logger().LogInformation($"Connection with parameters: {Helpers.Message.CreateMessage(connectionString)} is open");
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
            catch (DbException ex)
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

        public void UsingTransaction(Action<DbTransaction> onExecute, Action<System.Exception> onError, Action onSuccess = null)
        {
            var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                onExecute(transaction);
                transaction.Commit();
            }
            catch (System.Exception ex)
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

        public DbCommand SetupCommand(string query, int? timeout = null)
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
            try
            {
                if (connection == null)
                {
                    return true;
                }

                if (connection.State != ConnectionState.Closed && connection.State != ConnectionState.Broken)
                {
                    try
                    {
                        connection.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Log.Logger().LogError($"Connection not closed {ex.Message}");
                    }
                }

                connection.Dispose();
                connection = null;
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Logger().LogError($"Connection not closed: {ex.Message}");
                return false;
            }
        }
        public DbConnection Open(string connectionString)
        {
            var _connection = new SqlConnection(connectionString);
            _connection.Open();
            return _connection;
        }
    }
}

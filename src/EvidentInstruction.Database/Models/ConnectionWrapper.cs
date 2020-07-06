using EvidentInstruction.Database.Models.Interfaces;
using EvidentInstruction.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace EvidentInstruction.Database.Models
{
    public abstract class ConnectionWrapper : IConnectionWrapper, IDisposable, ICloneable
    {
        public DbConnectionParams ConnectionParams { get; set; }

        protected DateTime LastConnect { get; set; } = DateTime.Now;
        protected int ConnectPeriod { get; set; } = 3;

        public DbConnection DbConnection { get; set; } = null;

        protected virtual DbCommand CreateCommand() => GetDb().CreateCommand();

        protected virtual bool Disconnect()
        {
            try
            {
                if (DbConnection == null)
                {
                    return true;
                }

                if (DbConnection.State != ConnectionState.Closed && DbConnection.State != ConnectionState.Broken)
                {
                    try
                    {
                        DbConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(ex.Message);
                    }
                }

                DbConnection.Dispose();
                DbConnection = null;
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return false;
            }
        }

        protected virtual void CreateCommandParameter(ref DbCommand command, DbCommandParameter dbCommandParams)
        {
            var cmdParam = command.CreateParameter();
            cmdParam.ParameterName = dbCommandParams.Name;
            cmdParam.Value = dbCommandParams.Value;
            cmdParam.DbType = dbCommandParams.DbType;
            cmdParam.Direction = ParameterDirection.Input;
            command.Parameters.Add(cmdParam);
        }

        protected virtual DbTransaction CreateTransaction(IsolationLevel il = IsolationLevel.ReadUncommitted)
        {
            return DbConnection.BeginTransaction(il);
        }

        protected void UsingTransaction(Action<DbTransaction> onExecute, Action<Exception> onError, Action onSuccess = null, bool readUncommitted = true)
        {
            var transaction = readUncommitted ? CreateTransaction(IsolationLevel.ReadUncommitted) : CreateTransaction();

            try
            {
                onExecute(transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                transaction.Rollback();
                onError(ex.GetBaseException());
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public abstract DbConnection GetDb();

        public abstract (object, int) SelectQuery(string query, string tableName = null,int? timeout = null);

        public abstract (object, int) InsertRows(string tableName, object records, int? timeout = null);

        public abstract int UpdateRows(string query, string tableName = null, int? timeout = null);

        public abstract int DeleteRows(string query, string tableName = null, int? timeout = null);

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void Dispose()
        {
            Disconnect();
        }

        public virtual (DataTable, int) ExecuteQuery(string query, ICollection<DbCommandParameter> dbCommandParams = null, int? timeout = null)
        {
            var results = new DataTable();
            var command = CreateCommand();
            command.CommandTimeout = Math.Min(300, Math.Max(0, timeout ?? 0));
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            if (dbCommandParams?.Count > 0)
            {
                foreach (var p in dbCommandParams)
                {
                    CreateCommandParameter(ref command, p);
                }
            }

            IDataReader reader = null;
            var tmpResults = results;
            UsingTransaction(
                (transaction) =>
                {
                    command.Transaction = transaction;
                    reader = command.ExecuteReader();
                    tmpResults.Load(reader);

                    reader.Dispose();
                },
                (ex) =>
                {
                    Log.Logger.Error($"Failed to execute SQL Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{Helpers.Message.CreateMessage(command)}");
                    reader?.Dispose();
                    tmpResults = null;
                },
                () =>
                {
                    Log.Logger.Information("SQL Query: {0}", Helpers.Message.CreateMessage(command));
                });

            if (tmpResults == null)
            {
                return (null, 0);
            }

            results = tmpResults;
            if (string.IsNullOrWhiteSpace(results.TableName))
            {
                results.TableName = "SQLResults";
            }

            var affectedRows = tmpResults?.Rows?.Count ?? 0;
            tmpResults.Dispose();
            return (results, affectedRows);

        }

        public virtual int ExecuteNonQuery(string query, ICollection<DbCommandParameter> dbCommandParams = null, int ? timeout = null)
        {
            var command = CreateCommand();
            command.CommandTimeout = Math.Min(300, Math.Max(0, timeout ?? 0));
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            if (dbCommandParams?.Count > 0)
            {
                foreach (var p in dbCommandParams)
                {
                    CreateCommandParameter(ref command, p);
                }
            }

            var affectedRows = 0;
            UsingTransaction(
                (transaction) =>
                {
                    command.Transaction = transaction;
                    affectedRows = command.ExecuteNonQuery();
                },
                (ex) =>
                {
                    Log.Logger.Error($"Failed to execute SQL Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{Helpers.Message.CreateMessage(command)}");
                    affectedRows = 0;
                },
                () =>
                {
                    Log.Logger.Information("SQL Non-Query: {0}", Helpers.Message.CreateMessage(command));
                },
                false);

            return affectedRows;
        }

        public virtual object ExecuteScalar(string query, ICollection<DbCommandParameter> dbCommandParams = null, int ? timeout = null)
        {
            var command = CreateCommand();
            command.CommandTimeout = Math.Min(300, Math.Max(0, timeout ?? 0));
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            if (dbCommandParams?.Count > 0)
            {
                foreach (var p in dbCommandParams)
                {
                    CreateCommandParameter(ref command, p);
                }
            }

            object result = null;
            UsingTransaction(
                (transaction) =>
                {
                    command.Transaction = transaction;
                    result = command.ExecuteScalar();
                },
                (ex) => 
                {
                    Log.Logger.Error(ex.Message);
                    result = null;
                },
                () =>
                {
                    Log.Logger.Information("SQL Query (Scalar): {0}", Helpers.Message.CreateMessage(command));
                });
            return result;
        }
    }
}

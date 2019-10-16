using AlfaBank.AFT.Core.Data.DataBase.DbObjects;
using AlfaBank.AFT.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper
{
    public abstract class DbConnectionWrapper : IDbConnectionWrapper, IDisposable, ICloneable
    {
        protected DateTime LastConnect { get; set; } = DateTime.Now;
        protected int ConnectPeriod { get; set; } = 3;

        protected DbConnection DbConnection
        {
            get; set;
        }

        protected dynamic DbTables { get; set; } = null;

        public void Dispose()
        {
            Disconnect();
        }

        public virtual DbConnection GetDb()
        {
            return DbConnection;
        }

        public virtual void CreateCommandParameter(ref DbCommand command, DbCommandParameter.DbCommandParameter parameter)
        {
            var cmdParam = command.CreateParameter();
            cmdParam.ParameterName = parameter.Name;
            cmdParam.Value = parameter.Value;
            cmdParam.DbType = parameter.DbType;
            cmdParam.Direction = ParameterDirection.Input;
            command.Parameters.Add(cmdParam);
        }

        public abstract DbConnection GetDb(IDictionary<string, object> parameters);

        public abstract (object, int, IEnumerable<Error>) SelectQuery(
            string query, string tableName = null,
            ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null);

        public abstract (object, int, IEnumerable<Error>) InsertRows(string tableName, DataTable records,
            ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null);

        public abstract (int, IEnumerable<Error>) UpdateRows(
            string query, string tableName = null,
            ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null);

        public abstract (int, IEnumerable<Error>) DeleteRows(
            string query, string tableName = null,
            ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null);

        public object Clone()
        {
            return MemberwiseClone();
        }

        public (DataTable, int, IEnumerable<Error>) ExecuteQuery(string query, int? timeout = null, ICollection<DbCommandParameter.DbCommandParameter> parameters = null)
        {
            var results = new DataTable();
            var errors = new List<Error>();
            var command = CreateCommand();
            command.CommandTimeout = Math.Min(300, Math.Max(0, timeout ?? 0));
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            if(parameters?.Count > 0)
            {
                foreach(var p in parameters)
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
                    reader?.Dispose();
                    errors.Add(new Error
                    {
                        TargeBase = ex.TargetSite,
                        Message = $"Failed to execute SQL Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{FormatQueryLog(command)}",
                        Type = ex.GetType()
                    });
                },
                () =>
                {
                    System.Diagnostics.Debug.Print("SQL Query: {0}", FormatQueryLog(command));
                });
            results = tmpResults;
            if(string.IsNullOrWhiteSpace(results.TableName))
            {
                results.TableName = "SQLResults";
            }

            var affectedRows = tmpResults?.Rows?.Count ?? 0;
            tmpResults.Dispose();
            return (results, affectedRows, errors);
        }

        public (int, IEnumerable<Error>) ExecuteNonQuery(string query, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            var command = CreateCommand();
            var errors = new List<Error>();
            command.CommandTimeout = Math.Min(300, Math.Max(0, timeout ?? 0));
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            if(@params?.Count > 0)
            {
                foreach(var p in @params)
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
                (ex) => errors.Add(new Error
                {
                    TargeBase = ex.TargetSite,
                    Message = $"Failed to execute SQL Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{FormatQueryLog(command)}",
                    Type = ex.GetType()
                }),
                () =>
                {
                    System.Diagnostics.Debug.Print("SQL Non-Query: {0}", FormatQueryLog(command));
                },
                false);

            return (affectedRows, errors);
        }

        public (object, IEnumerable<Error>) ExecuteScalar(string query, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            var errors = new List<Error>();
            var command = CreateCommand();
            command.CommandTimeout = Math.Min(300, Math.Max(0, timeout ?? 0));
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            if(@params?.Count > 0)
            {
                foreach(var p in @params)
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
                (ex) => errors.Add(new Error
                {
                    TargeBase = ex.TargetSite,
                    Message = $"Failed to execute SQL Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{FormatQueryLog(command)}",
                    Type = ex.GetType()
                }),
                () =>
                {
                    System.Diagnostics.Debug.Print("SQL Query (Scalar): {0}", FormatQueryLog(command));
                });
            return (result, errors);
        }

        protected virtual (bool, IEnumerable<Error>) Disconnect()
        {
            var errors = new List<Error>();
            try
            {
                if(DbConnection == null)
                {
                    return (true, errors);
                }

                if(DbConnection.State != ConnectionState.Closed && DbConnection.State != ConnectionState.Broken)
                {
                    try
                    {
                        DbConnection.Close();
                    }
                    catch(Exception ex)
                    {
                        errors.Add(new Error
                        {
                            TargeBase = ex.TargetSite,
                            Message = ex.Message,
                            Type = ex.GetType()
                        });
                        return (false, errors);
                    }
                }

                DbConnection.Dispose();
                DbConnection = null;
                return (true, errors);
            }
            catch(Exception ex)
            {
                errors.Add(new Error
                {
                    TargeBase = ex.TargetSite,
                    Message = ex.Message,
                    Type = ex.GetType()
                });
                return (false, errors);
            }
        }

        protected virtual DbCommand CreateCommand() => GetDb().CreateCommand();
        protected virtual DbTransaction CreateTransaction()
        {
            return GetDb().BeginTransaction();
        }

        protected virtual DbTransaction CreateTransaction(IsolationLevel il)
        {
            return GetDb().BeginTransaction(il);
        }

        protected void UsingTransaction(Action<DbTransaction> onExecute, Action<Exception> onError, Action onSuccess = null, bool readUncommitted = true)
        {
            var transaction = readUncommitted ? CreateTransaction(IsolationLevel.ReadUncommitted) : CreateTransaction();

            try
            {
                onExecute(transaction);
                transaction.Commit();
            }
            catch(Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // ignored
                }

                onError(ex.GetBaseException());
            }
            finally
            {
                transaction.Dispose();
            }
        }

        protected string FormatQueryLog(IDbCommand command)
        {
            return
                $"{command.CommandText}{Environment.NewLine}-- Params: " +
                $"{string.Join(", ", command.Parameters.Cast<IDbDataParameter>().Select(p => $"{p.ParameterName}='{p.Value?.ToString()}' ({Enum.GetName(typeof(DbType), p.DbType)})"))}";
        }

        protected virtual DbTable GetTableByName(string tableName, string schemaName)
        {
            try
            {
                return ((IEnumerable)DbTables).Cast<dynamic>().Single(t => t.Schema == schemaName && t.Name == tableName);
            }
            catch(InvalidOperationException ex)
            {
                throw new Exception($"Table '{tableName}' was not found", ex);
            }
        }

        protected virtual DbTable GetTableByName(string tableName)
        {
            try
            {
                return ((IEnumerable)DbTables).Cast<dynamic>().Single(t => t.Name == tableName);
            }
            catch(InvalidOperationException ex)
            {
                throw new Exception($"Table '{tableName}' was not found", ex);
            }
        }
    }
}

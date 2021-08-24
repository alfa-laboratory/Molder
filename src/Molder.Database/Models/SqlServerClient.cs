using System;
using System.Data;
using System.Data.SqlClient;
using Molder.Helpers;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Molder.Database.Models.Providers;
using System.Text;
using System.Data.Common;
using Molder.Database.Infrastructures;

namespace Molder.Database.Models
{
    public class SqlServerClient : IDbClient, IDisposable
    {
        public ISqlProvider _provider;
        public SqlServerClient()
        {
            _provider = new SqlProvider();
        }

        [ExcludeFromCodeCoverage]
        public IDbConnection Get()
        {
            return (_provider as SqlProvider)?.Connection!;
        }

        public bool Create(DbConnectionStringBuilder sqlConnectionStringBuilder)
        {
            try
            {
                var connectionString = sqlConnectionStringBuilder as SqlConnectionStringBuilder;
                
                Log.Logger().LogInformation($"Connection has parameters: {Helpers.Message.CreateMessage(connectionString)}");
                
                var connect = _provider.Create(connectionString.ToString());

                return connect;
            }
            catch (ConnectSqlException ex)
            {
                Log.Logger().LogError($"Connection failed.{ex.Message}");
                throw new ConnectSqlException($"Connection failed. {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Connection failed: {ex.Message}");
                throw new ConnectSqlException($"Connection failed: {ex.Message}");
            }
        }
        public bool IsConnectAlive()
        {
            var result = _provider.IsConnectAlive();

            Log.Logger().LogInformation(result ? "Connect is alive" : "Connect isn't alive");

            return result;
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _provider.Disconnect();
        }

        public int ExecuteNonQuery(string query, int? timeout = null)
        {
            var command = _provider.SetupCommand(query, timeout);

            var affectedRows = 0;
            _provider.UsingTransaction(
                (transaction) =>
                {
                    command.Transaction = transaction;
                    affectedRows = command.ExecuteNonQuery();
                },
                (ex) =>
                {
                    Log.Logger().LogError($"Failed to execute SQL Non-Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{query}");
                    throw new SqlQueryException($"Failed to execute SQL Non-Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{query}");
                },
                () =>
                {
                    Log.Logger().LogInformation($"SQL Non-Query: {query}");
                });

            return affectedRows;
        }

        public (object, int) ExecuteQuery(string query, int? timeout = null)
        {
            var results = new DataTable();

            var command = _provider.SetupCommand(query, timeout);

            IDataReader reader = null;
            var tmpResults = results;

            _provider.UsingTransaction(
                (transaction) =>
                {
                    command.Transaction = transaction;
                    reader = command.ExecuteReader();
                    tmpResults.Load(reader);

                    reader.Dispose();
                },
                (ex) =>
                {
                    Log.Logger().LogError($"Failed to execute SQL Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{query}");
                    reader?.Dispose();
                    throw new SqlQueryException($"Failed to execute SQL Query.{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{query}");
                },
                () =>
                {
                    Log.Logger().LogInformation($"SQL Query: {query}");
                });

            results = tmpResults;
            if (string.IsNullOrWhiteSpace(results.TableName))
            {
                results.TableName = "SQLResults";
            }

            var affectedRows = tmpResults?.Rows?.Count ?? 0;
            tmpResults.Dispose();
            return (results, affectedRows);
        }

        public string ExecuteStringQuery(string query, int? timeout = null)
        {
            var command = _provider.SetupCommand(query, timeout);

            SqlDataReader reader = null;
            var sb = new StringBuilder();

            _provider.UsingTransaction(
                (transaction) =>
                {
                    command.Transaction = transaction;
                    reader = command.ExecuteReader();
                    if (reader.HasRows && reader.FieldCount == 1)
                    {
                        while(reader.Read())
                        {
                            sb.Append(reader[0]);
                        }
                    }
                    else
                    {
                        Log.Logger().LogError($"Failed to execute SQL Query (String).{Environment.NewLine}Error Message: sql query result returns more than one field {Environment.NewLine}Query:{Environment.NewLine}{query}");
                        throw new SqlQueryException($"Failed to execute SQL Query (String).{Environment.NewLine}Error Message: sql query result returns more than one field {Environment.NewLine}Query:{Environment.NewLine}{query}");
                    }

                    reader.Dispose();
                },
                (ex) =>
                {
                    Log.Logger().LogError($"Failed to execute SQL Query (String).{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{query}");
                    reader?.Dispose();
                    throw new SqlQueryException($"Failed to execute SQL Query (String).{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{query}");
                },
                () =>
                {
                    Log.Logger().LogInformation($"SQL Query (String): {query}");
                });
            return sb.ToString();
        }

        public object ExecuteScalar(string query, int? timeout = null)
        {
            var command = _provider.SetupCommand(query, timeout);

            object result = null;
            _provider.UsingTransaction(
                (transaction) =>
                {
                    command.Transaction = transaction;
                    result = command.ExecuteScalar();
                },
                (ex) =>
                {
                    Log.Logger().LogError($"Failed to execute SQL Query (Scalar).{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{query}");
                    throw new SqlQueryException($"Failed to execute SQL Query (Scalar).{Environment.NewLine}Error Message: {ex.Message}{Environment.NewLine}Query:{Environment.NewLine}{query}");
                },
                () =>
                {
                Log.Logger().LogInformation($"SQL Query (Scalar): {query}");
                });
            return result;
        }
    }
}
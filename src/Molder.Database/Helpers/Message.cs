using Molder.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Molder.Database.Helpers
{
    public static class Message
    {
        public static string CreateMessage(IDbCommand command)
        {
            string message = string.Empty;
            try
            {
                message =
                $"{command.CommandText}{Environment.NewLine}-- Params: " +
                $"{string.Join(", ", command.Parameters.Cast<IDbDataParameter>().Select(p => $"{p.ParameterName}='{p.Value?.ToString()}' ({Enum.GetName(typeof(DbType), p.DbType)})"))}";
            }
            catch (Exception)
            {
                Log.Logger().LogWarning("DbCommand is Empty (null).");
                return null;
            }
            return message;
        }
        public static string CreateMessage(string connectionParams)
        {
            return string.Join($"{Environment.NewLine}", connectionParams.Split(';'));
        }

        public static string CreateMessage(SqlConnectionStringBuilder sqlConnectionString)
        {
            var message = string.Empty;
            var connectionString = new SqlConnectionStringBuilder(sqlConnectionString.ToString());
            connectionString.Password = "**********";
            message = $@"{Environment.NewLine}{connectionString.ConnectionString.Replace(";", Environment.NewLine)}{Environment.NewLine}";
            return message;
        }
    }
}

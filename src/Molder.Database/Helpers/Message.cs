using Molder.Database.Models;
using Molder.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;

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

        public static string CreateMessage(DbConnectionParams connectionParams)
        {
            var message = string.Empty;
            message = $"{Environment.NewLine}Data Source={connectionParams.Source}{Environment.NewLine}" +
                $"Initial Catalog={connectionParams.Database}{Environment.NewLine}" +
                $"User ID={connectionParams.Login}{Environment.NewLine}" +
                $"Password={connectionParams.Password}{Environment.NewLine}" +
                $"Connect Timeout={connectionParams.Timeout}{Environment.NewLine}" +
                $"Load Balance Timeout={connectionParams.Timeout} failed.{Environment.NewLine}";
            return message;
        }
    }
}

using Microsoft.SqlServer.TransactSql.ScriptDom;
using EvidentInstruction.Database.Models;
using EvidentInstruction.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EvidentInstruction.Database.Helpers
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
            catch (ArgumentNullException)
            {
                Log.Logger.Warning("Комманда не передана (null).");
                return null;
            }
            return message;
        }

        public static string CreateMessage(IList<ParseError> errors)
        {
            string message = string.Empty;
            var lst = new List<string>();
            errors.ToList().ForEach(i => lst.Add(i.Message));
            try
            {
                message = lst.Aggregate((i, j) => i + Environment.NewLine + j);
            }
            catch (ArgumentNullException)
            {
                Log.Logger.Warning("Массив ошибок для преобразования в строку не передан (null).");
                return null;
            }
            catch (InvalidOperationException)
            {
                Log.Logger.Warning("Массив ошибок для преобразования в строку пустой.");
                return null;
            }
            return message;
        }

        public static string CreateMessage(DbConnectionParams connectionParams)
        {
            var message = string.Empty;
            message = $"Source: {connectionParams.Source}; {Environment.NewLine}" +
                $"DataBase: {connectionParams.Database}; {Environment.NewLine}" +
                $"UserID: {connectionParams.Login}; {Environment.NewLine}" +
                $"Password: {connectionParams.Password}";
            return message;
        }
    }
}

using System;
using System.Data.Common;

namespace EvidentInstruction.Database.Exceptions
{
    [Serializable]
    public class ConnectSqlException : DbException
    {
        public ConnectSqlException(string message) : base(message) { }
    }
}

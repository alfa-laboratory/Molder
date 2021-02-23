using System;
using System.Data.Common;

namespace Molder.Database.Exceptions
{
    [Serializable]
    public class ConnectSqlException : DbException
    {
        public ConnectSqlException(string message) : base(message) { }
    }
}

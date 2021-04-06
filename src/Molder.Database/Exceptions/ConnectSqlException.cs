using System;
using System.Data.Common;

namespace Molder.Database
{
    [Serializable]
    public class ConnectSqlException : DbException
    {
        public ConnectSqlException(string message) : base(message) { }
    }
}

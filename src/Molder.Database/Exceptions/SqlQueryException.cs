using System;

namespace Molder.Database.Exceptions
{
    public class SqlQueryException : ArgumentException
    {
        public SqlQueryException(string message) : base(message) { }
    }
}
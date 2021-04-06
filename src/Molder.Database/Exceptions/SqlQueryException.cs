using System;

namespace Molder.Database
{
    public class SqlQueryException : ArgumentException
    {
        public SqlQueryException(string message) : base(message) { }
    }
}
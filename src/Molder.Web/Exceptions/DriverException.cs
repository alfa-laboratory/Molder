using System;

namespace Molder.Web.Exceptions
{
    public class DriverException : Exception
    {
        public DriverException(string message) : base(message) { }
    }
}

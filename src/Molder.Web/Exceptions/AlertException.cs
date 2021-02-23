using System;

namespace Molder.Web.Exceptions
{
    public class AlertException : Exception
    {
        public AlertException(string message) : base(message) { }
    }
}

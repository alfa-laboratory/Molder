using System;

namespace EvidentInstruction.Web.Exceptions
{
    public class AlertException : Exception
    {
        public AlertException(string message) : base(message) { }
    }
}

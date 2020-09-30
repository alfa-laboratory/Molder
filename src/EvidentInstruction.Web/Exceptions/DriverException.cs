using System;

namespace EvidentInstruction.Web.Exceptions
{
    public class DriverException : Exception
    {
        public DriverException(string message) : base(message) { }
    }
}

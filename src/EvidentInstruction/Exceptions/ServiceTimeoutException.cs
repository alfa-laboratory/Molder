using System;

namespace EvidentInstruction.Exceptions
{
    public class ServiceTimeoutException: Exception
    {
        public ServiceTimeoutException(string message, Exception e) : base(message, e) { }
    }
}

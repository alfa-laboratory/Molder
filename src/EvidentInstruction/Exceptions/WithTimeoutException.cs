using System;

namespace EvidentInstruction.Exceptions
{
    public class WithTimeoutException: Exception
    {
        public WithTimeoutException(string message, Exception e) : base(message, e) { }
    }
}

using System;

namespace EvidentInstruction.Exceptions
{
    public class WithHeadersException: Exception
    {
        public WithHeadersException(string message, Exception e): base(message, e) {}
    }
}

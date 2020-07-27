using System;

namespace EvidentInstruction.Exceptions
{
    public class ServiceException: Exception
    {
        public ServiceException(string message) : base(message) { }
    }
}

using System;
using Flurl.Http;

namespace EvidentInstruction.Exceptions
{
    public class ServiceTimeoutException: ServiceException
    {
        public ServiceTimeoutException(HttpCall call, string message, Exception e) : base(call, message, e) { }
    }
}

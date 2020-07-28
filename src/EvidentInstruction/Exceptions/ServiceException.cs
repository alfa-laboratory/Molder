using System;
using Flurl.Http;

namespace EvidentInstruction.Exceptions
{
    public class ServiceException: FlurlHttpException
    {
        public ServiceException(HttpCall call,string message, Exception inner) : base(call,message,inner) { }
    }
}

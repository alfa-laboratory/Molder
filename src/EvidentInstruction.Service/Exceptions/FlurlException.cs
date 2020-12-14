using Flurl.Http;
using System;

namespace EvidentInstruction.Service.Exceptions
{
    [Serializable]
    public class FlurlException: FlurlHttpTimeoutException
    {
        public FlurlException(FlurlCall flurlCall, Exception ex) : base(flurlCall, ex) { }
    }
}

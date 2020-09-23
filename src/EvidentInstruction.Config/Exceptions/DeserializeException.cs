using Newtonsoft.Json;
using System;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class DeserializeException: JsonException
    {
        public DeserializeException(string message, Exception ex) : base(message, ex) { }
    }
}

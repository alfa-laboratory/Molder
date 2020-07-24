using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class SerializeException: JsonException
    {
        public SerializeException(string message)
       : base(message) { }
    }
}

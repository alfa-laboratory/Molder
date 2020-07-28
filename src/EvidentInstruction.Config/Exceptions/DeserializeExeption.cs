using Newtonsoft.Json;
using System;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class DeserializeExeption: JsonException
    {
        public DeserializeExeption(string message) : base(message) { }
    }
}

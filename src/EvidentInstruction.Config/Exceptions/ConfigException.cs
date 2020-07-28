using System;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class ConfigException : Exception
    {
        public ConfigException(string message) : base(message) { }
    }
}

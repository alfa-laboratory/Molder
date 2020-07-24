using System;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ConfigException : Exception
    {
        public ConfigException(string message)
        : base(message) { }
    }
}

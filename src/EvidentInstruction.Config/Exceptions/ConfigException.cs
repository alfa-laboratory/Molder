using System;

namespace EvidentInstruction.Configuration.Exceptions
{
    public class ConfigException : ArgumentException
    {
        public ConfigException(string message) : base(message)
        { }
    }
}
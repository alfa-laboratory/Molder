using System;

namespace Molder.Configuration.Exceptions
{
    public class ConfigException : ArgumentException
    {
        public ConfigException(string message) : base(message) { }
    }
}
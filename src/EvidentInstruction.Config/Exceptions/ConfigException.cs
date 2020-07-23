using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class ConfigException : Exception
    {
        public ConfigException(string message)
        : base(message) { }
    }
}

using System;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class DublicateTagsException: ConfigException
    {
        public DublicateTagsException(string message) : base(message) { }
    }
}

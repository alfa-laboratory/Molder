using System;

namespace EvidentInstruction.Exceptions
{
    [Serializable]
    public class NoFileNameException : ArgumentException
    {
        public NoFileNameException(string message) : base(message) { }
    }
}

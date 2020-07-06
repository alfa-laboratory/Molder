using System;

namespace EvidentInstruction.Exceptions
{
    [Serializable]
    public class GeneratorException : Exception
    {
        public GeneratorException(string message)
        : base(message) {  }
    }
}
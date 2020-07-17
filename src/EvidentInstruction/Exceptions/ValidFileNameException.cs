using System;

namespace EvidentInstruction.Exceptions
{
    [Serializable]
    public class ValidFileNameException: ArgumentException
    {
        public ValidFileNameException(string message) : base(message) { }
    }
}

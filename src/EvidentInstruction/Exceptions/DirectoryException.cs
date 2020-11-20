using System;

namespace EvidentInstruction.Exceptions
{
    [Serializable]
    public class DirectoryException : Exception
    {
        public DirectoryException(string message) : base(message) { }
    }
}

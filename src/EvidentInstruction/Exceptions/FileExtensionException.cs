using System;

namespace EvidentInstruction.Exceptions
{
    [Serializable]
    public class FileExtensionException : ArgumentException
    {
        public FileExtensionException(string message) : base(message) { }

    }
}

using System;

namespace EvidentInstruction.Exceptions
{
    [Serializable]
    public class FileExistException: ArgumentNullException
    {
        public FileExistException(string message) : base(message) { }
    }
}

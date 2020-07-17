using System;

namespace EvidentInstruction.Exceptions
{
    [Serializable]
    public class FileExistException: ArgumentNullException
    {
        private string path;

        public FileExistException(string message) : base(message) { }


    }
}

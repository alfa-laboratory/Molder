using System;

namespace Molder.Exceptions
{
    [Serializable]
    public class FileExistException: ArgumentNullException
    {
        public FileExistException(string message) : base(message) { }
    }
}

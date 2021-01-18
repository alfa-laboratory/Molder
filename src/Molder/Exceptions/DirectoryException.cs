using System;

namespace Molder.Exceptions
{
    [Serializable]
    public class DirectoryException : Exception
    {
        public DirectoryException(string message) : base(message) { }
    }
}

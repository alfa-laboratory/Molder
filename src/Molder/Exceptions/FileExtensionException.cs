using System;

namespace Molder.Exceptions
{
    [Serializable]
    public class FileExtensionException : ArgumentException
    {
        public FileExtensionException(string message) : base(message) { }
    }
}

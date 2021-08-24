using System;

namespace Molder.Exceptions
{
    [Serializable]
    public class NoFileNameException : ArgumentException
    {
        public NoFileNameException(string message) : base(message) { }
    }
}
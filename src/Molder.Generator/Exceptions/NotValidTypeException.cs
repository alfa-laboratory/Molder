using System;

namespace Molder.Generator.Exceptions
{
    public class NotValidTypeException : Exception
    {
        public NotValidTypeException(string message)
            : base(message) { }
    }
}

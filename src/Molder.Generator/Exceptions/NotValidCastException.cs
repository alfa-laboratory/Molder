using System;

namespace Molder.Generator.Exceptions
{
    public class NotValidCastException : Exception
    {
        public NotValidCastException(string message)
            : base(message) { }
    }
}

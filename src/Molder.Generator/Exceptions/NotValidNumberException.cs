using System;

namespace Molder.Generator.Exceptions
{
    public class NotValidNumberException : Exception
    {
        public NotValidNumberException(string message)
            : base(message) { }
    }
}

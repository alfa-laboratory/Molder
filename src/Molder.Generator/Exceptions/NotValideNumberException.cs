using System;

namespace Molder.Generator.Exceptions
{
    public class NotValideNumberException : Exception
    {
        public NotValideNumberException(string message)
            : base(message) { }
    }
}

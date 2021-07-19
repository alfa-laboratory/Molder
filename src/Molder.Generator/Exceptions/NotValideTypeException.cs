using System;

namespace Molder.Generator.Exceptions
{
    public class NotValideTypeException : Exception
    {
        public NotValideTypeException(string message)
            : base(message) { }
    }
}

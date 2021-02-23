using System;

namespace Molder.Web.Exceptions
{
    public class ElementException : Exception
    {
        public ElementException(string message) : base(message) { }
    }
}

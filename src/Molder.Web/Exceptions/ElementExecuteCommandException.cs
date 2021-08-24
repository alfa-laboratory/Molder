using System;

namespace Molder.Web.Exceptions
{
    public class ElementExecuteCommandException : Exception
    {
        public ElementExecuteCommandException(string element, string message) : base($"{element} not available. {message}") { }
    }
}
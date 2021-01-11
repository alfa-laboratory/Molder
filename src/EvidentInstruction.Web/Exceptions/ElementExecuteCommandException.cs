using System;

namespace EvidentInstruction.Web.Exceptions
{
    public class ElementExecuteCommandException : Exception
    {
        public ElementExecuteCommandException(string element, string message) : base($"{element} not available. {message}") { }
    }
}

using System;

namespace EvidentInstruction.Web.Exceptions
{
    public class PageException : Exception
    {
        public PageException(string message) : base(message)
        { }
    }
}

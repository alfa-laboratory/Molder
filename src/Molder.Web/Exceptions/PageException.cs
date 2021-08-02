using System;

namespace Molder.Web.Exceptions
{
    public class PageException : Exception
    {
        public PageException(string message) : base(message) { }
    }
}
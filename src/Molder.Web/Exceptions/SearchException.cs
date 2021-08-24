using System;

namespace Molder.Web.Exceptions
{
    public class SearchException : ArgumentNullException
    {
        public SearchException(string message) : base(paramName: string.Empty, message: message) { }
    }
}
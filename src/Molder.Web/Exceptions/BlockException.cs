using System;

namespace Molder.Web.Exceptions
{
    public class BlockException : Exception
    {
        public BlockException(string message) : base(message)
        { }
    }
}

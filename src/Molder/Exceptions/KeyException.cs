using System;

namespace Molder.Exceptions
{
    class IEnumerableException : Exception
    {
        public IEnumerableException(string message)
        :base(message){ }
    }
}

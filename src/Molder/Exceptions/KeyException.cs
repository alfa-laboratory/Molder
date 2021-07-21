using System;
using System.Collections.Generic;
using System.Text;

namespace Molder.Exceptions
{
    class IndexIsNotValidException : Exception
    {
        public IndexIsNotValidException(string message)
        :base(message){ }
    }
    public class KeyIsNotValidException : Exception
    {
        public KeyIsNotValidException(string message)
            : base(message) { }
    }
}

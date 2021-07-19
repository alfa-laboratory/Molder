using System;

namespace Molder.Generator.Exceptions
{
    public class NotValideCastException:Exception
    {
        public NotValideCastException(string message)
            : base(message) { }
    }
}

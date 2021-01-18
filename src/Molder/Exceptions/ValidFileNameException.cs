using System;

namespace Molder.Exceptions
{
    [Serializable]
    public class ValidFileNameException: ArgumentException
    {
        public ValidFileNameException(string message) : base(message) { }
    }
}

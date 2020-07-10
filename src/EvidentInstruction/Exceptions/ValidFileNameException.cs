using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Exceptions
{
    public class ValidFileNameException : Exception
    {
        public ValidFileNameException(string message) : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Exceptions
{
    public class ValidFileNameException: ArgumentException
    {
        public ValidFileNameException(string message) : base(message) { }
    }
}

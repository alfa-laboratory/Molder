using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Exceptions
{
    public class NoFileNameException : Exception
    {
        public NoFileNameException(string message) : base(message) { }
    }
}

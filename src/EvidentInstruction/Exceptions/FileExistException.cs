using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Exceptions
{
    public class FileExistException: ArgumentNullException
    {
        public FileExistException(string message) : base(message) { }
    }
}

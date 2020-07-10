using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Exceptions
{
    public class FileExtensionException : Exception
    {
        public FileExtensionException(string message) : base(message) { }
    }
}

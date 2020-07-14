using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Exceptions
{
    public class FileExtensionException : ArgumentException
    {
        private string tXT;

        public FileExtensionException(string message) : base(message) { }

        public FileExtensionException(string message, string paramName) : base(message, paramName)
        {
        }

        public FileExtensionException(string message, string paramName, string tXT) : this(message, paramName)
        {
            this.tXT = tXT;
        }
    }
}

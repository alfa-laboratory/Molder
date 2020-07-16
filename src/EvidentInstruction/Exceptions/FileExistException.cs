using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Exceptions
{
    public class FileExistException: ArgumentNullException
    {
        private string path;

        public FileExistException(string message) : base(message) { }

        public FileExistException(string paramName, string message) : base(paramName, message)
        {
        }

        public FileExistException(string paramName, string message, string path) : base(paramName, message)
        {
            this.path = path;
        }
    }
}

using EvidentInstruction.Exceptions;
using System;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class FileIsExistException: NoFileNameException
    {
        public FileIsExistException(string message) : base(message) { }
    }
}

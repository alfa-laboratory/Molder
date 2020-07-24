using EvidentInstruction.Exceptions;
using System;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class GetContentException: FileExistException
    {
        public GetContentException(string message) : base(message) { }
    }
}

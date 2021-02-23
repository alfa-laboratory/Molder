using System;

namespace EvidentInstruction.Service.Exceptions
{
    [Serializable]
    public class FlurlException : Exception
    {
        public string ExceptionName { get; }       
        public FlurlException(string message, Exception ex) : base(message) 
        {           
            ExceptionName = ex.GetType().Name;           
        }
    }
}

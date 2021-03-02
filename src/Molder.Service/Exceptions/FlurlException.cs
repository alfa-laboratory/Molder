using System;

namespace Molder.Service.Exceptions
{
    [Serializable]
    public class FlurlException : Exception
    {
        public string ExceptionName { get; } 
        public Exception Exception { get; }
        public FlurlException(Exception ex) : base(ex.Message) 
        {           
            ExceptionName = ex.GetType().Name;
            Exception = ex;
        }
    }
}
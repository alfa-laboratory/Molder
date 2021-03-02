using System;

namespace Molder.Service.Exceptions
{
    [Serializable]
    public class FlurlException : Exception
    {
        public string ExceptionName { get; }       
        public FlurlException(Exception ex) : base(ex.Message) 
        {           
            ExceptionName = ex.GetType().Name;           
        }
    }
}
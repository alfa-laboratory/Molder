using System;
using System.Reflection;

namespace AlfaBank.AFT.Core.Exceptions
{
    public class Error
    {
        public MethodBase TargeBase { get; set; }
        public Type Type { get; set; }
        public string Message { get; set; }
    }
}

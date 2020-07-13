using System;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Models
{

    public enum TypeOfAccess
    {
        Global,
        Local
    }
    [ExcludeFromCodeCoverage]
    public class Variable
    {
        public Type Type
        {
            get; set;
        }

        public object Value
        {
            get; set;
        }

        public TypeOfAccess TypeOfAccess
        {
            get; set;
        }


    }
}

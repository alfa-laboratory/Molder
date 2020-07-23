using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Config.Exceptions
{
    [Serializable]
    public class NewconfigExeption: JsonException
    {
        public NewconfigExeption(string message)
       : base(message) { }
    }
}

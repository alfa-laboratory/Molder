using EvidentInstruction.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Config.Models
{
    public class BinDirectory : IDirectory
    {
        public string Get()
        {
            return Environment.CurrentDirectory;
        }
    }
}

using EvidentInstruction.Models.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Models
{
    [ExcludeFromCodeCoverage]
    public class BinDirectory : IDirectory
    {
        public string Get()
        {
            return Environment.CurrentDirectory;
        }
    }
}

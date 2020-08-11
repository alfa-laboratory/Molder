using EvidentInstruction.Models.Directory.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Models.Directory
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

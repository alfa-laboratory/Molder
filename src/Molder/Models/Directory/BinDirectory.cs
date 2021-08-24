using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Models.Directory
{
    [ExcludeFromCodeCoverage]
    public class BinDirectory : DefaultDirectory
    {
        public override string Get()
        {
            return Environment.CurrentDirectory;
        }
    }
}
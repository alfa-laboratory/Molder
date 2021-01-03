using System;

namespace EvidentInstruction.Models.Directory
{
    public class BinDirectory : DefaultDirectory
    {
        public override string Get()
        {
            return Environment.CurrentDirectory;
        }
    }
}

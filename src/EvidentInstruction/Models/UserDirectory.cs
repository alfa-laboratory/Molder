using System;
using System.Diagnostics.CodeAnalysis;
using EvidentInstruction.Models.Interfaces;

namespace EvidentInstruction.Models
{
    [ExcludeFromCodeCoverage]
    public class UserDirectory: IDirectory
    { 
        public string Get()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }
}

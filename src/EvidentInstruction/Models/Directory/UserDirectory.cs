using System;
using System.Diagnostics.CodeAnalysis;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Directory.Interfaces;

namespace EvidentInstruction.Models
{
    [ExcludeFromCodeCoverage]
    public class UserDirectory: IDirectory
    { 
        public string Get()
        {
            Log.Logger.Information("Work with UserDirectory");
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }
}

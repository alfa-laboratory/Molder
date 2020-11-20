using System;
using System.Diagnostics.CodeAnalysis;
using EvidentInstruction.Helpers;
using Microsoft.Extensions.Logging;

namespace EvidentInstruction.Models.Directory
{
    [ExcludeFromCodeCoverage]
    public class UserDirectory: DefaultDirectory
    {
        public override string Get()
        {
            Log.Logger().LogInformation("Work with UserDirectory");
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }
}

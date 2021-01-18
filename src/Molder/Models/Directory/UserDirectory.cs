using System;
using System.Diagnostics.CodeAnalysis;
using Molder.Helpers;
using Microsoft.Extensions.Logging;

namespace Molder.Models.Directory
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

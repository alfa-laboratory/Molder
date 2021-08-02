using Molder.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Models.Directory
{
    [ExcludeFromCodeCoverage]
    public class BaseDirectory : DefaultDirectory
    {
        public override string Get()
        {
            Log.Logger().LogInformation("Work with BaseDirectory");
            return AppContext.BaseDirectory;
        }
    }
}
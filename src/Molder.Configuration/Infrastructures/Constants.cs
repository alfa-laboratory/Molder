using System.Diagnostics.CodeAnalysis;

namespace Molder.Configuration.Infrastructures
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const string CONFIG_BLOCK = "Molder";
        public const string DEFAULT_JSON = "appsettings";
        public const string LAUNCH_PROFILE = "ASPNETCORE_ENVIRONMENT";
    }
}

using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationFactory
    {
        public static IConfiguration Create()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}

using EvidentInstruction.Configuration.Infrastructures;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Configuration.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationFactory
    {
        public static IConfiguration Create()
        {
            return new ConfigurationBuilder()
                .AddJsonFile(Constants.DEFAULT_JSON)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
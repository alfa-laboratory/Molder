using EvidentInstruction.Configuration.Exceptions;
using EvidentInstruction.Configuration.Infrastructures;
using EvidentInstruction.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace EvidentInstruction.Configuration.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationFactory
    {
        public static IConfiguration Create()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile($"{Constants.DEFAULT_JSON}.json")
                    .AddJsonFile($"{Constants.DEFAULT_JSON}.{Environment.GetEnvironmentVariable(Constants.LAUNCH_PROFILE)}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                Log.Logger().LogInformation("Config file connected successfully.");

                return configuration;
            }
            catch(FileNotFoundException ex)
            {
                Log.Logger().LogError($"Configuration file not found. Check the connection of the appsettings.json file to the project. Exception is \"{ex.Message}\"");
                throw new ConfigException($"Configuration file not found. Check the connection of the appsettings.json file to the project. Exception is \"{ex.Message}\"");
            }
        }
    }
}
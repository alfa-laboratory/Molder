using Molder.Configuration.Exceptions;
using Molder.Configuration.Infrastructures;
using Molder.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Molder.Models.Directory;

namespace Molder.Configuration.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationFactory
    {
        public static IConfiguration Create(IDirectory directory)
        {
            try
            {
                var ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable(Constants.LAUNCH_PROFILE);
                Log.Logger().LogInformation($"Variable \"ASPNETCORE_ENVIRONMENT\" is \"{(ASPNETCORE_ENVIRONMENT ?? "not set")}\"");

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(directory.Get(), $"{Constants.DEFAULT_JSON}.json"), optional: true, reloadOnChange: true)
                    .AddJsonFile(Path.Combine(directory.Get(), $"{Constants.DEFAULT_JSON}{(ASPNETCORE_ENVIRONMENT != null ? $".{ASPNETCORE_ENVIRONMENT}" : string.Empty )}.json"), optional: true, reloadOnChange: true)
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
using EvidentInstruction.Config.Extension;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Config.Infrastructures;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Config.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConfigCreateTests
    {
        private readonly string validConfigurationBlock =
            "{" +
                $"\"{Constants.CONFIG_BLOCK}\":" +
                "[ " +
                    "{" +
                        "\"tag\": \"WebService\"," +
                        "\"parameters\":" + "{" +
                            "\"first\": 1," +
                            "\"second\": \"test\"" +
                        "}" +
                    "}" +
                "]" +
            "}";

        [Fact]
        public void Create()
        {
            var testConfiguration = new ConfigurationBuilder()
                .AddJsonStream(validConfigurationBlock.ToStream())
                .Build();

            var configConfiguration = ConfigOptionsFactory.Create(testConfiguration);
        }
    }
}

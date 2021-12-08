using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Confluent.Kafka;
using FluentAssertions;
using Molder.Configuration.Models;
using Molder.Kafka.Helpers;
using Molder.Kafka.Infrastructures;
using Molder.Kafka.Models;
using Molder.Kafka.Tests.Extensions;
using Xunit;

namespace Molder.Kafka.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConfigCreateTests
    {
        public static IEnumerable<object[]> JsonData()
        {
                yield return new object[] 
                {
                    "{" + $"\"{Constants.CONFIG_BLOCK}\":" +
                        @"
                        [{
                            ""Settings"": {
                            ""BootstrapServers"": ""localhost"",
                            ""GroupId"": ""foo"",
                            ""AutoOffsetReset"": ""Earliest"",
                            ""AutoCommitIntervalMs"": 5000,
                            ""SessionTimeoutMs"": 6000,
                            ""EnableAutoCommit"": true
                            },
                        ""Topic"": ""test-topic"",
                        ""Name"": ""test""
                        }]
                    }",
                    new List<Settings>
                    {
                        new()
                        {
                            Name = "test",
                            Topic = "test-topic",
                            Config = new ConsumerConfig
                            {
                                BootstrapServers = "localhost",
                                GroupId = "foo",
                                AutoOffsetReset = AutoOffsetReset.Earliest,
                                AutoCommitIntervalMs = 5000,
                                SessionTimeoutMs = 6000,
                                EnableAutoCommit = true
                            }
                        }
                    }
                };
        }

        [Theory]
        [MemberData(nameof(JsonData))]
        public void CreateConfiguration_ValidJson_ReturnOptionModel(string json, IEnumerable<Settings> settings)
        {
            var testConfiguration = new ConfigurationBuilder()
                .AddJsonStream(json.ToStream())
                .Build();

            var configConfiguration = ConfigOptionsFactory.Create(testConfiguration);

            var _settings = settings as List<Settings>;
            configConfiguration.Value.Count.Should().Be(_settings.Count);

            for(var i = 0; i < configConfiguration.Value.Count; i++ )
            {
                configConfiguration.Value.ToList()[i].Should().BeEquivalentTo(_settings[i]);
            }
        }
    }
}

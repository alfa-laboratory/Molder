using EvidentInstruction.Configuration.Helpers;
using EvidentInstruction.Configuration.Infrastructures;
using EvidentInstruction.Configuration.Models;
using EvidentInstruction.Configuration.Tests.Extension;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace EvidentInstruction.Configuration.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConfigCreateTests
    {
        public static IEnumerable<object[]> JsonData()
        {
                yield return new object[] 
                {
                    "{" + $"\"{Constants.CONFIG_BLOCK}\":" +
                        "{\"WebService\": {" +
                            "\"Key1\": 1," +
                            "\"Key2\": 2" +
                        "}," +
                        "\"DataBase\": {" +
                            "\"Key1\": \"Value1\"," +
                            "\"Key2\": \"Value2\"" +
                        "}}" + "}",
                    new List<ConfigFile>
                    {
                        new ConfigFile
                        {
                            Tag = "DataBase",
                            Parameters = new Dictionary<string, object>
                            {
                                { "Key1", "Value1" },
                                { "Key2", "Value2" }
                            }
                        },
                        new ConfigFile
                        {
                            Tag = "WebService",
                            Parameters = new Dictionary<string, object>
                            {
                                { "Key1", "1" },
                                { "Key2", "2" }
                            }
                        },
                    }
                };
                yield return new object[] 
                {
                    "{" + $"\"{Constants.CONFIG_BLOCK}\":" +
                        "{\"WebService\": {" +
                            "\"Key1\": \"Value1\"," +
                            "\"Key2\": 2" +
                        "}}" + "}",
                    new List<ConfigFile>
                    {
                        new ConfigFile
                        {
                            Tag = "WebService",
                            Parameters = new Dictionary<string, object>
                            {
                                { "Key1", "Value1" },
                                { "Key2", "2" }
                            }
                        },
                    }
                };
        }

        [Theory]
        [MemberData(nameof(JsonData))]
        public void CreateConfiguration_ValidJson_ReturnOptionModel(string json, IEnumerable<ConfigFile> config)
        {
            var testConfiguration = new ConfigurationBuilder()
                .AddJsonStream(json.ToStream())
                .Build();

            var configConfiguration = ConfigOptionsFactory.Create(testConfiguration);

            configConfiguration.Value.Count().Should().Be(config.Count());

            for(var i = 0; i < configConfiguration.Value.Count(); i++ )
            {
                configConfiguration.Value.ToList()[i].Should().BeEquivalentTo(config.ToList()[i]);
            }
        }

        public static IEnumerable<object[]> JsonDataWithAnotherSegment()
        {
            yield return new object[]
            {
                "{" +
                "\"AnotherSegment\": {}," + 
                $"\"{Constants.CONFIG_BLOCK}\":" +
                    "{\"WebService\": {" +
                        "\"Key1\": \"Value1\"," +
                        "\"Key2\": 2" +
                    "}}" + "}",
                new List<ConfigFile>
                {
                    new ConfigFile
                    {
                        Tag = "WebService",
                        Parameters = new Dictionary<string, object>
                        {
                            { "Key1", "Value1" },
                            { "Key2", "2" }
                        }
                    },
                }
            };
            yield return new object[]
            {
                "{" +
                "\"AnotherSegment\": {}," +
                $"\"{Constants.CONFIG_BLOCK}\":" +
                    "{\"WebService\": {" +
                        "\"Key1\": \"Value1\"," +
                        "\"Key2\": 2" +
                    "}}," 
                    + "\"SecondSegment\": {}"
                    + "}",
                new List<ConfigFile>
                {
                    new ConfigFile
                    {
                        Tag = "WebService",
                        Parameters = new Dictionary<string, object>
                        {
                            { "Key1", "Value1" },
                            { "Key2", "2" }
                        }
                    },
                }
            };
        }

        [Theory]
        [MemberData(nameof(JsonDataWithAnotherSegment))]
        public void CreateConfiguration_ValidJsonWithSegments_ReturnOptionModel(string json, IEnumerable<ConfigFile> config)
        {
            var testConfiguration = new ConfigurationBuilder()
                .AddJsonStream(json.ToStream())
                .Build();

            var configConfiguration = ConfigOptionsFactory.Create(testConfiguration);

            configConfiguration.Value.Count().Should().Be(config.Count());

            for (var i = 0; i < configConfiguration.Value.Count(); i++)
            {
                configConfiguration.Value.ToList()[i].Should().BeEquivalentTo(config.ToList()[i]);
            }
        }

        [Fact]
        public void CreateConfiguration_EmptyConfigBlock_ReturnEmptyList()
        {
            string json = "{\"AnotherSegment\": {}}";

            var testConfiguration = new ConfigurationBuilder()
                .AddJsonStream(json.ToStream())
                .Build();

            var configConfiguration = ConfigOptionsFactory.Create(testConfiguration);
            configConfiguration.Value.Count().Should().Be(0);
        }

        [Fact]
        public void CreateConfiguration_EmptyTag_ReturnEmptyList()
        {
            string json = "{" + $"\"{Constants.CONFIG_BLOCK}\"" + ": {}}";

            var testConfiguration = new ConfigurationBuilder()
                .AddJsonStream(json.ToStream())
                .Build();

            var configConfiguration = ConfigOptionsFactory.Create(testConfiguration);
            configConfiguration.Value.Count().Should().Be(0);
        }

        [Fact]
        public void CreateConfiguration_EmptyTagBlock_ReturnEmptyList()
        {
            string json =
                "{" + $"\"{Constants.CONFIG_BLOCK}\":" +
                        "{\"WebService\": {" +
                        "}}" + "}";

            var testConfiguration = new ConfigurationBuilder()
                .AddJsonStream(json.ToStream())
                .Build();

            var configConfiguration = ConfigOptionsFactory.Create(testConfiguration);
            configConfiguration.Value.Count().Should().Be(0);
        }
    }
}

using EvidentInstruction.Configuration.Models;
using EvidentInstruction.Configuration.Extension;
using EvidentInstruction.Controllers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using FluentAssertions;
using System.Collections.Concurrent;
using EvidentInstruction.Models;
using System;
using EvidentInstruction.Configuration.Exceptions;

namespace EvidentInstruction.Configuration.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConfigExtensionTests
    {
        private readonly VariableController variableContext;

        public ConfigExtensionTests()
        {
            variableContext = new VariableController();
        }

        public static IEnumerable<object[]> ConfigFileData()
        {
            yield return new object[]
            {
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
                },
                new List<string> { "WebService" },
                new Dictionary<string, Variable>
                {
                    { "Key1", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "Value1" } },
                    { "Key2", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "2" } }
                }
            };
            yield return new object[]
            {
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
                            { "Key3", "1" },
                            { "Key4", "2" }
                        }
                    },
                },
                new List<string> { "DataBase", "WebService" },
                new Dictionary<string, Variable>
                {
                    { "Key1", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "Value1" } },
                    { "Key2", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "Value2" } },
                    { "Key3", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "1" } },
                    { "Key4", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "2" } }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ConfigFileData))]
        public void AddConfig_ValidDataAndTags_ReturnVariables(List<ConfigFile> config, List<string> tags, Dictionary<string, Variable> variables)
        {
            // Act 
            var optionConfig = Options.Create(config);
            var variableDictionary = new ConcurrentDictionary<string, Variable>(variables);

            // Arrange
            var res = variableContext.AddConfig(optionConfig, tags);

            // Assert
            res.Variables.Should().BeEquivalentTo(variableDictionary);
        }

        public static IEnumerable<object[]> ConfigFileDataWithSingleTag()
        {
            yield return new object[]
            {
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
                            { "Key3", "1" },
                            { "Key4", "2" }
                        }
                    },
                },
                new List<string> { "WebService" },
                new Dictionary<string, Variable>
                {
                    { "Key3", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "1" } },
                    { "Key4", new Variable { Type = typeof(string), TypeOfAccess = EvidentInstruction.Infrastructures.TypeOfAccess.Global, Value = "2" } }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ConfigFileDataWithSingleTag))]
        public void AddConfig_ValidDataAndSingleTags_ReturnVariables(List<ConfigFile> config, List<string> tags, Dictionary<string, Variable> variables)
        {
            // Act 
            var optionConfig = Options.Create(config);
            var variableDictionary = new ConcurrentDictionary<string, Variable>(variables);

            // Arrange
            var res = variableContext.AddConfig(optionConfig, tags);

            // Assert
            res.Variables.Should().BeEquivalentTo(variableDictionary);
        }






        public static IEnumerable<object[]> ErrorConfigFileData()
        {
            yield return new object[]
            {
                new List<ConfigFile>
                {
                    new ConfigFile
                    {
                        Tag = "DataBase",
                        Parameters = new Dictionary<string, object>
                        {
                            { "Key1", "Value1" }

                        }
                    },
                    new ConfigFile
                    {
                        Tag = "WebService",
                        Parameters = new Dictionary<string, object>
                        {
                            { "Key1", "1" }
                        }
                    },
                },
                new List<string> { "WebService", "DataBase" }
            };
        }

        [Theory]
        [MemberData(nameof(ErrorConfigFileData))]
        public void AddConfig_InvalidData_ReturnException(List<ConfigFile> config, List<string> tags)
        {
            // Act 
            var optionConfig = Options.Create(config);

            // Arrange

            Action act = () => variableContext.AddConfig(optionConfig, tags);

            // Assert
            act
              .Should().Throw<ConfigException>()
              .WithMessage($"A value has already been written for the \"Key1\" key. Check the \"Key1\" key in the \"DataBase\" tag. Exception message is: \"An item with the same key has already been added. Key: Key1\"");
        }
    }
}

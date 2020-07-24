using EvidentInstruction.Controllers;
using EvidentInstruction.Config.Extension;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using EvidentInstruction.Models.Interfaces;
using Moq;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Config.Exceptions;

namespace EvidentInstruction.Config.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConfigExtensionTests
    {
        private readonly VariableController variableContext;

        public ConfigExtensionTests()
        {
            variableContext = new VariableController();           
        }

        private readonly string json =
             @"{
               'config': [
                           {
                              'tag': 'WebServiceAuth',
                              'parameters': {
                                'auth_login': 'login',
                                'auth_pass': 'awe',
                                'auth_token': 'Cddf32'
                              }
                            }                           
                          ]
              }";

        private readonly string jsonWithDublicates =
              @"{
                  'config': [
                    {
                      'tag': 'WebServiceAuth',
                      'parameters': {
                        'auth_login': 'login2'                        
                      }
                    },
                    {
                      'tag': 'Service',
                      'parameters': {
                        'auth_login': 'U_00AZC'                        
                      }
                    }
                  ]
                }";

        private readonly string incorrectjson =
           @"{
               'library': [
                           {
                            'books': 'Action'                              
                           }                                                      
                          ]
              }";

        [Theory]
        [InlineData(@"C:\\")]
        [InlineData("windir")]
        public void AddConfig_IncorrectPath_ReturnExeption(string content)
        {
            var mockPathProvider = new Mock<IPathProvider>();

            mockPathProvider.Setup(f => f.GetEnviromentVariable(It.IsAny<string>())).Returns(content);
            ConfigExtension.PathProvider = mockPathProvider.Object;

            Action act = () => ConfigExtension.AddConfig(variableContext);
            act
              .Should().Throw<FileIsExistException>()
              .WithMessage($"File \"{content}\" not found.");
        }

       [Fact]
        public void AddConfig_WithoutFiles_ReturnExeption()
        {          
            Action act = () => ConfigExtension.AddConfig(variableContext);
            act
              .Should().Throw<FileIsExistException>()
              .WithMessage($"File \"{string.Empty}\" not found.");
        }

        [Fact]
        public void AddConfig_DefaultFile_ReturnVariables()
        {
            var mockFile = new Mock<IFile>();           

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(json);           

            ConfigHelper.File = mockFile.Object;          

            var result = ConfigExtension.AddConfig(variableContext);
           
            result.Variables.Count.Should().Be(3);
        }

        [Fact]
        public void AddConfig_JsonWithDublicates_ReturnVariables()
        {
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(jsonWithDublicates);

            ConfigHelper.File = mockFile.Object;

            Action act = () => ConfigExtension.AddConfig(variableContext);
            act
              .Should().Throw<ConfigException>()
              .WithMessage($"Json Exeption. Json has 1 dublicates:"+"\nauth_login");
        }

        [Fact]
        public void AddConfig_IncorrectJson_ReturnVariables()
        {
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(incorrectjson);

            ConfigHelper.File = mockFile.Object;

            var result = ConfigExtension.AddConfig(variableContext);

            result.Variables.Count.Should().Be(0);
        }
    }
}

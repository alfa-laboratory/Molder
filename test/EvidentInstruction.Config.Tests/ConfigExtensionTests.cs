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
                              'tag': 'Browser',
                              'parameters': {
                                'broId': '1231',
                                'version': '2.3',
                                'name': 'Opera'
                              }
                            }                           
                          ]
              }";

        private readonly string jsonWithDublicates =
              @"{
                  'config': [
                    {
                      'tag': 'Browser',
                      'parameters': {
                        'broId': '1231'                        
                      }
                    },
                    {
                      'tag': 'Programm',
                      'parameters': {
                        'broId': '1231213123'                        
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
        /*[Fact]
        public void AddConfig_WithoutFiles_ReturnExeption()
        {
            Action act = () => ConfigExtension.AddConfig(variableContext);
            act
                .Should().Throw<FileIsExistException>()
                .WithMessage($"File is empty or not found.");
        }*/

       /* [Theory]
        [InlineData(@"C:\\")]        
        public void AddConfig_IncorrectPath_ReturnExeption(string content)
        {
            var mockPathProvider = new Mock<IPathProvider>();

            mockPathProvider.Setup(f => f.GetEnviromentVariable(It.IsAny<string>())).Returns(content);
            ConfigExtension.PathProvider = mockPathProvider.Object;

            Action act = () => ConfigExtension.AddConfig(variableContext);
            act
              .Should().Throw<FileIsExistException>()
              .WithMessage($"File is empty or not found.");
        }*/

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
              .Should().Throw<DublicateTagsException>()
              .WithMessage($"Json Exeption. Json has 1 dublicates:"+ "\nbroId");
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

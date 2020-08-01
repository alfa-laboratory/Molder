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
using EvidentInstruction.Exceptions;

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

        [Fact]
        public void AddConfig_DefaultFile_ReturnVariables()
        {
            var mockFile = new Mock<IFile>();           

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(json);           

            ConfigHelper.File = mockFile.Object;          

            var result = ConfigExtension.AddConfig(variableContext);
           
            result.Variables.Count.Should().Be(3);
        }


        [Theory]
        [InlineData("filepath")]
        public void AddConfig_NotEmptyExternalVariable_ReturnVariables(string path)
        {
            var mockFile = new Mock<IFile>();
            var mockPath = new Mock<IPathProvider>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(json);
            mockPath.Setup(p => p.GetEnviromentVariable(It.IsAny<string>())).Returns(path);
            mockPath.Setup(p => p.CutFullpath(It.IsAny<string>())).Returns((Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

            ConfigHelper.File = mockFile.Object;
            ConfigExtension.PathProvider = mockPath.Object;

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
              .WithMessage($"Json Exception in config file: Json has 1 dublicates:" + "\nbroId");
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GetContent_EmptyFileName_ReturnNoFileNameException(string filename)
        {
            // Act
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Throws(new NoFileNameException(Guid.NewGuid().ToString()));

            ConfigHelper.File = mockFile.Object;

            // Arrange 
            Action action = () => ConfigHelper.GetDictionary(filename, Guid.NewGuid().ToString());

            // Assert
            action.Should().Throw<NoFileNameException>()
                .WithMessage($"Config filename is empty");
        }

        [Theory]
        [InlineData("filename", "path")]
        public void GetContent_FileIsNotExist_ReturnFileExistException(string filename, string path)
        {
            // Act
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Throws(new FileExistException(Guid.NewGuid().ToString()));

            ConfigHelper.File = mockFile.Object;

            // Arrange 
            Action action = () => ConfigHelper.GetDictionary(filename, path);

            // Assert
            action.Should().Throw<FileExistException>()
                .And.Message.Contains($"Config file \"{filename}\" not found in path \"{path}\"");
        }


        [Theory]
        [InlineData("filename", "path")]
        public void AddConfig_FileIsNotExist_ReturnFileExistException(string filename, string path)
        {
            // Act
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Throws(new FileExistException(Guid.NewGuid().ToString()));

            ConfigHelper.File = mockFile.Object;

            // Arrange 
            Action action = () => ConfigExtension.AddConfig(variableContext);

            // Assert
            action.Should().Throw<FileExistException>()
                .And.Message.Contains($"Config file \"{filename}\" not found in path \"{path}\"");
        }

        [Fact]
        public void AddConfig_FileNameIsEmpty_ReturnNoFileNameException()
        {
            // Act
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Throws(new NoFileNameException(Guid.NewGuid().ToString()));

            ConfigHelper.File = mockFile.Object;

            // Arrange 
            Action action = () => ConfigExtension.AddConfig(variableContext);

            // Assert
            action.Should().Throw<NoFileNameException>()
                .WithMessage($"Config filename is empty");
        }

        [Fact]
        public void AddConfig_FileWithDublicates_ReturnDublicateTagsException()
        {
            // Act
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(jsonWithDublicates);

            ConfigHelper.File = mockFile.Object;

            // Arrange 
            Action action = () => ConfigExtension.AddConfig(variableContext);

            // Assert
            action.Should().Throw<DublicateTagsException>()
                .WithMessage($"Json Exception in config file: Json has 1 dublicates:broId");
        }
    }
}

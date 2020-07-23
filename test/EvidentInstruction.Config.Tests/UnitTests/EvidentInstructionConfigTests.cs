using EvidentInstruction.Controllers;
using EvidentInstruction.Config.Extension;
using EvidentInstruction.Config.Infrastructures;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using EvidentInstruction.Models.Interfaces;
using Moq;
using EvidentInstruction.Models;
using EvidentInstruction.Config.Helpers;

namespace EvidentInstruction.Config.Tests
{
    [ExcludeFromCodeCoverage]
    public class EvidentInstructionConfigTests
    {
       
        private readonly string json =
             @"{
               'config': [
                           {
                              'tag': 'WebServiceAuth',
                              'parameters': {
                                'auth_login': 'U_00ASC',
                                'auth_pass': 'awe',
                                'auth_token': 'Cddf32'
                              }
                            }                           
                          ]
              }";

        [Fact]
        public void IsExist_NULLPath_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.json",
                Path = null
            };
            var mockUserDir = new Mock<IDirectory>();
            var mockFileProvider = new Mock<IFileProvider>();
            var mockPathProvider = new Mock<IPathProvider>();

            mockUserDir.Setup(f => f.Get()).Returns(It.IsAny<string>());
            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);

            file.UserDirectory = mockUserDir.Object; 
            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;
            bool result = file.IsExist(file.Filename, file.Path);
            result.Should().BeTrue();
        }

        [Fact]
        public void GetContent_ReturnFileNotEmpty()
        {
            var file = new TextFile()
            {
                Filename = "test.json"                
            };           
            var mockFileProvider = new Mock<IFileProvider>();
         
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);            
            mockFileProvider.Setup(f => f.ReadAllText(It.IsAny<string>(), It.IsAny<string>())).Returns(json);
         
            file.FileProvider = mockFileProvider.Object;        
        
            string result = file.GetContent(file.Filename, file.Path);
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void IsDeserializeObjectNotEmpty_ReturnNotEmpty()
        {
            var file = new TextFile()
            {
                Filename = "test.json"
            };
            var mockFileProvider = new Mock<IFileProvider>();

            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.ReadAllText(It.IsAny<string>(), It.IsAny<string>())).Returns(json);

            file.FileProvider = mockFileProvider.Object;

            string content = file.GetContent(file.Filename, file.Path);

            var result = DeserializeHelper.DeserializeObject<Models.Config>(content);
            result.Should().NotBeNull();
        }

     

      




    }
}

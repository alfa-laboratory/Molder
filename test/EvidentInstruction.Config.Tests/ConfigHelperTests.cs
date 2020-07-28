using EvidentInstruction.Config.Exceptions;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Config.Tests.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class ConfigHelperTests
    {
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

        private readonly string jsonWithEmptyParameters =
             @"{
               'config': [
                           {
                              'tag': 'Browser',
                              'parameters': {                                
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
        public void AddParameters_CorrectJson_ReturnDictionaryAndList() 
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(json);

            var result = ConfigHelper.AddParameters(config);            

            result.Should().NotBeNull();
            result.Item1.Should().HaveCount(3);
            result.Item2.Should().HaveCount(0);
        }

        [Fact]
        public void AddParameters_InCorrectJson_ReturnEmptyDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(incorrectjson);

            var result = ConfigHelper.AddParameters(config);

            result.Item1.Should().HaveCount(0);
            result.Item2.Should().HaveCount(0);
        }

        [Fact]
        public void AddParameters_JsonWithEmptyParameters_ReturnEmptyDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(jsonWithEmptyParameters);

            var result = ConfigHelper.AddParameters(config);

            result.Should().NotBeNull();
            result.Item1.Should().HaveCount(0);
            result.Item2.Should().HaveCount(0);
        } 

        [Fact]
        public void GetTagsDictionary_JsonWithDublicates_ReturnExeption()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(jsonWithDublicates);  
            var (tags, dublicatesTags) = ConfigHelper.AddParameters(config);

            Action action = () => ConfigHelper.GetTagsDictionary(config);

            action.Should()
                .Throw<ConfigException>()
                .WithMessage($"Json has {dublicatesTags.Count} dublicates:" + System.Environment.NewLine + $"{Message.CreateMessage(dublicatesTags)}");
        }

        [Fact]
        public void GetTagsDictionary_JsonCorrect_ReturnDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(json);

            var result = ConfigHelper.GetTagsDictionary(config);

            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetTagsDictionary_JsonWithEmptyParametes_ReturnDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(jsonWithEmptyParameters);

            var result = ConfigHelper.GetTagsDictionary(config);            

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetTagsDictionary_IncorrectJson_ReturnDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(incorrectjson);

            var result = ConfigHelper.GetTagsDictionary(config);

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }        

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetDictionary_EmpryFile_ReturnDictWithoutElement(string content)
        {
            var mockFile = new Mock<IFile>();
            
            mockFile.Setup(f=>f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(content);
            ConfigHelper.File = mockFile.Object;

            var result = ConfigHelper.GetDictionary(new Guid().ToString(), new Guid().ToString());

            result.Should().HaveCount(0);  
        }

        [Fact]
        public void GetDictionary_CorrectJson_ReturnCorrectDict() 
        {
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(json);
            ConfigHelper.File = mockFile.Object;

            var result = ConfigHelper.GetDictionary(new Guid().ToString(), new Guid().ToString());

            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetDictionary_InCorrect_ReturnEmptyDict() 
        {
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(incorrectjson);
            ConfigHelper.File = mockFile.Object;

            var result = ConfigHelper.GetDictionary(new Guid().ToString(), new Guid().ToString());

            result.Should().HaveCount(0);
        }

        [Theory]        
        [InlineData("null")]        
        public void GetDictionary_IncorrectContent_ReturnEmptyDict( string content)
        {
            var mockFile = new Mock<IFile>();

            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(content);
            ConfigHelper.File = mockFile.Object;

            var result = ConfigHelper.GetDictionary(new Guid().ToString(), new Guid().ToString());

            result.Should().HaveCount(0);
        }
    }
}

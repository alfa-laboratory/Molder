using EvidentInstruction.Config.Exceptions;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Interfaces;
using FluentAssertions;
using Moq;
using System;

using Xunit;

namespace EvidentInstruction.Config.Tests.UnitTests
{
    public class ConfigHelperTests
    {
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
                        'auth_login': 'login'                        
                      }
                    },
                    {
                      'tag': 'Service',
                      'parameters': {
                        'auth_login': 'login'                        
                      }
                    }
                  ]
                }";

        private readonly string jsonWithEmptyParameters =
             @"{
               'config': [
                           {
                              'tag': 'WebServiceAuth',
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
            result.Item1.Should().HaveCountLessOrEqualTo(3);
            result.Item2.Should().HaveCountLessOrEqualTo(0);
        }

        [Fact]
        public void AddParameters_InCorrectJson_ReturnEmptyDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(incorrectjson);

            var result = ConfigHelper.AddParameters(config);

            result.Item1.Should().HaveCountLessOrEqualTo(0);
            result.Item2.Should().HaveCountLessOrEqualTo(0);
        }

        [Fact]
        public void AddParameters_JsonWithEmptyParameters_ReturnEmptyDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>(jsonWithEmptyParameters);

            var result = ConfigHelper.AddParameters(config);

            result.Should().NotBeNull();
            result.Item1.Should().HaveCountLessOrEqualTo(0);
            result.Item2.Should().HaveCountLessOrEqualTo(0);
        }        
/*
        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("null")] //проверка, что не может прийти null
        public void AddParameters_NULLValue_ReturnNULL(string config)
        {
            var jsonconfig = DeserializeHelper.DeserializeObject<Models.Config>(config);
            var result = ConfigHelper.AddParameters(jsonconfig);

            result.Should().NotBeNull();
            result.Item1.Should().HaveCountLessOrEqualTo(0);
            result.Item2.Should().HaveCountLessOrEqualTo(0);
        }
*/
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

        /*[Fact] //надо проверку на пустую строку, что она не придет
        public void GetTagsDictionary_EmptyJson_ReturnDictionaryAndList()
        {
            var config = DeserializeHelper.DeserializeObject<Models.Config>("");            

            var result = ConfigHelper.GetTagsDictionary(config);

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }*/        

        [Fact]
        public void GetDictionary_EmpryFile_ReturnFileNotEmpty()
        {
            var mockFile = new Mock<IFile>();
            mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(json);

            ConfigHelper.File = mockFile.Object;
            var res = ConfigHelper.GetDictionary(new Guid().ToString(), new Guid().ToString());

            res.Should().NotBeNull();
//
//
//            var mockUserDir = new Mock<IDirectory>()
//            {
//                CallBase = true
//            };
//            var mockFileProvider = new Mock<IFileProvider>()
//            {
//                CallBase = true
//            };
//            var mockPathProvider = new Mock<IPathProvider>()
//            {
//                CallBase = true
//            };
//           mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
//           mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
//           mockFile.Setup(f => f.GetContent(It.IsAny<string>(), It.IsAny<string>())).Returns(" ");
//
//                        file.UserDirectory = mockUserDir.Object;
//                        file.FileProvider = mockFileProvider.Object;
//                        file.PathProvider = mockPathProvider.Object;            
//
//            var result = ConfigHelper.GetDictionary(file.Filename, file.Path);           
        }

    }
}

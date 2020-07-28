using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Config.Exceptions;
using FluentAssertions;
using System;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Config.Tests.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class DeserializeHelperTests
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

        private readonly string incorrectjson =
           @"{
               'library': [
                           {
                            'books': 'Action'                              
                           }                                                      
                          ]
              }";

        [Theory]
        [InlineData("config")]
        public void DeserializeObject_InCorrectValue_ReturnExeption(string config)
        {
            Action action = () => DeserializeHelper.DeserializeObject<Models.Config>(config);
            action.Should().Throw<DeserializeExeption>().WithMessage($"Deserialize string \"{config}\" failed");
        }

        [Theory] 
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("null")]
        public void DeserializeObject_NULLValue_ReturnNULL(string config)
        {
            var result = DeserializeHelper.DeserializeObject<Models.Config>(config);
            result.Should().BeNull();
        }

        [Fact]
        public void DeserializeObject_CorrectJson_ReturnJson()
        {
            var result = DeserializeHelper.DeserializeObject<Models.Config>(json);
            result.Should().BeOfType(typeof(Models.Config));
            result.Parameters.Should().NotBeNull();
        }

        [Fact]
        public void DeserializeObject_InCorrectJson_ReturnEmptyParameter()
        {
            var result = DeserializeHelper.DeserializeObject<Models.Config>(incorrectjson);
            result.Should().BeOfType(typeof(Models.Config));
            result.Parameters.Should().BeNull();
        }
    }
}

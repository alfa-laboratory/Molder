using EvidentInstruction.Service.Helpers;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Service.Tests
{
    [ExcludeFromCodeCoverage]
    public class ServiceHelpersTests
    {        

        [Theory]
        [InlineData("{'Test': 'Test', " +
                    "'Val': 30 }")]
        [InlineData("{'test': " +
                    "{'val': 23, " +
                    "'participants': ['john', 'ann']}" +
                    "}")]
        public void GetObjectFromString_CorrectString_ReturnJObject(string str)
        {
            var result = ServiceHelpers.GetObjectFromString(str);

            result.GetType().Name.Should().Be("JObject");
        }

        [Theory]
        [InlineData("<p>Test</p>")]
        [InlineData("<b><i>Test</i></b>")]
        public void GetObjectFromString_CorrectString_ReturnXDoc(string str)
        {
            var result = ServiceHelpers.GetObjectFromString(str);
            result.GetType().Name.Should().Be("XDocument");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        public void GetObjectFromString_EmptyString_ReturnString(string str)
        {
            var result = ServiceHelpers.GetObjectFromString(str);
            result.GetType().Name.Should().Be("String");
        }

        [Fact]
        public void GetObjectFromString_Null_ReturnError()
        {
            Action action = () => ServiceHelpers.GetObjectFromString(null);

            action.Should()
                .Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("  ", "  ")]
        [InlineData("", "")]
        public void AddQueryInURL_EmptyString_ReturnString(string url, string query)
        {
            var result = ServiceHelpers.AddQueryInURL(url, query);
            result.Should().Contain("?");
        }

        [Theory]
        [InlineData("test", "?test/")]        
        public void AddQueryInURL_String_ReturnString(string url, string query)
        {
            var result = ServiceHelpers.AddQueryInURL(url, query);
            result.Should().Be("test?test/");
        }

        [Fact]
        public void AddQueryInURL_Null_ReturnError()
        {           
            Action action = () => ServiceHelpers.AddQueryInURL(null, null);

            action.Should()
                .Throw<NullReferenceException>();
        }

        [Fact]
        public void GetStringContent_String_ReturnStringContent()
        {
            string str = "test";
            var type = ServiceHelpers.GetObjectFromString(str);

            var result = ServiceHelpers.GetStringContent(type, str);

            result.Headers.ContentType.ToString().Should().Be("text/plain; charset=utf-8");
            result.Headers.ContentLength.Should().Be(4);            
        }

        [Fact]
        public void GetStringContent_JObject_ReturnStringContent()
        {
            string str = "{'Test': 'Test'}";
            var type = ServiceHelpers.GetObjectFromString(str);

            var result = ServiceHelpers.GetStringContent(type, str);

            result.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");            
        }

        [Fact]
        public void GetStringContent_XDoc_ReturnStringContent()
        {
            string str = "<p>Test</p>";
            var type = ServiceHelpers.GetObjectFromString(str);

            var result = ServiceHelpers.GetStringContent(type, str);

            result.Headers.ContentType.ToString().Should().Be("text/xml; charset=utf-8");
        }

        [Theory]
        [InlineData("test", null)]
        [InlineData(null, null)]
        public void GetStringContent_Null_ReturnError(object type, string str)
        {
            Action action = () => ServiceHelpers.GetStringContent(type, str);

            action.Should()
                .Throw<ArgumentNullException>();
        }
    }
}

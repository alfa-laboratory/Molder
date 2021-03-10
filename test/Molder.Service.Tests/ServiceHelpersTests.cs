using Molder.Service.Helpers;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using System.Collections.Generic;

namespace Molder.Service.Tests
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
            var result = str.GetObject();

            result.GetType().Name.Should().Be("JObject");
        }

        [Theory]
        [InlineData("<p>Test</p>")]
        [InlineData("<b><i>Test</i></b>")]
        public void GetObjectFromString_CorrectString_ReturnXDoc(string str)
        {
            var result = str.GetObject();
            result.GetType().Name.Should().Be("XDocument");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        public void GetObjectFromString_EmptyString_ReturnString(string str)
        {
            var result = str.GetObject(); ;
            result.GetType().Name.Should().Be("String");
        }

        [Fact]
        public void GetObjectFromString_Null_ReturnError()
        {
            string str = null;
            Action action = () => str.GetObject();

            action.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddQueryInURL_NullUrl_ReturnError()
        {
            string url = null;
            Action action = () => url.AddQueryInURL(new Dictionary<string, string>());

            action.Should()
                .Throw<Exception>().WithMessage("Expected url not to be <null> because web service address not specified.");
        }

        [Fact]
        public void AddQueryInURL_NullQuery_ReturnError()
        {
            string url = string.Empty;
            Action action = () => url.AddQueryInURL(null);

            action.Should()
                .Throw<Exception>().WithMessage("Expected query not to be <null> because web service query not specified.");
        }

        [Fact]
        public void GetStringContent_String_ReturnStringContent()
        {
            string str = "test";
            var type = str.GetObject();
            var result = type.GetHttpContent(str);

            result.Headers.ContentType.ToString().Should().Be("text/plain; charset=utf-8");
            result.Headers.ContentLength.Should().Be(4);            
        }

        [Fact]
        public void GetStringContent_JObject_ReturnStringContent()
        {
            string str = "{'Test': 'Test'}";
            var type = str.GetObject();
            var result = type.GetHttpContent(str);

            result.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");            
        }

        [Fact]
        public void GetStringContent_XDoc_ReturnStringContent()
        {
            string str = "<p>Test</p>";
            var type = str.GetObject();
            var result = type.GetHttpContent(str);

            result.Headers.ContentType.ToString().Should().Be("text/xml; charset=utf-8");
        }

        [Theory]
        [InlineData("test", null)]
        [InlineData(null, null)]
        public void GetStringContent_Null_ReturnError(object type, string str)
        {
            Action action = () => type.GetHttpContent(str);

            action.Should()
                .Throw<ArgumentNullException>();
        }
    }
}

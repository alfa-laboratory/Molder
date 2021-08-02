using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Molder.Web.Helpers;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using Xunit;

namespace Molder.Web.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class MessageTests
    {
        private static readonly string chromeOptions = $@"
        {{
        ""browserName"": ""chrome"",
              ""goog:chromeOptions"": {{
                  ""args"": [
                  ""--start-maximized"",
                    ""--ignore-certificate-errors""
                      ]
              }}
         }}".Replace("\n", "").Replace("\r", "").RemoveWhitespace();
        private static readonly string operaOptions = $@"
        {{
        ""browserName"": ""opera"",
              ""operaOptions"": {{
                  ""args"": [
                  ""--start-maximized"",
                    ""--ignore-certificate-errors""
                      ]
              }}
         }}".Replace("\n", "").Replace("\r", "").RemoveWhitespace();
        private static readonly string firefoxOptions = $@"
        {{
        ""browserName"": ""firefox"",
              ""moz:firefoxOptions"": {{
                  ""args"": [
                  ""--start-maximized"",
                    ""--ignore-certificate-errors""
                      ]
              }}
         }}".Replace("\n", "").Replace("\r", "").RemoveWhitespace();
        private static readonly string edgeOptions = $@"
        {{
            ""browserName"": ""MicrosoftEdge"",
            ""a"":""1""
        }}".Replace("\n", "").Replace("\r", "").RemoveWhitespace();
        
        [Fact]
        public void CreateMessage_ChromeOptions_ReturnString()
        {
            var options = new ChromeOptions();
            options.AddArguments(new List<string> {"--start-maximized", "--ignore-certificate-errors"});
            var str = options.CreateMessage().Replace("\n", "").Replace("\r", "").RemoveWhitespace();
            str.Should().Be(chromeOptions);
        }
        
        [Fact]
        public void CreateMessage_OperaOptions_ReturnString()
        {
            var options = new OperaOptions();
            options.AddArguments(new List<string> {"--start-maximized", "--ignore-certificate-errors"});
            var str = options.CreateMessage().Replace("\n", "").Replace("\r", "").RemoveWhitespace();
            str.Should().Be(operaOptions);
        }
        
        [Fact]
        public void CreateMessage_EdgeOptions_ReturnString()
        {
            var options = new EdgeOptions();
            options.AddAdditionalCapability("a", "1");
            var str = options.CreateMessage().Replace("\n", "").Replace("\r", "").RemoveWhitespace();
            str.Should().Be(edgeOptions);
        }
        
        [Fact]
        public void CreateMessage_FirefoxOptions_ReturnString()
        {
            var options = new FirefoxOptions();
            options.AddArguments(new List<string> {"--start-maximized", "--ignore-certificate-errors"});
            var str = options.CreateMessage().Replace("\n", "").Replace("\r", "").RemoveWhitespace();
            str.Should().Be(firefoxOptions);
        }
        
        [Fact]
        public void CreateMessage_nullOptions_ReturnNull()
        {
            var str = Message.CreateMessage(null);
            str.Should().Be(string.Empty);
        }
    }
}
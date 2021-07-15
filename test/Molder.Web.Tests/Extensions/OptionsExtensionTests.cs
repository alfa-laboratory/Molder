using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Molder.Web.Extensions;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Molder.Web.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class OptionsExtensionTests
    {
        [Fact]
        public void AddCapabilities_AddDictionary_ReturnOptionsWithCapability()
        {
            var expectedOptions = new ChromeOptions();
            expectedOptions.AddAdditionalCapability("a", "1");
            expectedOptions.AddAdditionalCapability("b", "2");

            var dict = new Dictionary<string, string>()
            {
                {"a", "1"},
                {"b", "2"}
            };

            var options = new ChromeOptions();

            options.AddCapabilities(dict);
            
            options.Should().BeEquivalentTo(expectedOptions);
        }
        
        [Fact]
        public void AddCapabilities_AddNullDictionary_ReturnOptionsWithoutCapability()
        {
            var expectedOptions = new ChromeOptions();
            
            var options = new ChromeOptions();

            options.AddCapabilities(null);
            
            options.Should().BeEquivalentTo(expectedOptions);
        }
        
        [Fact]
        public void AddCapabilities_AddEmptyDictionary_ReturnOptionsWithoutCapability()
        {
            var expectedOptions = new ChromeOptions();
            
            var options = new ChromeOptions();

            options.AddCapabilities(new Dictionary<string, string>());
            
            options.Should().BeEquivalentTo(expectedOptions);
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Molder.Web.Helpers;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Settings;
using Xunit;

namespace Molder.Web.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class ConfigOptionsFactoryTests
    {
        private IConfiguration _configurationBuilder;
        public ConfigOptionsFactoryTests()
        {
            var _configurationDictionary = new Dictionary<string, string>
            {
                {"Molder.Web:Settings:Browser", "Chrome"},
                {"Molder.Web:Settings:Options:0", "--start-maximized"},
                {"Molder.Web:Settings:Options:1", "--ignore-certificate-errors"},
                {"Molder.Web:Settings:Options:2", "--disable-cache"},
                {"Molder.Web:Settings:Timeout", "60"},
                {"Molder.Web:Settings:BinaryLocation", "C:\abc\abc\abc.exe"},
                {"Molder.Web:Settings:Capabilities:a", "1"},
                {"Molder.Web:Settings:Capabilities:a1", "11"},
                {"Molder.Web:Settings:IsRemote", "True"},
                {"Molder.Web:Settings:Remote:url", "url.ru"},
                {"Molder.Web:Settings:Remote:version", "67.0"},
                {"Molder.Web:Settings:Remote:project", "Molder"},
                {"Molder.Web:Settings:Remote:platform", "win"}
            };
            
            _configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(_configurationDictionary)
                .Build();
        }

        [Fact]
        public void CreateSettings_WithValidConfiguration_ReturnSettings()
        {
            // Act
            var settings = new Settings()
            {
                Browser = BrowserType.CHROME,
                Options = new List<string>() {"--start-maximized", "--ignore-certificate-errors", "--disable-cache"},
                Timeout = 60,
                BinaryLocation = "C:\abc\abc\abc.exe",
                Capabilities = new Dictionary<string, string>()
                {
                    {"a", "1"},
                    {"a1", "11"}
                },
                IsRemote = true,
                Remote = new Remote()
                {
                    Platform = "win",
                    Project = "Molder",
                    Url = "url.ru",
                    Version = "67.0"
                }
            };
            // Arrange
            var optionSettings = ConfigOptionsFactory.Create(_configurationBuilder);
            // Assert
            optionSettings.Value.Should().BeEquivalentTo(settings);
        }
        
        [Fact]
        public void CreateSettings_WithIncorrectConfiguration_ReturnNull()
        {
            var configurationDictionary = new Dictionary<string, string>
            {
                {"Molder:Browser", "Chrome"}
            };
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationDictionary)
                .Build();
            
            var optionSettings = ConfigOptionsFactory.Create(configurationBuilder);
            optionSettings.Value.Should().BeNull();
        }
    }
}
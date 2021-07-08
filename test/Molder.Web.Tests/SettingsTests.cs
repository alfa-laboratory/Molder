using System.Collections.Generic;
using FluentAssertions;
using Molder.Web.Models.Settings;
using Xunit;

namespace Molder.Web.Tests
{
    public class SettingsTests
    {
        [Fact]
        public void IsRemoteRun_RemoteRun_ReturnTrue()
        {
            // Act
            var settings = new Settings()
            {
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
            // Assert
            settings.IsRemoteRun().Should().BeTrue();
        }
        
        [Fact]
        public void IsRemoteRun_IsRemoteFalse_ReturnFalse()
        {
            // Act
            var settings = new Settings()
            {
                IsRemote = false,
                Remote = new Remote()
                {
                    Platform = "win",
                    Project = "Molder",
                    Url = "url.ru",
                    Version = "67.0"
                }
            };
            
            // Arrange
            // Assert
            settings.IsRemoteRun().Should().BeFalse();
        }
        
        [Fact]
        public void IsRemoteRun_RemoteNull_ReturnFalse()
        {
            // Act
            var settings = new Settings()
            {
                IsRemote = true,
                Remote = null
            };
            
            // Arrange
            // Assert
            settings.IsRemoteRun().Should().BeFalse();
        }
        
        [Fact]
        public void IsOptions_OptionsNotNull_ReturnTrue()
        {
            // Act
            var settings = new Settings()
            {
                Options = new List<string>()
            };
            
            // Arrange
            // Assert
            settings.IsOptions().Should().BeTrue();
        }
        
        [Fact]
        public void IsOptions_OptionsNull_ReturnFalse()
        {
            // Act
            var settings = new Settings()
            {
                Options = null
            };
            
            // Arrange
            // Assert
            settings.IsOptions().Should().BeFalse();
        }
        
        [Fact]
        public void IsBinaryPath_BinaryPathIsNotNull_ReturnTrue()
        {
            // Act
            var settings = new Settings()
            {
                BinaryLocation = "test"
            };
            
            // Arrange
            // Assert
            settings.IsBinaryPath().Should().BeTrue();
        }
        
        [Fact]
        public void IsBinaryPath_BinaryPathIsNull_ReturnFalse()
        {
            // Act
            var settings = new Settings()
            {
                BinaryLocation = null
            };
            
            // Arrange
            // Assert
            settings.IsOptions().Should().BeFalse();
        }

        [Fact]
        public void CheckRemoteRun_RemoteRunIsGood_ReturnTrue()
        {
            // Act
            var settings = new Settings()
            {
                IsRemote = true,
                Remote = new Remote()
                {
                    Url = "url"
                }
            };
            
            // Arrange
            // Assert
            settings.CheckRemoteRun().Should().BeTrue();
        }
        
        [Fact]
        public void CheckRemoteRun_IsRemoteFalse_ReturnFalse()
        {
            // Act
            var settings = new Settings()
            {
                IsRemote = false,
                Remote = new Remote()
                {
                    Url = "url"
                }
            };
            
            // Arrange
            // Assert
            settings.CheckRemoteRun().Should().BeFalse();
        }
        
        [Fact]
        public void CheckRemoteRun_IsRemoteUrlNull_ReturnFalse()
        {
            // Act
            var settings = new Settings()
            {
                IsRemote = true,
                Remote = new Remote()
                {
                    Url = null
                }
            };
            
            // Arrange
            // Assert
            settings.CheckRemoteRun().Should().BeFalse();
        }

        [Fact]
        public void CheckCapability_CapabilityNotNull_ReturnTrue()
        {
            // Act
            var settings = new Settings()
            {
                Capabilities = new Dictionary<string, string>()
            };
            
            // Arrange
            // Assert
            settings.CheckCapability().Should().BeTrue();
        }
        
        [Fact]
        public void CheckCapability_CapabilityIsNull_ReturnFalse()
        {
            // Act
            var settings = new Settings()
            {
                Capabilities = null
            };
            
            // Arrange
            // Assert
            settings.CheckCapability().Should().BeFalse();
        }
    }
}
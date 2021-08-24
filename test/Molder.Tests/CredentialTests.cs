using FluentAssertions;
using Molder.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using Molder.Infrastructures;
using Xunit;

namespace Molder.Tests
{
    /// <summary>
    /// Тесты проверки credentials.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CredentialTests
    {
        private const string _tmp = "test";
        private string _url = string.Empty;
        private string _password = string.Empty;

        public CredentialTests()
        {
            _url = "http://" + _tmp;
            _password = _tmp;
        }

        [Fact]
        public void CreateCredential_CorrectParams_ReturnCredential()
        {
            var credential = Credentials.CreateCredential(_url, AuthType.Anonymous, _tmp, _tmp, _password);
            var network = credential.GetCredential(new Uri(_url), AuthType.Anonymous.ToString());
            network.Domain.Should().Be(_tmp);
            network.UserName.Should().Be(_tmp);
            network.Password.Should().Be(_tmp);
            network.SecurePassword.Should().NotBeNull();
        }

    }
}

using FluentAssertions;
using EvidentInstruction.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Tests
{
    /// <summary>
    /// Тесты проверки credentials.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CredentialTests
    {
        private readonly string _tmp = "test";
        private string _url = string.Empty;
        private string _password = string.Empty;

        public CredentialTests()
        {
            _url = "http://" + _tmp;
            _password = Encryptor.Encrypt(_tmp);
        }

        [Fact]
        public void CreateCredential_CorrectParams_ReturnCredential()
        {
            var credential = Credentials.CreateCredential(_url, Infrastructure.AuthType.Anonymous, _tmp, _tmp, _password);
            var network = credential.GetCredential(new Uri(_url), Infrastructure.AuthType.Anonymous.ToString());
            network.Domain.Should().Be(_tmp);
            network.UserName.Should().Be(_tmp);
            network.Password.Should().Be(_tmp);
            network.SecurePassword.Should().NotBeNull();
        }

    }
}

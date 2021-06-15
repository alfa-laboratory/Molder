using Molder.Helpers;
using Molder.Tests.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;

namespace Molder.Tests
{
    /// <summary>
    /// Тесты проверки валидации модели.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ValidateTests
    {
        private const string Xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><addresses xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation='test.xsd'><address><name>Joe Tester</name><street>Baker street</street><house>5</house></address></addresses>";

        [Fact]
        public void ValidateModel_CorrectModel_ReturnTrue()
        {
            // Act
            var urlModel = new UrlModel()
            {
                Url = new Guid().ToString()
            };
            // Arrange

            var (isValid, results) = Validate.ValidateModel(urlModel);
            // Assert

            isValid.Should().BeTrue();
            results.Any().Should().BeFalse();
        }

        [Fact]
        public void ValidateModel_CorrectModel_ReturnFalse()
        {
            // Act
            var urlModel = new UrlModel();
            // Arrange

            var (isValid, results) = Validate.ValidateModel(urlModel);
            // Assert

            isValid.Should().BeFalse();
            results.Any(v => v.ErrorMessage == "Url is required" && v.MemberNames.Contains("Url")).Should().BeTrue();
        }

        [Fact]
        public void TryParseToXml_ReturnFalse()
        {
            // Arrange

            var isValid = Validate.TryParseToXml(Xml.Substring(15));
            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void TryParseToXml_ReturnTrue()
        {
            // Arrange

            var isValid = Validate.TryParseToXml(Xml);
            // Assert
            isValid.Should().BeTrue();
        }
    }
}

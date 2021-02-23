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
    }
}

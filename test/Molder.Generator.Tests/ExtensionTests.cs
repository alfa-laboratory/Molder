using Molder.Generator.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class ExtensionTests
    {
        [Theory]
        [InlineData(1), InlineData(5), InlineData(10)]
        public void Check_NotThrow(int length)
        {
            Action act = () => length.Check();
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(0), InlineData(-1), InlineData(-10)]
        public void Check_ThrowException(int length)
        {
            Action act = () => length.Check();
            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"Expected length to be greater than 0 because the length must be positive, but found {length}.");
        }

        [Theory]
        [InlineData(10, "A", ""), InlineData(10, "", "A"), InlineData(10, "A", "A")]
        public void CheckPostfixAndPrefix_NotThrow(int length, string prefix, string postfix)
        {
            Action act = () => length.Check(prefix, postfix);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(1, "A", ""), InlineData(1, "", "A"), InlineData(1, "A", "A")]
        public void CheckPostfixAndPrefix_ThrowException(int length, string prefix, string postfix)
        {
            Action act = () => length.Check(prefix, postfix);
            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"*postfix and/or prefix are longer than the string itself*");
        }

        [Theory]
        [InlineData(1), InlineData(5), InlineData(10)]
        public void Limit_NotThrow(int length)
        {
            Action act = () => CheckExtension.Limit(length);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(0), InlineData(-1), InlineData(-10)]
        public void Limit_ThrowException(int length)
        {
            Action act = () => CheckExtension.Limit(length);
            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage("*the decimal place limit cannot be negative*");
        }

        [Theory]
        [InlineData(1, 2), InlineData(5, 10), InlineData(10, 20)]
        public void GreaterInt_NotThrow(int min, int max)
        {
            Action act = () => max.BeGreaterThan(min);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(2, 1), InlineData(10, 1), InlineData(100, 1)]
        public void GreaterInt_ThrowException(int min, int max)
        {
            Action act = () => max.BeGreaterThan(min);
            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage("*the maximum value cannot be less than the minimum*");
        }

        [Theory]
        [InlineData(1, 2), InlineData(5, 10), InlineData(10, 20)]
        public void GreaterDouble_NotThrow(double min, double max)
        {
            Action act = () => max.BeGreaterThan(min);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(2, 1), InlineData(10, 1), InlineData(100, 1)]
        public void GreaterDouble_ThrowException(double min, double max)
        {
            Action act = () => max.BeGreaterThan(min);
            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage("*the maximum value cannot be less than the minimum*");
        }

        [Theory]
        [InlineData(1, 2, 3), InlineData(5, 10, 15), InlineData(10, 20, 30)]
        public void Numbers_NotThrow(int length, int min, int max)
        {
            Action act = () => length.Numbers(min, max);
            act.Should().NotThrow();
        }

        public static IEnumerable<object[]> IncorrectNumbersMinMax()
        {
            yield return new object[] { 0, 10, 1, "*the length must be positive*" };
            yield return new object[] { -1, 100, 2, "*the length must be positive*" };
            yield return new object[] { 1, 10, 3, "*the maximum value cannot be less than the minimum*" };
            yield return new object[] { 1, 1, 0, "*the maximum value cannot be less than the minimum*" };
            yield return new object[] { 1, 10, 9, "*the maximum value cannot be less than the minimum*" };
        }

        [Theory]
        [MemberData(nameof(IncorrectNumbersMinMax))]
        public void Numbers_IncorrectMinMaxLimit_ReturnException(int length, int min, int max, string message)
        {
            // Act 
            // Arrange

            Action act = () => length.Numbers(min, max);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage(message);
        }

        [Theory]
        [InlineData(1, 2, 3, 4), InlineData(5, 10, 15, 20), InlineData(10, 20, 30, 40)]
        public void Doubles_NotThrow(int length, double min, double max, int limit)
        {
            Action act = () => length.Doubles(min, max, limit);
            act.Should().NotThrow();
        }

        public static IEnumerable<object[]> IncorrectDoublesMinMax()
        {
            yield return new object[] { 0, 10, 1, 1, "*the length must be positive*" };
            yield return new object[] { -1, 100, 2, 1, "*the length must be positive*" };
            yield return new object[] { 1, 10, 3, 1, "*the maximum value cannot be less than the minimum*" };
            yield return new object[] { 1, 1, 0, 1, "*the maximum value cannot be less than the minimum*" };
            yield return new object[] { 1, 10, 9, 1, "*the maximum value cannot be less than the minimum*" };
            yield return new object[] { 1, 1, 2, 0, "*the decimal place limit cannot be negative*" };
            yield return new object[] { 1, 1, 2, -1, "*the decimal place limit cannot be negative*" };
        }

        [Theory]
        [MemberData(nameof(IncorrectDoublesMinMax))]
        public void Doubles_IncorrectMinMaxLimit_ReturnException(int length, double min, double max, int limit, string message)
        {
            // Act 
            // Arrange

            Action act = () => length.Doubles(min, max, limit);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage(message);
        }
    }
}
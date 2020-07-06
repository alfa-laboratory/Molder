using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.DateTimeHelpers;
using Xunit;

namespace EvidentInstruction.Tests
{
    /// <summary>
    /// Тесты проверки генераторов тестовых данных.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GeneratorTests
    {
        private readonly string _prefix = "test";

        public static IEnumerable<object[]> RandomDateTime()
        {
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2002, 1, 1) };
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2010, 1, 1) };
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2100, 1, 1) };
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10)]
        public void GetRandomString_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = Generator.GetRandomString(expectedLength, string.Empty);
            randomString.Should().HaveLength(expectedLength);
        }

        [Theory]
        [InlineData(-1), InlineData(0)]
        public void GetRandomStringWithPrefix_ZeroOrNegativeParams_ReturnException(int expectedLength)
        {
            Action act = () => Generator.GetRandomString(expectedLength, string.Empty); ;
            act
              .Should().Throw<GeneratorException>()
              .WithMessage("Размер генерируемой строки отрицательный или равен нулю");
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10)]
        public void GetRandomStringWithPrefix_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = Generator.GetRandomString(expectedLength, _prefix);
            randomString.Should().HaveLength(expectedLength + _prefix.Length);
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10)]
        public void GetRandomChars_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = Generator.GetRandomChars(expectedLength, string.Empty);
            randomString.Should().HaveLength(expectedLength);
        }

        [Theory]
        [InlineData(-1), InlineData(0)]
        public void GetRandomCharsWithPrefix_ZeroOrNegativeParams_ReturnException(int expectedLength)
        {
            Action act = () => Generator.GetRandomChars(expectedLength, string.Empty); ;
            act
              .Should().Throw<GeneratorException>()
              .WithMessage("Размер генерируемой строки отрицательный или равен нулю");
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10)]
        public void GetRandomCharsWithPrefix_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = Generator.GetRandomChars(expectedLength, _prefix);
            randomString.Should().HaveLength(expectedLength + _prefix.Length);
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10)]
        public void GetRandomNumbers_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = Generator.GetRandomNumbers(expectedLength, string.Empty);
            randomString.Should().HaveLength(expectedLength);
        }

        [Theory]
        [InlineData(-1), InlineData(0)]
        public void GetRandomNumbersWithPrefix_ZeroOrNegativeParams_ReturnException(int expectedLength)
        {
            Action act = () => Generator.GetRandomNumbers(expectedLength, string.Empty); ;
            act
              .Should().Throw<GeneratorException>()
              .WithMessage("Размер генерируемой строки отрицательный или равен нулю");
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10)]
        public void GetRandomNumbersWithPrefix_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = Generator.GetRandomNumbers(expectedLength, _prefix);
            randomString.Should().HaveLength(expectedLength + _prefix.Length);
        }

        [Fact]
        public void TryValidGuid()
        {
            var str = Generator.GetGuid();
            bool isValid = Guid.TryParse(str, out Guid guid);
            isValid.Should().BeTrue();

        }

        [Theory]
        [InlineData("+7XXXXXXXXXX"), InlineData("+7XXX-XXX-XX-XX"), InlineData("+7 XXX XXX XX XX"), InlineData("+7(XXX)XXX-XX-XX")]
        public void GetRandomPhone_CorrectParams_ReturnRandomPhone(string expectedMask)
        {
            var randomPhone = Generator.GetRandomPhone(expectedMask);
            randomPhone.Should().HaveLength(randomPhone.Length);
        }

        [Theory]
        [InlineData("+7XXXXXXXXXXX"), InlineData("+7 XXX XXX XX XX XX XX")]
        public void GetRandomPhone_OverTenParams_ReturnException(string expectedMask)
        {
            Action act = () => Generator.GetRandomPhone(expectedMask);
            act
              .Should().Throw<FormatException>()
              .WithMessage("Phone number can have up to 10 digits");
        }

        [Theory]
        [InlineData("+7X"), InlineData("+7")]
        public void GetRandomPhone_LessTenParams_ReturnException(string expectedMask)
        {
            Action act = () => Generator.GetRandomPhone(expectedMask);
            act
              .Should().Throw<FormatException>()
              .WithMessage("Номер телефона должен содержать не менее 10 цифр.");
        }

        [Theory]
        [InlineData(1,1,1990), InlineData(15, 6, 1990), InlineData(31, 12, 1990)]
        public void GetDate_CorrectParams_ReturnDate(int day, int month, int year)
        {
            var expDt = new DateTime(year, month, day);
            var dt = Generator.GetDate(day, month, year);

            dt.Should().Be(expDt);
        }

        [Theory]
        [InlineData(0, 1, 1990), InlineData(40, 6, 1990), InlineData(31, 13, 1990)]
        public void GetDate_InCorrectParams_ReturnNull(int day, int month, int year)
        {
            var dt = Generator.GetDate(day, month, year);

            dt.Should().BeNull();
        }

        [Theory]
        [InlineData(0, 0, 0, 1), InlineData(0, 0, 1, 0), InlineData(0, 1, 0, 0), InlineData(1, 0, 0, 0),
            InlineData(12, 30, 30, 30), InlineData(23, 59, 59, 59)]
        public void GetTime_CorrectParams_ReturnTime(int hours, int minutes, int seconds, int milliseconds)
        {
            var now = DateTime.Now;
            var expTm = new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds, milliseconds);

            var tm = Generator.GetTime(hours, minutes, seconds, milliseconds);

            tm.Should().Be(expTm);
        }

        [Theory]
        [InlineData(0, 0, 0, -1), InlineData(0, 0, -1, 0), InlineData(0, -1, 0, 0), InlineData(-1, 0, 0, 0),
            InlineData(24, 60, 60, 60), InlineData(100, 100, 100, 100)]
        public void GetTime_InCorrectParams_ReturnNull(int hours, int minutes, int seconds, int milliseconds)
        {
            var tm = Generator.GetTime(hours, minutes, seconds, milliseconds);

            tm.Should().BeNull();
        }

        [Fact]
        public void GetCurrentDateTime_ReturnCurrentDateTime()
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1, 1, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);
            Generator.DateTimeHelper = mockDateTimeHelper.Object;
            var dt = Generator.GetCurrentDateTime();
            dt.Should().Be(fakeDate);
        }

        [Theory]
        [InlineData(1, 1, 1990, 0, 0, 0, 1), InlineData(1, 1, 1990, 0, 0, 1, 0), InlineData(1, 1, 1990, 0, 1, 0, 0), InlineData(1, 1, 1990, 1, 0, 0, 0),
            InlineData(15, 6, 1990, 12, 30, 30, 30), InlineData(31, 12, 1990, 23, 59, 59, 59)]
        public void GetDateTime_CorrectParams_ReturnDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds)
        {
            var expDt = new DateTime(year, month, day, hours, minutes, seconds, milliseconds);

            var dt = Generator.GetDateTime(day, month, year, hours, minutes, seconds, milliseconds);

            dt.Should().Be(expDt);
        }

        [Theory]
        [InlineData(1, 1, 1990, 0, 0, 0, -1), InlineData(1, 1, 1990, 0, 0, -1, 0), InlineData(1, 1, 1990, 0, -1, 0, 0), InlineData(1, 1, 1990, -1, 0, 0, 0),
            InlineData(0, 0, -1, 0, 0, 0, 0), InlineData(0, -1, 0, 0, 0, 0, 0), InlineData(-1, 0, 0, 0, 0, 0, 0),
            InlineData(40, 6, 1990, 24, 60, 60, 60), InlineData(31, 13, 1990, 100, 100, 100, 100)]
        public void GetDateTime_InCorrectParams_ReturnNull(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds)
        {
            var dt = Generator.GetDateTime(day, month, year, hours, minutes, seconds, milliseconds);

            dt.Should().BeNull();
        }

        [Theory]
        [InlineData(1, 2, 1), InlineData(15, 16, 1), InlineData(31, 1, 2)]
        public void GetOtherFutureDate_CorrectDay_ReturnFutureDate(int day, int nDay, int nMouth)
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            Generator.DateTimeHelper = mockDateTimeHelper.Object;

            var dt = Generator.GetOtherDate(day, 0, 0).Value;

            dt.Year.Should().Be(2000);
            dt.Month.Should().Be(nMouth);
            dt.Day.Should().Be(nDay);
        }

        [Theory]
        [InlineData(1, 2, 2000), InlineData(6, 7, 2000), InlineData(12, 1, 2001)]
        public void GetOtherFutureDate_CorrectMonth_ReturnFutureDate(int month, int nMonth, int nYear)
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            Generator.DateTimeHelper = mockDateTimeHelper.Object;

            var dt = Generator.GetOtherDate(0, month, 0).Value;

            dt.Year.Should().Be(nYear);
            dt.Month.Should().Be(nMonth);
            dt.Day.Should().Be(1);
        }

        [Theory]
        [InlineData(1, 2001), InlineData(10, 2010), InlineData(100, 2100)]
        public void GetOtherFutureDate_CorrectYear_ReturnFutureDate(int year, int nYear)
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            Generator.DateTimeHelper = mockDateTimeHelper.Object;

            var dt = Generator.GetOtherDate(0, 0, year).Value;

            dt.Year.Should().Be(nYear);
            dt.Month.Should().Be(1);
            dt.Day.Should().Be(1);
        }

        [Theory]
        [InlineData(0, 0, -1), InlineData(0, -1, 0), InlineData(-1, 0, 0)]
        public void GetOtherFutureDate_InCorrectParams_ReturnNull(int day, int month, int year)
        {
            var dt = Generator.GetOtherDate(day, month, year);
            dt.Should().BeNull();
        }

        [Theory]
        [InlineData(1, 31, 1), InlineData(16, 16, 1), InlineData(0, 1, 2)]
        public void GetOtherPastDate_CorrectDay_ReturnPastDate(int day, int nDay, int nMouth)
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 2, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            Generator.DateTimeHelper = mockDateTimeHelper.Object;

            var dt = Generator.GetOtherDate(day, 0, 0, false).Value;

            dt.Year.Should().Be(2000);
            dt.Month.Should().Be(nMouth);
            dt.Day.Should().Be(nDay);
        }

        [Theory]
        [InlineData(1, 11, 2001), InlineData(6, 6, 2001), InlineData(12, 12, 2000)]
        public void GetOtherPastDate_CorrectMonth_ReturnPastDate(int month, int nMonth, int nYear)
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2001, 12, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            Generator.DateTimeHelper = mockDateTimeHelper.Object;

            var dt = Generator.GetOtherDate(0, month, 0, false).Value;

            dt.Year.Should().Be(nYear);
            dt.Month.Should().Be(nMonth);
            dt.Day.Should().Be(1);
        }

        [Theory]
        [InlineData(1, 2099), InlineData(10, 2090), InlineData(100, 2000)]
        public void GetOtherPastDate_CorrectYear_ReturnPastDate(int year, int nYear)
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2100, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            Generator.DateTimeHelper = mockDateTimeHelper.Object;

            var dt = Generator.GetOtherDate(0, 0, year, false).Value;

            dt.Year.Should().Be(nYear);
            dt.Month.Should().Be(1);
            dt.Day.Should().Be(1);
        }

        [Theory]
        [InlineData(0, 0, -1), InlineData(0, -1, 0), InlineData(-1, 0, 0)]
        public void GetOtherPastDate_InCorrectParams_ReturnNull(int day, int month, int year)
        {
            var dt = Generator.GetOtherDate(day, month, year, false);
            dt.Should().BeNull();
        }


        public static IEnumerable<object[]> CustomPastDateTime()
        {
            yield return new object[] { 1, 31, 1, new DateTime(2000, 2, 1) };
            yield return new object[] { 16, 16, 1, new DateTime(2000, 2, 1) };
            yield return new object[] { 0, 1, 2, new DateTime(2000, 2, 1) };
        }

        [Theory]
        [MemberData(nameof(CustomPastDateTime))]
        public void GetOtherCustomDate_CorrectDay_ReturnPastDate(int day, int nDay, int nMouth, DateTime date)
        {
            var dt = Generator.GetOtherDate(day, 0, 0, false, date).Value;

            dt.Year.Should().Be(2000);
            dt.Month.Should().Be(nMouth);
            dt.Day.Should().Be(nDay);
        }

        public static IEnumerable<object[]> CustomFutureDateTime()
        {
            yield return new object[] { 1, 2, 1, new DateTime(2000, 1, 1) };
            yield return new object[] { 15, 16, 1, new DateTime(2000, 1, 1) };
            yield return new object[] { 31, 1, 2, new DateTime(2000, 1, 1) };
        }

        [Theory]
        [MemberData(nameof(CustomFutureDateTime))]
        public void GetOtherCustomDate_CorrectDay_ReturnFutureDate(int day, int nDay, int nMouth, DateTime date)
        {
            var dt = Generator.GetOtherDate(day, 0, 0, date:date).Value;

            dt.Year.Should().Be(2000);
            dt.Month.Should().Be(nMouth);
            dt.Day.Should().Be(nDay);
        }

        [Theory]
        [MemberData(nameof(RandomDateTime))]
        public void GetRandomDateTime_CorrectParams_ReturnRandomDateTime(DateTime start, DateTime end)
        {
            var randomDateTime = Generator.GetRandomDateTime(start, end);

            randomDateTime.Should().BeAfter(start);
            randomDateTime.Should().BeBefore(end);
        }

        [Fact]
        public void GetRandomDateTime_NullParams_ReturnRandomDateTime()
        {
            var randomDateTime = Generator.GetRandomDateTime();
            randomDateTime.Should().BeAfter(Generator.DefaultStart);
            randomDateTime.Should().BeBefore(Generator.DefaultEnd);
        }

    }
}
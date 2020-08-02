using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using EvidentInstruction.Exceptions;
using Xunit;
using EvidentInstruction.Generator.Models;
using Xunit.Sdk;

namespace EvidentInstruction.Generator.Tests
{
    /// <summary>
    /// Тесты проверки генераторов тестовых данных.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BogusGeneratorTests
    {
        private readonly string _prefix = "test";
        private readonly IGenerator generator = new BogusGenerator();

        public static IEnumerable<object[]> RandomDateTime()
        {
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2002, 1, 1) };
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2010, 1, 1) };
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2100, 1, 1) };
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10), InlineData(30)]
        public void GetRandomString_CorrectLength_ReturnRandomRuString(int expectedLength)
        {
            var randomString = generator.GetRandomString(expectedLength, string.Empty, string.Empty, Constants.russian);
            randomString.Should().HaveLength(expectedLength);
            foreach(char ch in randomString)
            {
                (Constants.ruChars + Constants.digits).Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10), InlineData(30)]
        public void GetRandomString_CorrectLength_ReturnRandomEnString(int expectedLength)
        {
            var randomString = generator.GetRandomString(expectedLength, string.Empty, string.Empty, Constants.english);
            randomString.Should().HaveLength(expectedLength);
            foreach (char ch in randomString)
            {
                (Constants.chars + Constants.digits).Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(-1), InlineData(0)]
        public void GetRandomString_ZeroOrNegativeLength_ReturnException(int expectedLength)
        {
            Action act = () => generator.GetRandomString(expectedLength, string.Empty, string.Empty, Constants.english); ;
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Длина строки должна быть положительной.");
        }

        [Theory]
        [InlineData(""), InlineData("de")]
        public void GetRandomString_IncorrectLanguage_ReturnNull(string expectedLocale)
        {
            var randomString = generator.GetRandomString(10, string.Empty, string.Empty, expectedLocale);
            randomString.Should().BeNull();
        }

        [Theory]
        [InlineData(""), InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomString_CorrectParams_ReturnRandomStringWithPrefix(string expectedPrefix)
        {
            var randomString = generator.GetRandomString(10, expectedPrefix, string.Empty, Constants.english);
            randomString.Should().HaveLength(10);
            randomString.IndexOf(expectedPrefix).Should().Be(0);
        }

        [Theory]
        [InlineData(""), InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomString_CorrectParams_ReturnRandomStringWithPostfix(string expectedPostfix)
        {
            var randomString = generator.GetRandomString(10, string.Empty, expectedPostfix, Constants.english);
            randomString.Should().HaveLength(10);
            if(expectedPostfix != "")
            {
                randomString.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
            }
        }

        [Theory]
        [InlineData("", ""), InlineData("AUTO_", "АВТО")]
        public void GetRandomString_CorrectParams_ReturnRandomStringWithPrefixAndPostfix(string expectedPrefix, string expectedPostfix)
        {
            var randomString = generator.GetRandomString(10, expectedPrefix, expectedPostfix, Constants.english);
            randomString.Should().HaveLength(10);
            randomString.IndexOf(expectedPrefix).Should().Be(0);
            if (expectedPostfix != "")
            {
                randomString.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
            }

        }

        [Theory]
        [InlineData("AUTOAUTOAUTO", ""), InlineData("", "AUTOAUTOAUTO"), InlineData("AUTOA", "TOAUT")]
        public void GetRandomString_IncorrectParams_ReturnException(string expectedPrefix, string expectedPostfix)
        {
            Action act = () => generator.GetRandomString(10, expectedPrefix, expectedPostfix, Constants.english);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Постфикс и префикс в сумме длинее самой строки.");
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10), InlineData(30)]
        public void GetRandomChars_CorrectLength_ReturnRandomRuChars(int expectedLength)
        {
            var randomChars = generator.GetRandomChars(expectedLength, string.Empty, string.Empty, Constants.russian);
            randomChars.Should().HaveLength(expectedLength);
            foreach (char ch in randomChars)
            {
                (Constants.ruChars).Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10), InlineData(30)]
        public void GetRandomChars_CorrectLength_ReturnRandomEnChars(int expectedLength)
        {
            var randomChars = generator.GetRandomChars(expectedLength, string.Empty, string.Empty, Constants.english);
            randomChars.Should().HaveLength(expectedLength);
            foreach (char ch in randomChars)
            {
                (Constants.chars).Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(-1), InlineData(0)]
        public void GetRandomChars_ZeroOrNegativeLength_ReturnException(int expectedLength)
        {
            Action act = () => generator.GetRandomChars(expectedLength, string.Empty, string.Empty, Constants.english); ;
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Длина строки должна быть положительной.");
        }

        [Theory]
        [InlineData(""), InlineData("de")]
        public void GetRandomChars_IncorrectLanguage_ReturnNull(string expectedLocale)
        {
            var randomChars = generator.GetRandomChars(10, string.Empty, string.Empty, expectedLocale);
            randomChars.Should().BeNull();
        }

        [Theory]
        [InlineData(""), InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomChars_CorrectParams_ReturnRandomCharsWithPrefix(string expectedPrefix)
        {
            var randomChars = generator.GetRandomChars(10, expectedPrefix, string.Empty, Constants.english);
            randomChars.Should().HaveLength(10);
            randomChars.IndexOf(expectedPrefix).Should().Be(0);
        }

        [Theory]
        [InlineData(""), InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomChars_CorrectParams_ReturnRandomCharsWithPostfix(string expectedPostfix)
        {
            var randomChars = generator.GetRandomChars(10, string.Empty, expectedPostfix, Constants.english);
            randomChars.Should().HaveLength(10);
            if (expectedPostfix != "")
            {
                randomChars.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
            }
        }

        [Theory]
        [InlineData("", ""), InlineData("AUTO_", "АВТО")]
        public void GetRandomChars_CorrectParams_ReturnRandomCharsWithPrefixAndPostfix(string expectedPrefix, string expectedPostfix)
        {
            var randomChars = generator.GetRandomChars(10, expectedPrefix, expectedPostfix, Constants.english);
            randomChars.Should().HaveLength(10);
            randomChars.IndexOf(expectedPrefix).Should().Be(0);
            if (expectedPostfix != "")
            {
                randomChars.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
            }

        }

        [Theory]
        [InlineData("AUTOAUTOAUTO", ""), InlineData("", "AUTOAUTOAUTO"), InlineData("AUTOA", "TOAUT")]
        public void GetRandomChars_IncorrectParams_ReturnException(string expectedPrefix, string expectedPostfix)
        {
            Action act = () => generator.GetRandomChars(10, expectedPrefix, expectedPostfix, Constants.english);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Постфикс и префикс в сумме длинее самой строки.");
        }

        [Theory]
        [InlineData(1), InlineData(2), InlineData(10), InlineData(30)]
        public void GetRandomStringNumbers_CorrectLength_ReturnRandomStringNumbers(int expectedLength)
        {
            var randomStringNumbers = generator.GetRandomStringNumbers(expectedLength, string.Empty, string.Empty);
            randomStringNumbers.Should().HaveLength(expectedLength);
            foreach (char ch in randomStringNumbers)
            {
                (Constants.digits).Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(-1), InlineData(0)]
        public void GetRandomStringNumbers_ZeroOrNegativeLength_ReturnException(int expectedLength)
        {
            Action act = () => generator.GetRandomStringNumbers(expectedLength, string.Empty, string.Empty); ;
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Длина строки должна быть положительной.");
        }

        [Theory]
        [InlineData(""), InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomStringNumbers_CorrectParams_ReturnRandomStringNumbersWithPrefix(string expectedPrefix)
        {
            var randomStringNumbers = generator.GetRandomStringNumbers(10, expectedPrefix, string.Empty);
            randomStringNumbers.Should().HaveLength(10);
            randomStringNumbers.IndexOf(expectedPrefix).Should().Be(0);
        }

        [Theory]
        [InlineData(""), InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomStringNumbers_CorrectParams_ReturnRandomStringNumbersWithPostfix(string expectedPostfix)
        {
            var randomStringNumbers = generator.GetRandomStringNumbers(10, string.Empty, expectedPostfix);
            randomStringNumbers.Should().HaveLength(10);
            if (expectedPostfix != "")
            {
                randomStringNumbers.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
            }
        }

        [Theory]
        [InlineData("", ""), InlineData("AUTO", "АВТО"), InlineData("AUTO_", "АВТО")]
        public void GetRandomStringNumbers_CorrectParams_ReturnRandomStringNumbersWithPrefixAndPostfix(string expectedPrefix, string expectedPostfix)
        {
            var randomStringNumbers = generator.GetRandomStringNumbers(10, expectedPrefix, expectedPostfix);
            randomStringNumbers.Should().HaveLength(10);
            randomStringNumbers.IndexOf(expectedPrefix).Should().Be(0);
            if (expectedPostfix != "")
            {
                randomStringNumbers.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
            }

        }

        [Theory]
        [InlineData("AUTOAUTOAUTO", ""), InlineData("", "AUTOAUTOAUTO"), InlineData("AUTOA", "TOAUT")]
        public void GetRandomStringNumbers_IncorrectParams_ReturnException(string expectedPrefix, string expectedPostfix)
        {
            Action act = () => generator.GetRandomStringNumbers(10, expectedPrefix, expectedPostfix);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Постфикс и префикс в сумме длинее самой строки.");
        }

        [Fact]
        public void GetGuid_ValidGuid()
        {
            var str = generator.GetGuid();
            bool isValid = Guid.TryParse(str, out Guid guid);
            isValid.Should().BeTrue();

        }

        [Theory]
        [InlineData(null), InlineData("+7##########"), InlineData("+7###-###-##-##"), InlineData("+7 ### ### ## ##"), InlineData("+7(###)###-##-##"), InlineData("+7###########"), InlineData("+7 ### ### ## ## ## ##"), InlineData("+7#"), InlineData("+7")]
        public void GetRandomPhone_CorrectParams_ReturnRandomPhone(string expectedMask)
        {
            var randomPhone = generator.GetRandomPhone(expectedMask);
            randomPhone.Should().HaveLength(randomPhone.Length);
        }

        //[Theory]
        //[InlineData("+7###########"), InlineData("+7 ### ### ## ## ## ##"), InlineData("+7#"), InlineData("+7")]
        //public void GetRandomPhone_OverTenParams_ReturnException(string expectedMask)
        //{
        //    Action act = () => generator.GetRandomPhone(expectedMask);
        //    act
        //      .Should().Throw<FormatException>()
        //      .WithMessage("Phone number can have up to 10 digits");
        //}
        //
        //[Theory]
        //[InlineData("+7#"), InlineData("+7")]
        //public void GetRandomPhone_LessTenParams_ReturnException(string expectedMask)
        //{
        //    Action act = () => generator.GetRandomPhone(expectedMask);
        //    act
        //      .Should().Throw<FormatException>()
        //      .WithMessage("Номер телефона должен содержать не менее 10 цифр.");
        //}

        [Theory]
        [InlineData(1,1,1990), InlineData(15, 6, 1990), InlineData(31, 12, 1990)]
        public void GetDate_CorrectParams_ReturnDate(int day, int month, int year)
        {
            var expDt = new DateTime(year, month, day);
            var dt = generator.GetDate(day, month, year);
            dt.Should().Be(expDt);
        }

        [Theory]
        [InlineData(0, 1, 1990), InlineData(40, 6, 1990), InlineData(31, 13, 1990)]
        public void GetDate_IncorrectParams_ReturnNull(int day, int month, int year)
        {
            var dt = generator.GetDate(day, month, year);
            dt.Should().BeNull();
        }

        [Theory]
        [InlineData(0, 0, 0, 1), InlineData(0, 0, 1, 0), InlineData(0, 1, 0, 0), InlineData(1, 0, 0, 0),
            InlineData(12, 30, 30, 30), InlineData(23, 59, 59, 59)]
        public void GetTime_CorrectParams_ReturnTime(int hours, int minutes, int seconds, int milliseconds)
        {
            var now = DateTime.Now;
            var expTm = new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds, milliseconds);

            var tm = generator.GetTime(hours, minutes, seconds, milliseconds);

            tm.Should().Be(expTm);
        }

        [Theory]
        [InlineData(0, 0, 0, -1), InlineData(0, 0, -1, 0), InlineData(0, -1, 0, 0), InlineData(-1, 0, 0, 0),
            InlineData(24, 60, 60, 60), InlineData(100, 100, 100, 100)]
        public void GetTime_IncorrectParams_ReturnNull(int hours, int minutes, int seconds, int milliseconds)
        {
            var tm = generator.GetTime(hours, minutes, seconds, milliseconds);

            tm.Should().BeNull();
        }

        //[Fact]
        //public void GetCurrentDateTime_ReturnCurrentDateTime()
        //{
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var fakeDate = new DateTime(2000, 1, 1, 1, 1, 1);
        //    mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
        //        .Returns(fakeDate);
        //    generator.dateTimeHelper = mockDateTimeHelper.Object;
        //    var dt = generator.GetCurrentDateTime();
        //    dt.Should().Be(fakeDate);
        //}

        [Theory]
        [InlineData(1, 1, 1990, 0, 0, 0, 1), InlineData(1, 1, 1990, 0, 0, 1, 0), InlineData(1, 1, 1990, 0, 1, 0, 0), InlineData(1, 1, 1990, 1, 0, 0, 0),
            InlineData(15, 6, 1990, 12, 30, 30, 30), InlineData(31, 12, 1990, 23, 59, 59, 59)]
        public void GetDateTime_CorrectParams_ReturnDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds)
        {
            var expDt = new DateTime(year, month, day, hours, minutes, seconds, milliseconds);

            var dt = generator.GetDateTime(day, month, year, hours, minutes, seconds, milliseconds);

            dt.Should().Be(expDt);
        }

        [Theory]
        [InlineData(1, 1, 1990, 0, 0, 0, -1), InlineData(1, 1, 1990, 0, 0, -1, 0), InlineData(1, 1, 1990, 0, -1, 0, 0), InlineData(1, 1, 1990, -1, 0, 0, 0),
            InlineData(0, 0, -1, 0, 0, 0, 0), InlineData(0, -1, 0, 0, 0, 0, 0), InlineData(-1, 0, 0, 0, 0, 0, 0),
            InlineData(40, 6, 1990, 24, 60, 60, 60), InlineData(31, 13, 1990, 100, 100, 100, 100)]
        public void GetDateTime_IncorrectParams_ReturnNull(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds)
        {
            var dt = generator.GetDateTime(day, month, year, hours, minutes, seconds, milliseconds);

            dt.Should().BeNull();
        }

        //[Theory]
        //[InlineData(1, 2, 1), InlineData(15, 16, 1), InlineData(31, 1, 2)]
        //public void GetOtherFutureDate_CorrectDay_ReturnFutureDate(int day, int nDay, int nMouth)
        //{
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var fakeDate = new DateTime(2000, 1, 1);
        //    mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
        //        .Returns(fakeDate);
        //
        //    generator.DateTimeHelper = mockDateTimeHelper.Object;
        //
        //    var dt = generator.GetOtherDate(day, 0, 0).Value;
        //
        //    dt.Year.Should().Be(2000);
        //    dt.Month.Should().Be(nMouth);
        //    dt.Day.Should().Be(nDay);
        //}
        //
        //[Theory]
        //[InlineData(1, 2, 2000), InlineData(6, 7, 2000), InlineData(12, 1, 2001)]
        //public void GetOtherFutureDate_CorrectMonth_ReturnFutureDate(int month, int nMonth, int nYear)
        //{
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var fakeDate = new DateTime(2000, 1, 1);
        //    mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
        //        .Returns(fakeDate);
        //
        //    generator.DateTimeHelper = mockDateTimeHelper.Object;
        //
        //    var dt = generator.GetOtherDate(0, month, 0).Value;
        //
        //    dt.Year.Should().Be(nYear);
        //    dt.Month.Should().Be(nMonth);
        //    dt.Day.Should().Be(1);
        //}
        //
        //[Theory]
        //[InlineData(1, 2001), InlineData(10, 2010), InlineData(100, 2100)]
        //public void GetOtherFutureDate_CorrectYear_ReturnFutureDate(int year, int nYear)
        //{
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var fakeDate = new DateTime(2000, 1, 1);
        //    mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
        //        .Returns(fakeDate);
        //
        //    generator.DateTimeHelper = mockDateTimeHelper.Object;
        //
        //    var dt = generator.GetOtherDate(0, 0, year).Value;
        //
        //    dt.Year.Should().Be(nYear);
        //    dt.Month.Should().Be(1);
        //    dt.Day.Should().Be(1);
        //}

        //[Theory]
        //[InlineData(0, 0, -1), InlineData(0, -1, 0), InlineData(-1, 0, 0)]
        //public void GetOtherFutureDate_IncorrectParams_ReturnNull(int day, int month, int year)
        //{
        //    var dt = generator.GetOtherDate(day, month, year);
        //    dt.Should().BeNull();
        //}

        //[Theory]
        //[InlineData(1, 31, 1), InlineData(16, 16, 1), InlineData(0, 1, 2)]
        //public void GetOtherPastDate_CorrectDay_ReturnPastDate(int day, int nDay, int nMouth)
        //{
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var fakeDate = new DateTime(2000, 2, 1);
        //    mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
        //        .Returns(fakeDate);
        //
        //    generator.DateTimeHelper = mockDateTimeHelper.Object;
        //
        //    var dt = generator.GetOtherDate(day, 0, 0, false).Value;
        //
        //    dt.Year.Should().Be(2000);
        //    dt.Month.Should().Be(nMouth);
        //    dt.Day.Should().Be(nDay);
        //}
        //
        //[Theory]
        //[InlineData(1, 11, 2001), InlineData(6, 6, 2001), InlineData(12, 12, 2000)]
        //public void GetOtherPastDate_CorrectMonth_ReturnPastDate(int month, int nMonth, int nYear)
        //{
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var fakeDate = new DateTime(2001, 12, 1);
        //    mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
        //        .Returns(fakeDate);
        //
        //    generator.DateTimeHelper = mockDateTimeHelper.Object;
        //
        //    var dt = generator.GetOtherDate(0, month, 0, false).Value;
        //
        //    dt.Year.Should().Be(nYear);
        //    dt.Month.Should().Be(nMonth);
        //    dt.Day.Should().Be(1);
        //}
        //
        //[Theory]
        //[InlineData(1, 2099), InlineData(10, 2090), InlineData(100, 2000)]
        //public void GetOtherPastDate_CorrectYear_ReturnPastDate(int year, int nYear)
        //{
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var fakeDate = new DateTime(2100, 1, 1);
        //    mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
        //        .Returns(fakeDate);
        //
        //    generator.DateTimeHelper = mockDateTimeHelper.Object;
        //
        //    var dt = generator.GetOtherDate(0, 0, year, false).Value;
        //
        //    dt.Year.Should().Be(nYear);
        //    dt.Month.Should().Be(1);
        //    dt.Day.Should().Be(1);
        //}

        //[Theory]
        //[InlineData(0, 0, -1), InlineData(0, -1, 0), InlineData(-1, 0, 0)]
        //public void GetOtherPastDate_InCorrectParams_ReturnNull(int day, int month, int year)
        //{
        //    var dt = generator.GetOtherDate(day, month, year, false);
        //    dt.Should().BeNull();
        //}


        public static IEnumerable<object[]> CustomPastDateTime()
        {
            yield return new object[] { 1, 31, 1, new DateTime(2000, 2, 1) };
            yield return new object[] { 16, 16, 1, new DateTime(2000, 2, 1) };
            yield return new object[] { 0, 1, 2, new DateTime(2000, 2, 1) };
        }

        //[Theory]
        //[MemberData(nameof(CustomPastDateTime))]
        //public void GetOtherCustomDate_CorrectDay_ReturnPastDate(int day, int nDay, int nMouth, DateTime date)
        //{
        //    var dt = generator.GetOtherDate(day, 0, 0, false, date).Value;
        //
        //    dt.Year.Should().Be(2000);
        //    dt.Month.Should().Be(nMouth);
        //    dt.Day.Should().Be(nDay);
        //}

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
            var dt = generator.GetOtherDate(day, 0, 0, date:date).Value;

            dt.Year.Should().Be(2000);
            dt.Month.Should().Be(nMouth);
            dt.Day.Should().Be(nDay);
        }

        [Theory]
        [MemberData(nameof(RandomDateTime))]
        public void GetRandomDateTime_CorrectParams_ReturnRandomDateTime(DateTime start, DateTime end)
        {
            var randomDateTime = generator.GetRandomDateTime(start, end);

            randomDateTime.Should().BeAfter(start);
            randomDateTime.Should().BeBefore(end);
        }

        //[Fact]
        //public void GetRandomDateTime_NullParams_ReturnRandomDateTime()
        //{
        //    var randomDateTime = generator.GetRandomDateTime();
        //    randomDateTime.Should().BeAfter(generator.DefaultStart);
        //    randomDateTime.Should().BeBefore(generator.DefaultEnd);
        //}

    }
}
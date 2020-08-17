using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using EvidentInstruction.Exceptions;
using Xunit;
using EvidentInstruction.Generator.Models;
using Xunit.Sdk;
using System.Linq;

namespace EvidentInstruction.Generator.Tests
{
    /// <summary>
    /// Тесты проверки генераторов тестовых данных.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BogusGeneratorTests
    {
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
            Action act = () => generator.GetRandomString(expectedLength, string.Empty, string.Empty, Constants.english);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Длина строки должна быть положительной.");
        }

        [Theory]
        [InlineData(""), InlineData("de"), InlineData(null)]
        public void GetRandomString_IncorrectLanguage_ReturnException(string expectedLocale)
        {
            Action act = () => generator.GetRandomString(10, string.Empty, string.Empty, expectedLocale);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Выберите русский или английский язык.");
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
        [InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomString_CorrectParams_ReturnRandomStringWithPostfix(string expectedPostfix)
        {
            var randomString = generator.GetRandomString(10, string.Empty, expectedPostfix, Constants.english);
            randomString.Should().HaveLength(10);
            randomString.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
        }

        [Theory]
        [InlineData("Й", "Ц"), InlineData("AUTO_", "АВТО")]
        public void GetRandomString_CorrectParams_ReturnRandomStringWithPrefixAndPostfix(string expectedPrefix, string expectedPostfix)
        {
            var randomString = generator.GetRandomString(10, expectedPrefix, expectedPostfix, Constants.english);
            randomString.Should().HaveLength(10);
            randomString.IndexOf(expectedPrefix).Should().Be(0);
            randomString.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
        }

        [Fact]
        public void GetRandomString_CorrectParams_ReturnRandomStringWithEmptyPrefixAndEmptyPostfix()
        {
            var randomString = generator.GetRandomString(10, "", "", Constants.english);
            randomString.Should().HaveLength(10);
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
            Action act = () => generator.GetRandomChars(expectedLength, string.Empty, string.Empty, Constants.english);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Длина строки должна быть положительной.");
        }

        [Theory]
        [InlineData(""), InlineData("de"), InlineData(null)]
        public void GetRandomChars_IncorrectLanguage_ReturnException(string expectedLocale)
        {
            Action act = () => generator.GetRandomChars(10, string.Empty, string.Empty, expectedLocale);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Выберите русский или английский язык.");
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
        [InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomChars_CorrectParams_ReturnRandomCharsWithPostfix(string expectedPostfix)
        {
            var randomChars = generator.GetRandomChars(10, string.Empty, expectedPostfix, Constants.english);
            randomChars.Should().HaveLength(10);
            randomChars.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
        }

        [Theory]
        [InlineData("Й", "Ц"), InlineData("AUTO_", "АВТО")]
        public void GetRandomChars_CorrectParams_ReturnRandomCharsWithPrefixAndPostfix(string expectedPrefix, string expectedPostfix)
        {
            var randomChars = generator.GetRandomChars(10, expectedPrefix, expectedPostfix, Constants.english);
            randomChars.Should().HaveLength(10);
            randomChars.IndexOf(expectedPrefix).Should().Be(0);
            randomChars.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
        }

        [Fact]
        public void GetRandomChars_CorrectParams_ReturnRandomCharsWithEmptyPrefixAndEmptyPostfix()
        {
            var randomChars = generator.GetRandomChars(10, "", "", Constants.english);
            randomChars.Should().HaveLength(10);
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
            Action act = () => generator.GetRandomStringNumbers(expectedLength, string.Empty, string.Empty);
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
        [InlineData("AUTO"), InlineData("AUTOAUTOA")]
        public void GetRandomStringNumbers_CorrectParams_ReturnRandomStringNumbersWithPostfix(string expectedPostfix)
        {
            var randomStringNumbers = generator.GetRandomStringNumbers(10, string.Empty, expectedPostfix);
            randomStringNumbers.Should().HaveLength(10);
            randomStringNumbers.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
        }

        [Theory]
        [InlineData("Й", "Ц"), InlineData("AUTO_", "АВТО")]
        public void GetRandomStringNumbers_CorrectParams_ReturnRandomStringNumbersWithPrefixAndPostfix(string expectedPrefix, string expectedPostfix)
        {
            var randomStringNumbers = generator.GetRandomStringNumbers(10, expectedPrefix, expectedPostfix);
            randomStringNumbers.Should().HaveLength(10);
            randomStringNumbers.IndexOf(expectedPrefix).Should().Be(0);
            randomStringNumbers.IndexOf(expectedPostfix).Should().Be(10 - expectedPostfix.Length);
        }

        [Fact]
        public void GetRandomStringNumbers_CorrectParams_ReturnRandomStringNumbersWithEmptyPrefixAndEmptyPostfix()
        {
            var randomStringNumbers = generator.GetRandomStringNumbers(10, "", "");
            randomStringNumbers.Should().HaveLength(10);
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
        [InlineData(null), InlineData("+7##########"), InlineData("+7###-###-##-##"), InlineData("+7 ### ### ## ##"), 
            InlineData("+7(###)###-##-##"), InlineData("+7###########"), InlineData("+7 ### ### ## ## ## ##"), 
            InlineData("+7#"), InlineData("+7")]
        public void GetRandomPhone_CorrectParams_ReturnRandomPhone(string expectedMask)
        {
            var randomPhone = generator.GetRandomPhone(expectedMask);
            randomPhone.Should().HaveLength(randomPhone.Length);
        }

        [Theory]
        [InlineData(1,1,1990), InlineData(15, 6, 1990), InlineData(31, 12, 2100)]
        public void GetDate_CorrectParams_ReturnDate(int day, int month, int year)
        {
            var expDt = new DateTime(year, month, day);
            var dt = generator.GetDate(day, month, year);
            dt.Should().Be(expDt);
        }

        [Theory]
        [InlineData(0, 1, 1990), InlineData(40, 6, 1990), InlineData(31, 13, 2100)]
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
        [InlineData(0, 0, 1), InlineData(0, 1, 0), InlineData(1, 0, 0), InlineData(23, 59, 59)]
        public void GetTime_CorrectParamsWithoutMS_ReturnTime(int hours, int minutes, int seconds)
        {
            var now = DateTime.Now;
            var expTm = new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds);
            var tm = generator.GetTime(hours, minutes, seconds);
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

        [Fact]
        public void GetCurrentDateTime_ReturnCurrentDateTime()
        {
            var regenerator = new BogusGenerator();
            var mockDateTimeHelper = new Mock<DateTimeHelper>();
            DateTime fakeDate = new DateTime(2000, 1, 1, 1, 1, 1, millisecond: 111);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);
            regenerator.dateTimeHelper = mockDateTimeHelper.Object;
            var dt = regenerator.GetCurrentDateTime();
            dt.Should().Be(fakeDate);
        }

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

        [Theory]
        [InlineData(1), InlineData(5), InlineData(10)]
        public void GetRandomInt_CorrectParams_ReturnIntWithDefinedLength(int length)
        {
            var randomInt = generator.GetRandomInt(length);
            randomInt.ToString().Should().HaveLength(length);
        }

        [Theory]
        [InlineData(1, 1), InlineData(-1, 0), InlineData(-10000, 10000), InlineData(-2147483647, 2147483647)]
        public void GetRandomInt_CorrectParams_ReturnIntAccordingToLimits(int min, int max)
        {
            var randomInt = generator.GetRandomInt(min: min, max: max);
            randomInt.Should().BeGreaterOrEqualTo(min);
            randomInt.Should().BeLessOrEqualTo(max);
        }

        [Fact]
        public void GetRandomInt_ZeroParams_ReturnException()
        {
            Action act = () => generator.GetRandomInt(0, 0, 0);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Невозможно создать число с нулевой длиной, которое больше и меньше 0.");
        }

        [Theory]
        [InlineData(-1), InlineData(-10)]
        public void GetRandomInt_NegativeLength_ReturnException(int expectedLength)
        {
            Action act = () => generator.GetRandomInt(expectedLength);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Длина строки не может быть отрицательной.");
        }

        [Theory]
        [InlineData(6, 5), InlineData(2147483647, -2147483647)]
        public void GetRandomInt_IncorrectParams_ReturnException(int min, int max)
        {
            Action act = () => generator.GetRandomInt(min: min, max: max);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Максимальное значение не может быть меньше минимального.");
        }

        [Theory]
        [InlineData(5, 1, 1), InlineData(5, -2147483647, -2147483646)]
        public void GetRandomInt_NoZeroParams_ReturnIntWithDefinedLength(int length, int min, int max)
        {
            var randomInt = generator.GetRandomInt(length, min, max);
            randomInt.ToString().Should().HaveLength(length);
            randomInt.Should().BeGreaterThan(min);
            randomInt.Should().BeGreaterThan(max);
        }

        [Theory]
        [InlineData(1), InlineData(5), InlineData(10)]
        public void GetRandomDouble_CorrectParams_ReturnDoubleWithDefinedLength(int length)
        {
            var randomInt = generator.GetRandomDouble(0, length);
            randomInt.ToString().Should().HaveLength(length);
        }

        [Theory]
        [InlineData(1, 1), InlineData(-1, 0), InlineData(-10000, 10000), InlineData(-2147483647, 2147483647)]
        public void GetRandomDouble_CorrectParams_ReturnDoubleAccordingToLimits(int min, int max)
        {
            var randomInt = generator.GetRandomDouble(0, min: min, max: max);
            randomInt.Should().BeGreaterOrEqualTo(min);
            randomInt.Should().BeLessOrEqualTo(max);
        }

        [Fact]
        public void GetRandomDouble_ZeroParams_ReturnException()
        {
            Action act = () => generator.GetRandomDouble(0, 0, 0, 0);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Невозможно создать число с нулевой длиной, которое больше и меньше 0.");
        }

        [Theory]
        [InlineData(-1), InlineData(-10)]
        public void GetRandomDouble_NegativeLength_ReturnException(int expectedLength)
        {
            Action act = () => generator.GetRandomDouble(0, expectedLength);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Длина строки не может быть отрицательной.");
        }

        [Theory]
        [InlineData(6, 5), InlineData(2147483647, -2147483647)]
        public void GetRandomDouble_IncorrectParams_ReturnException(int min, int max)
        {
            Action act = () => generator.GetRandomDouble(0, min: min, max: max);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Максимальное значение не может быть меньше минимального.");
        }

        [Theory]
        [InlineData(5, 1, 1), InlineData(5, -2147483647, -2147483646)]
        public void GetRandomDouble_NoZeroParams_ReturnDoubleWithDefinedLength(int length, int min, int max)
        {
            var randomInt = generator.GetRandomDouble(0, length, min, max);
            randomInt.ToString().Should().HaveLength(length);
            randomInt.Should().BeGreaterThan(min);
            randomInt.Should().BeGreaterThan(max);
        }

        [Theory]
        [InlineData(1), InlineData(5), InlineData(10)]
        public void GetRandomDouble_CorrectParams_ReturnDoubleWithDoublePart(int limit)
        {
            var randomInt = generator.GetRandomDouble(limit, 3);
            randomInt.ToString().IndexOf(',').Should().Be(3);
            randomInt.ToString().Should().HaveLength(4 + limit);
        }

        [Theory]
        [InlineData(-1), InlineData(-10)]
        public void GetRandomDouble_NegativeLimit_ReturnException(int expectedLimit)
        {
            Action act = () => generator.GetRandomDouble(expectedLimit, 3);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Ограничение знаков после запятой не может быть отрицательным.");
        }

        [Fact]
        public void GetMonth_CorrectParams_ReturnRandomEnMonth()
        {
            var randomMonth = generator.GetMonth(Constants.english);
            foreach (char ch in randomMonth)
            {
                Constants.chars.Should().Contain(ch.ToString());
            }
        }

        [Fact]
        public void GetMonth_CorrectParams_ReturnRandomRuMonth()
        {
            var randomMonth = generator.GetMonth(Constants.russian);
            foreach (char ch in randomMonth)
            {
                Constants.ruChars.Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(""), InlineData("de"), InlineData(null)]
        public void GetMonth_IncorrectLanguage_ReturnException(string expectedLocale)
        {
            Action act = () => generator.GetMonth(expectedLocale);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Выберите русский или английский язык.");
        }

        [Fact]
        public void GetWeekday_CorrectParams_ReturnRandomEnWeekday()
        {
            var randomWeekday = generator.GetWeekday(Constants.english);
            foreach (char ch in randomWeekday)
            {
                Constants.chars.Should().Contain(ch.ToString());
            }
        }

        [Fact]
        public void GetWeekday_CorrectParams_ReturnRandomRuWeekday()
        {
            var randomWeekday = generator.GetWeekday(Constants.russian);
            foreach (char ch in randomWeekday)
            {
                Constants.ruChars.Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(""), InlineData("de"), InlineData(null)]
        public void GetWeekday_IncorrectLanguage_ReturnException(string expectedLocale)
        {
            Action act = () => generator.GetWeekday(expectedLocale);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Выберите русский или английский язык.");
        }

        [Fact]
        public void GetFirstName_CorrectParams_ReturnRandomEnFirstName()
        {
            var randomFirstName = generator.GetFirstName(Constants.english);
            foreach (char ch in randomFirstName)
            {
                Constants.chars.Should().Contain(ch.ToString());
            }
        }

        [Fact]
        public void GetFirstName_CorrectParams_ReturnRandomRuFirstName()
        {
            var randomFirstName = generator.GetFirstName(Constants.russian);
            foreach (char ch in randomFirstName)
            {
                Constants.ruChars.Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(""), InlineData("de"), InlineData(null)]
        public void GetFirstName_IncorrectLanguage_ReturnException(string expectedLocale)
        {
            Action act = () => generator.GetFirstName(expectedLocale);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Выберите русский или английский язык.");
        }

        [Fact]
        public void GetLastName_CorrectParams_ReturnRandomEnLastName()
        {
            var randomLastName = generator.GetLastName(Constants.english);
            foreach (char ch in randomLastName)
            {
                Constants.chars.Should().Contain(ch.ToString());
            }
        }

        [Fact]
        public void GetLastName_CorrectParams_ReturnRandomRuLastName()
        {
            var randomLastName = generator.GetLastName(Constants.russian);
            foreach (char ch in randomLastName)
            {
                Constants.ruChars.Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(""), InlineData("de"), InlineData(null)]
        public void GetLastName_IncorrectLanguage_ReturnException(string expectedLocale)
        {
            Action act = () => generator.GetLastName(expectedLocale);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Выберите русский или английский язык.");
        }

        [Fact]
        public void GetFullName_CorrectParams_ReturnRandomEnFullName()
        {
            var randomFullName = generator.GetFullName(Constants.english);
            foreach (char ch in randomFullName)
            {
                (' ' + Constants.chars).Should().Contain(ch.ToString());
            }
        }

        [Fact]
        public void GetFullName_CorrectParams_ReturnRandomRuFullName()
        {
            var randomFullName = generator.GetFullName(Constants.russian);
            foreach (char ch in randomFullName)
            {
                (' ' + Constants.ruChars).Should().Contain(ch.ToString());
            }
        }

        [Theory]
        [InlineData(""), InlineData("de"), InlineData(null)]
        public void GetFullName_IncorrectLanguage_ReturnException(string expectedLocale)
        {
            Action act = () => generator.GetFullName(expectedLocale);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Выберите русский или английский язык.");
        }

        [Fact]
        public void GetEmail_NoParams_ReturnRandomEmail()
        {
            var randomEmail = generator.GetEmail();
            randomEmail.IndexOf("@gmail.com").Should().Be(randomEmail.Length - "@gmail.com".Length);
        }

        [Theory]
        [InlineData("yandex.ru"), InlineData("qwerty")]
        public void GetEmail_CorrectParams_ReturnRandomEmail(string domen)
        {
            var randomEmail = generator.GetEmail(domen);
            randomEmail.IndexOf('@' + domen).Should().Be(randomEmail.Length - ('@' + domen).Length);
        }

        [Theory]
        [InlineData(""), InlineData("   "), InlineData(null)]
        public void GetEmail_IncorrectParams_ReturnException(string domen)
        {
            Action act = () => generator.GetEmail(domen);
            act.Should().Throw<XunitException>()
                .And.Message.Contains("Введите домен.");
        }

        [Fact]
        public void GetIp_NoParams_ReturnRandomIp()
        {
            var randomIp = generator.GetIp();
            randomIp.Count(f => f == '.').Should().Be(3);
            foreach (char ch in randomIp)
            {
                ('.' + Constants.digits).Should().Contain(ch.ToString());
            }
        }

        [Fact]
        public void GetUrl_NoParams_ReturnRandomUrl()
        {
            var randomUrl = generator.GetUrl();
            randomUrl.IndexOf("https://").Should().Be(0);
            randomUrl.Should().Contain(".");
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
        //public void GetOtherPastDate_IncorrectParams_ReturnNull(int day, int month, int year)
        //{
        //    var dt = generator.GetOtherDate(day, month, year, false);
        //    dt.Should().BeNull();
        //}


        //public static IEnumerable<object[]> CustomPastDateTime()
        //{
        //    yield return new object[] { 1, 31, 1, new DateTime(2000, 2, 1) };
        //    yield return new object[] { 16, 16, 1, new DateTime(2000, 2, 1) };
        //    yield return new object[] { 0, 1, 2, new DateTime(2000, 2, 1) };
        //}

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

        //public static IEnumerable<object[]> CustomFutureDateTime()
        //{
        //    yield return new object[] { 1, 2, 1, new DateTime(2000, 1, 1) };
        //    yield return new object[] { 15, 16, 1, new DateTime(2000, 1, 1) };
        //    yield return new object[] { 31, 1, 2, new DateTime(2000, 1, 1) };
        //}
        //
        //[Theory]
        //[MemberData(nameof(CustomFutureDateTime))]
        //public void GetOtherCustomDate_CorrectDay_ReturnFutureDate(int day, int nDay, int nMouth, DateTime date)
        //{
        //    var dt = generator.GetOtherDate(day, 0, 0, date:date).Value;
        //
        //    dt.Year.Should().Be(2000);
        //    dt.Month.Should().Be(nMouth);
        //    dt.Day.Should().Be(nDay);
        //}
        //
        //[Theory]
        //[MemberData(nameof(RandomDateTime))]
        //public void GetRandomDateTime_CorrectParams_ReturnRandomDateTime(DateTime start, DateTime end)
        //{
        //    var randomDateTime = generator.GetRandomDateTime(start, end);
        //
        //    randomDateTime.Should().BeAfter(start);
        //    randomDateTime.Should().BeBefore(end);
        //}

        //[Fact]
        //public void GetRandomDateTime_NullParams_ReturnRandomDateTime()
        //{
        //    var randomDateTime = generator.GetRandomDateTime();
        //    randomDateTime.Should().BeAfter(generator.DefaultStart);
        //    randomDateTime.Should().BeBefore(generator.DefaultEnd);
        //}

    }
}
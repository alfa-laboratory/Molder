using Molder.Generator.Infrastructures;
using Molder.Generator.Models.Generators;
using Molder.Generator.Models.Providers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Xunit;
using Molder.Models.DateTimeHelpers;

namespace Molder.Generator.Tests
{
    /// <summary>
    /// Тесты проверки генераторов тестовых данных.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GeneratorTests
    {
        private readonly string _prefix = "test";
        private readonly IFakerGenerator generator = new FakerGenerator();

        public static IEnumerable<object[]> RandomDateTime()
        {
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2002, 1, 1) };
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2010, 1, 1) };
            yield return new object[] { new DateTime(2000, 1, 1), new DateTime(2100, 1, 1) };
        }
        
        [Theory]
        [InlineData(10), InlineData(5), InlineData(1)]
        public void GetRandomString_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = generator.String(expectedLength);
            randomString.Should().HaveLength(expectedLength);
        }

        [Theory]
        [InlineData(0), InlineData(-1)]
        public void GetRandomString_IncorrectLength_ReturnException(int expectedLength)
        {
            // Act 
            // Arrange

            Action act = () => generator.String(expectedLength);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"Expected length to be greater than 0 because The length must be positive, but found {expectedLength}.");
        }

        [Theory]
        [InlineData(10), InlineData(5), InlineData(1)]
        public void GetRandomNumbers_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = generator.Numbers(expectedLength);
            randomString.Should().HaveLength(expectedLength);
        }

        [Theory]
        [InlineData(0), InlineData(-1)]
        public void GetRandomNumbers_IncorrectLength_ReturnException(int expectedLength)
        {
            // Act 
            // Arrange

            Action act = () => generator.Numbers(expectedLength);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"Expected length to be greater than 0 because The length must be positive, but found {expectedLength}.");
        }

        [Theory]
        [InlineData(10), InlineData(5), InlineData(1)]
        public void GetRandomChars_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = generator.Chars(expectedLength);
            randomString.Should().HaveLength(expectedLength);
        }

        [Theory]
        [InlineData(0), InlineData(-1)]
        public void GetRandomChars_IncorrectLength_ReturnException(int expectedLength)
        {
            // Act 
            // Arrange

            Action act = () => generator.Chars(expectedLength);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"Expected length to be greater than 0 because The length must be positive, but found {expectedLength}.");
        }

        [Theory]
        [InlineData(10), InlineData(5), InlineData(1)]
        public void GetSentence_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = generator.Sentence(expectedLength);

            randomString.Split(" ").Should().HaveCount(expectedLength);
        }

        [Theory]
        [InlineData(0), InlineData(-1)]
        public void GetSentence_IncorrectLength_ReturnException(int expectedLength)
        {
            // Act 
            // Arrange

            Action act = () => generator.Sentence(expectedLength);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"Expected length to be greater than 0 because The length must be positive, but found {expectedLength}.");
        }

        [Theory]
        [InlineData(10), InlineData(5), InlineData(1)]
        public void GetRandomParagraph_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = generator.Paragraphs(expectedLength);
            randomString.Split("\n\n").Should().HaveCount(expectedLength);
        }

        [Theory]
        [InlineData(0), InlineData(-1)]
        public void GetRandomParagraph_IncorrectLength_ReturnException(int expectedLength)
        {
            // Act 
            // Arrange

            Action act = () => generator.Paragraphs(expectedLength);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"Expected length to be greater than 0 because The length must be positive, but found {expectedLength}.");
        }

        [Theory]
        [InlineData("ru"), InlineData("en")]
        public void SetLocale_Locale_ReturnLocale(string locale)
        {
            generator.Locale = locale;
            ((FakerGenerator)generator).ReloadLocale();
            var gen = generator.Get();
            gen.Locale.Should().Be(locale);
        }

        [Fact]
        public void GetDefaultLocale_ReturnLocaleEn()
        {
            var gen = generator.Get();
            gen.Locale.Should().Be(Constants.DEFAULT_LOCALE);
        }

        [Fact]
        public void GetLocale_ReturnLocale()
        {
            generator.Locale.Should().Be(Constants.DEFAULT_LOCALE);
        }

        [Fact]
        public void TryValidGuid()
        {
            var guid = generator.Guid();
            var isValid = Guid.TryParse(guid, out _);
            isValid.Should().BeTrue();
        }

        [Fact]
        public void GetCurrentDateTime_ReturnCurrentDateTime()
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1, 1, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);
            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;
            var dt = generator.Current();
            dt.Should().Be(fakeDate);
        }

        [Fact]
        public void GetRandomDateTime_ReturnDateTime()
        {
            var dt = generator.Between();
            dt.Should().BeAfter(Constants.START_DATETIME).And.BeBefore(Constants.LAST_DATETIME);
        }

        [Theory]
        [MemberData(nameof(RandomDateTime))]
        public void GetRandomDateTime_RandomDateTime_ReturnDateTime(DateTime start, DateTime end)
        {
            var dt = generator.Between(start, end);
            dt.Should().BeAfter(start).And.BeBefore(end);
        }

        [Theory]
        [InlineData("+7##########"), InlineData("+7###-###-##-##"), InlineData("+7 ### ### ## ##"), InlineData("+7(###)###-##-##")]
        public void GetRandomPhone_CorrectParams_ReturnRandomPhone(string expectedMask)
        {
            var randomPhone = generator.Phone(expectedMask);
            randomPhone.Should().HaveLength(expectedMask.Length);
        }

        [Theory]
        [InlineData("+7###########"), InlineData("+7 ### ### ## ## ## ##")]
        public void GetRandomPhone_OverTenParams_ReturnPhone(string expectedMask)
        {
            var randomPhone = generator.Phone(expectedMask);
            randomPhone.Should().HaveLength(expectedMask.Length);
        }

        [Theory]
        [InlineData("+7#"), InlineData("+7")]
        public void GetRandomPhone_LessTenParams_ReturnPhone(string expectedMask)
        {
            var randomPhone = generator.Phone(expectedMask);
            randomPhone.Should().HaveLength(expectedMask.Length);
        }

        [Theory]
        [InlineData("May"), InlineData("September")]
        public void GetMonth(string month)
        {
            // Act
            var bogus = new Mock<IBogusProvider>();
            bogus.Setup(o => o.Month())
                .Returns(month);
            ((FakerGenerator)generator).bogus.Value = bogus.Object;
            generator.Month().Should().Be(month);
        }

        [Fact]
        public void GetRandomMonth()
        {
            // Act
            var months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            // Arrange
            var month = generator.Month();

            // Assert
            months.Should().Contain(month);
        }

        [Theory]
        [InlineData("Sunday"), InlineData("Monday")]
        public void GetWeekday(string weekday)
        {
            // Act
            var bogus = new Mock<IBogusProvider>();
            bogus.Setup(o => o.Weekday())
                .Returns(weekday);
            ((FakerGenerator)generator).bogus.Value = bogus.Object;
            generator.Weekday().Should().Be(weekday);
        }

        [Fact]
        public void GetRandomWeekday()
        {
            // Act
            var weekdays = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            // Arrange
            var weekday = generator.Weekday();

            // Assert
            weekdays.Should().Contain(weekday);
        }

        [Fact]
        public void TryValidIp()
        {
            var ip = generator.Ip();
            var isValid = IPAddress.TryParse(ip, out _);
            isValid.Should().BeTrue();
        }

        [Fact]
        public void TryValidEmail()
        {
            var email = generator.Email("mail");
            var address = new System.Net.Mail.MailAddress(email).Address;
            address.Should().Be(email);
        }

        [Fact]
        public void TryValidUrl()
        {
            var url = generator.Url();
            var isValid = Uri.TryCreate(url, UriKind.Absolute, out _);
            isValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("Ivan"), InlineData("Max")]
        public void GetFirstName_ReturnFirstName(string name)
        {
            // Act
            var bogus = new Mock<IBogusProvider>();
            bogus.Setup(o => o.FirstName())
                .Returns(name);
            ((FakerGenerator)generator).bogus.Value = bogus.Object;
            generator.FirstName().Should().Be(name);
        }

        [Theory]
        [InlineData("Ivanov"), InlineData("Petrov")]
        public void GetLastName_ReturnLastName(string lastname)
        {
            // Act
            var bogus = new Mock<IBogusProvider>();
            bogus.Setup(o => o.LastName())
                .Returns(lastname);
            ((FakerGenerator)generator).bogus.Value = bogus.Object;
            generator.LastName().Should().Be(lastname);
        }


        [Theory]
        [InlineData("Ivanov Ivan"), InlineData("Petrov Petr")]
        public void GetFullName_ReturnFullName(string fullname)
        {
            // Act
            var bogus = new Mock<IBogusProvider>();
            bogus.Setup(o => o.FullName())
                .Returns(fullname);
            ((FakerGenerator)generator).bogus.Value = bogus.Object;
            generator.FullName().Should().Be(fullname);
        }

        [Theory]
        [InlineData(10), InlineData(5), InlineData(1)]
        public void GetRandomString2_CorrectParams_ReturnRandomString(int expectedLength)
        {
            var randomString = generator.String2(expectedLength);
            randomString.Should().HaveLength(expectedLength);
        }

        [Theory]
        [InlineData(0), InlineData(-1)]
        public void GetRandomString2_IncorrectLength_ReturnException(int expectedLength)
        {
            // Act 
            // Arrange

            Action act = () => generator.String2(expectedLength);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage($"Expected length to be greater than 0 because The length must be positive, but found {expectedLength}.");
        }

        public static IEnumerable<object[]> RandomIntMinMax()
        {
            yield return new object[] { 1, 10 };
            yield return new object[] { 1, 100 };
            yield return new object[] { 10, 100 };
        }

        [Theory]
        [MemberData(nameof(RandomIntMinMax))]
        public void GetRandomInt_CorrectParams_ReturnRandomString(int min, int max)
        {
            var randomInt = generator.IntFromTo(min, max);
            randomInt.Should().BeGreaterOrEqualTo(min).And.BeLessOrEqualTo(max);
        }

        public static IEnumerable<object[]> IncorrectIntMinMax()
        {
            yield return new object[] { 10, 10 };
            yield return new object[] { 1000, 100 };
            yield return new object[] { 10, -1 };
        }

        [Theory]
        [MemberData(nameof(IncorrectIntMinMax))]
        public void GetRandomInt_IncorrectMinMax_ReturnException(int min, int max)
        {
            // Act 
            // Arrange

            Action act = () => generator.IntFromTo(min, max);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage("*The maximum value cannot be less than the minimum*");
        }

        public static IEnumerable<object[]> RandomDoubleMinMax()
        {
            yield return new object[] { 1, 10, 1 };
            yield return new object[] { 1, 100, 2 };
            yield return new object[] { 10, 100, 3 };
            yield return new object[] { 1, 2, 5 };
        }

        [Theory]
        [MemberData(nameof(RandomDoubleMinMax))]
        public void GetRandomDouble_CorrectParams_ReturnRandomDouble(double min, double max, int limit)
        {
            var randomInt = generator.DoubleFromTo(min, max, limit);
            randomInt.Should().BeGreaterOrEqualTo(min).And.BeLessOrEqualTo(max);
        }

        public static IEnumerable<object[]> IncorrectDoubleMinMax()
        {
            yield return new object[] { 10, 10, 1, "*The maximum value cannot be less than the minimum*" };
            yield return new object[] { 1000, 100, 2, "*The maximum value cannot be less than the minimum*" };
            yield return new object[] { 10, -1, 3, "*The maximum value cannot be less than the minimum*" };
            yield return new object[] { 1, 2, 0, "*The decimal place limit cannot be negative*" };
            yield return new object[] { 1, 2, -1, "*The decimal place limit cannot be negative*" };
        }

        [Theory]
        [MemberData(nameof(IncorrectDoubleMinMax))]
        public void GetRandomDouble_IncorrectMinMaxLimit_ReturnException(double min, double max, int limit, string message)
        { 
            // Act 
            // Arrange

            Action act = () => generator.DoubleFromTo(min, max, limit);

            // Assert
            act
              .Should().Throw<Exception>()
              .WithMessage(message);
        }

        [Theory]
        [InlineData(1, 1, 1990), InlineData(15, 6, 1990), InlineData(31, 12, 1990)]
        public void GetDate_CorrectParams_ReturnDate(int day, int month, int year)
        {
            var expDt = new DateTime(year, month, day);
            var dt = generator.GetDate(day, month, year);

            dt.Should().Be(expDt);
        }

        [Theory]
        [InlineData(0, 1, 1990), InlineData(40, 6, 1990), InlineData(31, 13, 1990)]
        public void GetDate_InCorrectParams_ReturnNull(int day, int month, int year)
        {
            var dt = generator.GetDate(day, month, year);

            dt.Should().BeNull();
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
        public void GetDateTime_InCorrectParams_ReturnNull(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds)
        {
            var dt = generator.GetDateTime(day, month, year, hours, minutes, seconds, milliseconds);

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

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = generator.GetDate(day, 0, 0, true).Value;

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

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = generator.GetDate(0, month, 0, true).Value;

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

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = generator.GetDate(0, 0, year, true).Value;

            dt.Year.Should().Be(nYear);
            dt.Month.Should().Be(1);
            dt.Day.Should().Be(1);
        }

        [Theory]
        [InlineData(0, 0, -1), InlineData(0, -1, 0), InlineData(-1, 0, 0)]
        public void GetOtherFutureDate_InCorrectParams_ReturnNull(int day, int month, int year)
        {
            var dt = generator.GetDate(day, month, year, true);
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

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = generator.GetDate(day, 0, 0, false).Value;

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

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = generator.GetDate(0, month, 0, false).Value;

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

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var dt = generator.GetDate(0, 0, year, false).Value;

            dt.Year.Should().Be(nYear);
            dt.Month.Should().Be(1);
            dt.Day.Should().Be(1);
        }

        [Theory]
        [InlineData(0, 0, -1), InlineData(0, -1, 0), InlineData(-1, 0, 0)]
        public void GetOtherPastDate_InCorrectParams_ReturnNull(int day, int month, int year)
        {
            var dt = generator.GetDate(day, month, year, false);
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
            var dt = generator.GetDate(day, 0, 0, false, date).Value;

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
            var dt = generator.GetDate(day, 0, 0, true, date).Value;

            dt.Year.Should().Be(2000);
            dt.Month.Should().Be(nMouth);
            dt.Day.Should().Be(nDay);
        }

        [Fact]
        public void GetSoonDateTime_ReturnSoonDateTime()
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var soon = generator.Soon();
            soon.Should().BeAfter(fakeDate);
        }

        [Theory]
        [InlineData(2), InlineData(5), InlineData(10)]
        public void GetSoonDateTime_WithDays_ReturnSoonDateTime(int day)
        {
            var expectedBefore = new DateTime(2000, 1, day + 1);

            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var soon = generator.Soon(day);
            soon.Should().BeAfter(fakeDate).And.BeBefore(expectedBefore);
        }

        public static IEnumerable<object[]> CustomSoonDateTime()
        {
            yield return new object[] { new DateTime(2000, 1, 1) };
            yield return new object[] { new DateTime(2010, 1, 1) };
            yield return new object[] { new DateTime(2100, 1, 1) };
        }

        [Theory]
        [MemberData(nameof(CustomSoonDateTime))]
        public void GetSoonDate_CorrectDate_ReturnSoonDate(DateTime date)
        {
            var soon = generator.Soon(refDate: date);
            soon.Should().BeAfter(date);
        }

        [Fact]
        public void GetPastDateTime_ReturnSoonDateTime()
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var soon = generator.Past();
            soon.Should().BeBefore(fakeDate);
        }

        [Theory]
        [InlineData(2), InlineData(5), InlineData(10)]
        public void GetPastDateTime_WithDays_ReturnSoonDateTime(int year)
        {
            var nYear = 2000 - year;
            var expectedAfter = new DateTime(nYear, 1, 1);

            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var soon = generator.Past();
            soon.Should().BeAfter(expectedAfter).And.BeBefore(fakeDate);
        }

        public static IEnumerable<object[]> CustomPastDate()
        {
            yield return new object[] { new DateTime(2000, 1, 1) };
            yield return new object[] { new DateTime(2010, 1, 1) };
            yield return new object[] { new DateTime(2100, 1, 1) };
        }

        [Theory]
        [MemberData(nameof(CustomPastDate))]
        public void GetPastDate_CorrectDate_ReturnSoonDate(DateTime date)
        {
            var soon = generator.Past(refDate: date);
            soon.Should().BeBefore(date);
        }

        [Fact]
        public void GetFutureDateTime_ReturnSoonDateTime()
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var soon = generator.Future();
            soon.Should().BeAfter(fakeDate);
        }

        [Theory]
        [InlineData(2), InlineData(5), InlineData(10)]
        public void GetFutureDateTime_WithDays_ReturnSoonDateTime(int year)
        {
            var nYear = 2000 + year;
            var expectedBefore = new DateTime(nYear, 1, 1);

            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2000, 1, 1);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow())
                .Returns(fakeDate);

            ((FakerGenerator)generator).DateTimeHelper.Value = mockDateTimeHelper.Object;

            var soon = generator.Future();
            soon.Should().BeBefore(expectedBefore).And.BeAfter(fakeDate);
        }

        public static IEnumerable<object[]> CustomFutureDate()
        {
            yield return new object[] { new DateTime(2000, 1, 1) };
            yield return new object[] { new DateTime(2010, 1, 1) };
            yield return new object[] { new DateTime(2100, 1, 1) };
        }

        [Theory]
        [MemberData(nameof(CustomFutureDate))]
        public void GetFutureDate_CorrectDate_ReturnSoonDate(DateTime date)
        {
            var soon = generator.Future(refDate: date);
            soon.Should().BeAfter(date);
        }
    }
}

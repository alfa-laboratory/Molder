using Bogus;
using Molder.Generator.Infrastructures;
using Molder.Generator.Models.Providers.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Generator.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class BogusProvider : IBogusProvider
    {
        private readonly Faker faker;

        // Locale
        public BogusProvider(string locale)
        {
            faker = new Faker(locale);
        }

        public Faker Get()
        {
            return faker;
        }

        public string Guid()
        {
            return faker.Random.Guid().ToString();
        }

        public DateTime Between(DateTime? start = null, DateTime? end = null)
        {
            start = start ?? Constants.START_DATETIME;
            end = end ?? Constants.LAST_DATETIME;
            return faker.Date.Between
                ((DateTime)start, (DateTime)end);
        }

        public DateTime Soon(int days = 1, DateTime? refDate = null)
        {
            return faker.Date.Soon(days, refDate);
        }

        public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
        {
            return faker.Date.Future(yearsToGoForward, refDate);
        }

        public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
        {
            return faker.Date.Past(yearsToGoBack, refDate);
        }

        public string Numbers(int len)
        {
            return new Randomizer().Replace(new string(Constants.NUMBER_CHAR, len));
        }

        public string Chars(int len)
        {
            return new Randomizer().Replace(new string(Constants.LETTER_CHAR, len));
        }

        public string Phone(string format = Constants.PHONE_FORMAT)
        {
            return faker.Phone.PhoneNumber(format);
        }

        public string String2(int len)
        {
            return faker.Random.String2(len);
        }

        public int IntFromTo(int min, int max)
        {
            return faker.Random.Int(min, max);
        }

        public double DoubleFromTo(double min = 0, double max = 1, int limit = 10)
        {
            var number = faker.Random.Double(min, max);
            return Math.Round(number, limit);
        }

        public string Month()
        {
            return faker.Date.Month();
        }

        public string Weekday()
        {
            return faker.Date.Weekday();
        }

        public string Email(string provider)
        {
            return faker.Internet.Email(provider: provider);
        }

        public string Ip()
        {
            return faker.Internet.Ip();
        }

        public string Url()
        {
            return faker.Internet.Url();
        }

        public string Sentence(int count)
        {
            return faker.Lorem.Sentence(count);
        }

        public string Paragraphs(int min)
        {
            return faker.Lorem.Paragraphs(min);
        }

        public string FirstName()
        {
            return faker.Name.FirstName();
        }

        public string LastName()
        {
            return faker.Name.LastName();
        }

        public string FullName()
        {
            return faker.Name.FullName();
        }
    }
}

using Bogus;
using Molder.Generator.Extensions;
using Molder.Generator.Infrastructures;
using Molder.Generator.Models.Providers;
using Molder.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Molder.Models.DateTimeHelpers;
using System.Threading;

namespace Molder.Generator.Models.Generators
{
    public class FakerGenerator : IFakerGenerator
    {
        private AsyncLocal<string> _locale = new AsyncLocal<string> { Value = Constants.DEFAULT_LOCALE };

        public string Locale
        {
            get
            {
                return _locale.Value;
            }
            set
            {
                _locale.Value = value;
            }
        }

        public void ReloadLocale()
        {
            bogus.Value = new BogusProvider(Locale);
        }

        public FakerGenerator()
        {
            bogus.Value = new BogusProvider(Locale);
        }

        public AsyncLocal<IBogusProvider> bogus = new AsyncLocal<IBogusProvider> { Value = null };

        public AsyncLocal<IDateTimeHelper> DateTimeHelper = new AsyncLocal<IDateTimeHelper> { Value = new DateTimeHelper() };

        public Faker Get()
        {
            return bogus.Value.Get();
        }

        public string Guid()
        {
            return bogus.Value.Guid();
        }

        public DateTime Current()
        {
            return DateTimeHelper.Value.GetDateTimeNow();
        }

        public DateTime? GetDate(int day, int month, int year)
        {
            try
            {
                var dt = new DateTime(year, month, day, 0, 0, 0, 0);
                return dt;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Log.Logger().LogWarning($"Date {day}.{month}.{year} is incorrect. Exception is {ex.Message}");
                return null;
            }
        }

        public DateTime? GetDate(int day, int month, int year, bool future = true, DateTime? refDate = null)
        {
            int trigger = 1;
            if (!future) trigger = -1;

            refDate = refDate ?? DateTimeHelper.Value.GetDateTimeNow();

            if (day >= 0 && month >= 0 && year >= 0)
            {
                try
                {
                    return refDate?
                        .AddDays((trigger) * day)
                        .AddMonths((trigger) * month)
                        .AddYears((trigger) * year);
                }catch(ArgumentOutOfRangeException ex)
                {
                    Log.Logger().LogWarning($"Date {day}.{month}.{year} is incorrect. Exception is {ex.Message}");
                    return null;
                }
            }
            Log.Logger().LogWarning($"Use only positive numbers when specifying a date other than the specified date.");
            return null;
        }

        public DateTime? GetDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds = 0)
        {
            try
            {
                var dt = new DateTime(year, month, day, hours, minutes, seconds, milliseconds);
                return dt;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Logger().LogWarning($"Time {hours}:{minutes}:{seconds}.{milliseconds} is incorrect.");
                return null;
            }
        }

        public DateTime Between(DateTime? start = null, DateTime? end = null)
        {
            return bogus.Value.Between(start, end);
        }

        public DateTime Soon(int days = 1, DateTime? refDate = null)
        {
            days.Check();
            refDate = refDate ?? DateTimeHelper.Value.GetDateTimeNow();
            return bogus.Value.Soon(days, refDate);
        }

        public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
        {
            yearsToGoForward.Check();
            refDate = refDate ?? DateTimeHelper.Value.GetDateTimeNow();
            return bogus.Value.Future(yearsToGoForward, refDate);
        }

        public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
        {
            yearsToGoBack.Check();
            refDate = refDate ?? DateTimeHelper.Value.GetDateTimeNow();
            return bogus.Value.Past(yearsToGoBack, refDate);
        }

        public string Numbers(int length)
        {
            length.Check();
            return bogus.Value.Numbers(length);
        }

        public string Chars(int length)
        {
            length.Check();
            return bogus.Value.Chars(length);
        }

        public string Phone(string format = Constants.PHONE_FORMAT)
        {
            return bogus.Value.Phone(format);
        }

        public string String(int length)
        {
            length.Check();
            var code = Enumerable.Repeat(Constants.RANDOM_CHAR.ToString(), length).Aggregate((s, n) => s + n);
            return new Randomizer().Replace(code);
        }

        public string String2(int length)
        {
            length.Check();
            return bogus.Value.String2(length);
        }

        public int IntFromTo(int min, int max)
        {
            max.BeGreaterThan(min);
            return bogus.Value.IntFromTo(min, max);
        }

        public double DoubleFromTo(double min = 0, double max = 1, int limit = 10)
        {
            max.BeGreaterThan(min);
            CheckExtension.Limit(limit);
            return bogus.Value.DoubleFromTo(min, max, limit);
        }

        public string Month()
        {
            return bogus.Value.Month();
        }

        public string Weekday()
        {
            return bogus.Value.Weekday();
        }

        public string Email(string provider)
        {
            return bogus.Value.Email(provider);
        }

        public string Ip()
        {
            return bogus.Value.Ip();
        }

        public string Url()
        {
            return bogus.Value.Url();
        }

        public string Sentence(int count)
        {
            count.Check();
            return bogus.Value.Sentence(count);
        }

        public string Paragraphs(int min)
        {
            min.Check();
            return bogus.Value.Paragraphs(min);
        }

        public string FirstName()
        {
            return bogus.Value.FirstName();
        }

        public string LastName()
        {
            return bogus.Value.LastName();
        }

        public string FullName()
        {
            return bogus.Value.FullName();
        }
    }
}
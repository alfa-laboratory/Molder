﻿using Bogus;
using EvidentInstruction.Generator.Extensions;
using EvidentInstruction.Generator.Infrastructures;
using EvidentInstruction.Generator.Models.Generators.Interfaces;
using EvidentInstruction.Generator.Models.Providers;
using EvidentInstruction.Generator.Models.Providers.Interfaces;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models;
using EvidentInstruction.Models.DateTimeHelpers.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace EvidentInstruction.Generator.Models.Generators
{
    public class FakerGenerator : IFakerGenerator
    {
        [ThreadStatic]
        private string _locale = Constants.DEFAULT_LOCALE;

        public string Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        public void ReloadLocale()
        {
            bogus = new BogusProvider(_locale);
        }

        public FakerGenerator()
        {
            bogus = new BogusProvider(_locale);
        }

        [ThreadStatic]
        public IBogusProvider bogus = null;

        public IDateTimeHelper DateTimeHelper = new DateTimeHelper();

        public Faker Get()
        {
            return bogus.Get();
        }

        public string Guid()
        {
            return bogus.Guid();
        }

        public DateTime Current()
        {
            return DateTimeHelper.GetDateTimeNow();
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

            refDate = refDate ?? DateTimeHelper.GetDateTimeNow();

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
            return bogus.Between(start, end);
        }

        public DateTime Soon(int days = 1, DateTime? refDate = null)
        {
            days.Check();
            refDate = refDate ?? DateTimeHelper.GetDateTimeNow();
            return bogus.Soon(days, refDate);
        }

        public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
        {
            yearsToGoForward.Check();
            refDate = refDate ?? DateTimeHelper.GetDateTimeNow();
            return bogus.Future(yearsToGoForward, refDate);
        }

        public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
        {
            yearsToGoBack.Check();
            refDate = refDate ?? DateTimeHelper.GetDateTimeNow();
            return bogus.Past(yearsToGoBack, refDate);
        }

        public string Numbers(int length)
        {
            length.Check();
            return bogus.Numbers(length);
        }

        public string Chars(int length)
        {
            length.Check();
            return bogus.Chars(length);
        }

        public string Phone(string format = Constants.PHONE_FORMAT)
        {
            return bogus.Phone(format);
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
            return bogus.String2(length);
        }

        public int IntFromTo(int min, int max)
        {
            max.BeGreaterThan(min);
            return bogus.IntFromTo(min, max);
        }

        public double DoubleFromTo(double min = 0, double max = 1, int limit = 10)
        {
            max.BeGreaterThan(min);
            CheckExtension.Limit(limit);
            return bogus.DoubleFromTo(min, max, limit);
        }

        public string Month()
        {
            return bogus.Month();
        }

        public string Weekday()
        {
            return bogus.Weekday();
        }

        public string Email(string provider)
        {
            return bogus.Email(provider);
        }

        public string Ip()
        {
            return bogus.Ip();
        }

        public string Url()
        {
            return bogus.Url();
        }

        public string Sentence(int count)
        {
            count.Check();
            return bogus.Sentence(count);
        }

        public string Paragraphs(int min)
        {
            min.Check();
            return bogus.Paragraphs(min);
        }

        public string FirstName()
        {
            return bogus.FirstName();
        }

        public string LastName()
        {
            return bogus.LastName();
        }

        public string FullName()
        {
            return bogus.FullName();
        }
    }
}
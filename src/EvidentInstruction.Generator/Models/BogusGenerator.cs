using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace EvidentInstruction.Generator.Models
{
    public class BogusGenerator : IGenerator
    {
        public BogusProvider bogusProvider = new BogusProvider(Constants.english);
        public BogusProvider ruBogusProvider = new BogusProvider(Constants.russian);
        public DateTimeHelper dateTimeHelper = new DateTimeHelper();

        public DateTime GetCurrentDateTime()
        {
            return dateTimeHelper.GetDateTimeNow();
        }

        public DateTime? GetDate(int day, int month, int year)
        {
            try
            {
                var dt = new DateTime(year, month, day, 0, 0, 0, 0);
                return dt;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Logger.Warning($"Date {day}.{month}.{year} is incorrect.");
                return null;
            }
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
                Log.Logger.Warning($"Time {hours}:{minutes}:{seconds}.{milliseconds} is incorrect.");
                return null;
            }
        }

        public string GetGuid()
        {
            return bogusProvider.Guid();
        }

        public DateTime? GetOtherDate(int day, int month, int year, DateTime? date = null)
        {
            DateTime? dt;
            if (date == null)
            {
                dt = dateTimeHelper.GetDateTimeNow();
            }
            else
            {
                dt = date;
            }

            return dt?
                .AddDays(day)
                .AddMonths(month)
                .AddYears(year);
        }

        public DateTime? GetRandomDateTime(DateTime? start = null, DateTime? end = null)
        {
            return bogusProvider.Between(start, end);
        }

        public string GetRandomStringNumbers(int len, string prefix, string postfix)
        {
            CorrectParams.ForString(len, prefix, postfix);
            return prefix + bogusProvider.DoubleNumbers(len - prefix.Length - postfix.Length, 0).ToString("F0") + postfix;
        }

        public string GetRandomPhone(string mask = Constants.phoneMask)
        {
            return bogusProvider.Phone(mask);
        }

        public string GetRandomString(int len, string prefix, string postfix, string locale)
        {
            CorrectParams.ForString(len, prefix, postfix);
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return prefix + bogusProvider.String(len - prefix.Length - postfix.Length) + postfix;
            }
            return prefix + ruBogusProvider.RuString(len - prefix.Length - postfix.Length) + postfix;
        }

        public string GetRandomChars(int len, string prefix, string postfix, string locale)
        {
            CorrectParams.ForString(len, prefix, postfix);
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return prefix + bogusProvider.Chars(len - prefix.Length - postfix.Length) + postfix;
            }
            return prefix + ruBogusProvider.RuChars(len - prefix.Length - postfix.Length) + postfix;
        }

        public DateTime? GetTime(int hours, int minutes, int seconds, int milliseconds = 0)
        {
            try
            {
                var dt = dateTimeHelper.GetDateTimeNow();
                var tm = new DateTime(dt.Year, dt.Month, dt.Day, hours, minutes, seconds, milliseconds);
                return tm;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Logger.Warning($"Time {hours}:{minutes}:{seconds}.{milliseconds} is incorrect.");
                return null;
            }
        }

        public int GetRandomInt(int len = 0, int min = 0, int max = 0)
        {
            CorrectParams.ForNumbers(len, min, max);
            if (len == 0)
            {
                return bogusProvider.IntFromTo(min, max);
            }
            return bogusProvider.IntNumbers(len);
        }

        public double GetRandomDouble(int limit, int len = 0, double min = 0, double max = 0)
        {
            CorrectParams.ForNumbers(limit, len, min, max);
            if (len == 0)
            {
                return bogusProvider.DoubleFromTo(min, max, limit);
            }
            return bogusProvider.DoubleNumbers(len, limit);
        }

        public string GetMonth(string locale)
        {
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return bogusProvider.Month();
            }
            return ruBogusProvider.Month();
        }

        public string GetWeekday(string locale)
        {
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return bogusProvider.Weekday();
            }
            return ruBogusProvider.Weekday();
        }

        public string GetEmail(string domen = "gmail.com")
        {
            domen.Should().NotBeNullOrWhiteSpace("Введите домен.");
            return bogusProvider.Email(domen);
        }

        public string GetIp()
        {
            return bogusProvider.Ip();
        }

        public string GetUrl()
        {
            return bogusProvider.Url();
        }

        public string GetSentence(int count, string locale)
        {
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return bogusProvider.Sentence(count);
            }
            return ruBogusProvider.Sentence(count);
        }

        public string GetParagraph(int min, string locale)
        {
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return bogusProvider.Paragraph(min);
            }
            return ruBogusProvider.Paragraph(min);
        }

        public string GetFirstName(string locale)
        {
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return bogusProvider.FirstName();
            }
            return ruBogusProvider.FirstName();
        }

        public string GetLastName(string locale)
        {
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return bogusProvider.LastName();
            }
            return ruBogusProvider.LastName();
        }

        public string GetFullName(string locale)
        {
            CorrectParams.ForLocale(locale);
            if (locale == Constants.english)
            {
                return bogusProvider.FullName();
            }
            return ruBogusProvider.FullName();
        }
    }
}

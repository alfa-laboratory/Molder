using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using FluentAssertions;
using System;

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
        //Как это тестировать?
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
            switch (locale)
            {
                case Constants.english:
                    return prefix + bogusProvider.String(len - prefix.Length - postfix.Length) + postfix;
                case Constants.russian:
                    return prefix + ruBogusProvider.RuString(len - prefix.Length - postfix.Length) + postfix;
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }

        public string GetRandomChars(int len, string prefix, string postfix, string locale)
        {
            CorrectParams.ForString(len, prefix, postfix);
            switch (locale)
            {
                case Constants.english:
                    return prefix + bogusProvider.Chars(len - prefix.Length - postfix.Length) + postfix;
                case Constants.russian:
                    return prefix + ruBogusProvider.RuChars(len - prefix.Length - postfix.Length) + postfix;
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
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

        public int? GetRandomInt(int len = 0, int min = 0, int max = 0)
        {
            if (len == 0 && min == 0 && max == 0)
            {
                Log.Logger.Warning($"Every parameter cannot equal 0.");
                return null;
            }
            if (len == 0)
            {
                return bogusProvider.IntFromTo(min, max);
            }
            return bogusProvider.IntNumbers(len);
        }

        public double? GetRandomDouble(int limit, int len = 0, double min = 0, double max = 0)
        {
            if (len == 0 && min == 0 && max == 0)
            {
                Log.Logger.Warning($"Every parameter cannot equal 0.");
                return null;
            }
            if (len == 0)
            {
                return bogusProvider.DoubleFromTo(min, max, limit);
            }
            return bogusProvider.DoubleNumbers(len, limit);
        }

        public string GetMonth(string locale)
        {
            switch (locale)
            {
                case Constants.english:
                    return bogusProvider.Month();
                case Constants.russian:
                    return ruBogusProvider.Month();
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }

        public string GetWeekday(string locale)
        {
            switch (locale)
            {
                case Constants.english:
                    return bogusProvider.Weekday();
                case Constants.russian:
                    return ruBogusProvider.Weekday();
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }

        public string GetEmail(string domen = "gmail.com")
        {
            return ruBogusProvider.Email(domen);
        }

        public string GetIp()
        {
            return ruBogusProvider.Ip();
        }

        public string GetUrl()
        {
            return ruBogusProvider.Url();
        }

        public string GetSentence(int count, string locale)
        {
            switch (locale)
            {
                case Constants.english:
                    return bogusProvider.Sentence(count);
                case Constants.russian:
                    return ruBogusProvider.Sentence(count);
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }

        public string GetParagraph(int min, string locale)
        {
            switch (locale)
            {
                case Constants.english:
                    return bogusProvider.Paragraph(min);
                case Constants.russian:
                    return ruBogusProvider.Paragraph(min);
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }

        public string GetFirstName(string locale)
        {
            switch (locale)
            {
                case Constants.english:
                    return bogusProvider.FirstName();
                case Constants.russian:
                    return ruBogusProvider.FirstName();
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }

        public string GetLastName(string locale)
        {
            switch (locale)
            {
                case Constants.english:
                    return bogusProvider.LastName();
                case Constants.russian:
                    return ruBogusProvider.LastName();
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }

        public string GetFullName(string locale)
        {
            switch (locale)
            {
                case Constants.english:
                    return bogusProvider.FullName();
                case Constants.russian:
                    return ruBogusProvider.FullName();
                default:
                    Log.Logger.Warning("Choose russian or english language.");
                    return null;
            }
        }
    }
}

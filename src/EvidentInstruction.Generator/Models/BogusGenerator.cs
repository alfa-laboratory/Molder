using EvidentInstruction.Helpers;
using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace EvidentInstruction.Generator.Models
{
    public class BogusGenerator : IGenerator
    {
        public BogusProvider bogusProvider = new BogusProvider();
        public BogusProvider ruBogusProvider = new BogusProvider("ru");
        public DateTimeHelper dateTimeHelper;

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
            return prefix + bogusProvider.DoubleNumbers(len - prefix.Length - postfix.Length, 0).ToString() + postfix;
        }

        public string GetRandomPhone(string mask)
        {
            return bogusProvider.Phone(mask);
        }

        public string GetRandomString(int len, string prefix, string postfix, bool isEng = true)
        {
            if (isEng)
            {
                return prefix + bogusProvider.String(len - prefix.Length - postfix.Length) + postfix;
            }
            return prefix + ruBogusProvider.String(len - prefix.Length - postfix.Length) + postfix;
        }

        public string GetRandomChars(int len, string prefix, string postfix, bool isEng = true)
        {
            if (isEng)
            {
                return prefix + bogusProvider.Chars(len - prefix.Length - postfix.Length) + postfix;
            }
            return prefix + ruBogusProvider.Chars(len - prefix.Length - postfix.Length) + postfix;
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
            if (len == 0)
            {
                return bogusProvider.IntFromTo(min, max);
            }
            return bogusProvider.IntNumbers(len);
        }

        public double GetRandomDouble(int limit, int len = 0, double min = 0, double max = 0)
        {
            if (len == 0)
            {
                return bogusProvider.DoubleFromTo(min, max, limit);
            }
            return bogusProvider.DoubleNumbers(len, limit);
        }

        public string GetMonth(bool isEng = true)
        {
            if (isEng)
            {
                return bogusProvider.Month();
            }
            return ruBogusProvider.Month();
        }

        public string GetWeekday(bool isEng = true)
        {
            if (isEng)
            {
                return bogusProvider.Weekday();
            }
            return ruBogusProvider.Weekday();
        }

        public string GetEmail(string provider = "gmail.com")
        {
            return ruBogusProvider.Email(provider);
        }

        public string GetIp()
        {
            return ruBogusProvider.Ip();
        }

        public string GetUrl()
        {
            return ruBogusProvider.Url();
        }

        public string GetSentence(int count, bool isEng = true)
        {
            if (isEng)
            {
                return bogusProvider.Sentence(count);
            }
            return ruBogusProvider.Sentence(count);
        }

        public string GetParagraph(int min, bool isEng = true)
        {
            if (isEng)
            {
                return bogusProvider.Paragraph(min);
            }
            return ruBogusProvider.Paragraph(min);
        }

        public string GetFirstName(bool isEng = true)
        {
            if (isEng)
            {
                return bogusProvider.FirstName();
            }
            return ruBogusProvider.FirstName();
        }

        public string GetLastName(bool isEng = true)
        {
            if (isEng)
            {
                return bogusProvider.LastName();
            }
            return ruBogusProvider.LastName();
        }

        public string GetFullName(bool isEng = true)
        {
            if (isEng)
            {
                return bogusProvider.FullName();
            }
            return ruBogusProvider.FullName();
        }
    }
}

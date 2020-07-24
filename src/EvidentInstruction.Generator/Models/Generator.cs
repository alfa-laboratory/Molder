using EvidentInstruction.Helpers;
using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace EvidentInstruction.Generator.Models
{
    public class Generator : IGenerator
    {
        private readonly BogusProvider bogusProvider;
        public Generator(BogusProvider bogusProvider)
        {
            this.bogusProvider = bogusProvider;
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
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
                dt = DateTime.Now;
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

        public string GetRandomNumbers(int len, string prefix)
        {
            return prefix + bogusProvider.Numbers(len - prefix.Length);
        }

        public string GetRandomPhone(string mask)
        {
            return bogusProvider.Phone(mask);
        }

        public string GetRandomString(int len, string prefix)
        {
            return prefix + bogusProvider.String(len - prefix.Length);
        }

        public string GetRandomChars(int len, string prefix)
        {
            return prefix + bogusProvider.Chars(len - prefix.Length);
        }

        public DateTime? GetTime(int hours, int minutes, int seconds, int milliseconds = 0)
        {
            try
            {
                var dt = DateTime.Now;
                var tm = new DateTime(dt.Year, dt.Month, dt.Day, hours, minutes, seconds, milliseconds);
                return tm;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Logger.Warning($"Time {hours}:{minutes}:{seconds}.{milliseconds} is incorrect.");
                return null;
            }
        }
    }
}

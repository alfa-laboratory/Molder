using EvidentInstruction.Exceptions;
using EvidentInstruction.Models;
using EvidentInstruction.Models.DateTimeHelpers.Interfaces;
using System;

namespace EvidentInstruction.Helpers
{
    public static class Generator
    {
        private static readonly Random Random = new Random((int)DateTime.UtcNow.AddYears(-2000).Ticks);

        public static DateTime DefaultStart { get; } = new DateTime(1900, 01, 01);
        public static DateTime DefaultEnd { get; } = new DateTime(2100, 12, 31);

        public static IDateTimeHelper DateTimeHelper = new DateTimeHelper();

        public static string UpperChars { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string LowerChars { get; } = "abcdefghijklmnopqrstuvwxyz";
        public static string Numbers { get; } = "0123456789";

        public static string GetRandomString(int len, string prefix)
        {
            return prefix != string.Empty ? prefix + RandomString(len, UpperChars + LowerChars + Numbers) : RandomString(len, UpperChars + LowerChars + Numbers);
        }
        public static string GetRandomChars(int len, string prefix)
        {
            return prefix != string.Empty ? prefix + RandomString(len, UpperChars + LowerChars) : RandomString(len, UpperChars + LowerChars);
        }
        public static string GetRandomNumbers(int len, string prefix, bool withZero = true)
        {
            string numbers = withZero ? Numbers : Numbers.Remove(0, 1);
            return prefix != string.Empty ? prefix + RandomString(len, numbers) : RandomString(len, numbers);
        }
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static string GetRandomPhone(string mask)
        {
            var count = 0;
            var str = string.Empty;
            foreach (var ch in mask)
            {
                if(ch == ' ' || ch == '-' || ch == '+' || ch == '(' || ch == ')')
                {
                    str += ch;
                }
                else
                {
                    if(count > 10)
                    {
                        throw new FormatException("Phone number can have up to 10 digits");
                    }

                    if(ch == '0' || ch == '1' || ch == '2' || ch == '3' || ch == '4'
                        || ch == '5' || ch == '6' || ch == '7' || ch == '8' || ch == '9')
                    {
                        str += ch;
                    }
                    else
                    {
                        str += Random.Next(0, 9);
                    }

                    count++;
                }
            }

            if(count < 3)
            {
                throw new FormatException("Номер телефона должен содержать не менее 10 цифр.");
            }

            return str;
        }

        public static DateTime? GetDate(int day, int month, int year)
        {
            try
            {
                var dt = new DateTime(year, month, day, 0, 0, 0, 0);
                return dt;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Logger.Warning($"Дата {day}.{month}.{year} не корректна.");
                return null;
            }
        }
        public static DateTime? GetTime(int hours, int minutes, int seconds, int milliseconds = 0)
        {
            try
            {
                var dt = DateTime.Now;
                var tm = new DateTime(dt.Year, dt.Month, dt.Day, hours, minutes, seconds, milliseconds);
                return tm;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Logger.Warning($"Время {hours}:{minutes}:{seconds}.{milliseconds} не корректно.");
                return null;
            }
        }
        public static DateTime? GetDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds = 0)
        {
            try
            {
                var dt = new DateTime(year, month, day, hours, minutes, seconds, milliseconds);
                return dt;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Logger.Warning($"Время {hours}:{minutes}:{seconds}.{milliseconds} не корректно.");
                return null;
            }
        }
        public static DateTime GetCurrentDateTime()
        {
            var dt = DateTimeHelper.GetDateTimeNow();
            return dt;
        }
        public static DateTime? GetOtherDate(int day, int month, int year, bool future = true, DateTime? date = null)
        {
            int trigger = 1;
            if (!future) trigger = -1;

            DateTime? dt = DefaultStart;
            if(date == null)
            {
                dt = DateTimeHelper.GetDateTimeNow();
            }
            else
            {
                dt = date;
            }

            if (day >= 0 && month >= 0 && year >= 0)
            {
                return dt?
                    .AddDays((trigger) * day)
                    .AddMonths((trigger) * month)
                    .AddYears((trigger) * year);
            }
            Log.Logger.Warning($"Используйте только положительные числа при указании отличной даты от заданной.");
            return null;
        }

        public static DateTime? GetRandomDateTime(DateTime? start = null, DateTime? end = null)
        {
            var rnd = new Random();
            start = start ?? DefaultStart;
            end = end ?? DefaultEnd;

            var range = end.Value - start.Value;
            var randomUpperBound = (Int32)range.TotalSeconds;
            if (randomUpperBound <= 0)
                randomUpperBound = rnd.Next(1, Int32.MaxValue);

            var randTimeSpan = TimeSpan.FromSeconds((Int64)(range.TotalSeconds - rnd.Next(0, randomUpperBound)));
            return start.Value.Add(randTimeSpan);
        }

        private static string RandomString(int len, string chars)
        {
            var str = string.Empty;
            if (len <= 0)
            {
                Log.Logger.Error("Размер генерируемой строки отрицательный или равен нулю");
                throw new GeneratorException("Размер генерируемой строки отрицательный или равен нулю");
            }

            for (var c = 0; c < len; c++)
            {
                str += chars[Random.Next() % chars.Length];
            }

            return str;
        }
    }
}

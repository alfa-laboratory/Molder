using System;
using System.Linq;

namespace AlfaBank.AFT.Core.Helpers
{
    public static class DataGenerator
    {
        private static readonly Random Random = new Random((int)DateTime.UtcNow.AddYears(-2000).Ticks);
        public static string UpperChars { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string LowerChars { get; } = "abcdefghijklmnopqrstuvwxyz";
        public static string Numbers { get; } = "0123456789";

        public static string GetRandomStringWithPrefix(int len, string prefix)
        {
            return prefix != string.Empty ? prefix + GetRandomString(len + prefix.Count(), UpperChars + LowerChars + Numbers) : GetRandomString(len, UpperChars + LowerChars + Numbers);
        }

        public static string GetRandomString(int len)
        {
            return GetRandomString(len, UpperChars + LowerChars + Numbers);
        }

        public static string GetRandomCharWithPrefix(int len, string prefix)
        {
            return prefix != string.Empty ? prefix + GetRandomString(len + prefix.Count(), UpperChars + LowerChars) : GetRandomString(len, UpperChars + LowerChars);
        }

        public static string GetRandomChars(int len)
        {
            return GetRandomString(len, UpperChars + LowerChars);
        }

        public static string GetRandomNumberWithPrefix(int len, string prefix)
        {
            return prefix != string.Empty ? prefix + GetRandomString(len + prefix.Count(), Numbers) : GetRandomString(len, Numbers);
        }

        public static string GetRandomNumbers(int len)
        {
            return GetRandomString(len, Numbers);
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
                if(
                    ch == ' '
                    || ch == '-'
                    || ch == '+')
                {
                    str += ch;
                }
                else
                {
                    if(count >= 10)
                    {
                        throw new FormatException("Phone number can have up to 10 digits");
                    }

                    if(
                           ch == '0'
                        || ch == '1'
                        || ch == '2'
                        || ch == '3'
                        || ch == '4'
                        || ch == '5'
                        || ch == '6'
                        || ch == '7'
                        || ch == '8'
                        || ch == '9')
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
                throw new FormatException("Phone number must contain at least 10 digits");
            }

            return str;
        }

        private static string GetRandomString(int len, string chars)
        {
            var str = string.Empty;
            for(var c = 0; c < len; c++)
            {
                str += chars[Random.Next() % chars.Length];
            }

            return str;
        }
    }
}

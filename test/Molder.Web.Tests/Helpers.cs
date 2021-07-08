using System;

namespace Molder.Web.Tests
{
    public static class TestHelpers
    {
        public static string RemoveWhitespace(this string str) {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
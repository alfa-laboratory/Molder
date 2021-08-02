using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Tests
{
    [ExcludeFromCodeCoverage]
    public static class TestHelpers
    {
        public static string RemoveWhitespace(this string str) {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
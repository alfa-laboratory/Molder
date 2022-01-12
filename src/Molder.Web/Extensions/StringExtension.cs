using System.Text.RegularExpressions;

namespace Molder.Web.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultString">String to process</param>
        /// <param name="regex">the default regex removes slashes and dots from the beginning of the string</param>
        /// <returns></returns>
        public static string GetStringByRegex(this string defaultString, string regex = "[^/.](.*)")
        {
            var str = defaultString;
            str = Regex.Match(str, regex).Value.Trim();
            return str;
        }
    }
}

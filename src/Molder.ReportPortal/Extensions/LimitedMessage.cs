using Molder.ReportPortal.Infrastructures;

namespace Molder.ReportPortal.Extensions
{
    public static class LimitedMessage
    {
        public static string ToLimitedMessage(this string str, int? size)
        {
            var _str = str;
            if (size is not null)
            {
                _str = _str.Remove((int)size) + Constants.END_STRING;
            }
            return _str;
        }
    }
}
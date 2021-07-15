using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Infrastructures
{
    [ExcludeFromCodeCoverage]
    public static class CommandSetting
    {
        public static int RETRY = 5;
        public static TimeSpan INTERVAL = TimeSpan.FromMilliseconds(25);
    }
}

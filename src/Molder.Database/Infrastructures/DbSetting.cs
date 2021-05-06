using System.Diagnostics.CodeAnalysis;

namespace Molder.Database.Infrastructures
{
    [ExcludeFromCodeCoverage]
    public static class DbSetting
    {
        public static int TIMEOUT = 60;
        public static int PERIOD = 3;
        public static int ConnectRetryCount = 1;
        public static int ConnectRetryInterval = 3;
    }

    [ExcludeFromCodeCoverage]
    public static class Default
    {
        public static int ConnectRetryCount = 1;
        public static int ConnectRetryInterval = 10;
        public static int ConnectTimeout = 15;
        public static int LoadBalanceTimeout = 0;
    }

}

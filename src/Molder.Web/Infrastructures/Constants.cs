using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Infrastructures
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        #region Configuration constants
        public const string CONFIG_BLOCK = "Molder.Web";
        public const string SETTINGS_BLOCK = "Settings";
        #endregion

        public const string DEFAULT_VERSION = "latest";
        public const string DEFAULT_PLATFORM = "ANY";
        public const string DEFAULT_PROJECT = "MolderDefault";

        public const int MS_IN_SEC = 1000;
        public const int DEFAULT_TIMEOUT = 60;
        public const long TIMEOUT = DEFAULT_TIMEOUT * MS_IN_SEC;
    }
}
using System.Diagnostics.CodeAnalysis;

namespace Molder.ReportPortal.Infrastructures
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        #region Configuration constants
        public const string CONFIG_BLOCK = "Molder.ReportPortal";
        public const string SETTINGS_BLOCK = "Settings";
        #endregion

        public const bool ENABLE_RP_REPORT = false;

        public const string DEFAULT_RP_CONFIG = "{\"$schema\": \"https://raw.githubusercontent.com/reportportal/agent-net-specflow/master/ReportPortal.SpecFlowPlugin/ReportPortal.config.schema\",\"enabled\":true,\"server\":{\"url\":\"\",\"project\":\"\",\"authentication\":{\"uuid\":\"\"}}}";
    }
}

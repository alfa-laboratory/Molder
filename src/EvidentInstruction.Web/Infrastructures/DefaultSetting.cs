using EvidentInstruction.Web.Models.Settings;

namespace EvidentInstruction.Web.Infrastructures
{
    public static class DefaultSetting
    {
        public static readonly bool REMOTE_RUN = false;
        public static readonly bool HEADLESS = false;
        public static readonly BrowserType BROWSER = BrowserType.CHROME;
        public static readonly string BROWSER_PATH = null;
        public static readonly string REMOTE_URL = null;
        public static readonly string BROWSER_VERSION = "latest";
        public static readonly int? BROWSER_HEIGHT = null;
        public static readonly int? BROWSER_WIDTH = null;
        public static readonly int BROWSER_TIMEOUT = 60;
    }

    public enum Setting
    {
        [EnumValue("REMOTE_RUN")]
        REMOTE_RUN,

        [EnumValue("HEADLESS")]
        HEADLESS,

        [EnumValue("BROWSER")]
        BROWSER,

        [EnumValue("BROWSER_PATH")]
        BROWSER_PATH,

        [EnumValue("REMOTE_URL")]
        REMOTE_URL,

        [EnumValue("BROWSER_VERSION")]
        BROWSER_VERSION,

        [EnumValue("BROWSER_HEIGHT")]
        BROWSER_HEIGHT,

        [EnumValue("BROWSER_WIDTH")]
        BROWSER_WIDTH,

        [EnumValue("BROWSER_TIMEOUT")]
        BROWSER_TIMEOUT
    }
}
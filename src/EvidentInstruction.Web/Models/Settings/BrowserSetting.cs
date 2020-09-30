using EvidentInstruction.Controllers;
using EvidentInstruction.Web.Helpers;
using EvidentInstruction.Web.Infrastructures;
using EvidentInstruction.Web.Models.Settings.Interfaces;

namespace EvidentInstruction.Web.Models.Settings
{
    public class BrowserSetting : ISetting
    {
        private VariableController _variableController;

        public bool? Remote { get; set; } = null;
        public bool? Headless { get; set; } = null;
        public BrowserType? BrowserType { get; set; } = null;
        public string BrowserPath { get; set; } = null;
        public string RemoteUrl { get; set; } = null;
        public string RemoteVersion { get; set; } = null;
        public int? Height { get; set; } = null;
        public int? Width { get; set; } = null;
        public int? Timeout { get; set; } = null;

        public BrowserSetting(VariableController variableController)
        {
            _variableController = variableController;
        }

        public void Create()
        {
            var fields = SettingHelper.GetAllFields(typeof(DefaultSetting));

            foreach(var field in fields)
            {
                if(field.Name == Setting.REMOTE_RUN.GetValue())
                {
                    Remote = Remote == null ? (bool?)_variableController.GetVariableValue(Setting.REMOTE_RUN.GetValue()) : DefaultSetting.REMOTE_RUN;
                }

                if (field.Name == Setting.HEADLESS.GetValue())
                {
                    Headless = Headless == null ? (bool?)_variableController.GetVariableValue(Setting.HEADLESS.GetValue()) : DefaultSetting.HEADLESS;
                }

                if (field.Name == Setting.BROWSER.GetValue())
                {
                    BrowserType = BrowserType == null ? (BrowserType)_variableController.GetVariableValue(Setting.BROWSER.GetValue()) : DefaultSetting.BROWSER;
                }

                if (field.Name == Setting.BROWSER_PATH.GetValue())
                {
                    BrowserPath = BrowserPath == null ? (string)_variableController.GetVariableValue(Setting.BROWSER_PATH.GetValue()) : DefaultSetting.BROWSER_PATH;
                }

                if (field.Name == Setting.REMOTE_URL.GetValue())
                {
                    RemoteUrl = RemoteUrl == null ? (string)_variableController.GetVariableValue(Setting.REMOTE_URL.GetValue()) : DefaultSetting.REMOTE_URL;
                }

                if (field.Name == Setting.BROWSER_VERSION.GetValue())
                {
                    RemoteVersion = RemoteVersion == null ? (string)_variableController.GetVariableValue(Setting.BROWSER_VERSION.GetValue()) : DefaultSetting.BROWSER_VERSION;
                }

                if (field.Name == Setting.BROWSER_HEIGHT.GetValue())
                {
                    Height = Height == null ? (int)_variableController.GetVariableValue(Setting.BROWSER_HEIGHT.GetValue()) : DefaultSetting.BROWSER_HEIGHT;
                }

                if (field.Name == Setting.BROWSER_WIDTH.GetValue())
                {
                    Width = Width == null ? (int)_variableController.GetVariableValue(Setting.BROWSER_WIDTH.GetValue()) : DefaultSetting.BROWSER_WIDTH;
                }

                if (field.Name == Setting.BROWSER_TIMEOUT.GetValue())
                {
                    Timeout = Timeout == null ? (int)_variableController.GetVariableValue(Setting.BROWSER_TIMEOUT.GetValue()) : DefaultSetting.BROWSER_TIMEOUT;
                }
            }
        }
    }
}
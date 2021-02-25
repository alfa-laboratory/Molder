using Molder.Controllers;
using Molder.Helpers;
using Molder.Web.Helpers;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Proxy;
using System;

namespace Molder.Web.Models.Settings
{
    public class BrowserSetting : ISetting
    {
        private VariableController _variableController;

        public string Project { get; set; } = null;
        public bool? Remote { get; set; } = null;
        public bool? Headless { get; set; } = null;
        public BrowserType? BrowserType { get; set; } = null;
        public string BrowserPath { get; set; } = null;
        public string RemoteUrl { get; set; } = null;
        public string RemoteVersion { get; set; } = null;
        public int? Timeout { get; set; } = null;
        public int? ElementTimeout { get; set; } = null;
        public Authentication Authentication { get; set; } = null;

        public BrowserSetting()
        {
            _variableController = new VariableController();
        }

        public BrowserSetting(VariableController variableController)
        {
            _variableController = variableController;
        }

        public void Create()
        {
            var fields = SettingHelper.GetAllFields(typeof(DefaultSetting));

            foreach(var field in fields)
            {
                if (field.Name == Setting.PROJECT.GetValue())
                {
                    Project = (string)_variableController.GetVariableValue(Setting.PROJECT.GetValue()) ?? DefaultSetting.PROJECT;
                }

                if (field.Name == Setting.REMOTE_RUN.GetValue())
                {
                    Remote = _variableController.GetVariableValue(Setting.REMOTE_RUN.GetValue()).ToString().GetValueOrNull<bool>() ?? DefaultSetting.REMOTE_RUN;
                }

                if (field.Name == Setting.HEADLESS.GetValue())
                {
                    Headless = _variableController.GetVariableValue(Setting.HEADLESS.GetValue()).ToString().GetValueOrNull<bool>() ?? DefaultSetting.HEADLESS;
                }

                if (field.Name == Setting.BROWSER.GetValue())
                {
                    BrowserType = _variableController.GetVariableValue(Setting.BROWSER.GetValue()) != null ? (BrowserType)Enum.Parse(typeof(BrowserType), _variableController.GetVariableValueText(Setting.BROWSER.GetValue())) : DefaultSetting.BROWSER;
                }

                if (field.Name == Setting.BROWSER_PATH.GetValue())
                {
                    BrowserPath = (string)_variableController.GetVariableValue(Setting.BROWSER_PATH.GetValue()) ?? DefaultSetting.BROWSER_PATH;
                }

                if (field.Name == Setting.REMOTE_URL.GetValue())
                {
                    RemoteUrl = (string)_variableController.GetVariableValue(Setting.REMOTE_URL.GetValue()) ?? DefaultSetting.REMOTE_URL;
                }

                if (field.Name == Setting.BROWSER_VERSION.GetValue())
                {
                    RemoteVersion = (string)_variableController.GetVariableValue(Setting.BROWSER_VERSION.GetValue()) ?? DefaultSetting.BROWSER_VERSION;
                }

                if (field.Name == Setting.BROWSER_TIMEOUT.GetValue())
                {
                    Timeout = ((int?)(int.TryParse(_variableController.GetVariableValue(Setting.BROWSER_TIMEOUT.GetValue()).ToString(), out var f) ? f : default)) ?? DefaultSetting.BROWSER_TIMEOUT;
                }

                if (field.Name == Setting.ELEMENT_TIMEOUT.GetValue())
                {
                    ElementTimeout = ((int?)(int.TryParse(_variableController.GetVariableValue(Setting.ELEMENT_TIMEOUT.GetValue()).ToString(), out var f) ? f : default)) ?? DefaultSetting.ELEMENT_TIMEOUT;
                }
            }
        }
    }
}
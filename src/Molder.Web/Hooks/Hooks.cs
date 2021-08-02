using Microsoft.Extensions.Logging;
using Molder.Controllers;
using Molder.Helpers;
using Molder.Models.Configuration;
using Molder.Web.Controllers;
using Molder.Web.Helpers;
using Molder.Web.Infrastructures;
using Molder.Web.Models;
using Molder.Web.Models.Settings;
using Molder.Web.Extensions;
using TechTalk.SpecFlow;

namespace Molder.Web.Hooks
{
    [Binding]
    public class Hooks : TechTalk.SpecFlow.Steps
    {
        [BeforeTestRun(Order = -9000000)]
        public static void InitializeConfiguration()
        {
            var settings = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);
            if (settings.Value is null)
            {
                Log.Logger().LogInformation($@"appsettings is not contains {Constants.CONFIG_BLOCK} block. Standard settings selected.");
                BrowserSettings.Settings = new Settings();
            }
            else
            {
                Log.Logger().LogInformation($@"appsettings contains {Constants.CONFIG_BLOCK} block. Settings selected.");
                BrowserSettings.Settings = settings.Value;
            }
        }

        [BeforeFeature]
        public static void BeforeFeature(VariableController variableController)
        {
            BrowserController.SetVariables(variableController);
            TreePages.SetVariables(variableController);
            TreePages.Get();
            var pageObject = TreePages.Get();
            Log.Logger().LogDebug(LogPageObjectExtensions.PageObjectToString(pageObject));
        }

        [AfterScenario()]
        public void AfterScenario(ScenarioContext scenario)
        {
            if (scenario.TestError != null)
            {
                //TODO
                // Add create screenshot: FeatureDirectory/ScenarioDirectory/time_ScenarioName_StepName.png 
            }
        }
    }
}

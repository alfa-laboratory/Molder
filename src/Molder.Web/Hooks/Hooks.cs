using Microsoft.Extensions.Logging;
using Molder.Controllers;
using Molder.Helpers;
using Molder.Models.Configuration;
using Molder.Web.Controllers;
using Molder.Web.Helpers;
using Molder.Web.Infrastructures;
using Molder.Web.Models;
using Molder.Web.Models.Settings;
using System.Collections.Generic;
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
            var _var = TreePages.Get();
            LogPageObject(_var);
        }

        public static void LogPageObject(IEnumerable<Node> pages, int level = 0)
        {
            string s = "";
            for (int i = 0; i < level; i++)
            {
                s += "|   ";
            }
            foreach (Node page in pages)
            {
                if (!(page.Childrens is null))
                {
                    Log.Logger().LogDebug(s + "└───" + page.Type.ToString() + "(" + page.Name + ")");
                    LogPageObject(page.Childrens, level + 1);
                }
                else Log.Logger().LogDebug(s + "|   " + page.Type.ToString() + "(" + page.Name + ")");
            }
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

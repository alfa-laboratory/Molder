using Molder.Controllers;
using Molder.Models.Configuration;
using Molder.Web.Controllers;
using Molder.Web.Helpers;
using Molder.Web.Models;
using Molder.Web.Models.Settings;
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
            BrowserSettings.Settings = settings.Value;
        }

        [BeforeFeature]
        public static void BeforeFeature(VariableController variableController)
        {
            BrowserController.SetVariables(variableController);

            TreePages.SetVariables(variableController);
            TreePages.Get();
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

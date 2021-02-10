using Molder.Controllers;
using Molder.Web.Extensions;
using Molder.Web.Models;
using TechTalk.SpecFlow;

namespace Molder.Web.Steps
{
    [Binding]
    public class Hooks : TechTalk.SpecFlow.Steps
    {
        private VariableController _variableController;
        private ScenarioContext _scenario;

        public Hooks(VariableController controller, ScenarioContext scenario)
        {
            _variableController = controller;
            _scenario = scenario;
        }

        [BeforeScenario(Order = -25000)]
        public void BeforeScenario()
        {
            _variableController.AddSettings();
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            if (_scenario.TestError != null)
            {
                /// TODO
                /// Add create screenshot: FeatureDirectory/ScenarioDirectory/time_ScenarioName_StepName.png 
            }
        }
    }
}

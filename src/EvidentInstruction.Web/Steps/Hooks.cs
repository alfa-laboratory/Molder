using EvidentInstruction.Controllers;
using EvidentInstruction.Web.Extensions;
using EvidentInstruction.Web.Models.Settings;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Web.Steps
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
            PageCollection.GetPages();
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

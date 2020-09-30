using EvidentInstruction.Controllers;
using EvidentInstruction.Web.Helpers;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Web.Steps
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public sealed class Hooks
    {
        private VariableController variableController;

        public Hooks(VariableController controller)
        {
            variableController = controller;
        }

        [BeforeTestRun(Order = -40000)]
        public void BeforeTestRun()
        {
            variableController.AddSettings();
        }

        [BeforeScenario(Order = -20000)]
        public void BeforeScenario()
        {
            // collect page
        }
    }
}

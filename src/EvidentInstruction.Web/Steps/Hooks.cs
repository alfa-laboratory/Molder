using EvidentInstruction.Controllers;
using EvidentInstruction.Web.Extensions;
using EvidentInstruction.Web.Models.Settings;
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

        [BeforeScenario(Order = -25000)]
        public void BeforeScenario()
        {
            variableController.AddSettings();
            PageCollection.GetPages();
        }
    }
}

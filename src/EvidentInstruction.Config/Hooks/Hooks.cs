using EvidentInstruction.Config.Extension;
using EvidentInstruction.Controllers;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Config.Hooks
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

        [BeforeTestRun(Order = -30000)]
        public void BeforeTestRun()
        {
            variableController.AddConfig();
        }
    }
}

using EvidentInstruction.Config.Extension;
using EvidentInstruction.Controllers;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Config.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private VariableController variableController;

        public Hooks(VariableController controller)
        {
            variableController = controller;
        }

        [BeforeTestRun(Order =-3000)]
        public void BeforeTestRun()
        {
            variableController.AddConfig();
        }
    }
}

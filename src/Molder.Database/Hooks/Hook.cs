using Molder.Database.Controllers;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace Molder.Database.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class Hook : TechTalk.SpecFlow.Steps
    {
        private readonly DatabaseController databaseController;

        public Hook(DatabaseController databaseController)
        {
            this.databaseController = databaseController;
        }

        /// <summary>
        /// Очистка подключений к базам данных в конце сценария.
        /// </summary>
        [AfterScenario]
        public void AfterScenario()
        {
            foreach (var kvp in this.databaseController.Connections)
            {
                kvp.Value.connection?.Dispose();
            }

            this.databaseController.Connections.Clear();
        }
    }
}
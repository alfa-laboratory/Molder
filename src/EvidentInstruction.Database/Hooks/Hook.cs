using EvidentInstruction.Database.Controllers;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Database.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public sealed class Hook
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
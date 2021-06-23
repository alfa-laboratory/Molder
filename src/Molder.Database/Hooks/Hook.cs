using Molder.Database.Controllers;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace Molder.Database.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class Hook : TechTalk.SpecFlow.Steps
    {
        /// <summary>
        /// Очистка подключений к базам данных в конце сценария.
        /// </summary>
        [AfterScenario]
        public void AfterScenario(DatabaseController databaseController)
        {
            foreach (var kvp in databaseController.Connections)
            {
                if (kvp.Value.typeOfAccess == Molder.Infrastructures.TypeOfAccess.Local)
                {
                    kvp.Value.connection?.Dispose();
                    databaseController.Connections.TryRemove(kvp.Key, out var connection);
                }
            }
        }

        [AfterFeature]
        public static void AfterFeature(DatabaseController databaseController)
        {
            foreach (var kvp in databaseController.Connections)
            {
                kvp.Value.connection?.Dispose();
            }
            databaseController.Connections.Clear();
        }
    }
}
using Molder.Controllers;
using Molder.Database.Controllers;
using Molder.Database.Models;
using System.Data.SqlClient;
using TechTalk.SpecFlow;

namespace Molder.Database.Example.Steps
{
    [Binding]
    public class Hooks
    {
        [BeforeFeature("Test", Order = -1)]
        public static void Before(VariableController variableController, DatabaseController databaseController)
        {
            var connectionsStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = variableController.GetVariableValueText("SOURCE"),
                InitialCatalog = variableController.GetVariableValueText("DATABASE"),
                UserID = variableController.GetVariableValueText("LOGIN"),
                Password = variableController.GetVariableValueText("PASSWORD"),
                ConnectTimeout = 60
            };

            var connection = new SqlServerClient();
            connection.Create(connectionsStringBuilder);
            connection.IsConnectAlive();
            databaseController.Connections.TryAdd("QA", (connection, typeOfAccess: Molder.Infrastructures.TypeOfAccess.Global ,connectionsStringBuilder.ConnectTimeout));
        }
    }
}
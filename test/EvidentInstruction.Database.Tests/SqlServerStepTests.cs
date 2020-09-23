using EvidentInstruction.Controllers;
using EvidentInstruction.Database.Controllers;
using EvidentInstruction.Database.Models;
using EvidentInstruction.Database.Steps;
using EvidentInstruction.Database.Exceptions;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Database.Tests
{
    [ExcludeFromCodeCoverage]
    public class SqlServerStepTests
    {

        private DbConnectionParams dbConnectionParams;
        private const string dbConnectionString = "TestConnectionString";
        private DatabaseController databaseController;
        private VariableController variableController;


        public SqlServerStepTests()
        {
            dbConnectionParams = new DbConnectionParams() { Database = "Test", Source = "test", Login = "test", Password = "W9qNIafQbJCZzEafUaYmQw==" };
            databaseController = new DatabaseController();
            variableController = new VariableController();
        }

        [Fact]
        public void ConnectToDB_SqlServer_IncorrectDbParams_ReturnThrow()
        {
            SqlServerSteps step = new SqlServerSteps(databaseController, variableController);

            Action action = () => step.ConnectToDB_SqlServer(dbConnectionString, dbConnectionParams);
            action.Should()
                .Throw<ConnectSqlException>()
                .WithMessage($"Connection failed. Connection with parameters: {Database.Helpers.Message.CreateMessage(dbConnectionParams)}" +
                " A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)");
        }

        [Fact]
        public void ConnectToDB_SqlServer_DbParamsIsNull_ReturnThrow()
        {
            SqlServerSteps step = new SqlServerSteps(databaseController, variableController);

            dbConnectionParams = new DbConnectionParams() { Database = "", Source = "", Login = "", Password = "", Timeout = 0 };

            Action action = () => step.ConnectToDB_SqlServer(dbConnectionString, dbConnectionParams);
            action.Should()
                .Throw<Xunit.Sdk.XunitException>();
        }
    }
}

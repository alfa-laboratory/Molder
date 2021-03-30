using Molder.Controllers;
using Molder.Database.Controllers;
using Molder.Database.Models;
using Molder.Database.Steps;
using Molder.Database.Exceptions;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Moq;
using Molder.Database.Infrastructures;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using Molder.Database.Models.Parameters;

namespace Molder.Database.Tests
{
    [ExcludeFromCodeCoverage]
    public class SqlServerStepTests
    {
        private DbConnectionParams dbConnectionParams;
        private const string dbConnectionString = "TestConnectionString";
        private DatabaseController databaseController;
        private VariableController variableController;
        private SqlServerSteps step;

        public SqlServerStepTests()
        {
            dbConnectionParams = new DbConnectionParams() { Database = "Test", Source = "test", Login = "test", Password = "W9qNIafQbJCZzEafUaYmQw==", Timeout = 1, ConnectRetryCount = 0, ConnectRetryInterval = 1 };
            databaseController = new DatabaseController();
            variableController = new VariableController();
            step = new SqlServerSteps(databaseController, variableController);
        }

        [Fact]
        public void GetDataBaseParametersFromTableSqlServer_CorrectTable_ReturnDbConnectionParams()
        {
            var table = new Table(new string[] { "Source", "Database", "Login", "Password" });
            table.AddRow("Db1", "Test", "User", "test");

            var result = step.GetDataBaseParametersFromTableSqlServer(table);

            result.Should().NotBeNull();
            result.Login.Should().Be("User");
            result.Source.Should().Be("Db1");
            result.Database.Should().Be("Test");
            result.Password.Should().Be("test");
        }

        [Fact]
        public void TransformationTableToString_CorrectTable_ReturnIEnumerable()
        {
            var table = new Table(new string[] { "id", "User", "Balance", "Date" });
            table.AddRow("1234", "Иван Иванов", "10000.10", "10.10.2000");
            table.AddRow("1243", "Петор Петров", "100000", "2000-12-12");

            var result = step.TransformationTableToString(table);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void TransformationTableToString_TableIsEmpty_ReturnThrow()
        {
            var table = new Table(new string[] { "id", "User", "Balance", "Date" });

            Action action = () => step.TransformationTableToString(table);
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null.{Environment.NewLine}Parameter name: List with table patameters is Empty.");
        }

        [Fact]
        public void ConnectToDB_SqlServer_DbParamsIsNull_ReturnThrow()
        {
            dbConnectionParams = new DbConnectionParams() { Database = "", Source = "", Login = "", Password = "", Timeout = 0 };

            Action action = () => step.ConnectToDB_SqlServer(dbConnectionString, dbConnectionParams);
            action.Should()
                .Throw<Xunit.Sdk.XunitException>();
        }

        [Fact]
        public void ExecuteQuery_IncorrectParams_ReturnNull()
        {
            var mockSqlProvider = new Mock<IDbClient>();           
            var connectName = "NewConnect";
            var query = "SELECT top 100 * test111";

            IDbClient connection = new SqlServerClient();
            mockSqlProvider.Setup(c => c.Create(It.IsAny<DbConnectionParams>())).Returns(true);

            connection = mockSqlProvider.Object;

            this.databaseController.Connections.TryAdd(connectName, (connection, 30));

            step.ExecuteQueryType(QueryType.SELECT, connectName, new QueryParam { Query = query });            
        }

        [Fact]
        public void ExecuteQuery_CorrectParams_ReturnNewVariable()
        {
            var mockSqlProvider = new Mock<IDbClient>();
            var varName = "newVariable";
            var connectName = "NewConnect";
            var query = "INSERT INTO test111 (f1) VALUES (1) ";


            IDbClient connection = new SqlServerClient();
            mockSqlProvider.Setup(c => c.Create(It.IsAny<DbConnectionParams>())).Returns(true);

            connection = mockSqlProvider.Object;

            this.databaseController.Connections.TryAdd(connectName, (connection, 30));

            step.ExecuteQueryTypeWithVarName(QueryType.INSERT, connectName, varName, new QueryParam { Query = query });

            this.variableController.Variables.Should().NotBeEmpty();
        }

        [Fact]
        public void ExecuteInsertQueryFromTable_CorrectParams_ReturnThrow()
        {
            var mockSqlProvider = new Mock<IDbClient>();

            var tableName = "newVariable";
            var connectName = "NewConnect";

            var insertQuery = new List<Dictionary<string, object>>();
            insertQuery.Add(new Dictionary<string, object>());

            IDbClient connection = new SqlServerClient();
            mockSqlProvider.Setup(c => c.Create(It.IsAny<DbConnectionParams>())).Returns(true);
            mockSqlProvider.Setup(c => c.IsConnectAlive()).Returns(true);

            connection = mockSqlProvider.Object;

            this.databaseController.Connections.TryAdd(connectName, (connection, 30));
            Action action = () => step.ExecuteInsertQueryFromTable(tableName, connectName, insertQuery);
            action.Should()
                .Throw<Xunit.Sdk.XunitException>();
        }
    }
}
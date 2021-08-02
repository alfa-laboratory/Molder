using Molder.Controllers;
using Molder.Database.Controllers;
using Molder.Database.Steps;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;
using Xunit;
using Molder.Database.Extensions;

namespace Molder.Database.Tests
{
    [ExcludeFromCodeCoverage]
    public class ParserToStringTests
    {
        private DatabaseController databaseController;
        private VariableController variableController;
        private SqlServerSteps step;
        private const string tableName = "Table";
        public ParserToStringTests()
        {
            databaseController = new DatabaseController();
            variableController = new VariableController();
            step = new SqlServerSteps(databaseController, variableController);

        }

        [Fact]
        public void ToSqlQuery_CorrectTable_ReturnString()
        {
            var table = new Table(new string[] { "id", "User", "Balance", "Date", "Bool" });
            table.AddRow("1243", "Петор Петров", "100000", "2000-12-12", "false");

            var ListParams = step.TransformationTableToString(table);

            var result = ListParams.ToSqlQuery(tableName);

            result.Should().BeEquivalentTo("INSERT INTO Table (id,User,Balance,Date,Bool)" +
                " VALUES (1243,'Петор Петров',100000,'2000-12-12',False)");
        }

        [Fact]
        public void ToSqlQuery_TableNameIsEmpty_ReturnThrow()
        {
            var tableName = string.Empty;
            var table = new Table(new string[] { "id", "User", "Balance", "Date", "Bool", "idClient" });
            table.AddRow("1243", "Петор Петров", "100000", "2000-12-12", "false", "");

            var ListParams = step.TransformationTableToString(table);

            Action action = () => ListParams.ToSqlQuery(tableName);
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null. (Parameter 'Table name is Empty.')");
        }

        [Fact]
        public void ToSqlQuery_TableIsEmpty_ReturnThrow()
        {
            var ListParams = new List<Dictionary<string, object>>();

            Action action = () => ListParams.ToSqlQuery(tableName);
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null. (Parameter 'List with table parameters is Empty.')");
        }
    }
}
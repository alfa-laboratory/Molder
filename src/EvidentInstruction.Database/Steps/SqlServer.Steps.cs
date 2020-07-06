using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using EvidentInstruction.Controllers;
using EvidentInstruction.Database.Controllers;
using EvidentInstruction.Database.Helpers;
using EvidentInstruction.Database.Models;
using EvidentInstruction.Helpers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace EvidentInstruction.Database.Steps
{
    /// <summary>
    /// Шаги для работы с SqlServer.
    /// </summary>
    [Binding]
    public class SqlServerSteps
    {
        private readonly DatabaseController databaseController;
        private readonly VariableController variableController;

        private Table ReplaceTableContent(Table dataTable)
        {
            var table = new Table(dataTable.Header.ToArray());
            dataTable.Rows.ToList().ForEach(row =>
            {
                var tr = new List<string>();
                row.Values.ToList().ForEach(elem =>
                {
                    tr.Add(variableController.ReplaceVariables(elem));
                });
                table.AddRow(tr.ToArray());
                tr.Clear();
            });
            return table;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerSteps"/> class.
        /// Привязка шагов работы с базами данных к работе с переменным через контекст.
        /// </summary>
        /// <param name="databaseController">Контекст для работы с базой данных.</param>
        /// <param name="variableController">Контекст для работы с переменными.</param>
        public SqlServerSteps(DatabaseController databaseController, VariableController variableController)
        {
            this.databaseController = databaseController;
            this.variableController = variableController;
        }

        /// <summary>
        /// Трансформация параметров подключения к SqlServer в DbConnectionParams.
        /// </summary>
        /// <param name="dataTable">Параметры подключения.</param>
        /// <returns>Параметры подключения к SqlServer.</returns>
        [StepArgumentTransformation]
        [Scope(Tag = "SqlServer")]
        public DbConnectionParams GetDataBaseParametersFromTableSqlServer(Table dataTable)
        {
            var table = ReplaceTableContent(dataTable);
            return table.CreateInstance<DbConnectionParams>();
        }
        [StepArgumentTransformation]
        [Scope(Tag = "SqlServer")]
        public DataTable TransformationTableToDatatable(Table table)
        {
            var inRecords = new DataTable();
            inRecords.Columns.AddRange(table.Header.Select(_ => new DataColumn(_)).ToArray());
            foreach (var row in table.Rows)
            {
                inRecords.Rows.Add(
                    row.Values
                        .Select(_ =>
                        {
                            var s = _;
                            s = this.variableController.ReplaceVariables(s);
                            return s;
                        })
                        .ToArray());
            }

            return inRecords;
        }

        /// <summary>
        /// Подключение к SQLServer.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="params">Параметры подключения.</param>
        [Scope(Tag = "SqlServer")]
        [Given(@"я подключаюсь к БД MS SQL Server с названием ""(.+)"":")]
        public void ConnectToDB_SqlServer(string connectionName, DbConnectionParams connectionParams)
        {
            var (isValid, results) = EvidentInstruction.Helpers.Validate.ValidateModel(connectionParams);
            isValid.Should().BeTrue(EvidentInstruction.Helpers.Message.CreateMessage(results));
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeFalse($"Подключение с названием \"{connectionName}\" уже существует");

            var connection = new SqlServerConnectionWrapper(connectionParams);
            var dbConnection = connection.GetDb();
            dbConnection.Should().NotBeNull($"Подключение к базе данных \"{connectionName}\" с параметрами: {Helpers.Message.CreateMessage(connectionParams)} не успешно.");

            this.databaseController.Connections.TryAdd(connectionName, (connection, connectionParams.Timeout));
        }

        /// <summary>
        /// Шаг выборки записей из базы данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "SqlServer")]
        [StepDefinition(@"я выбираю несколько записей из БД ""(.+)"" и сохраняю их в переменную ""(.+)"":")]
        public void SelectRowsFromDbSetVariable(string connectionName, string varName, string query)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Подключение с названием \"{connectionName}\" не существует");
            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;
            query = this.variableController.ReplaceVariables(query);

            var (outRecords, _) = connection.SelectQuery(query:query, timeout:timeout);
            outRecords.Should().NotBeNull($"При выполнении запроса: " +
                $"{Environment.NewLine}" + $"{query} " + $"{Environment.NewLine}" +
                $" возникли ошибки");
            (outRecords is DataTable).Should().BeTrue("Выходные данные не являются типом DataTable");
            this.variableController.SetVariable(varName, typeof(DataTable), outRecords);

            Log.Logger.Information($"Выходные данные запроса:" + Environment.NewLine + $"{query}" + Environment.NewLine + EvidentInstruction.Helpers.Message.CreateMessage((DataTable)outRecords));
        }

        /// <summary>
        /// Шаг выборки записи из базы данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "SqlServer")]
        [StepDefinition(@"я выбираю единственную запись из БД ""(.+)"" и сохраняю её в переменную ""(.+)"":")]
        public void SelectSingleRowFromDbSetVariable(string connectionName, string varName, string query)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Подключение с названием \"{connectionName}\" не существует");
            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;
            query = this.variableController.ReplaceVariables(query);

            var (outRecords, count) = connection.SelectQuery(query: query, timeout: timeout);
            outRecords.Should().NotBeNull($"При выполнении запроса: " +
                $"{Environment.NewLine}" + $"{query} " + $"{Environment.NewLine}" +
                $" возникли ошибки");
            count.Should().Be(1, "Запрос: " + $"{Environment.NewLine}" + $"{query}" + $"{ Environment.NewLine}" + 
                $" вернул не одну запись");

            (outRecords is DataTable).Should().BeTrue("Выходные данные не являются типом DataTable");
            this.variableController.SetVariable(varName, typeof(DataRow), ((DataTable)outRecords).Rows[0]);
            Log.Logger.Information($"Выходные данные запроса:" + Environment.NewLine + $"{query}" + Environment.NewLine + EvidentInstruction.Helpers.Message.CreateMessage((DataTable)outRecords));
        }

        /// <summary>
        /// Шаг выборки единственной ячейки из базы данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "SqlServer")]
        [StepDefinition(@"я сохраняю значение единственной ячейки из выборки из БД ""(.+)"" в переменную ""(.+)"":")]
        public void SelectScalarFromDbSetVariable(string connectionName, string varName, string query)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Подключение с названием \"{connectionName}\" не существует");
            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;
            query = this.variableController.ReplaceVariables(query);

            var (outRecords, count) = connection.SelectQuery(query: query, timeout: timeout);

            outRecords.Should().NotBeNull($"При выполнении запроса: " +
                $"{Environment.NewLine}" + $"{query} " + $"{Environment.NewLine}" +
                $" возникли ошибки");
            count.Should().Be(1, "Запрос: " + $"{Environment.NewLine}" + $"{query}" + $"{ Environment.NewLine}" +
                $" вернул не одну запись");
            (outRecords is DataTable).Should().BeTrue("Выходные данные не являются типом DataTable");

            this.variableController.SetVariable(
                varName,
                ((DataTable)outRecords).Columns[0].DataType,
                ((DataTable)outRecords).Rows[0][0]);
            Log.Logger.Information($"Выходные данные запроса:" + Environment.NewLine + $"{query}" + Environment.NewLine + EvidentInstruction.Helpers.Message.CreateMessage((DataTable)outRecords));
        }

        /// <summary>
        /// Шаг занесения данных в базу данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название таблицы.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="data">Данные.</param>
        [Scope(Tag = "SqlServer")]
        [When(@"я заношу записи в БД ""(.+)"" в таблицу ""(.+)"" и сохраняю результат в переменную ""(.+)"":")]
        public void InsertRowsIntoDbSetVariable(string connectionName, string tableName, string varName, DataTable data)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Подключение с названием \"{connectionName}\" не существует");
            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;

            var (outRecords, count) = connection.InsertRows(tableName, data, timeout);
            count.Should().Be(data.Rows.Count, "Были добавлены не все записи.");
            this.variableController.SetVariable(varName, typeof(DataTable), outRecords);
            Log.Logger.Information($"Выходные данные INSERT запроса " + Environment.NewLine + EvidentInstruction.Helpers.Message.CreateMessage((DataTable)outRecords));
        }

        /// <summary>
        /// Шаг занесения данных в базу данных.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название таблицы.</param>
        /// <param name="data">Данные.</param>
        [Scope(Tag = "SqlServer")]
        [When(@"я заношу записи в БД ""(.+)"" в таблицу ""(.+)"" без сохранения занесения в переменную:")]
        public void InsertRowsIntoDb(string connectionName, string tableName, DataTable data)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Подключение с названием \"{connectionName}\" не существует");
            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;

            var (outRecords, count) = connection.InsertRows(tableName, data, timeout);
            count.Should().Be(data.Rows.Count, "Были добавлены не все записи.");
            Log.Logger.Information($"Выходные данные INSERT запроса " + Environment.NewLine + EvidentInstruction.Helpers.Message.CreateMessage((DataTable)outRecords));
        }

        /// <summary>
        /// Шаг обновления данных в базе данных.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "SqlServer")]
        [When(@"я обновляю записи в БД ""(.+)"" без занесения в переменную:")]
        public void UpdateRowsInDb(string connectionName, string query)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Подключение с названием \"{connectionName}\" не существует");
            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;

            query = this.variableController.ReplaceVariables(query);

            var count = connection.UpdateRows(query:query, timeout:timeout);
            count.Should().NotBe(0, "Запрос: " + $"{Environment.NewLine}" + $"{query}" + $"{ Environment.NewLine}" +
                $" вернул не одну запись");
            Log.Logger.Information($"UPDATE запрос:" + Environment.NewLine + $"{query}" + $" вернул {count} запись");
        }

        /// <summary>
        /// Шаг выполнения кода в базе данных.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "SqlServer")]
        [When(@"я выполняю NonQuery запрос в БД ""(.+)"":")]
        public void ExecuteQueryInDb(string connectionName, string query)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Подключение с названием \"{connectionName}\" не существует");
            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;

            query = this.variableController.ReplaceVariables(query);

            Log.Logger.Information($"NonQuery запрос:" + Environment.NewLine + $"{query}");

            var count = connection.ExecuteNonQuery(query:query, timeout:timeout);
            Log.Logger.Information($"NonQuery запрос:" + Environment.NewLine + $"{query}" + $" вернул {count} запись");
        }
    }
}

// <copyright file="DatabaseSteps.Steps.cs" company="AlfaBank">
// Copyright (c) AlfaBank. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AlfaBank.AFT.Core.Data.DataBase.DbCommandParameter;
using AlfaBank.AFT.Core.Data.DataBase.DbConnectionParams;
using AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper;
using AlfaBank.AFT.Core.Data.DataBase.DbQueryParameters;
using AlfaBank.AFT.Core.Exceptions;
using AlfaBank.AFT.Core.Helpers;
using AlfaBank.AFT.Core.Models.Context;
using FluentAssertions;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AlfaBank.AFT.Core.Library.Database
{
    /// <summary>
    /// Шаги для работы с базами данных.
    /// </summary>
    [Binding]
    public class DatabaseSteps
    {
        private readonly DatabaseContext databaseContext;
        private readonly VariableContext variableContext;
        private readonly ConfigContext config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSteps"/> class.
        /// Привязка шагов работы с базами данных к работе с переменным через контекст.
        /// </summary>
        /// <param name="databaseContext">Контекст для работы с базой данных.</param>
        /// <param name="variableContext">Контекст для работы с переменными.</param>
        public DatabaseSteps(DatabaseContext databaseContext, VariableContext variableContext, ConfigContext config)
        {
            this.databaseContext = databaseContext;
            this.config = config;
            this.variableContext = variableContext;
        }

        /// <summary>
        /// Инициализация подключений к базам данных перед сценарием.
        /// </summary>
        [BeforeScenario]
        [Scope(Tag = "DBAccess")]
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        public void BeforeScenario()
        {
            this.databaseContext.DbConnections = new Dictionary<string, (DbConnectionWrapper, int?)>();
        }

        /// <summary>
        /// Очистка подключений к базам данных в конце сценария.
        /// </summary>
        [AfterScenario]
        [Scope(Tag = "DBAccess")]
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        public void AfterScenario()
        {
            foreach (var kvp in this.databaseContext.DbConnections)
            {
                kvp.Value.connection?.Dispose();
            }

            this.databaseContext.DbConnections.Clear();
        }

        /// <summary>
        /// Трансформация параметров подключения к SqlServer в SqlServerConnectionParams.
        /// </summary>
        /// <param name="dataTable">Параметры подключения.</param>
        /// <returns>Параметры подключения к SqlServer.</returns>
        [StepArgumentTransformation]
        [Scope(Tag = "DBAccess")]
        public SqlServerConnectionParams GetDataBaseParametersFromTableSqlServer(Table dataTable)
        {
            return dataTable.CreateInstance<SqlServerConnectionParams>();
        }

        /// <summary>
        /// Трансформация параметров подключения к MongoDB в MongoConnectionParams.
        /// </summary>
        /// <param name="dataTable">Параметры подключения.</param>
        /// <returns>Параметры подключения к MongoDB.</returns>
        [StepArgumentTransformation]
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        public MongoDBConnectionParams GetDataBaseParametersFromTableMongoDB(Table dataTable)
        {
            return dataTable.CreateInstance<MongoDBConnectionParams>();
        }

        /// <summary>
        /// Трансформация запросов в DbQueryParameters.
        /// </summary>
        /// <param name="dataTable">Таблица запроса.</param>
        /// <returns>DbQueryParameters.</returns>
        [StepArgumentTransformation]
        [Scope(Tag = "DBAccess")]
        public DbQueryParameters GetDataBaseQueryParametersSelect(Table dataTable)
        {
            var @params = new DbQueryParameters();

            if (!(dataTable?.RowCount > 0))
            {
                return @params;
            }

            if (dataTable.ContainsColumn("Таблицы"))
            {
                @params.Tables = dataTable.Rows[0]["Таблицы"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (dataTable.ContainsColumn("Столбцы"))
            {
                @params.Columns = dataTable.Rows[0]["Столбцы"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (dataTable.ContainsColumn("Условия"))
            {
                @params.Conditions = dataTable.Rows[0]["Условия"];
            }

            if (dataTable.ContainsColumn("ЛимитСтрок"))
            {
                @params.LineLimit = int.Parse(dataTable.Rows[0]["ЛимитСтрок"]);
            }

            if (dataTable.ContainsColumn("ПропускСтрок"))
            {
                @params.LineSkip = int.Parse(dataTable.Rows[0]["ПропускСтрок"]);
            }

            return @params;
        }

        /// <summary>
        /// Подключение к SQLServer.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="params">Параметры подключения.</param>
        [Scope(Tag = "DBAccess")]
        [Given(@"я подключаюсь к БД MS SQL Server с названием ""(.+)"":")]
        public void ConnectToDB_SqlServer(string connectionName, SqlServerConnectionParams @params)
        {
            @params.Should().NotBeNull("Параметры не заданы.");

            this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value.connection.Should()
                .BeNull($"Подключение с названием '{connectionName}' уже существует");

            var parameters = new Dictionary<string, object>()
            {
                { "DataSource", this.variableContext.ReplaceVariablesInXmlBody(@params.Source) },
                { "InitialCatalog", this.variableContext.ReplaceVariablesInXmlBody(@params.Database) },
                { "UserID", this.variableContext.ReplaceVariablesInXmlBody(@params.Login) },
                { "Password", this.variableContext.ReplaceVariablesInXmlBody(@params.Password) }
            };

            var connection = new SqlServerConnectionWrapper();
            connection.GetDb(parameters);

            this.databaseContext.DbConnections.Add(connectionName, (connection, @params.Timeout));

        }

        /// <summary>
        /// Подключение к Mongo.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="params">Параметры подключения.</param>
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        [Given(@"я подключаюсь к MongoDB с названием ""(.+)"":")]
        public void ConnectToDB_Mongo(string connectionName, MongoDBConnectionParams @params)
        {
            @params.Should().NotBeNull("Параметры не заданы.");

            this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value.Should()
            .BeNull($"Подключение с названием '{connectionName}' уже существует");

            var parameters = new Dictionary<string, object>()
                {
                    { "DataSource", this.variableContext.ReplaceVariablesInXmlBody(@params.Source) },
                    { "InitialCatalog", this.variableContext.ReplaceVariablesInXmlBody(@params.Database) },
                    { "UserID", this.variableContext.ReplaceVariablesInXmlBody(@params.Login) },
                    { "Password", this.variableContext.ReplaceVariablesInXmlBody(@params.Password) },
                };

            var connection = new MongoDBConnectionWrapper();
            var (_, error) = connection.GetDb(parameters);
            error.Any().Should().BeFalse($"При выполнении подключения возникли ошибки.");
            this.databaseContext.DbConnections.Add(connectionName, (connection, @params.Timeout));
        }

        /// <summary>
        /// Шаг выборки записей из базы данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "DBAccess")]
        [StepDefinition(@"я выбираю несколько записей из БД ""(.+)"" и сохраняю их в переменную ""(.+)"":")]
        public void SelectRowsFromDbSetVariable(string connectionName, string varName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключения с названием '{connectionName}' не существует");

            query.Should().NotBeEmpty("Запрос не может быть пустым.");
            query = this.variableContext.ReplaceVariablesInXmlBody(query);

            var @params = this.variableContext.Variables
                .Where(_ => _.Value?.Type.IsValueType == true)
                .Select(_ => new DbCommandParameter { Name = _.Key, DbType = DbType.Object, Value = _.Value.Value })
                .ToList();

            var sqlError = this.databaseContext.IsSqlQueryValid(query);
            sqlError.Any().Should().BeFalse($"Запрос '{query}' не корректен");

            var (outRecords, _, error) = conn.connection.SelectQuery(query, null, @params, conn.timeout);
            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            (outRecords is DataTable).Should().BeTrue("Выходные данные не являются типом DataTable");
            this.variableContext.SetVariable(varName, typeof(DataTable), outRecords);
        }

        /// <summary>
        /// Шаг выборки записей из Mongo и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        [StepDefinition(@"я выбираю несколько записей из MongoDB ""(.+)"" из коллекции ""(.+)"" и сохраняю их в переменную ""(.+)"":")]
        public void SelectRowsFromMongoDbSetVariable(string connectionName, string tableName, string varName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");
            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            query = this.variableContext.ReplaceVariablesInJsonBody(query, (val) =>
            System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));

            var act = new Action(() => JToken.Parse(query));
            act.Should().NotThrow<Exception>("Некорректный запрос для поиска.");

            var (outResponse, count, error) = conn.connection.SelectQuery(query, tableName);
            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            count.Should().NotBe(0, "Запрос вернул пустую выборку.");

            this.variableContext.SetVariable(
                varName,
                outResponse.GetType(),
                outResponse);
        }

        /// <summary>
        /// Шаг выборки записи из базы данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "DBAccess")]
        [StepDefinition(@"я выбираю единственную запись из БД ""(.+)"" и сохраняю её в переменную ""(.+)"":")]
        public void SelectSingleRowFromDbSetVariable(string connectionName, string varName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");

            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            query = this.variableContext.ReplaceVariablesInXmlBody(query);

            var @params = this.variableContext.Variables
                .Where(_ => _.Value?.Type.IsValueType == true)
                .Select(_ => new DbCommandParameter { Name = _.Key, DbType = DbType.Object, Value = _.Value.Value })
                .ToList();

            var sqlError = this.databaseContext.IsSqlQueryValid(query);
            sqlError.Any().Should().BeFalse($"Запрос '{query}' не корректен");

            var (outRecords, count, error) = conn.connection.SelectQuery(query, null, @params, conn.timeout);
            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            count.Should().Be(1, "Запрос вернул не одну запись");

            (outRecords is DataTable).Should().BeTrue("Выходные данные не являются типом DataTable");
            this.variableContext.SetVariable(varName, typeof(DataRow), ((DataTable)outRecords).Rows[0]);
        }

        /// <summary>
        /// Шаг выборки единственной записи из Mongo и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        [StepDefinition(@"я выбираю единственную запись из MongoDB ""(.+)"" из коллекции ""(.+)"" и сохраняю её в переменную ""(.+)"":")]
        public void SelectSingleRowFromMongoDbSetVariable(string connectionName, string tableName, string varName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");
            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            query = this.variableContext.ReplaceVariablesInJsonBody(query, (val) =>
            System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));

            var act = new Action(() => JToken.Parse(query));
            act.Should().NotThrow<Exception>("Некорректный запрос для поиска.");

            var (outResponse, count, error) = conn.connection.SelectQuery(query, tableName);
            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            count.Should().Be(1, "Выборка по запросу содержит не одну запись.");

            this.variableContext.SetVariable(
                varName,
                ((IEnumerable<BsonDocument>)outResponse).Single().GetType(),
                ((IEnumerable<BsonDocument>)outResponse).Single());
        }

        /// <summary>
        /// Шаг выборки единственной ячейки из базы данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "DBAccess")]
        [StepDefinition(@"я сохраняю значение единственной ячейки из выборки из БД ""(.+)"" в переменную ""(.+)"":")]
        public void SelectScalarFromDbSetVariable(string connectionName, string varName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");

            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            var @params = this.variableContext.Variables
                .Where(_ => _.Value?.Type.IsValueType == true)
                .Select(_ => new DbCommandParameter { Name = _.Key, DbType = DbType.Object, Value = _.Value.Value })
                .ToList();

            query = this.variableContext.ReplaceVariablesInXmlBody(query);
            var sqlError = this.databaseContext.IsSqlQueryValid(query);
            sqlError.Any().Should().BeFalse($"Запрос '{query}' не корректен");

            var (outRecords, count, error) = conn.connection.SelectQuery(query, null, @params, conn.timeout);
            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            count.Should().Be(1, "Запрос вернул не одну запись");

            (outRecords is DataTable).Should().BeTrue("Выходные данные не являются типом DataTable");

            this.variableContext.SetVariable(
                varName,
                ((DataTable)outRecords).Columns[0].DataType,
                ((DataTable)outRecords).Rows[0][0]);
        }

        /// <summary>
        /// Шаг выборки одной записи из Mongo и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        [StepDefinition(@"я сохраняю значение первой ячейки из выборки из MongoDB ""(.+)"" из коллекции ""(.+)"" в переменную ""(.+)"":")]
        public void SelectScalarFromMongoDbSetVariable(string connectionName, string tableName, string varName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");
            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            query = this.variableContext.ReplaceVariablesInJsonBody(query, (val) =>
            System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));

            var act = new Action(() => JToken.Parse(query));
            act.Should().NotThrow<Exception>("Некорректный запрос для поиска.");

            var (outResponse, count, error) = conn.connection.SelectQuery(query, tableName);
            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            count.Should().NotBe(0, "Ничего не вернулось.");

            this.variableContext.SetVariable(
                varName,
                ((IEnumerable<BsonDocument>)outResponse).First().GetType(),
                ((IEnumerable<BsonDocument>)outResponse).First());
        }

        /// <summary>
        /// Шаг занесения данных в базу данных и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название таблицы.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="data">Данные.</param>
        [Scope(Tag = "DBAccess")]
        [When(@"я заношу записи в БД ""(.+)"" в таблицу ""(.+)"" и сохраняю результат в переменную ""(.+)"":")]
        public void InsertRowsIntoDbSetVariable(string connectionName, string tableName, string varName, Table data)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");

            var @params = this.variableContext.Variables
                .Where(_ => _.Value?.Type.IsValueType == true)
                .Select(_ => new DbCommandParameter { Name = _.Key, DbType = DbType.Object, Value = _.Value.Value })
                .ToList();

            var inRecords = this.TransformationTableToDatatable(data);

            var (outRecords, count, error) = conn.connection.InsertRows(tableName, inRecords, @params, conn.timeout);
            var enumerable = error as Error[] ?? error.ToArray();
            //enumerable.Any().Should().BeFalse($"При добавлении данных возникли ошибки");
            error.Any().Should().BeFalse($"При добавлении данных возникли ошибки");
            count.Should().Be(data.RowCount, "Были добавлены не все записи.");
            this.variableContext.SetVariable(varName, typeof(DataTable), outRecords);
        }

        /// <summary>
        /// Шаг занесения данных в Mongo и сохранения в переменную.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="data">Данные.</param>
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        [When(@"я заношу записи в MongoDB ""(.+)"" в коллекцию ""(.+)"" и сохраняю результат в переменную ""(.+)"":")]
        public void InsertRowsIntoMongoDbSetVariable(string connectionName, string tableName, string varName, string data)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");
            data.Should().NotBeEmpty("Запрос не может быть пустым.");

            data = this.variableContext.ReplaceVariablesInJsonBody(data, (val) =>
            System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));

            int amountOfRecords = 0;
            var act = new Action(() => amountOfRecords = JToken.Parse(data).ToList().Count);
            act.Should().NotThrow<Exception>("Некорректный запрос для вставки.");

            var (outResponse, count, error) = conn.connection.InsertRows(tableName, data);
            error.Any().Should().BeFalse($"При добавлении данных возникли ошибки");
            count.Should().Be(amountOfRecords, $"'{count}' из '{amountOfRecords}' добавлены в базу данных.");

            this.variableContext.SetVariable(
                varName,
                outResponse.GetType(),
                outResponse);
        }

        /// <summary>
        /// Шаг занесения данных в базу данных.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название таблицы.</param>
        /// <param name="data">Данные.</param>
        [Scope(Tag = "DBAccess")]
        [When(@"я заношу записи в БД ""(.+)"" в таблицу ""(.+)"" без сохранения занесения в переменную:")]
        public void InsertRowsIntoDb(string connectionName, string tableName, Table data)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");

            var @params = this.variableContext.Variables
                .Where(_ => _.Value?.Type.IsValueType == true)
                .Select(_ => new DbCommandParameter { Name = _.Key, DbType = DbType.Object, Value = _.Value.Value })
                .ToList();

            var inRecords = this.TransformationTableToDatatable(data);

            var (_, count, error) = conn.connection.InsertRows(tableName, inRecords, @params, conn.timeout);
            error.Any().Should().BeFalse($"При добавлении данных возникли ошибки");
            count.Should().Be(data.RowCount, "Были добавлены не все записи.");
        }

        /// <summary>
        /// Шаг занесения данных в Mongo.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        /// <param name="data">Данные.</param>
        [Scope(Tag = "Mongo")]
        [Scope(Tag = "mongo")]
        [When(@"я заношу записи в MongoDB ""(.+)"" в коллекцию ""(.+)"" без сохранения занесения в переменную:")]
        public void InsertRowsIntoMongoDb(string connectionName, string tableName, string varName, string data)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");
            data.Should().NotBeEmpty("Запрос не может быть пустым.");

            data = this.variableContext.ReplaceVariablesInJsonBody(data, (val) =>
            System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));
            int amountOfRecords = 0;

            var act = new Action(() => amountOfRecords = JToken.Parse(data).ToList().Count);
            act.Should().NotThrow<Exception>("Некорректный запрос для вставки.");

            var (_, count, error) = conn.connection.InsertRows(tableName, data);

            error.Any().Should().BeFalse($"При добавлении данных возникли ошибки");
            count.Should().Be(amountOfRecords, $"'{count}' из '{amountOfRecords}' добавлены в базу данных.");
        }

        /// <summary>
        /// Шаг обновления данных в базе данных.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "DBAccess")]
        [When(@"я обновляю записи в БД ""(.+)"" без занесения в переменную:")]
        public void UpdateRowsInDb(string connectionName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");

            query.Should().NotBeEmpty("Запрос не может быть пустым.");
            var @params = this.variableContext.Variables
                .Where(_ => _.Value?.Type.IsValueType == true)
                .Select(_ => new DbCommandParameter { Name = _.Key, DbType = DbType.Object, Value = _.Value.Value })
                .ToList();

            query = this.variableContext.ReplaceVariablesInXmlBody(query);

            var sqlError = this.databaseContext.IsSqlQueryValid(query);
            sqlError.Any().Should().BeFalse($"Запрос '{query}' не корректен");

            var (count, error) = conn.connection.UpdateRows(query, null, @params, conn.timeout);

            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            count.Should().NotBe(0, "Запрос ничего не обновил");
        }

        /// <summary>
        /// Шаг выполнения кода в базе данных.
        /// </summary>
        /// <param name="connectionName">Название подключения.</param>
        /// <param name="query">Запрос.</param>
        [Scope(Tag = "DBAccess")]
        [When(@"я выполняю запрос в БД ""(.+)"":")]
        public void ExecuteQueryInDb(string connectionName, string query)
        {
            var conn = this.databaseContext.DbConnections.SingleOrDefault(_ => _.Key == connectionName).Value;
            conn.Should().NotBeNull($"Подключение с названием '{connectionName}' не существует");

            query.Should().NotBeEmpty("Запрос не может быть пустым.");

            query = this.variableContext.ReplaceVariablesInXmlBody(query);

            var sqlError = this.databaseContext.IsSqlQueryValid(query);
            sqlError.Any().Should().BeFalse($"Запрос '{query}' не корректен");

            var (_, count, error) = conn.connection.ExecuteQuery(query, conn.timeout);

            error.Any().Should().BeFalse($"При выполнении запроса возникли ошибки");
            count.Should().NotBe(0, "Запрос ничего не сделал");
        }

        private DataTable TransformationTableToDatatable(Table table)
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
                            s = this.variableContext.ReplaceVariablesInXmlBody(s);
                            return s;
                        })
                        .ToArray());
            }

            return inRecords;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using EvidentInstruction.Controllers;
using EvidentInstruction.Database.Controllers;
using EvidentInstruction.Database.Models;
using EvidentInstruction.Helpers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using EvidentInstruction.Database.Infrastructures;
using System.Data;
using EvidentInstruction.Database.Helpers;

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
        /// Выполнение ExecuteQuery или ExecuteNonQuery
        /// </summary>      
        private (object, int) ExecuteAnyRequest(QueryType queryType, string connectionName, string query)
        {
            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Connection: \"{connectionName}\" does not exist");
            query.Should().NotBeEmpty("Query cannot be empty.");

            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;

            query = this.variableController.ReplaceVariables(query);

            Log.Logger.Information($"{queryType} query:" + Environment.NewLine + $"{query}");

            switch (queryType)
            {
                case (QueryType.SELECT):
                    Log.Logger.Information($"Choose {queryType} query. Query type is ExecuteQuery");
                    var (outRecords, count) = connection.ExecuteQuery(query, timeout);
                    Log.Logger.Information($"Request returned: {Environment.NewLine} {EvidentInstruction.Helpers.Message.CreateMessage((DataTable)outRecords)}");
                    return (outRecords, count);
                default:
                    Log.Logger.Information($"Choose {queryType} query. Query type is ExecuteNonQuery");
                    return (null, connection.ExecuteNonQuery(query, timeout));
            }
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

        /// <summary>
        /// Преобразование Таблицы с параметрами в Список словарей для создания запроса
        /// </summary>        
        [StepArgumentTransformation]
        [Scope(Tag = "SqlServer")]
        public IEnumerable<Dictionary<string, object>> TransformationTableToString(Table table)
        {
            var tableParameters = new List<Dictionary<string, object>>();

            IEnumerable<dynamic> insertTable = table.CreateDynamicSet();

            var list = Enumerable.ToList(insertTable);

            if (!list.Any())
            {
                Log.Logger.Warning("List with table patameters is empty.");
                throw new ArgumentNullException("List with table patameters is empty.");
            }

            foreach (var element in list)
            {
                DateTime date;

                tableParameters
                    .Add(((IDictionary<string, object>)element)
                    .ToDictionary(e => e.Key, e =>
                    {
                        if (string.IsNullOrWhiteSpace(e.Value.ToString()))
                        {
                            return "NULL";
                        }

                        if (DateTime.TryParse(e.Value.ToString(), out date))
                        {
                            var result = date.ToString("yyyy-M-dd");
                            return $"'{result}'";
                        }

                        if (e.Value.ToString().ToUpper() == "TRUE" || e.Value.ToString().ToUpper() == "FALSE")
                        {
                            return e.Value;
                        }

                        if (e.Value.ToString().Any(c => char.IsLetter(c)) & e.Value.ToString().ToUpper() != "NULL")
                        {
                            return $"'{e.Value}'";
                        }

                        return e.Value;
                    }));
            }
            return tableParameters;
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

            this.databaseController.Connections.ContainsKey(connectionName).Should().BeFalse($"Connection \"{connectionName}\" already exists");

            var connection = new SqlServerClient();
            connection.Create(connectionParams).Should().BeTrue();

            connection.IsConnectAlive().Should().BeTrue();

            this.databaseController.Connections.TryAdd(connectionName, (connection, connectionParams.Timeout));
            this.databaseController.Connections.Should().NotBeEmpty();
        }

        /// <summary>
        /// Выполнение ExecuteQuery или ExecuteNonQuery БЕЗ сохранения результата
        /// </summary>        
        [Scope(Tag = "SqlServer")]
        [StepDefinition(@"я выполняю ""(.+)"" запрос в БД ""(.+)"" и не сохраняю результат:")]
        public void ExecuteQuery(QueryType queryType, string connectionName, string query)
        {
            var (_, count) = ExecuteAnyRequest(queryType, connectionName, query);
            Log.Logger.Information($"Request returned {count} row(s)");
        }

        /// <summary>
        /// Выполнение ExecuteQuery или ExecuteNonQuery C сохранением результата
        /// </summary>        
        [Scope(Tag = "SqlServer")]
        [StepDefinition(@"я выполняю ""(.+)"" запрос в БД ""(.+)"" и сохраняю результат в переменную ""(.+)"":")]
        public void ExecuteQuery(QueryType queryType, string connectionName, string varName, string query)
        {
            var (outRecords, count) = ExecuteAnyRequest(queryType, connectionName, query);

            this.variableController.SetVariable(varName, typeof(DataTable), outRecords);

            Log.Logger.Information($"Request returned {count} row(s)");
        }

        /// <summary>
        /// Шаг выполнения Insert в БД с указанными значениями
        /// </summary>        
        [Scope(Tag = "SqlServer")]
        [StepDefinition(@"я добавляю записи в таблицу ""(.+)"" в БД ""(.+)"":")]
        public void ExecuteInsertQueryFromTable(string tableName, string connectionName, IEnumerable<Dictionary<string, object>> insertQuery)
        {

            this.databaseController.Connections.ContainsKey(connectionName).Should().BeTrue($"Connection: \"{connectionName}\" does not exist");
            var (connection, timeout) = this.databaseController.Connections.SingleOrDefault(_ => _.Key == connectionName).Value;

            connection.IsConnectAlive().Should().BeTrue();

            var query = insertQuery.ToSqlQuery(tableName);

            var count = connection.ExecuteNonQuery(query, timeout);

            count.Should().NotBe(0, "INSERT failed. Check table names or values");
            Log.Logger.Information($"INSERT completed {Environment.NewLine} {query}. {Environment.NewLine} Changed {count} row(s).");


        }
    }
}

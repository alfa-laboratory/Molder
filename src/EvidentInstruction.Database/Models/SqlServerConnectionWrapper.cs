using Microsoft.SqlServer.TransactSql.ScriptDom;
using EvidentInstruction.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace EvidentInstruction.Database.Models
{
    public class SqlServerConnectionWrapper : ConnectionWrapper
    {
        public SqlServerConnectionWrapper(DbConnectionParams connectionParams)
        {
            ConnectionParams = connectionParams;
        }

        public override DbConnection GetDb()
        {
            try
            {
                if (DbConnection != null)
                {
                    return DbConnection;
                }

                if ((DateTime.Now - LastConnect).TotalSeconds < ConnectPeriod)
                {
                    System.Threading.Thread.Sleep((int)Math.Ceiling(ConnectPeriod - (DateTime.Now - LastConnect).TotalSeconds) * 1000);
                }

                var csb = new SqlConnectionStringBuilder()
                {
                    DataSource = ConnectionParams.Source,
                    InitialCatalog = ConnectionParams.Database,
                    UserID = ConnectionParams.Login,
                    Password = ConnectionParams.Password
                };

                if (csb.ConnectTimeout <= 0)
                {
                    csb.ConnectTimeout = 60;
                }

                if (csb.LoadBalanceTimeout <= 0)
                {
                    csb.LoadBalanceTimeout = 60;
                }

                DbConnection = new SqlConnection(csb.ToString());
                DbConnection.Open();

                LastConnect = DateTime.Now;
                return DbConnection;
            }
            catch(SqlException ex)
            {
                Log.Logger.Error(ex.Message);
                return null;
            }
            catch(InvalidOperationException ex)
            {
                Log.Logger.Error(ex.Message);
                return null;
            }
        }

        public override int DeleteRows(string query, string tableName = null, int? timeout = null)
        {
            Log.Logger.Information($"DELETE запрос:" + Environment.NewLine + $"{query}");
            return ExecuteNonQuery(query: query, timeout: timeout);
        }

        public override (object, int) InsertRows(string tableName, object records, int? timeout = null)
        {
            var (query, listParams) = CreateInsertStatement(tableName, ((DataTable)records));
            Log.Logger.Information($"INSERT запрос:" + Environment.NewLine + $"{query}");
            return ExecuteQuery(query: query, timeout: timeout);
        }

        public override (object, int) SelectQuery(string query, string tableName = null, int? timeout = null)
        {
            Log.Logger.Information($"SELECT запрос:" + Environment.NewLine + $"{query}");
            return ExecuteQuery(query: query, timeout:timeout);
        }

        public override int UpdateRows(string query, string tableName = null, int? timeout = null)
        {
            Log.Logger.Information($"UPDATE запрос:" + Environment.NewLine + $"{query}");
            return ExecuteNonQuery(query: query, timeout: timeout);
        }

        protected override void CreateCommandParameter(ref DbCommand command, DbCommandParameter parameter)
        {
            (command as SqlCommand)?.Parameters.AddWithValue(parameter.Name, parameter.Value);
        }

        private InsertStatement CreateStatement(string tableName, string identifier = "INSERTED")
        {
            var statement = new InsertStatement()
            {
                InsertSpecification = new InsertSpecification()
            };

            var schemaObjectName = new SchemaObjectName();
            schemaObjectName.Identifiers.Add(new Identifier() { Value = tableName });

            statement.InsertSpecification.Target = new NamedTableReference()
            {
                SchemaObject = schemaObjectName
            };

            statement.InsertSpecification.OutputClause = new OutputClause();
            statement.InsertSpecification.OutputClause.SelectColumns.Add(new SelectStarExpression()
            {
                Qualifier = new MultiPartIdentifier()
            });

            ((SelectStarExpression)statement.InsertSpecification.OutputClause.SelectColumns.Single()).Qualifier.Identifiers.Add(new Identifier()
            {
                Value = identifier
            });

            return statement;
        }

        private InsertStatement AddColumnReferenceExpression(InsertStatement statement, DataTable records)
        {
            var insertStatement = statement;

            foreach (DataColumn column in records.Columns)
            {
                var schemaObjectName = new SchemaObjectName();
                schemaObjectName.Identifiers.Add(new Identifier() { Value = column.ColumnName });
                insertStatement.InsertSpecification.Columns.Add(new ColumnReferenceExpression()
                {
                    MultiPartIdentifier = schemaObjectName
                });
            }

            return insertStatement;
        }

        private (string, List<DbCommandParameter>) CreateInsertStatement(string tableName, DataTable records, ICollection<DbCommandParameter> parameter = null)
        {
            var statement = CreateStatement(tableName);
            statement = AddColumnReferenceExpression(statement, records);
            statement.InsertSpecification.InsertOption = InsertOption.Into;

            var listParams = new List<DbCommandParameter>(parameter ?? new DbCommandParameter[] { });
            var numberParams = 0;
            statement.InsertSpecification.InsertSource = new ValuesInsertSource();

            IList<ParseError> parseErrors;
            foreach (DataRow row in records.Rows)
            {
                var rv = new RowValue();
                foreach (DataColumn column in records.Columns)
                {
                    var cell = row[column];
                    if (cell is string name)
                    {
                        var exp = new TSql100Parser(false).ParseExpression(
                            new System.IO.StringReader(name), out parseErrors);
                        if (exp.GetType() == typeof(VariableReference) || exp.GetType() == typeof(GlobalVariableExpression))
                        {
                            rv.ColumnValues.Add(new VariableReference() { Name = name });
                            continue;
                        }
                    }

                    while (listParams.Any(_ => _.Name == $"p{numberParams}"))
                    {
                        numberParams++;
                    }

                    rv.ColumnValues.Add(new VariableReference() { Name = $"@p{numberParams}" });
                    listParams.Add(
                        new DbCommandParameter(
                            $"p{numberParams}",
                            DbType.Object,
                            cell));
                    numberParams++;
                }

                ((ValuesInsertSource)statement.InsertSpecification.InsertSource).RowValues.Add(rv);
            }

            var generator = new Sql100ScriptGenerator();
            generator.GenerateScript(statement, out var query, out parseErrors);

            if (parseErrors.Any())
            {
                Log.Logger.Error(Helpers.Message.CreateMessage(parseErrors));
            }

            return (query, listParams);
        }
    }
}

using AlfaBank.AFT.Core.Exceptions;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using FluentAssertions;

namespace AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper
{
    public class SqlServerConnectionWrapper : DbConnectionWrapper
    {
        public override (DbConnection, IEnumerable<Error>) GetDb(IDictionary<string, object> @params)
        {
            if (DbConnection != null)
            {
                return (DbConnection, null);
            }

            if ((DateTime.Now - LastConnect).TotalSeconds < ConnectPeriod)
            {
                System.Threading.Thread.Sleep((int)Math.Ceiling(ConnectPeriod - (DateTime.Now - LastConnect).TotalSeconds) * 1000);
            }

            var csb = new SqlConnectionStringBuilder();
            foreach (var p in @params)
            {
                typeof(SqlConnectionStringBuilder).GetProperty(p.Key)?.SetValue(csb, p.Value);
            }

            if (csb.ConnectTimeout <= 0)
            {
                csb.ConnectTimeout = 30;
            }

            if (csb.LoadBalanceTimeout <= 0)
            {
                csb.LoadBalanceTimeout = 30;
            }

            DbConnection = new SqlConnection(csb.ToString());
            DbConnection.Open();

            LastConnect = DateTime.Now;
            return (DbConnection, null);
        }

        public override (object, int, IEnumerable<Error>) SelectQuery(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null)
        {
            return ExecuteQuery(query, timeout, parameter);
        }

        public override (object, int, IEnumerable<Error>) InsertRows(string tableName, object records, ICollection<DbCommandParameter.DbCommandParameter> parameter = null,
            int? timeout = null)
        {
            records.GetType().Should().Be(typeof(DataTable), "Входные данные должны быть ввиде таблицы.");

            var (query, listParams, parseErrors) = CreateInsertStatement(tableName, ((DataTable)records), parameter);
            var listErrors = new List<Error>();

            if (!parseErrors.Any())
            {
                return ExecuteQuery(query, timeout, listParams);
            }

            listErrors.AddRange(parseErrors.Select(error => new Error()
            {
                Type = typeof(ParseError),
                TargeBase = MethodBase.GetCurrentMethod(),
                Message = string.Join(Environment.NewLine, parseErrors.Select(_ => $"Error #{_.Number} at line {_.Line}, column {_.Column}: {_.Message}"))
            }));
            return (null, 0, listErrors);
        }

        public override (int, IEnumerable<Error>) UpdateRows(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> parameters = null, int? timeout = null)
        {
            return ExecuteNonQuery(query, parameters, timeout);
        }

        public override (int, IEnumerable<Error>) DeleteRows(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> parameters = null, int? timeout = null)
        {
            return ExecuteNonQuery(query, parameters, timeout);
        }

        public override void CreateCommandParameter(ref DbCommand command, DbCommandParameter.DbCommandParameter parameter)
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

        private (string, List<DbCommandParameter.DbCommandParameter>, IList<ParseError>) CreateInsertStatement(string tableName, DataTable records, ICollection<DbCommandParameter.DbCommandParameter> parameter = null)
        {
            var statement = CreateStatement(tableName);
            statement = AddColumnReferenceExpression(statement, records);
            statement.InsertSpecification.InsertOption = InsertOption.Into;

            var listParams = new List<DbCommandParameter.DbCommandParameter>(parameter ?? new DbCommandParameter.DbCommandParameter[] { });
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
                        new DbCommandParameter.DbCommandParameter(
                            $"p{numberParams}",
                            DbType.Object,
                            cell));
                    numberParams++;
                }

                ((ValuesInsertSource)statement.InsertSpecification.InsertSource).RowValues.Add(rv);
            }

            var generator = new Sql100ScriptGenerator();
            generator.GenerateScript(statement, out var query, out parseErrors);

            return (query, listParams, parseErrors);
        }
    }
}

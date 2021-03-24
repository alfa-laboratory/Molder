using Molder.Database.Exceptions;
using Molder.Database.Models;
using FluentAssertions;
using Moq;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Database.Tests
{
    [ExcludeFromCodeCoverage]
    public class SqlServerClientTests
    {
        private DbConnectionParams dbConnectionParams;
        private const string dbConnectionString = "TestConnectionString";


        public SqlServerClientTests()
        {
            dbConnectionParams = new DbConnectionParams() { Database = "Test", Source = "test", Login = "test", Password = "W9qNIafQbJCZzEafUaYmQw==", Timeout = 1, ConnectRetryCount = 0, ConnectRetryInterval = 1 };
        }

        [Fact]
        public void Create_CorrectConnectionParams_ReturnTrue()
        {
            var mockSqlProvider = new Mock<ISqlProvider>();

            var client = new SqlServerClient();
            mockSqlProvider.Setup(c => c.Create(It.IsAny<string>())).Returns(true);

            client._provider = mockSqlProvider.Object;
            client.Create(dbConnectionParams).Should().BeTrue();
        }

        [Fact]
        public void Create_IncorrectConnectionParams_ReturnThrow()
        {
            var mockSqlProvider = new Mock<ISqlProvider>();

            var client = new SqlServerClient();
            mockSqlProvider.Setup(c => c.Create(It.IsAny<string>())).Throws(new ConnectSqlException("test"));
            client._provider = mockSqlProvider.Object;

            Action action = () => client.Create(dbConnectionParams);
            action.Should()
                .Throw<ConnectSqlException>()
                 .WithMessage("Connection failed. test");
        }

        [Theory]
        [InlineData(null)]
        public void Create_ConnectionParamsIsNull_ReturnThrow(DbConnectionParams parameter)
        {

            var client = new SqlServerClient();
            Action action = () => client.Create(parameter).Should().BeFalse();
            action.Should()
                .Throw<System.Exception>()
                .WithMessage("Connection failed: Object reference not set to an instance of an object.");
        }

        [Fact]
        public void IsConnectAlive_CorrectConnectionParams_ReturnTrue()
        {
            var mockSqlProvider = new Mock<ISqlProvider>();

            var client = new SqlServerClient();
            mockSqlProvider.Setup(c => c.IsConnectAlive()).Returns(true);

            client._provider = mockSqlProvider.Object;
            client.IsConnectAlive().Should().BeTrue();
        }

        [Fact]
        public void IsConnectAlive_CorrectConnectionParams_ReturnFalse()
        {
            var mockSqlProvider = new Mock<ISqlProvider>();

            var client = new SqlServerClient();
            mockSqlProvider.Setup(c => c.IsConnectAlive()).Returns(false);

            client._provider = mockSqlProvider.Object;
            client.IsConnectAlive().Should().BeFalse();
        }

        [Fact]
        public void ExecuteQuery_IncorrectQuery_ReturnNull()
        {
            var query = "select top 1 * test111";

            var mockSqlProvider = new Mock<ISqlProvider>();
            var connect = new SqlConnection();

            var client = new SqlServerClient();
            mockSqlProvider.Setup(c => c.SetupCommand(It.IsAny<string>(), null)).Returns(connect.CreateCommand);

            client._provider = mockSqlProvider.Object;
            var (outResult, count) = client.ExecuteQuery(query);

            count.Should().Be(0);
        }

        [Fact] 
        public void ExecuteQuery_CorrectQuery_ReturnTable()
        {
            var query = "select top 1 * from test111";

            var mockSqlProvider = new Mock<ISqlProvider>();

            var client = new SqlServerClient();

            mockSqlProvider
                .Setup(u => u.UsingTransaction(It.IsAny<Action<DbTransaction>>(), It.IsAny<Action<Exception>>(), null))
                .Callback((Action<DbTransaction> action, Action<Exception> ex, Action success) => new DataTable());

            client._provider = mockSqlProvider.Object;

            var (outResult, count) = client.ExecuteQuery(query);

            count.Should().Be(0); //TODO (not 0)
        }

        [Fact] 
        public void ExecuteNonQuery_CorrectQuery_ReturnTable()
        {
            var query = "INSERT INTO test111 (f1) VALUES (1)"; 

            var mockSqlProvider = new Mock<ISqlProvider>();

            var client = new SqlServerClient();

            mockSqlProvider
                .Setup(u => u.UsingTransaction(It.IsAny<Action<DbTransaction>>(), It.IsAny<Action<Exception>>(), null))
                .Callback((Action<DbTransaction> action, Action<Exception> ex, Action success) => new DataTable());

            client._provider = mockSqlProvider.Object;

            var count = client.ExecuteNonQuery(query);

            count.Should().Be(0); //TODO (not 0)
        }

        [Fact]
        public void ExecuteNonQuery_IncorrectQuery_ReturnNull()
        {
            var query = "INSERT INTO test111 f1 VALUES (10)";

            var mockSqlProvider = new Mock<ISqlProvider>();
            var connect = new SqlConnection();

            var client = new SqlServerClient();
            mockSqlProvider.Setup(c => c.SetupCommand(It.IsAny<string>(), null)).Returns(connect.CreateCommand);

            client._provider = mockSqlProvider.Object;
            var count = client.ExecuteNonQuery(query);

            count.Should().Be(0);
        }

        [Fact]
        public void ExecuteScalar_CorrectQuery_ReturnObject()
        {
            var query = "select top 1 * from test111";

            var mockSqlProvider = new Mock<ISqlProvider>();

            var client = new SqlServerClient();

            mockSqlProvider
                .Setup(u => u.UsingTransaction(It.IsAny<Action<DbTransaction>>(), It.IsAny<Action<Exception>>(), null))
                .Callback((Action<DbTransaction> action, Action<Exception> ex, Action success) => new object());

            client._provider = mockSqlProvider.Object;

            var outResult = client.ExecuteScalar(query);

            outResult.Should().BeNull();
        }

    }
}

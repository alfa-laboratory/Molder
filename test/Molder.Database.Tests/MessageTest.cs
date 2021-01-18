using Molder.Database.Helpers;
using Molder.Database.Models;
using Molder.Database.Models.Interfaces;
using FluentAssertions;
using Moq;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Database.Tests
{
    [ExcludeFromCodeCoverage]
    public class MessageTests
    {
        [Fact]
        public void CreateMessage_CorrectCommand_ReturnString()
        {
            var query = "select * from test111";
            var mockSqlProvider = new Mock<ISqlProvider>();
            var connect = new SqlConnection();
            var client = new SqlServerClient();

            mockSqlProvider.Setup(c => c.SetupCommand(It.IsAny<string>(), null)).Returns(connect.CreateCommand);

            client._provider = mockSqlProvider.Object;

            var command = client._provider.SetupCommand(query, null);

            var result = Message.CreateMessage(command);
            result.Should().NotBeNullOrEmpty();
        }        

        [Fact]
        public void CreateMessage_CorrectConnectionString_ReturnString()
        {
            var connectionString = "DataBase = Test; Source = Test";

            var result = Message.CreateMessage(connectionString);
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void CreateMessage_CommandIsEmpty_ReturnNull()
        {
            var query = "select * from test111";
            var mockSqlProvider = new Mock<ISqlProvider>();
            var connect = new SqlConnection();

            var client = new SqlServerClient();
            mockSqlProvider.Setup(c => c.SetupCommand(It.IsAny<string>(), null)).Returns(connect.CreateCommand);

            client._provider = mockSqlProvider.Object;

            var command = client._provider.SetupCommand(query, null);
            command = null;


            var result = Message.CreateMessage(command);
            result.Should().BeNull();


        }
    }
}

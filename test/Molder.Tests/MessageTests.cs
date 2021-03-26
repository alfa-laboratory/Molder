using FluentAssertions;
using Molder.Helpers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System;

namespace Molder.Tests
{
    /// <summary>
    /// Тесты проверки создания сообщений из List.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MessageTests
    {
        [Fact]
        public void CreateMessage_CorrectList_ReturnStr()
        {
            var list = new List<string>
            {
                "1", "2", "3"
            };

            var message = Message.CreateMessage(list);

            message.Should().ContainAll(list);
        }

        [Fact] 
        public void CreateMessage_OneElement_ReturnStr()
        {
            var list = new List<string>
            {
                "1"
            };
            var message = Message.CreateMessage(list);

            message.Should().ContainAll(list);
            message.Should().HaveLength(1);
        }

        [Fact]
        public void CreateMessage_EmptyList_ReturnNull()
        {
            var list = new List<string>();
            var message = Message.CreateMessage(list);

            message.Should().BeNull();
        }

        [Fact]
        public void CreateMessage_NullList_ReturnNull()
        {
            List<string> list = null;
            var message = Message.CreateMessage(list);

            message.Should().BeNull();
        }

        [Fact]
        public void CreateMessage_CorrectValidationResult_ReturnStr()
        {
            var list = new List<ValidationResult>
            {
                new ValidationResult("1"),
                new ValidationResult("2"),
                new ValidationResult("3")
            };

            var message = Message.CreateMessage(list);

            message.Should().Be("1\r\n2\r\n3\r\n");
        }

        [Fact]
        public void CreateMessage_EmptyValidationResult_ReturnNull()
        {
            var list = new List<ValidationResult>();

            var message = Message.CreateMessage(list);

            message.Should().BeNull();
        }

        [Fact]
        public void CreateMessage_NullValidationResult_ReturnNull()
        {
            List<ValidationResult> list = null;

            var message = Message.CreateMessage(list);

            message.Should().BeNull();
        }

        [Fact]
        public void CreateMessage_CorrectDataTable_ReturnStr()
        {
            var dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("First");
                dt.Columns.Add("Second");
            DataRow row = dt.NewRow();
                row["First"] = "1";
                row["Second"] = "2";
            dt.Rows.Add(row);

            var message = Message.CreateMessage(dt);

            message.Should().NotBeEmpty();
        }

        [Fact]
        public void CreateMessage_CorrectDataTableWithMaxRow_ReturnStr()
        {
            var dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            DataRow row = dt.NewRow();
            row["1"] = "1"; row["2"] = "2";
            row["1"] = "1"; row["2"] = "2";
            row["1"] = "1"; row["2"] = "2";
            row["1"] = "1"; row["2"] = "2";
            row["1"] = "1"; row["2"] = "2";
            row["1"] = "1"; row["2"] = "2";
            dt.Rows.Add(row);

            var message = Message.CreateMessage(dt);

            message.Should().NotBeEmpty();
        }

        [Fact]
        public void CreateMessage_CorrectDataRow_ReturnStr()
        {
            var dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            DataRow row = dt.NewRow();
            row["1"] = "1"; row["2"] = "2";

            dt.Rows.Add(row);

            var message = Message.CreateMessage(dt.Rows[0]);

            message.Should().NotBeEmpty();
        }

        [Fact]
        public void CreateMessage_NullDataTable_ReturnException()
        {
            DataTable dt = null;
            Action action = () => Message.CreateMessage(dt); ;
            action.Should().Throw<ArgumentNullException>().WithMessage("The table to convert to string is null\nParameter name: dataTable");
        }
    }
}

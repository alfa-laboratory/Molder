using FluentAssertions;
using Molder.Extensions;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Tests
{
    /// <summary>
    /// Тесты проверки extension для работы с таблицей DataTable.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TableExtensionTests
    {
        private readonly DataTable dataTable;
        public TableExtensionTests()
        {
            this.dataTable = new DataTable();
                dataTable.Columns.Add("Number", typeof(int));
                dataTable.Columns.Add("Patient", typeof(string));
                    dataTable.Rows.Add(25, "David");
                    dataTable.Rows.Add(50, "Sam");
                    dataTable.Rows.Add(10, "Christoff");
                    dataTable.Rows.Add(21, "Janet");
        }

        [Fact]
        public void ConvertToString_ValidTable_ReturnValidString()
        {
            var (str, isMoreMaxRows) = dataTable.ConvertToString();
            str.Should().NotBeNullOrEmpty();
            isMoreMaxRows.Should().BeFalse();
        }

        [Fact]
        public void ConvertToString_MoreMaxRow_ReturnValidString()
        {
            var dt = new DataTable();
            dt.Columns.Add("Number", typeof(int));
            dt.Rows.Add(1);dt.Rows.Add(1);
            dt.Rows.Add(1);dt.Rows.Add(1);
            dt.Rows.Add(1);dt.Rows.Add(1);
            dt.Rows.Add(1);dt.Rows.Add(1);
            dt.Rows.Add(1);dt.Rows.Add(1);
            dt.Rows.Add(1);dt.Rows.Add(1);
            var (str, isMoreMaxRows) = dt.ConvertToString();
            str.Should().NotBeNullOrEmpty();
            isMoreMaxRows.Should().BeTrue();
        }

        [Fact]
        public void ConvertToString_TableIsNull_ReturnException()
        {
            DataTable dt = null;
            Action action = () => dt.ConvertToString();
            action.Should().Throw<ArgumentNullException>().WithMessage("The table to convert to string is null\nParameter name: dataTable");
        }

        [Fact]
        public void ConvertToString_DataRowValid_ReturnString()
        {
            var str = dataTable.Rows[0].ConvertToString();
            str.Should().NotBeNullOrEmpty();
        }
    }
}

using FluentAssertions;
using Molder.Helpers;
using Molder.Tests.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Tests
{
    /// <summary>
    /// Тесты проверки сравнений.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CompareTests
    {
        [Fact]
        public void CompareTables_TableIsEqual_ReturnTrue()
        {
            var table = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });

            var isSame = table.AreTablesTheSame(table);
            isSame.Should().BeTrue();
        }

        [Fact]
        public void CompareTables_TableIsNotEqual_ReturnFalse()
        {
            var fTable = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a1;b1" });
            var sTable = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a2;b1" });
            var isSame = fTable.AreTablesTheSame(sTable);
            isSame.Should().BeFalse();
        }

        [Fact]
        public void CompareTables_TableIsNotEqualCount_ReturnFalse()
        {
            var fTable = CreateObject.CreateDataTable(new List<string>() { "a", "b", "c" }, new List<string>() { "a1;b1;c1" });
            var sTable = CreateObject.CreateDataTable(new List<string>() { "a", "b" }, new List<string>() { "a2;b1" });
            var isSame = fTable.AreTablesTheSame(sTable);
            isSame.Should().BeFalse();
        }
    }
}

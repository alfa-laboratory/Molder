using Molder.Controllers;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Molder.SpecFlow.Extensions;
using TechTalk.SpecFlow;
using Xunit;

namespace Molder.Tests
{
    [ExcludeFromCodeCoverage]
    public class ReplaceTests
    {
        private readonly VariableController variableContext;
        private readonly Table expectedTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceTests"/> class.
        /// Инициализация переменных.
        /// </summary>
        public ReplaceTests()
        {
            variableContext = new VariableController();
                variableContext.SetVariable("int", typeof(int), 1);
                variableContext.SetVariable("long", typeof(long), 100);
                variableContext.SetVariable("bool", typeof(bool), true);

            expectedTable = new Table("Name", "Value");
            expectedTable.AddRow("int", "1");
            expectedTable.AddRow("long", "100");
            expectedTable.AddRow("bool", "True");
        }
        
        /// <summary>
        /// Проверка замены для SpecFlow Table
        /// </summary>
        [Fact]
        public void ReplaceVariables_ValidSpecFlowTable_ReturnReplacedTable()
        {
            var table = new Table("Name", "Value");
            table.AddRow("int", "{{int}}");
            table.AddRow("long", "{{long}}");
            table.AddRow("bool", "{{bool}}");

            var actual = table.ReplaceWith(variableContext);

            expectedTable.Should().BeEquivalentTo(actual);
        }
        
        /// <summary>
        /// Проверка замены для null specflow table
        /// </summary>
        [Fact]
        public void ReplaceVariables_NullTable_ReturnNull()
        {
            Table _table = null;
            var table = _table.ReplaceWith(variableContext);
            table.Should().BeNull();
        }
    }
}

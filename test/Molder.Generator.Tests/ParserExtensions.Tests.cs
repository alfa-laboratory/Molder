using FluentAssertions;
using Molder.Controllers;
using Molder.Generator.Extensions;
using Molder.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;
using Xunit;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class ParserExtensions
    {
        private VariableController variableController;
        public ParserExtensions()
        {
            variableController = new VariableController();
        }

        [Fact]
        public void CheckToEnumerable_ReturnException()
        {
            var table = new Table(new string[] { "test", "qwerty" });
            table.AddRow("5", "6");

            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            Action act = () => table.ToEnumerable(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected table.Rows.ToList().Count to be less than 1 because Table must have only 1 row: Values, but found 1.");
        }

        [Fact]
        public void CheckToEnumerable_ReturnTrue()
        {
            var table = new Table(new string[] { "test", "qwerty" });

            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            var res = table.ToEnumerable(variableController).GetType().Name;
            res.Should().Be("List`1");
        }

        [Fact]
        public void ChecktoDoctionary_NoValues_ReturnException()
        {
            var table = new Table(new string[] { "test", "qwerty" });

            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            Action act = () => table.ToDictionary(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected table.Rows.ToList().Count to be between 1 and 1 because Table must have only 2 rows: Keys and Values, but found 0.");
        }

        [Fact]
        public void ChecktoDoctionary_ManyValues_ReturnException()
        {
            var table = new Table(new string[] { "test", "qwerty" });
            table.AddRow("test", "qwerty");
            table.AddRow("test", "qwerty");
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            Action act = () => table.ToDictionary(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected table.Rows.ToList().Count to be between 1 and 1 because Table must have only 2 rows: Keys and Values, but found 2.");
        }

        [Fact]
        public void ChecktoDoctionary_DictionaryOf2_ReturnTrue()
        {
            var table = new Table(new string[] { "test", "qwerty" });
            table.AddRow("test", "qwerty");
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            var res = table.ToDictionary(variableController).GetType().Name;
            res.Should().Be("Dictionary`2");
        }

        [Fact]
        public void ChecktoDoctionary_DictionaryOf1_ReturnTrue()
        {
            var table = new Table(new string[] { "test" });
            table.AddRow("test");
            var variable = new Variable() { Type = typeof(string), Value = null };
            variableController.Variables.TryAdd("test", variable);
            var res = table.ToDictionary(variableController);
            res["test"].Should().Be("test");
            res.GetType().Name.Should().Be("Dictionary`2");
        }

        [Fact]
        public void CheckTryParse_StringToInt_ReturnException()
        {
            var enumerable = new List<object>() {"test"};

            Action act = () => ((IEnumerable<object>)enumerable).TryParse<int>();
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void CheckTryParse_LongToInt_ReturnException()
        {
            var enumerable = new List<object>() { 999999999999999 };

            Action act = () => ((IEnumerable<object>)enumerable).TryParse<int>();
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void CheckTryParse_TypeInt_ReturnTrue()
        {
            var enumerable = new List<object>() {5};
            var res = enumerable.TryParse<int>();
            res.GetType().Name.Should().Be("List`1");
            ((List<int>)res)[0].Should().Be(5);
        }

        [Fact]
        public void CheckTryParse_TypeObject_ReturnTrue()
        {
            var enumerable = new List<object>() { 5 };
            var res = enumerable.TryParse<object>();
            res.GetType().Name.Should().Be("List`1");
            ((List<object>)res)[0].Should().Be(5);
        }

        [Fact]
        public void CheckTryParse_TypeDouble_ReturnTrue()
        {
            var enumerable = new List<object>() { 5.5 };
            var res = enumerable.TryParse<double>();
            res.GetType().Name.Should().Be("List`1");
            ((List<double>)res)[0].Should().Be(5.5);
        }

        [Fact]
        public void CheckTryParse_TypeBoolean_ReturnTrue()
        {
            var enumerable = new List<object>() { "True" };
            var res = enumerable.TryParse<bool>();
            res.GetType().Name.Should().Be("List`1");
            ((List<bool>)res)[0].Should().Be(true);
        }

        [Fact]
        public void CheckTryParse_TypeLong_ReturnTrue()
        {
            var enumerable = new List<object>() { 9999999999999L };
            var res = enumerable.TryParse<long>();
            res.GetType().Name.Should().Be("List`1");
            ((List<long>)res)[0].Should().Be(9999999999999L);
        }

        [Fact]
        public void CheckTryParse_TypeFloat_ReturnTrue()
        {
            var enumerable = new List<object>() { 16.5f };
            var res = enumerable.TryParse<float>();
            res.GetType().Name.Should().Be("List`1");
            ((List<float>)res)[0].Should().Be(16.5f);
        }

        [Fact]
        public void CheckTryParse_TypeString_ReturnTrue()
        {
            var enumerable = new List<object>() { "test" };
            var res = enumerable.TryParse<string>();
            res.GetType().Name.Should().Be("List`1");
            ((List<string>)res)[0].Should().Be("test");
        }
    }
}

using FluentAssertions;
using Molder.Controllers;
using Molder.Generator.Extensions;
using Molder.Generator.Exceptions;
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
        public void toEnumerable_ExtraRow_ReturnException()
        {
            var table = new Table(new string[] {"test"});
            table.AddRow("test");
            Action act = () => table.ToEnumerable(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected table.Rows.ToList().Count to be 0 because Table must have only 1 row: Values, but found 1.");
        }

        [Theory]
        [InlineData(5)]
        [InlineData(true)]
        [InlineData("test")]
        [InlineData(999999999999L)]
        [InlineData(3.45)]
        public void ToEnumerable_Object_ReturnTrue(object value)
        {
            var table = new Table(new string[] {value.ToString()});
            var res = table.ToEnumerable(variableController);
            (res is IEnumerable<object>).Should().BeTrue();
        }

        [Fact]
        public void toDoctionary_NoValues_ReturnException()
        {
            var table = new Table(new string[] {"test"});
            Action act = () => table.ToDictionary(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected table.Rows.ToList().Count to be 1 because Table must have only 2 rows: Keys and Values, but found 0.");
        }

        [Fact]
        public void toDoctionary_TooManyRows_ReturnException()
        {
            var table = new Table(new string[] { "test", "qwerty" });
            table.AddRow("test", "qwerty");
            table.AddRow("test", "qwerty");
            Action act = () => table.ToDictionary(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected table.Rows.ToList().Count to be 1 because Table must have only 2 rows: Keys and Values, but found 2.");
        }

        [Theory]
        [InlineData("int","5","double","3.54")]
        [InlineData("qwert", "asd", "zxcv", "poiu")]
        public void toDoctionary_Values_ReturnTrue(string key1, string value1, string key2, string value2)
        {
            var table = new Table(new string[] { key1, value1 });
            table.AddRow(key2,value2);
            var res = table.ToDictionary(variableController);
            (res is Dictionary<string, object>).Should().BeTrue();
        }

        [Fact]
        public void TryParse_StringToInt_ReturnException()
        {
            var enumerable = new List<object>() { "test" };
            Action act = () => ((IEnumerable<object>)enumerable).TryParse<int>();
            act.Should().Throw<NotValideCastException>();
        }

        [Fact]
        public void CheckTryParse_LongToInt_ReturnException()
        {
            var enumerable = new List<object>() { 999999999999999L };
            Action act = () => ((IEnumerable<object>)enumerable).TryParse<int>();
            act.Should().Throw<NotValideCastException>();
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { TypeCode.Int32, 5, 6},
            new object[] { TypeCode.Object, 5, 6},
            new object[] { TypeCode.Double, 5.64, 6.24},
            new object[] { TypeCode.Single, 5.5f, 6.6f},
            new object[] { TypeCode.Boolean, true, false},
            new object[] { TypeCode.Int64, 999999999999L, 6L},
            new object[] { TypeCode.String, "test", "qwerty"}
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void CheckTryParse_Values_ReturnTrue(TypeCode type, object value1, object value2)
        {
            switch (type)
            {
                case (TypeCode.Int32):
                    var resInt = new List<object>(){ value1, value2 }.TryParse<int>();
                    (resInt is IEnumerable<int>).Should().BeTrue();
                    break;
                case (TypeCode.Object):
                    var resObj = new List<object>() { value1, value2 }.TryParse<object>();
                    (resObj is IEnumerable<object>).Should().BeTrue();
                    break;
                case (TypeCode.Double):
                    var resDouble = new List<object>() { value1, value2 }.TryParse<double>();
                    (resDouble is IEnumerable<double>).Should().BeTrue();
                    break;
                case (TypeCode.Single):
                    var resFloat = new List<object>() { value1, value2 }.TryParse<float>();
                    (resFloat is IEnumerable<float>).Should().BeTrue();
                    break;
                case (TypeCode.Boolean):
                    var resBool = new List<object>() { value1, value2 }.TryParse<bool>();
                    (resBool is IEnumerable<bool>).Should().BeTrue();
                    break;
                case (TypeCode.Int64):
                    var resLong = new List<object>() { value1, value2 }.TryParse<long>();
                    (resLong is IEnumerable<long>).Should().BeTrue();
                    break;
                case (TypeCode.String):
                    var resString = new List<object>() { value1, value2 }.TryParse<string>();
                    (resString is IEnumerable<string>).Should().BeTrue();
                    break;
            }
        }
    }
}

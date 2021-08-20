using FluentAssertions;
using Molder.Controllers;
using Molder.Generator.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class CollectionExtensionTests
    {
        private readonly VariableController variableController;

        public CollectionExtensionTests()
        {
            variableController = new VariableController();
        }

        [Fact]
        public void IsEnumerable_NoColl_ReturnException()
        {
            Action act = () => "test".IsEnumerable(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected collection not to be <null> because Значения в переменной \"test\" нет.");
        }

        [Fact]
        public void IsEnumerable_NotColl_ReturnException()
        {
            const int variable = 5;
            variableController.SetVariable("test", variable.GetType(), variable);
            Action act = () => "test".IsEnumerable(variableController);
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"test\" не является коллекцией"));
        }

        public static IEnumerable<object[]> DataForEnumerable =>
        new List<object[]>
        {
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 } },
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 } },
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 } },
            new object[] { new List<object> { 0, 1, 2, 3, 4, 5, 6 } },
            new object[] { new List<object> { 0.0, 1.1, 2.2, 3.3, 4.4, 5.5, 6.6 } },
            new object[] { new List<object> { "0", "1", "2", "3", "4", "5", "6" } },
            new object[] { new List<object> { true, false } }
        };

        [Theory]
        [MemberData(nameof(DataForEnumerable))]
        public void IsEnumerable_Enumerable_ReturnTrue(List<object> collection)
        {
            variableController.SetVariable("test", collection.GetType(), collection);
            "test".IsEnumerable(variableController);
        }

        [Fact]
        public void IsDictionary_NoColl_ReturnException()
        {
            Action act = () => "test".IsDictionary(variableController);
            act.Should().Throw<Exception>()
                .WithMessage("Expected dictionary not to be <null> because Значения в переменной \"test\" нет.");
        }

        [Fact]
        public void IsDictionary_NotColl_ReturnException()
        {
            const int variable = 5;
            variableController.SetVariable("test", variable.GetType(), variable);
            Action act = () => "test".IsDictionary(variableController);
            act.Should().Throw<Exception>()
                .Where(e => e.Message.Contains("\"test\" не является словарем"));
        }

        public static IEnumerable<object[]> DataForDictionary =>
        new List<object[]>
        {
            new object[] 
            { 
                new Dictionary<string, object> { 
                    { "qwerty",456},
                    { "asdf",123},
                    { "zxcv","qwe"}
                } 
            }
        };

        [Theory]
        [MemberData(nameof(DataForDictionary))]
        public void IsDictionary_Enumerable_ReturnTrue(Dictionary<string, object> dictionary)
        {
            variableController.SetVariable("test", dictionary.GetType(), dictionary);
            "test".IsDictionary(variableController);
        }
    }
}
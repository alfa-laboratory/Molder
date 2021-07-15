using FluentAssertions;
using Molder.Controllers;
using Molder.Generator.Extensions;
using Molder.Generator.Steps;
using Molder.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class EnumerableExtension
    {
        private VariableController variableController;
        public EnumerableExtension()
        {
            variableController = new VariableController();
        }


        [Fact]
        public void CheckGetRandomValueFromEnumerable_TypeInt_ReturnTrue()
        {
            var enumerable = new List<object>() { 5 };
            var res = enumerable.TryParse<int>();
            var resVar = res.GetRandomValueFromEnumerable<int>();
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(5);
        }

        [Fact]
        public void CheckGetRandomValueFromEnumerable_TypeObject_ReturnTrue()
        {
            var enumerable = new List<object>() { 5 };
            var res = enumerable.TryParse<object>();
            var resVar = res.GetRandomValueFromEnumerable<object>();
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(5);
        }

        [Fact]
        public void CheckGetRandomValueFromEnumerable_TypeDouble_ReturnTrue()
        {
            var enumerable = new List<object>() { 5.5 };
            var res = enumerable.TryParse<double>();
            var resVar = res.GetRandomValueFromEnumerable<double>();
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(5.5);
        }

        [Fact]
        public void CheckGetRandomValueFromEnumerable_TypeBoolean_ReturnTrue()
        {
            var enumerable = new List<object>() { "True" };
            var res = enumerable.TryParse<bool>();
            var resVar = res.GetRandomValueFromEnumerable<bool>();
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(true);
        }

        [Fact]
        public void CheckGetRandomValueFromEnumerable_TypeLong_ReturnTrue()
        {
            var enumerable = new List<object>() { 9999999999999L };
            var res = enumerable.TryParse<long>();
            var resVar = res.GetRandomValueFromEnumerable<long>();
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(9999999999999L);
        }

        [Fact]
        public void CheckGetRandomValueFromEnumerable_TypeFloat_ReturnTrue()
        {
            var enumerable = new List<object>() { 16.5f };
            var res = enumerable.TryParse<float>();
            var resVar = res.GetRandomValueFromEnumerable<float>();
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(16.5f);
        }

        [Fact]
        public void CheckGetRandomValueFromEnumerable_TypeString_ReturnTrue()
        {
            var enumerable = new List<object>() { "test" };
            var res = enumerable.TryParse<string>();
            var resVar = res.GetRandomValueFromEnumerable<string>();
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be("test");
        }

        [Fact]
        public void CheckGetValueFromEnumerable_TypeInt_ReturnTrue()
        {
            var enumerable = new List<object>() { 5 };
            var res = enumerable.TryParse<int>();
            var resVar = res.GetValueFromEnumerable<int>(0);
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(5);
        }

        [Fact]
        public void CheckGetValueFromEnumerable_TypeObject_ReturnTrue()
        {
            var enumerable = new List<object>() { 5 };
            var res = enumerable.TryParse<object>();
            var resVar = res.GetValueFromEnumerable<object>(0);
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(5);
        }

        [Fact]
        public void CheckGetValueFromEnumerable_TypeDouble_ReturnTrue()
        {
            var enumerable = new List<object>() { 5.5 };
            var res = enumerable.TryParse<double>();
            var resVar = res.GetValueFromEnumerable<double>(0);
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(5.5);
        }

        [Fact]
        public void CheckGetValueFromEnumerable_TypeBoolean_ReturnTrue()
        {
            var enumerable = new List<object>() { "True" };
            var res = enumerable.TryParse<bool>();
            var resVar = res.GetValueFromEnumerable<bool>(0);
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(true);
        }

        [Fact]
        public void CheckGetValueFromEnumerable_TypeLong_ReturnTrue()
        {
            var enumerable = new List<object>() { 9999999999999L };
            var res = enumerable.TryParse<long>();
            var resVar = res.GetValueFromEnumerable<long>(0);
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(9999999999999L);
        }

        [Fact]
        public void CheckGetValueFromEnumerable_TypeFloat_ReturnTrue()
        {
            var enumerable = new List<object>() { 16.5f };
            var res = enumerable.TryParse<float>();
            var resVar = res.GetValueFromEnumerable<float>(0);
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be(16.5f);
        }

        [Fact]
        public void CheckGetValueFromEnumerable_TypeString_ReturnTrue()
        {
            var enumerable = new List<object>() { "test" };
            var res = enumerable.TryParse<string>();
            var resVar = res.GetValueFromEnumerable<string>(0);
            res.GetType().Name.Should().Be("List`1");
            resVar.Should().Be("test");
        }

        [Fact]
        public void CheckGetRandomValueFromDictionary_ReturnTrue()
        {
            var dictionary = new Dictionary<string,object>();
            dictionary.Add("test", 5);
            var resVar = dictionary.GetRandomValueFromDictionary();
            resVar.Should().Be(5);
        }

        [Fact]
        public void CheckGetValueFromDictionary_ReturnTrue()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("test", 5);
            var resVar = dictionary.GetValueFromDictionary("test");
            resVar.Should().Be(5);
        }
    }
}

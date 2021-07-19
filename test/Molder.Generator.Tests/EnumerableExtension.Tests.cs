using FluentAssertions;
using Molder.Generator.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Molder.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class EnumerableExtension
    {
        public EnumerableExtension() { }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { "test", 2, 3.45,999999999999999L, 5.5f, true }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void GetRandomValueFromEnumerable_ReturnTrue(string value1, int value2, double value3, long value4, float value5, bool value6)
        {
            var collection = new List<object>() { value1, value2, value3, value4, value5, value6 };
            var resVar = collection.GetRandomValueFromEnumerable();
            collection.Should().Contain(resVar);
        }

        public static IEnumerable<object[]> DataForDictionary =>
        new List<object[]>
        {
            new object[] { "test", "qwerty", "asd", 456, "wer", true }
        };

        [Theory]
        [MemberData(nameof(DataForDictionary))]
        public void GetRandomValueFromDictionary_ReturnTrue(string key1, object value1, string key2, object value2, string key3, object value3)
        {
            var dictionary = new Dictionary<string,object>();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);
            dictionary.Add(key3, value3);
            var resVar = dictionary.GetRandomValueFromDictionary();
            dictionary.Values.Should().Contain(resVar);
        }

        [Theory]
        [MemberData(nameof(DataForDictionary))]
        public void GetValueFromDictionary_ReturnTrue(string key1, object value1, string key2, object value2, string key3, object value3)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);
            dictionary.Add(key3, value3);
            var resVar = dictionary.GetValueFromDictionary(key2);
            resVar.Should().Be(value2);
        }
    }
}

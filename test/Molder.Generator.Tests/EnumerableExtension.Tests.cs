using FluentAssertions;
using Molder.Generator.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            new object[] { new List<object> { "test", 2, 3.45, 999999999999999L, 5.5f, true } }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void GetRandomValueFromEnumerable_ReturnTrue(List<object> collection)
        {
            var resVar = collection.GetRandomValueFromEnumerable();
            collection.Should().Contain(resVar);
        }

        public static IEnumerable<object[]> DataForDictionary =>
            new List<object[]>
            {
                new object[] { new Dictionary<string,object> 
                {
                    { "test", "qwerty"}, 
                    { "asd", 456 }, 
                    { "wer", true } 
                } 
            } 
        };

        [Theory]
        [MemberData(nameof(DataForDictionary))]
        public void GetRandomValueFromDictionary_ReturnTrue(Dictionary<string, object> dictionary)
        {
            var resVar = dictionary.GetRandomValueFromDictionary();
            dictionary.Values.Should().Contain(resVar);
        }

        [Theory]
        [MemberData(nameof(DataForDictionary))]
        public void GetValueFromDictionary_ReturnTrue(Dictionary<string, object> dictionary)
        {   var key = (string)(Enumerable.ToList(dictionary.Keys)[1]);
            var resVar = dictionary.GetValueFromDictionary(key);
            resVar.Should().Be(dictionary[key]);
        }
    }
}

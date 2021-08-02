using FluentAssertions;
using Molder.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Molder.Generator.Extensions
{
    public static class CollectionExtension
    {
        public static void IsEnumerable(this string collectionNameIndex, VariableController variableController) {
            var collectionName = collectionNameIndex.Split("[").First();
            var collection = variableController.GetVariableValue(collectionName);
            collection.Should().NotBeNull($"Значения в переменной \"{collectionName}\" нет");
            (collection is IEnumerable).Should().BeTrue($"\"{collectionName}\" не является коллекцией");
        }

        public static void IsDictionary(this string dictionaryNameKey, VariableController variableController)
        {
            var dictionaryName = dictionaryNameKey.Split("[").First();
            var dictionary = variableController.GetVariableValue(dictionaryName);
            dictionary.Should().NotBeNull($"Значения в переменной \"{dictionaryName}\" нет");
            (dictionary is Dictionary<string, object>).Should().BeTrue($"\"{dictionaryName}\" не является словарем");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molder.Generator.Extensions
{
    public static class EnumerableExtension
    {
        public static object GetRandomValueFromEnumerable<T>(this IEnumerable<T> enumerable) 
        {
            var rand = new Random();
            var param = rand.Next() % ((List<T>)enumerable).Count;
            return ((List<T>)enumerable)[param];
        }
        public static object GetValueFromEnumerable<T>(this IEnumerable<T> enumerable, int position)
        {
            return ((List<T>)enumerable)[position];
        }

        public static object GetRandomValueFromDictionary(this Dictionary<string, object> dictionary)
        {
            var rand = new Random();
            var param = rand.Next() % Enumerable.ToList(dictionary.Values).Count;
            return Enumerable.ToList(dictionary.Values)[param];
        }
        public static object GetValueFromDictionary(this Dictionary<string,object> dictionary, string position)
        {
            return dictionary[position];
        }
    }
}

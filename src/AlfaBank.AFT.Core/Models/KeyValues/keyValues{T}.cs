using System.Collections.Generic;

namespace AlfaBank.AFT.Core.Model.KeyValues
{
    public class KeyValues<T> : Dictionary<string, T>
    {
        public KeyValues(Dictionary<string, T> dictionary)
        {
            foreach(var keyValuePair in dictionary)
            {
                Add(key: keyValuePair.Key, value: keyValuePair.Value);
            }
        }

        public KeyValues()
        {
        }

        public new T this[string key]
        {
            get => ContainsKey(key) ? base[key] : default(T);
            set => Add(key, value);
        }

        public new void Add(string key, T value)
        {
            if(ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                base.Add(key, value);
            }
        }

        public new bool ContainsKey(string key)
        {
            return !string.IsNullOrWhiteSpace(key) && base.ContainsKey(key);
        }

        public new bool Remove(string key)
        {
            return ContainsKey(key) && base.Remove(key);
        }
    }
}

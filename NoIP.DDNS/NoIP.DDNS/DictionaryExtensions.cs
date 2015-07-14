using System.Collections.Generic;

namespace NoIP.DDNS
{
    public static class DictionaryExtensions
    {
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> value, IDictionary<TKey, TValue> merge)
        {
            foreach (var item in merge)
            {
                value[item.Key] = item.Value;
            }
        }
    }
}

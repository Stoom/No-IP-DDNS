using System.Collections.Generic;
using System.Linq;

namespace NoIP.DDNS
{
    internal static class DictionaryExtensions
    {
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> value, IDictionary<TKey, TValue> merge)
        {
            foreach (var item in merge)
            {
                value[item.Key] = item.Value;
            }
        }

        public static HashSet<TValue> ToHashSet<TValue>(this ICollection<TValue> value)
        {
            return new HashSet<TValue>(value.Distinct());
        }
    }
}

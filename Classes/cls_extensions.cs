using System;
using System.Collections.Generic;
using System.Linq;

namespace adv_of_code_2019.Classes
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
        {
            if (values.Count() == 1)
                return new[] { values };
            return values.SelectMany(v => Permutations(values.Where(x => x.Equals(v) == false)), (v, p) => p.Prepend(v));
        }

        public static Int32 Abs(this Int32 num)
        {
            return Math.Abs(num);
        }

        public static void ReplaceKey<T, U>(this Dictionary<T, U> source, T key, T newKey)
        {
            if (!source.TryGetValue(key, out var value))
                throw new ArgumentException("Key does not exist", nameof(key));
            source.Remove(key);
            source.Add(newKey, value);
        }

        public static void ReplaceValue<T, U>(this Dictionary<T, U> source, T key, U newVal)
        {
            source.Remove(key);
            source.Add(key, newVal);
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueGenerator)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = valueGenerator(key);
                dictionary.Add(key, value);
            }

            return value;
        }
    }
}
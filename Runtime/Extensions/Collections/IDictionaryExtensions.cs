using System;
using System.Collections.Generic;

namespace Ostium11.Extensions
{
    public static class IDictionaryExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            if (!dict.TryGetValue(key, out TValue ret))
                dict[key] = ret = new TValue();
            return ret;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            if (!dict.TryGetValue(key, out TValue ret))
                dict[key] = ret = defaultValue;
            return ret;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> create)
        {
            if (!dict.TryGetValue(key, out TValue ret))
                dict[key] = ret = create();
            return ret;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> create)
        {
            if (!dict.TryGetValue(key, out TValue ret))
                dict[key] = ret = create(key);
            return ret;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.TryGetValue(key, out TValue ret))
                return ret;
            return default;
        }
    }
}
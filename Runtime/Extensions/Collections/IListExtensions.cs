using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

namespace Ostium11.Extensions
{
    public static class IListExtensions
    {
        public static T GetRandom<T>(this IList<T> list) => list[Random.Range(0, list.Count)];

        public static T Last<T>(this IList<T> list) => list[list.Count - 1];

        public static void ForEachReversed<T>(this IList<T> list, Action<T> action)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                action(list[i]);
        }

        public static IEnumerable<T> FastReverse<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                yield return list[i];
        }
    }
}
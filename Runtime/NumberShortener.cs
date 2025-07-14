using UnityEngine;

namespace Ostium11
{
    public static class NumberShortener
    {
        public static string ToShortString(float value, string format = "0.#")
        {
            var (order, postfix) = Mathf.Abs(value) switch
            {
                >= 1_000_000_000 => (1_000_000_000, "B"),
                >= 1_000_000 => (1_000_000, "M"),
                >= 1_000 => (1_000, "K"),
                _ => (1, ""),
            };
            return (value / order).ToString(format) + postfix;
        }
    }
}

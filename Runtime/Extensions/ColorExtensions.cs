using UnityEngine;

namespace Ostium11.Extensions
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color c, float alpha)
        {
            c.a = alpha;
            return c;
        }
    }
}
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class Color32Extensions
    {
        public static bool IsEqualTo(this Color32 c1, Color32 c2)
        {
            return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b && c1.a == c2.a;
        }

        public static int ToInt(this Color32 c) => Bytes.ToInt(c);

        public static void FromInt(this ref Color32 c, int value) => c = Bytes.ToColor32(value);
    }
}
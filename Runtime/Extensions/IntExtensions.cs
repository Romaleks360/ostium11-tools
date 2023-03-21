using System.Runtime.CompilerServices;

namespace Ostium11.Extensions
{
    public static class IntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this int val, int minInclusive, int maxExclusive)
        {
            return val >= minInclusive && val < maxExclusive;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this int val, int maxExclusive)
        {
            return val >= 0 && val < maxExclusive;
        }
    }
}
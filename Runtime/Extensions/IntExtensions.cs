using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;

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

#if OSTIUM11_UNITASK_SUPPORT
        public static UniTask.Awaiter GetAwaiter(this int milliseconds) => UniTask.Delay(milliseconds).GetAwaiter();
#endif
    }
}
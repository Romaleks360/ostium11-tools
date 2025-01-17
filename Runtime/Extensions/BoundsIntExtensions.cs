using UnityEngine;

namespace Ostium11.Extensions
{
    public static class BoundsIntExtensions
    {
        public static Bounds ToBounds(this BoundsInt @this)
            => new(@this.center, @this.size);

        public static bool FullyContains(this BoundsInt @this, BoundsInt target)
            => @this.Contains(target.max) && @this.Contains(target.min);
    }
}
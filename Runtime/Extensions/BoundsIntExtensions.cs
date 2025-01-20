using UnityEngine;

namespace Ostium11.Extensions
{
    public static class BoundsIntExtensions
    {
        public static Bounds ToBounds(this BoundsInt @this)
            => new(@this.center, @this.size);

        public static bool FullyContains(this BoundsInt @this, BoundsInt target)
            => @this.IncludeContains(target.max) && @this.IncludeContains(target.min);

        public static bool IncludeContains(this BoundsInt @this, Vector3Int position)
            => position.x >= @this.xMin && position.y >= @this.yMin && position.z >= @this.zMin && position.x <= @this.xMax && position.y <= @this.yMax && position.z <= @this.zMax; 
    }
}
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class BoundsIntExtensions
    {
        public static Bounds ToBounds(this BoundsInt boundsInt)
            => new(boundsInt.center, boundsInt.size);

        public static bool FullyContains(this BoundsInt boundsInt, BoundsInt target)
            => boundsInt.IncludeContains(target.max) && boundsInt.IncludeContains(target.min);

        public static bool IncludeContains(this BoundsInt boundsInt, Vector3Int position)
            => position.x >= boundsInt.xMin && position.y >= boundsInt.yMin && position.z >= boundsInt.zMin && position.x <= boundsInt.xMax && position.y <= boundsInt.yMax && position.z <= boundsInt.zMax; 
    }
}
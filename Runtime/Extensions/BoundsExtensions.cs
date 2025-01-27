using UnityEngine;

namespace Ostium11.Extensions
{
    public static class BoundsExtensions
    {
        public static BoundsInt ToBoundsInt(this Bounds bounds)
            => new(bounds.min.RoundToVector3Int(), bounds.size.RoundToVector3Int());

        public static Bounds Floor(this Bounds bounds)
        {
            bounds.SetMinMax(bounds.min.Floor(), bounds.max.Floor());
            return bounds;
        }

        public static Bounds Ceil(this Bounds bounds)
        {
            bounds.SetMinMax(bounds.min.Ceil(), bounds.max.Ceil());
            return bounds;
        }

        public static Bounds Snap(this Bounds bounds, float snap)
        {
            bounds.SetMinMax(bounds.min.Snap(snap), bounds.max.Snap(snap));
            return bounds;
        }

        public static bool FullyContains(this Bounds bounds, Bounds target)
            => bounds.Contains(target.max) && bounds.Contains(target.min);
    }
}
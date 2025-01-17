using UnityEngine;

namespace Ostium11.Extensions
{
    public static class BoundsExtensions
    {
        public static BoundsInt ToBoundsInt(this Bounds @this)
            => new(@this.min.RoundToVector3Int(), @this.size.RoundToVector3Int());

        public static Bounds Floor(this Bounds @this)
        {
            @this.SetMinMax(@this.min.Floor(), @this.max.Floor());
            return @this;
        }

        public static Bounds Ceil(this Bounds @this)
        {
            @this.SetMinMax(@this.min.Ceil(), @this.max.Ceil());
            return @this;
        }

        public static bool FullyContains(this Bounds @this, Bounds target)
            => @this.Contains(target.max) && @this.Contains(target.min);
    }
}
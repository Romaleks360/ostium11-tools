using UnityEngine;

namespace Ostium11.Extensions
{
    public static class RectExtensions
    {
        public static Vector3 ClampPosition(this Rect rect, Vector3 pos)
        {
            pos.x = Mathf.Clamp(pos.x, rect.xMin, rect.xMax);
            pos.y = Mathf.Clamp(pos.y, rect.yMin, rect.yMax);
            return pos;
        }

    }
}
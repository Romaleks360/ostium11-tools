using UnityEngine;

namespace Ostium11.Extensions
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Set pivot without changing the position of the element
        /// </summary>
        public static void SetPivot(this RectTransform target, Vector2 pivot)
        {
            var offset = pivot - target.pivot;
            offset.Scale(target.rect.size);
            var pos = target.position + target.TransformVector(offset);

            target.pivot = pivot;
            target.position = pos;
        }

        public static void SetAnchors(this RectTransform rectTransform, Vector2 anchorPoint)
        {
            var pos = rectTransform.localPosition;
            rectTransform.anchorMin = anchorPoint;
            rectTransform.anchorMax = anchorPoint;
            rectTransform.localPosition = pos;
        }
    }
}
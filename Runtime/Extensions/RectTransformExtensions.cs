using UnityEngine;

namespace Ostium11.Extensions
{
    public static class RectTransformExtensions
    {
        static readonly Vector3[] _corners = new Vector3[4];

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

        public static void SetPivot(this RectTransform rectTransform, float pivotX, float pivotY) =>
            SetPivot(rectTransform, new Vector2(pivotX, pivotY));

        public static void SetAnchors(this RectTransform rectTransform, Vector2 anchorPoint)
        {
            var pos = rectTransform.localPosition;
            rectTransform.anchorMin = anchorPoint;
            rectTransform.anchorMax = anchorPoint;
            rectTransform.localPosition = pos;
        }

        public static void SetAnchors(this RectTransform rectTransform, float anchorX, float anchorY) =>
            SetAnchors(rectTransform, new Vector2(anchorX, anchorY));

        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_corners);
            return new Rect(_corners[0], _corners[2] - _corners[0]);
        }

        public static void SnapAnchorsToRect(this RectTransform rt)
        {
            var parent = (RectTransform)rt.parent;
            rt.anchorMin = PointToNormalizedUnclamped(parent.rect, (Vector2)rt.localPosition + rt.rect.min);
            rt.anchorMax = PointToNormalizedUnclamped(parent.rect, (Vector2)rt.localPosition + rt.rect.max);
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;

            // like Rect.PointToNormalzed but can go beyond 0-1
            static Vector2 PointToNormalizedUnclamped(Rect rectangle, Vector2 point)
            {
                return new Vector2(
                    InverseLerpUnclamped(rectangle.x, rectangle.xMax, point.x),
                    InverseLerpUnclamped(rectangle.y, rectangle.yMax, point.y));
            }

            // like Mathf.InverseLerp but can go beyond 0-1
            static float InverseLerpUnclamped(float a, float b, float value)
            {
                if (a != b)
                    return (value - a) / (b - a);
                return 0;
            }
        }
    }
}
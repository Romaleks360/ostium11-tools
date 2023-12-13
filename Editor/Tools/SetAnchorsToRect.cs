using UnityEditor;
using UnityEngine;

namespace Ostium11.Editors
{
    public static class SetAnchorsToRect
    {
        [MenuItem("Tools/Set RectTransform Anchors To Its Rect")]
        static void Run()
        {
            foreach (var tr in Selection.transforms)
            {
                if (tr is RectTransform rt && tr.parent is RectTransform parent)
                {
                    Undo.RecordObject(rt, "Set Anchors");
                    var pos = rt.position;
                    rt.anchorMin = Rect.PointToNormalized(parent.rect, (Vector2)rt.localPosition + rt.rect.min);
                    rt.anchorMax = Rect.PointToNormalized(parent.rect, (Vector2)rt.localPosition + rt.rect.max);
                    rt.sizeDelta = Vector2.zero;
                    rt.position = pos;
                }
            }
        }
    }
}
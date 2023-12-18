using Ostium11.Extensions;
using UnityEditor;
using UnityEngine;

namespace Ostium11.Editors
{
    public static class SnapAnchorsToRect
    {
        [MenuItem("Tools/Snap RectTransform Anchors To Its Rect")]
        static void Snap()
        {
            foreach (var tr in Selection.transforms)
            {
                if (tr is RectTransform rt && tr.parent is RectTransform parent)
                {
                    Undo.RecordObject(rt, "Snap Anchors");
                    rt.SnapAnchorsToRect();
                }
            }

        }
    }
}
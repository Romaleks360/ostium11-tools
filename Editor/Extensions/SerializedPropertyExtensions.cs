using Ostium11.Extensions;
using UnityEditor;
using UnityEngine;

namespace Ostium11.Editors
{
    public static class SerializedPropertyExtensions
    {
        public static bool IsInArray(this SerializedProperty property) => property.propertyPath[^1] == ']';

        public static int GetIndexInArray(this SerializedProperty property)
        {
            if (!property.IsInArray())
                throw new System.Exception("Invalid operation: SerializedProperty is not a member of an array!");
            int length = 0;
            int i = property.propertyPath.Length - 2;
            while (property.propertyPath[i].IsTrueDigit())
            {
                length++;
                i--;
            }
            return int.Parse(property.propertyPath[^(length + 1)..^1]);
        }

        public static void HighlightUnevenElement(this SerializedProperty property, Rect position)
        {
            if (property.IsInArray() && (property.propertyPath[^2] - '0') % 2 == 1)
                property.FillRect(position, new Color(0, 0, 0, .1f));
        }

        public static void FillRect(this SerializedProperty _, Rect position, Color color)
        {
            position.min -= new Vector2(30, 2);
            position.max += new Vector2(5, -2);
            EditorGUI.DrawRect(position, color);
        }
    }
}
using System.Linq;
using System.Text.RegularExpressions;
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
            while (property.propertyPath[i].IsDigit())
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

        public static SerializedProperty FindParentProperty(this SerializedProperty serializedProperty)
        {
            var propertyPaths = serializedProperty.propertyPath.Split('.');
            if (propertyPaths.Length <= 1)
            {
                return default;
            }

            var parentSerializedProperty = serializedProperty.serializedObject.FindProperty(propertyPaths.First());
            for (var index = 1; index < propertyPaths.Length - 1; index++)
            {
                if (propertyPaths[index] == "Array")
                {
                    if (index + 1 == propertyPaths.Length - 1)
                        break;
                    if (propertyPaths.Length > index + 1 && Regex.IsMatch(propertyPaths[index + 1], "^data\\[\\d+\\]$"))
                    {
                        var match = Regex.Match(propertyPaths[index + 1], "^data\\[(\\d+)\\]$");
                        var arrayIndex = int.Parse(match.Groups[1].Value);
                        parentSerializedProperty = parentSerializedProperty.GetArrayElementAtIndex(arrayIndex);
                        index++;
                    }
                }
                else
                {
                    parentSerializedProperty = parentSerializedProperty.FindPropertyRelative(propertyPaths[index]);
                }
            }
            return parentSerializedProperty;
        }

        public static object GetEnumValue(this SerializedProperty property) => property.boxedValue;

        public static bool CompareEnumValue(this SerializedProperty property, object value) => property.numericType switch
        {
            SerializedPropertyNumericType.Int8 => property.intValue == (sbyte)value,
            SerializedPropertyNumericType.UInt8 => property.intValue == (byte)value,
            SerializedPropertyNumericType.Int16 => property.intValue == (short)value,
            SerializedPropertyNumericType.UInt16 => property.intValue == (ushort)value,
            SerializedPropertyNumericType.Int32 => property.intValue == (int)value,
            SerializedPropertyNumericType.UInt32 => property.uintValue == (uint)value,
            SerializedPropertyNumericType.Int64 => property.longValue == (long)value,
            SerializedPropertyNumericType.UInt64 => property.ulongValue == (ulong)value,
            _ => throw new System.ArgumentException("Unknown numeric type!")
        };

        public static void SetEnumValue(this SerializedProperty property, object value)
        {
            switch (property.numericType)
            {
                case SerializedPropertyNumericType.Int8: property.intValue = (sbyte)value; break;
                case SerializedPropertyNumericType.UInt8: property.intValue = (byte)value; break;
                case SerializedPropertyNumericType.Int16: property.intValue = (short)value; break;
                case SerializedPropertyNumericType.UInt16: property.intValue = (ushort)value; break;
                case SerializedPropertyNumericType.Int32: property.intValue = (int)value; break;
                case SerializedPropertyNumericType.UInt32: property.uintValue = (uint)value; break;
                case SerializedPropertyNumericType.Int64: property.longValue = (long)value; break;
                case SerializedPropertyNumericType.UInt64: property.ulongValue = (ulong)value; break;
                default: throw new System.ArgumentException("Unknown numeric type!");
            }
        }
    }
}
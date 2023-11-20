using System;
using UnityEditor;
using UnityEngine;

namespace Ostium11.Editors
{
    [CustomPropertyDrawer(typeof(Interface<>))]
    public class InterfaceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Type targetType = fieldInfo.FieldType;
            if (targetType.IsArray)
                targetType = fieldInfo.FieldType.GetElementType();

            targetType = targetType.GenericTypeArguments[0];
            while (targetType.IsGenericType)
                targetType = targetType.GenericTypeArguments[0];

            var container = property.FindPropertyRelative("_container");
            container.objectReferenceValue =
                EditorGUI.ObjectField(position, label, container.objectReferenceValue, targetType, true);

            EditorGUI.EndProperty();
        }
    }
}

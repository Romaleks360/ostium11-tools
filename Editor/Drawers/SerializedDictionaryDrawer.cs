using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ostium11.Editors
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
    public class SerializedDictionaryDrawer : PropertyDrawer
    {
        const float verticalPadding = 5f;
        const float horizontalSpacing = 3f;
        const float foldoutHeaderHeight = 20f;
        const float arraySizeWidth = 48f;

        readonly Dictionary<string, ReorderableList> _reorderableLists = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            float height = base.GetPropertyHeight(property, label);
            if (property.isExpanded)
                height += _reorderableLists[property.propertyPath].GetHeight();
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            Init(prop);

            var foldoutRect = EditorGUI.IndentedRect(new Rect(position) { height = foldoutHeaderHeight });
            prop.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, prop.isExpanded, prop.displayName);
            EditorGUI.EndFoldoutHeaderGroup();

            var arraySizeRect = new Rect()
            {
                x = foldoutRect.x + foldoutRect.width - arraySizeWidth,
                y = foldoutRect.y,
                width = arraySizeWidth,
                height = EditorGUIUtility.singleLineHeight
            };
            var pairs = prop.FindPropertyRelative("_pairs");
            GUI.enabled = false;
            EditorGUI.IntField(arraySizeRect, pairs.arraySize);
            GUI.enabled = true;

            position.y += foldoutHeaderHeight;
            if (prop.isExpanded)
                _reorderableLists[prop.propertyPath].DoList(position);
        }

        void Init(SerializedProperty prop)
        {
            if (_reorderableLists.ContainsKey(prop.propertyPath))
                return;

            var unconstructedType = typeof(SerializedDictionary<,>);
            System.Type keysType = null;

            Queue<System.Type> queueOfTypes = new Queue<System.Type>();
            foreach (var t in fieldInfo.FieldType.GetGenericArguments())
                queueOfTypes.Enqueue(t);

            while (queueOfTypes.Count > 0)
            {
                keysType = queueOfTypes.Dequeue();

                if (keysType.IsGenericType)
                {
                    if (unconstructedType == keysType.GetGenericTypeDefinition())
                    {
                        keysType = keysType.GetGenericArguments()[0];
                        break;
                    }
                    else
                        foreach (var t in keysType.GetGenericArguments())
                            queueOfTypes.Enqueue(t);
                }
                else
                    break;
            }

            bool hasDontInitEnum = fieldInfo.GetCustomAttribute<DontInitEnumDictAttribute>() == null;
            bool isEnum = keysType.IsEnum && hasDontInitEnum;

            ReorderableList reorderableList = null;
            reorderableList = new ReorderableList(prop.serializedObject, prop.FindPropertyRelative("_pairs"),
                draggable: !isEnum,
                displayHeader: true,
                displayAddButton: !isEnum,
                displayRemoveButton: !isEnum)
            {
                drawElementCallback = OnDrawElement,
                drawElementBackgroundCallback = DrawElementBackground,
                elementHeightCallback = OnElementHeight,
                drawHeaderCallback = OnDrawHeader
            };
            _reorderableLists.Add(prop.propertyPath, reorderableList);

            void OnDrawHeader(Rect rect)
            {
                var keysRect = new Rect(rect)
                {
                    x = rect.x + 15,
                    width = rect.width * .25f
                };

                var valuesRect = new Rect(keysRect)
                {
                    x = keysRect.x + keysRect.width + 20
                };

                EditorGUI.LabelField(keysRect, new GUIContent("Keys"));
                EditorGUI.LabelField(valuesRect, new GUIContent("Values"));
            }

            float OnElementHeight(int index)
            {
                if (reorderableList.count == 0)
                    return 0;

                var pair = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty key = pair.FindPropertyRelative(nameof(key));
                SerializedProperty value = pair.FindPropertyRelative(nameof(value));

                return Mathf.Max(EditorGUI.GetPropertyHeight(key), EditorGUI.GetPropertyHeight(value)) + EditorGUIUtility.standardVerticalSpacing * 2;
            }

            void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                if (reorderableList.count == 0)
                    return;

                var pair = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty key = pair.FindPropertyRelative(nameof(key));
                SerializedProperty value = pair.FindPropertyRelative(nameof(value));

                var keyRect = new Rect()
                {
                    y = rect.y + verticalPadding * .5f,
                    x = rect.x,
                    width = rect.width * .25f,
                    height = EditorGUI.GetPropertyHeight(key)
                };

                var arrowRect = new Rect(keyRect)
                {
                    y = keyRect.y - verticalPadding * .25f,
                    x = rect.x + horizontalSpacing + keyRect.width,
                    width = 35,
                };

                var valueRect = new Rect(keyRect)
                {
                    x = arrowRect.x + arrowRect.width,
                    width = rect.width - keyRect.width - arrowRect.width,
                    height = EditorGUI.GetPropertyHeight(value)
                };

                if (key.propertyType == SerializedPropertyType.Enum &&
                        fieldInfo.GetCustomAttribute<DontInitEnumDictAttribute>() == null)
                    EditorGUI.LabelField(keyRect, key.enumDisplayNames[key.enumValueIndex]);
                else
                    EditorGUI.PropertyField(keyRect, key, GUIContent.none, true);

                EditorGUI.LabelField(arrowRect, new GUIContent("⟹"));

                EditorGUIUtility.labelWidth /= 2;
                var name = value.FindPropertyRelative("name")?.stringValue;
                EditorGUI.PropertyField(valueRect, value, name == null ? GUIContent.none : new GUIContent(name), true);
                EditorGUIUtility.labelWidth = 0;
            }

            void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused)
            {
                if (Event.current.type != EventType.Repaint || index == -1)
                    return;

                var pairs = reorderableList.serializedProperty;
                var key = pairs.GetArrayElementAtIndex(index).FindPropertyRelative("key");

                for (int i = 0; i < index; i++)
                {
                    var otherKey = pairs.GetArrayElementAtIndex(i).FindPropertyRelative("key");
                    if (SerializedProperty.DataEquals(key, otherKey))
                    {
                        // same key, draw red background
                        EditorGUI.DrawRect(rect, isFocused ? Color.HSVToRGB(0, 1, 1) : Color.HSVToRGB(0, 1, 0.7f));
                        return;
                    }
                }

                // draw usual background
                GUIStyle s = "RL Element";
                s.Draw(rect, isHover: false, isActive, isActive, isFocused);
            }
        }
    }
}
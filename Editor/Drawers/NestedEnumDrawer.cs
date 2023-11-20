using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Ostium11.Editors
{
    [CustomPropertyDrawer(typeof(NestedEnumAttribute))]
    public class NestedEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            rect = EditorGUI.PrefixLabel(rect, label);
            if (EditorGUI.DropdownButton(rect, new GUIContent(prop.enumDisplayNames[prop.enumValueIndex]), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                string currentSubMenu = "";
                int i = 0;

                foreach (var field in fieldInfo.FieldType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var subMenuAttribute = field.GetCustomAttribute<NestedEnumSubMenuAttribute>();
                    if (subMenuAttribute != null)
                        currentSubMenu = subMenuAttribute.SubMenuPath;

                    var path = $"{currentSubMenu}/{prop.enumDisplayNames[i]}";

                    menu.AddItem(new GUIContent(path), prop.enumValueIndex == i, Select, (prop, i));
                    i++;
                }

                menu.DropDown(rect);
            }
        }

        static void Select(object obj)
        {
            var (prop, enumValue) = ((SerializedProperty prop, int enumValue))obj;
            prop.enumValueIndex = enumValue;
            prop.serializedObject.ApplyModifiedProperties();
        }
    }
}

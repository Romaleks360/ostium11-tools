using Ostium11.UI;
using UnityEditor;
using UnityEditor.UI;

namespace Ostium11.Editors
{
    [CustomEditor(typeof(DoubleClickButton))]
    public class DoubleClickButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onDoubleClick"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}

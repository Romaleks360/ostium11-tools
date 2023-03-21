using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ostium11.Editor
{
    public class Spread : EditorWindow
    {
        [MenuItem("Tools/Spread")]
        static void Run() => GetWindow<Spread>();

        Vector3 _pos;
        Vector3 _rot;

        void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Evenly spread selected transforms.");
            GUILayout.Label("Starting from the first (top) object in hierarchy.");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            _pos = EditorGUILayout.Vector3Field("Position Step", _pos);
            _rot = EditorGUILayout.Vector3Field("Rotation Step", _rot);

            if (EditorGUI.EndChangeCheck() && Selection.transforms.Length > 0)
            {
                var transforms = Selection.transforms.OrderBy(t => t.GetSiblingIndex()).ToArray();
                var sPos = transforms[0].position;
                var sRot = transforms[0].eulerAngles;
                for (int i = 1; i < transforms.Length; i++)
                {
                    var t = transforms[i];
                    Undo.RecordObject(t, "Spread");
                    t.position = sPos + _pos * i;
                    t.eulerAngles = sRot + _rot * i;
                }
            }
        }
    }
}
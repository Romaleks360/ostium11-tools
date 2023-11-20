using UnityEditor;
using UnityEngine;

namespace Ostium11.Editors
{
    public class ComparePerformance : EditorWindow
    {
        readonly System.Action[] _testCases = new System.Action[]
        {
            () =>
            {
                // your code here
            },

            () =>
            {
                // your code here
            },
        };

        int _iterations = 1_000_000;
        string _results = "No results yet!";

        [MenuItem("Tools/Compare Performance")]
        static void Run() => GetWindow<ComparePerformance>("Compare Performance", true);

        void OnGUI()
        {
            GUILayout.Label("Runs the specified test cases and measures their execution time.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();
            _iterations = EditorGUILayout.IntField("Repeat count", _iterations);
            EditorGUILayout.Space();
            if (GUILayout.Button("Run"))
            {
                _results = "";

                for (int id = 0; id < _testCases.Length; id++)
                {
                    var testCase = _testCases[id];
                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    for (int i = 0; i < _iterations; i++)
                    {
                        testCase();
                    }

                    sw.Stop();
                    _results += $"{id + 1}: {sw.ElapsedMilliseconds} ms\n";
                }

                Debug.Log(_results);
            }
            EditorGUILayout.Space();
            GUILayout.Label(_results);
        }
    }
}
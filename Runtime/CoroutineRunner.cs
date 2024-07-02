using System;
using System.Collections;
using UnityEngine;

namespace Ostium11
{
    public class CoroutineRunner : MonoBehaviour
    {
        static readonly WaitForEndOfFrame _endOfFrame = new();

        static CoroutineRunner _instance;

        static CoroutineRunner Instance => _instance =
            _instance != null ? _instance : new GameObject(nameof(CoroutineRunner)).AddComponent<CoroutineRunner>();

        public static void Run(IEnumerator routine) => Instance.StartCoroutine(routine);

        public static void Run(Action callback, YieldInstruction yield) => Run(Routine(callback, yield));

        public static void WaitForEndOfFrame(Action callback) => Run(callback, _endOfFrame);

        public static void WaitOneFrame(Action callback) => Run(callback, null);

        static IEnumerator Routine(Action callback, YieldInstruction yield)
        {
            yield return yield;
            callback();
        }
    }
}
using UnityEngine;

namespace Ostium11
{
    public static class Logger
    {
        public static void LogThis(this object obj)
        {
            Debug.Log(obj);
        }

        public static void Log(this object obj, object msg)
        {
            Debug.Log($"<b>[{obj.GetType().Name}]</b> {msg}");
        }

        public static void LogWarning(this object obj, object msg)
        {
            Debug.LogWarning($"<b>[{obj.GetType().Name}]</b> {msg}");
        }

        public static void LogError(this object obj, object msg)
        {
            Debug.LogError($"<b>[{obj.GetType().Name}]</b> {msg}");
        }
    }
}
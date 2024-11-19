using UnityEngine;

namespace Ostium11.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent(out T component))
                component = go.AddComponent<T>();
            return component;
        }

        public static void SetLayer(this GameObject go, int layer)
        {
            go.layer = layer;
            foreach (Transform tr in go.transform)
                SetLayer(tr.gameObject, layer);
        }
    }
}
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class GameObjectExtensions
    {
        public static Component GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent(out T component))
                component = go.AddComponent<T>();
            return component;
        }
    }
}
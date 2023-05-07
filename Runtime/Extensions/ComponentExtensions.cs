using UnityEngine;

namespace Ostium11.Extensions
{
    public static class ComponentExtensions
    {
        public static RectTransform rectTransform(this Component c) => (RectTransform)c.transform;
    }
}
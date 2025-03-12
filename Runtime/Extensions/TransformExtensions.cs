using System.Collections.Generic;
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class TransformExtensions
    {
        public static void SetX(this Transform transform, float x) => transform.position = transform.position.SetX(x);

        public static void SetY(this Transform transform, float y) => transform.position = transform.position.SetY(y);

        public static void SetZ(this Transform transform, float z) => transform.position = transform.position.SetZ(z);

        public static Transform GetLastChild(this Transform transform) => transform.GetChild(transform.childCount - 1);

        public static IEnumerable<Transform> GetChildren(this Transform transform)
        {
            foreach (Transform tr in transform)
                yield return tr;
        }
    }
}
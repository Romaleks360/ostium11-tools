using UnityEngine;

namespace Ostium11.Extensions
{
    public static class RayExtensions
    {
        // https://palitri.com/vault/stuff/maths/Rays%20closest%20point.pdf - first formula
        /// <summary>
        /// Calculates the closest point between two rays.
        /// </summary>
        /// <returns> Distance along the first ray </returns>
        public static float GetClosestDistanceTo(this Ray from, Ray to)
        {
            var a = from.direction;
            var b = to.direction;
            var c = to.origin - from.origin;
            return (-Dot(a, b) * Dot(b, c) + Dot(a, c) * Dot(b, b)) / (Dot(a, a) * Dot(b, b) - Dot(a, b) * Dot(a, b));

            static float Dot(Vector3 a, Vector3 b) => Vector3.Dot(a, b);
        }
    }
}

using UnityEngine;

namespace Ostium11.Extensions
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns an unsigned angle between two points
        /// (as opposed to Vector2.Angle, that returns an angle between two directions) 
        /// </summary>
        public static float Angle(this Vector2 from, Vector2 to)
        {
            float signedAngle = AngleSigned(from, to);
            return (signedAngle < 0) ? signedAngle + 360 : signedAngle;
            // return ((signedAngle < 0) ? signedAngle + 360 : signedAngle) * -1 + 90;
        }

        /// <summary>
        /// Returns an unsigned angle between two points
        /// (as opposed to Vector2.Angle, that returns an angle between two directions) 
        /// </summary>
        public static float AngleSigned(this Vector2 from, Vector2 to)
        {
            return Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Creates directional vector from an unsigned angle
        /// </summary>
        public static Vector2 FromAngle(this Vector2 _, float a)
        {
            a *= Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        }

        /// <summary>
        /// Returns an unsigned angle from a directional vector
        /// </summary>
        public static float ToAngle(this Vector2 v)
        {
            return Angle(Vector2.zero, v);
        }

        /// <summary>
        /// Rotates a directional vector by a given angle
        /// </summary>
        public static Vector2 Rotate(this Vector2 v, float a)
        {
            a *= Mathf.Deg2Rad;
            float s = Mathf.Sin(a);
            float c = Mathf.Cos(a);
            return new Vector2(
                v.x * c + v.y * s,
                v.y * c - v.x * s
            );
        }

        /// <summary>
        /// Creates a new Vector3(x, 0, y)
        /// </summary>
        public static Vector3 ToVector3XZ(this Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }

        public static Vector2 SetX(this Vector2 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static bool IsInsideRadius(this Vector2 point, Vector2 center, float radius)
        {
            return (point - center).sqrMagnitude <= radius * radius;
        }

        public static Vector2 Clamp(this Vector2 vector, float min, float max)
        {
            return new Vector2(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max));
        }
    }
}
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 ZeroX(this Vector3 vector)
        {
            vector.x = 0f;
            return vector;
        }

        public static Vector3 ZeroY(this Vector3 vector)
        {
            vector.y = 0f;
            return vector;
        }

        public static Vector3 ZeroZ(this Vector3 vector)
        {
            vector.z = 0f;
            return vector;
        }

        public static Vector3 SetX(this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector3 SetY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }

        public static Vector3 Set(this Vector3 vector, int axis, float value)
        {
            vector[axis] = value;
            return vector;
        }

        public static Vector3 SwapYZ(this Vector3 vector)
        {
            (vector.z, vector.y) = (vector.y, vector.z);
            return vector;
        }

        public static Vector2 GetXY(this Vector3 vector)
        {
            return (Vector2)vector;
        }

        public static Vector2 GetXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static Vector3 Floor(this Vector3 point)
        {
            return new Vector3(Mathf.Floor(point.x), Mathf.Floor(point.y), Mathf.Floor(point.z));
        }

        public static Vector3 Ceil(this Vector3 point)
        {
            return new Vector3(Mathf.Ceil(point.x), Mathf.Ceil(point.y), Mathf.Ceil(point.z));
        }

        public static bool IsInsideRadius(this Vector3 point, Vector3 center, float radius)
        {
            return (point - center).sqrMagnitude <= radius * radius;
        }

        public static Vector3Int ToVector3Int(this Vector3 point)
        {
            return new Vector3Int((int)point.x, (int)point.y, (int)point.z);
        }

        public static Vector3Int RoundToVector3Int(this Vector3 point)
        {
            return new Vector3Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), Mathf.RoundToInt(point.z));
        }

        public static Vector3Int FloorToVector3Int(this Vector3 point)
        {
            return new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z));
        }

        public static Vector3 Clamp(this Vector3 vector, float min, float max)
        {
            return new Vector3(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max), Mathf.Clamp(vector.z, min, max));
        }

        public static Vector3 Divide(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static float MinComponent(this Vector3 vector)
        {
            return Mathf.Min(vector.x, Mathf.Min(vector.y, vector.z));
        }

        public static float MaxComponent(this Vector3 vector)
        {
            return Mathf.Max(vector.x, Mathf.Max(vector.y, vector.z));
        }
    }
}
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class Vector3IntExtensions
    {
        public static Vector3Int ZeroX(this Vector3Int vector)
        {
            vector.x = 0;
            return vector;
        }

        public static Vector3Int ZeroY(this Vector3Int vector)
        {
            vector.y = 0;
            return vector;
        }

        public static Vector3Int ZeroZ(this Vector3Int vector)
        {
            vector.z = 0;
            return vector;
        }

        public static Vector3Int SwapXZ(this Vector3Int vector)
        {
            (vector.z, vector.x) = (vector.x, vector.z);
            return vector;
        }

        public static Vector3Int SwapYZ(this Vector3Int vector)
        {
            (vector.z, vector.y) = (vector.y, vector.z);
            return vector;
        }

        public static Vector2Int GetXY(this Vector3Int vector)
        {
            return (Vector2Int)vector;
        }

        public static Vector2Int GetXZ(this Vector3Int vector)
        {
            return new Vector2Int(vector.x, vector.z);
        }

        public static Vector3 ToVector3(this Vector3Int vector)
        {
            return vector;
        }
    }
}
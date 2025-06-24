using System.Collections.Generic;
using UnityEngine;

namespace Ostium11
{
    public enum Side
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Back
    }

    public static class SideExtensions
    {
        static readonly Dictionary<Vector3, Quaternion> _lookRotationTable = new()
        {
            [Vector3.left] = Quaternion.Inverse(Quaternion.LookRotation(Vector3.left)),
            [Vector3.right] = Quaternion.Inverse(Quaternion.LookRotation(Vector3.right)),
            [Vector3.up] = Quaternion.Inverse(Quaternion.LookRotation(Vector3.up)),
            [Vector3.down] = Quaternion.Inverse(Quaternion.LookRotation(Vector3.down)),
            [Vector3.forward] = Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward)),
            [Vector3.back] = Quaternion.Inverse(Quaternion.LookRotation(Vector3.back)),
        };

        public static Vector3 ToVector3(this Side side) => side switch
        {
            Side.Left => Vector3.left,
            Side.Right => Vector3.right,
            Side.Up => Vector3.up,
            Side.Down => Vector3.down,
            Side.Forward => Vector3.forward,
            Side.Back => Vector3.back,
            _ => default
        };

        /// <summary>
        /// Works only for normalized straight directional vector (forward, right, up...)
        /// </summary>
        public static Quaternion InverseLookRotation(this Vector3 vector) => _lookRotationTable[vector];
    }
}

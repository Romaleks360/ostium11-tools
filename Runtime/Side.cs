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
    }
}

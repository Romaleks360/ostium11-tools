
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class Texture3DExtensions
    {
        public static Vector3Int Size(this Texture3D texture) => new(texture.width, texture.height, texture.depth);
    }
}
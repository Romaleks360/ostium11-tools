
using UnityEngine;

namespace Ostium11.Extensions
{
    public static class Texture2DExtensions
    {
        public static Sprite ToSprite(this Texture2D texture) =>
            Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect);
    }
}
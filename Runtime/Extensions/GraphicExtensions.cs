using UnityEngine.UI;

namespace Ostium11.Extensions
{
    public static class GraphicExtensions
    {
        public static void SetAlpha(this Graphic g, float alpha) => g.color = g.color.SetAlpha(alpha);
    }
}
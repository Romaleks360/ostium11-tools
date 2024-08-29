using Ostium11.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class SetColor : MonoBehaviour
    {
        [SerializeField] Color[] _colors;
        [SerializeField] Graphic[] _graphics;

        public void Set(int color)
        {
            if (color.IsInRange(0, _colors.Length))
                foreach (var g in _graphics)
                    g.color = _colors[color];
            else
                Debug.LogError($"Invalid color index {color}");
        }
    }
}

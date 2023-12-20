using UnityEngine;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class SwapSprite : MonoBehaviour
    {
        [SerializeField] Image _target;
        [SerializeField] Sprite _on;
        [SerializeField] Sprite _off;

        void Reset()
        {
            if (_target == null)
                _target = GetComponent<Image>();
        }

        public void SetSprite(bool isOn)
        {
            _target.sprite = isOn ? _on : _off;
        }
    }
}
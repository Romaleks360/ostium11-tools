using UnityEngine;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class ButtonKeyShortcut : MonoBehaviour
    {
        [SerializeField] Button _buttonToPress;
        [SerializeField] KeyCode[] _shortcut;

        void Update()
        {
            bool justPressed = false;

            for (int i = 0; i < _shortcut.Length; i++)
            {
                var key = _shortcut[i];

                if (!Input.GetKey(key))
                    return;

                if (Input.GetKeyDown(key))
                    justPressed = true;
            }

            if (justPressed)
                _buttonToPress.onClick.Invoke();
        }

        void Reset()
        {
            if (TryGetComponent(out Button button))
                _buttonToPress = button;
        }
    }
}

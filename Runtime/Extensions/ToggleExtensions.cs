using UnityEngine.UI;

namespace Ostium11.Extensions
{
    public static class ToggleExtensions
    {
        public static void SetWithNotification(this Toggle toggle, bool isOn)
        {
            if (toggle.isOn == isOn)
                toggle.onValueChanged?.Invoke(isOn);
            else
                toggle.isOn = isOn;
        }
    }
}
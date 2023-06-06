using UnityEngine;

namespace Ostium11.Components
{
    public class ToggleActive : MonoBehaviour
    {
        public void Toggle() => gameObject.SetActive(!gameObject.activeSelf);
    }
}
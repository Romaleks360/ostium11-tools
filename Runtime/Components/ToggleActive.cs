using UnityEngine;

namespace Ostium11
{
    public class ToggleActive : MonoBehaviour
    {
        public void Toggle() => gameObject.SetActive(!gameObject.activeSelf);
    }
}
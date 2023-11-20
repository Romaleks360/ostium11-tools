using UnityEngine;
using UnityEngine.Events;

namespace Ostium11.Components
{
    public class InvertBoolUnityEvent : MonoBehaviour
    {
        [SerializeField] UnityEvent<bool> invertedEvent;

        public void Invoke(bool value) => invertedEvent.Invoke(!value);
    }
}
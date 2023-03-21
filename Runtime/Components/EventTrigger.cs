using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ostium11
{
    public class EventTrigger : MonoBehaviour
    {
        public enum EventType
        {
            Awake,
            Start,
            OnEnable,
            OnDisable,
            OnDestroy
        }

        [System.Serializable]
        public struct Event
        {
            public EventType Type;
            public UnityEvent Action;
        }

        [SerializeField] List<Event> _events;

        void Awake()     => Invoke(EventType.Awake);
        void Start()     => Invoke(EventType.Start);
        void OnEnable()  => Invoke(EventType.OnEnable);
        void OnDisable() => Invoke(EventType.OnDisable);
        void OnDestroy() => Invoke(EventType.OnDestroy);

        void Invoke(EventType type)
        {
            foreach (var e in _events)
                if (e.Type == type)
                    e.Action?.Invoke();
        }
    }
}
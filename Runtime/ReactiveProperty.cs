using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Ostium11
{
    [Serializable]
    public class ReactiveProperty<T> : IEquatable<T> where T : IEquatable<T>
    {
        [SerializeField] T _value = default;
        [SerializeField] UnityEvent<T> _changed = new();

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    _changed.Invoke(value);
                }
            }
        }

        public ReactiveProperty() { }
        public ReactiveProperty(T value) => _value = value;

        public void Subscribe(UnityAction<T> callback) => _changed.AddListener(callback);
        public void Unsubscribe(UnityAction<T> callback) => _changed.RemoveListener(callback);
        public void RaiseNotification() => _changed.Invoke(_value);

        public void Set(T value) => Value = value;
        public void SetWithoutNotification(T value) => _value = value;
        public void SetWithNotification(T value)
        {
            _value = value;
            _changed.Invoke(value);
        }

#if OSTIUM11_UNITASK_SUPPORT
        public async UniTask WaitForChange() => await _changed;
        public async UniTask WaitForValue(T value)
        {
            while (!_value.Equals(value))
                await _changed;
        }
#endif

        public bool Equals(T other) => _value.Equals(other);

        public override bool Equals(object other)
        {
            if (other is T value)
                return _value.Equals(value);
            if (other is ReactiveProperty<T> prop)
                return _value.Equals(prop.Value);
            return false;
        }

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(ReactiveProperty<T> prop, T value) => prop.Equals(value);
        public static bool operator !=(ReactiveProperty<T> prop, T value) => !(prop == value);
        public static implicit operator T(ReactiveProperty<T> prop) => prop.Value;
    }
}
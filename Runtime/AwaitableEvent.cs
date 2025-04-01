using Cysharp.Threading.Tasks;
using System;

namespace Ostium11
{
    public class AwaitableEvent
    {
        Action _event;

        public void Invoke() => _event?.Invoke();
        public void Subscribe(Action callback) => _event += callback;
        public void Unsubscribe(Action callback) => _event -= callback;

        public UniTask.Awaiter GetAwaiter()
        {
            UniTaskCompletionSource tcs = new();

            void Callback()
            {
                tcs.TrySetResult();
                Unsubscribe(Callback);
            }

            Subscribe(Callback);
            return tcs.Task.GetAwaiter();
        }
    }

    public class AwaitableEvent<T>
    {
        Action<T> _event;

        public void Invoke(T value) => _event?.Invoke(value);
        public void Subscribe(Action<T> callback) => _event += callback;
        public void Unsubscribe(Action<T> callback) => _event -= callback;

        public UniTask<T>.Awaiter GetAwaiter()
        {
            UniTaskCompletionSource<T> tcs = new();

            void Callback(T value)
            {
                tcs.TrySetResult(value);
                Unsubscribe(Callback);
            }

            Subscribe(Callback);
            return tcs.Task.GetAwaiter();
        }
    }
}
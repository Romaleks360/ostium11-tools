using Cysharp.Threading.Tasks;
using System;

namespace Ostium11
{
    public class AwaitableEvent
    {
        Action _event;
        UniTaskCompletionSource _tcs = new();

        UniTaskCompletionSource Tcs => _tcs ??= new();

        public void Invoke()
        {
            _event?.Invoke();
            if (_tcs == null) return;
            var tcs = _tcs;
            _tcs = null;
            tcs.TrySetResult();
        }

        public void Subscribe(Action callback) => _event += callback;
        public void Unsubscribe(Action callback) => _event -= callback;

        public UniTask Task => Tcs.Task;
        public UniTask.Awaiter GetAwaiter() => Tcs.Task.GetAwaiter();
    }

    public class AwaitableEvent<T>
    {
        Action<T> _event;
        UniTaskCompletionSource<T> _tcs = new();

        UniTaskCompletionSource<T> Tcs => _tcs ??= new();

        public void Invoke(T value)
        {
            _event?.Invoke(value);
            if (_tcs == null) return;
            var tcs = _tcs;
            _tcs = null;
            tcs.TrySetResult(value);
        }

        public void Subscribe(Action<T> callback) => _event += callback;
        public void Unsubscribe(Action<T> callback) => _event -= callback;

        public UniTask<T> Task => Tcs.Task;
        public UniTask<T>.Awaiter GetAwaiter() => Tcs.Task.GetAwaiter();
    }

    public static class AwaitableEventExtensions
    {
        public static async UniTask WaitForValue<T>(this AwaitableEvent<T> e, T value) where T : IEquatable<T>
        {
            while (!(await e).Equals(value)) { }
        }

        public static async UniTask WaitForValue<T>(this AwaitableEvent<T> e, Func<T, bool> predicate)
        {
            while (!predicate(await e)) { }
        }
    }
}
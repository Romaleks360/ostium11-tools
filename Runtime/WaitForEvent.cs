using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ostium11
{
    /// <summary>
    /// Use this to await events.
    /// </summary>
    /// <typeparam name="T">Task return type</typeparam>
    public struct WaitForEvent<T>
    {
        readonly TaskCompletionSource<T> _tcs;
        readonly Action<Action<T>> _unsubscribe;

        /// <summary>
        /// Usage:
        /// <c> await new WaitForEvent(listener => someEvent += listener); </c>
        /// </summary>
        public WaitForEvent(Action<Action<T>> subscribe)
        {
            _tcs = new TaskCompletionSource<T>();
            _unsubscribe = null;
            subscribe(Stop);
        }

        /// <summary>
        /// Usage:
        /// <c> await new WaitForEvent(listener => someEvent += listener, listener => someEvent -= listener)); </c> 
        /// </summary>
        public WaitForEvent(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe)
        {
            _tcs = new TaskCompletionSource<T>();
            _unsubscribe = unsubscribe;
            subscribe(Stop);
        }

        public TaskAwaiter<T> GetAwaiter() => _tcs.Task.GetAwaiter();

        public Task<T> Task => _tcs.Task;

        public void Complete(T result) => Stop(result);

        void Stop(T result)
        {
            _unsubscribe?.Invoke(Stop);
            _tcs.SetResult(result);
        }
    }
}
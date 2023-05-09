using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ostium11
{
    /// <summary>
    /// Use this to await events.
    /// </summary>
    /// <typeparam name="T">Task return type</typeparam>
    public class WaitForEvent<T>
    {
        readonly TaskCompletionSource<T> _tcs;
        readonly Action<Action<T>> _unsubscribe;

        /// <summary>
        /// Usage:
        /// <c> await new WaitForEvent(listener => someEvent += listener); </c>
        /// </summary>
        public WaitForEvent(Action<Action<T>> subscribe) : this(subscribe, null) { }

        /// <summary>
        /// Usage:
        /// <c> await new WaitForEvent(listener => someEvent += listener, listener => someEvent -= listener)); </c> 
        /// </summary>
        public WaitForEvent(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe)
        {
            _tcs = new TaskCompletionSource<T>();
            _unsubscribe = unsubscribe;
            subscribe(Complete);
        }

        public TaskAwaiter<T> GetAwaiter() => _tcs.Task.GetAwaiter();

        public void Complete(T result)
        {
            _unsubscribe?.Invoke(Complete);
            _tcs.SetResult(result);
        }
    }
}
using System;

namespace Cysharp.Threading.Tasks
{
    /// <summary>
    /// Use this to await events.
    /// </summary>
    /// <typeparam name="T">Task return type</typeparam>
    public class WaitForEvent
    {
        readonly UniTaskCompletionSource _tcs;
        readonly Action<Action> _unsubscribe;

        /// <summary>
        /// Usage:
        /// <c> await new WaitForEvent(listener => someEvent += listener); </c>
        /// </summary>
        public WaitForEvent(Action<Action> subscribe) : this(subscribe, null) { }

        /// <summary>
        /// Usage:
        /// <c> await new WaitForEvent(listener => someEvent += listener, listener => someEvent -= listener)); </c> 
        /// </summary>
        public WaitForEvent(Action<Action> subscribe, Action<Action> unsubscribe)
        {
            _tcs = new UniTaskCompletionSource();
            _unsubscribe = unsubscribe;
            subscribe(Complete);
        }

        public UniTask.Awaiter GetAwaiter() => _tcs.Task.GetAwaiter();

        public void Complete()
        {
            _unsubscribe?.Invoke(Complete);
            _tcs.TrySetResult();
        }
    }
}
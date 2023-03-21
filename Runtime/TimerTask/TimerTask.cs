using System;
using System.Collections.Generic;

namespace Ostium11.TimerTasks
{
    /// <summary>
    /// Use this class to run repeating actions.
    /// </summary>
    public class TimerTask
    {
        #region Static API

        public static UpdateType DefaultUpdateType { get; set; } = UpdateType.Normal;

        public static void ManualUpdate(float time) => TaskUpdater.ManualUpdate(time);

        public static TimerTask Create(float duration) => Create(duration: duration, null);

        public static TimerTask Create(float? duration = null, UpdateType? updateType = null)
        {
            updateType ??= DefaultUpdateType;
            var task = new TimerTask(TaskUpdater.GetTime(updateType.Value), duration);
            TaskUpdater.Add(updateType.Value, task.Tick);
            task.OnStop(() => TaskUpdater.Remove(task.Tick));
            return task;
        }

        public static TimerTask<T> Create<T>(T data, float? duration = null, UpdateType? updateType = null)
        {
            updateType ??= DefaultUpdateType;
            var task = new TimerTask<T>(data, TaskUpdater.GetTime(updateType.Value), duration);
            TaskUpdater.Add(updateType.Value, task.Tick);
            task.OnStop(task => TaskUpdater.Remove(task.Tick));
            return task;
        }

        #endregion

        // TODO: add pooling
        // TODO: use delta time instead of absolute time

        readonly float _creationTime;
        readonly IntervalAction _stop;

        List<Action> _onTick;
        List<IntervalAction> _onInterval;
        List<Action> _onStop;

        float _lastTickTime;

        public float CreationTime => _creationTime;
        public float ElapsedTime => _lastTickTime - _creationTime;
        public float Progress => ElapsedTime / Duration;
        public float Duration => _stop?.Interval ?? float.PositiveInfinity;
        public bool IsRunning => Progress <= 1f;
        public bool HasFinished => Progress > 1f;

        internal TimerTask(float time, float? duration)
        {
            _lastTickTime = time;
            _creationTime = time;

            if (duration.HasValue)
            {
                OnInterval(duration.Value, Stop);
                _stop = _onInterval[^1];
            }
        }

        internal void Tick(float time)
        {
            _lastTickTime = time;

            if (_onTick != null)
                foreach (var a in _onTick)
                    a();

            if (_onInterval != null)
                foreach (var i in _onInterval)
                    i.Tick(time);
        }

        public void Stop()
        {
            if (_onStop != null)
                foreach (var a in _onStop)
                    a();
        }

        public TimerTask OnTick(Action action)
        {
            _onTick ??= new();
            _onTick.Add(action);
            return this;
        }

        public TimerTask OnStop(Action action)
        {
            _onStop ??= new();
            _onStop.Add(action);
            return this;
        }

        public TimerTask OnInterval(float timeInterval, Action action) => OnInterval(timeInterval, action, out _);

        public TimerTask OnInterval(float timeInterval, Action action, out IntervalAction interval)
        {
            interval = new IntervalAction(timeInterval, action, _lastTickTime);
            _onInterval ??= new();
            _onInterval.Add(interval);
            return this;
        }
    }

    /// <summary>
    /// Use this class to run repeating actions.
    /// Use Data property to access a shared data buffer.
    /// </summary>
    /// <typeparam name="T">Task data type</typeparam>
    public class TimerTask<T> : TimerTask
    {
        public delegate void TimerAction(TimerTask<T> task);

        public T Data { get; set; }

        internal TimerTask(T data, float time, float? duration) : base(time, duration)
        {
            Data = data;
        }

        public TimerTask<T> OnTick(TimerAction action)
        {
            base.OnTick(() => action(this));
            return this;
        }

        public TimerTask<T> OnInterval(float timeInterval, TimerAction action)
        {
            base.OnInterval(timeInterval, () => action(this));
            return this;
        }

        public TimerTask<T> OnInterval(float timeInterval, TimerAction action, out IntervalAction interval)
        {
            base.OnInterval(timeInterval, () => action(this), out interval);
            return this;
        }

        public TimerTask<T> OnStop(TimerAction action)
        {
            base.OnStop(() => action(this));
            return this;
        }

        #region Hidden Methods

        new public TimerTask<T> OnTick(Action action) => (TimerTask<T>)base.OnTick(action);

        new public TimerTask<T> OnStop(Action action) => (TimerTask<T>)base.OnStop(action);

        new public TimerTask<T> OnInterval(float timeInterval, Action action, out IntervalAction interval)
            => (TimerTask<T>)base.OnInterval(timeInterval, action, out interval);

        new public TimerTask<T> OnInterval(float timeInterval, Action action)
            => (TimerTask<T>)base.OnInterval(timeInterval, action);

        #endregion
    }
}
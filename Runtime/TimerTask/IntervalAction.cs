using System;

namespace Ostium11.TimerTasks
{
    public class IntervalAction
    {
        readonly Action _action;

        float _lastInvokeTime;

        public float Interval { get; set; }

        public float Rate
        {
            get => 1f / Interval;
            set => Interval = 1f / value;
        }

        internal IntervalAction(float interval, Action action, float time)
        {
            Interval = Math.Max(0.001f, interval);
            _action = action;
            _lastInvokeTime = time;
        }

        internal void Tick(float time)
        {
            if (time - _lastInvokeTime > Interval)
            {
                _lastInvokeTime = time;
                _action();
            }
        }
    }
}

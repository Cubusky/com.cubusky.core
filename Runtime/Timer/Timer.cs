using System;
using System.Timers;
using UnityEngine;

namespace Cubusky
{
    [Serializable]
    public class Timer : System.Timers.Timer, ISerializationCallbackReceiver
    {
        private const double secondsToMilliseconds = 1000d;
        private const double millisecondsToSeconds = 1d / secondsToMilliseconds;

        public Timer() : base() { }
        public Timer(double interval) : base(interval * secondsToMilliseconds) { }

        [SerializeField, TimeSpan, Delayed] private long _interval;

        public new double Interval
        {
            get => base.Interval * millisecondsToSeconds;
            set => base.Interval = value * secondsToMilliseconds;
        }

        public DateTime StartTime { get; set; }

        public new event Action<double> Elapsed
        {
            add => base.Elapsed += GetHandler(value);
            remove => base.Elapsed -= GetHandler(value);
        }

        private ElapsedEventHandler GetHandler(Action<double> value) => (object sender, ElapsedEventArgs e) => value.Invoke((e.SignalTime - StartTime).TotalSeconds);

        public new void Start()
        {
            base.Start();
            StartTime = DateTime.Now;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() => _interval = TimeSpan.FromSeconds(Interval).Ticks;
        void ISerializationCallbackReceiver.OnAfterDeserialize() => Interval = Math.Max(new TimeSpan(_interval).TotalSeconds, millisecondsToSeconds);
    }
}

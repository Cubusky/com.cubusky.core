using System;
using UnityEngine;

namespace Cubusky
{
    public enum TimeMode
    {
        Time = 0,
        FixedTime = 1,
        UnscaledTime = 2,
        FixedUnscaledTime = 3,
    }

    public static class TimeModeExtensions
    {
        private static InvalidOperationException GetInvalidOperationException(this TimeMode timeMode) => new($"Unhandled {nameof(timeMode)} value {timeMode}.");

        public static float GetTime(this TimeMode timeMode) => timeMode switch
        {
            TimeMode.Time => Time.time,
            TimeMode.FixedTime => Time.fixedTime,
            TimeMode.UnscaledTime => Time.unscaledTime,
            TimeMode.FixedUnscaledTime => Time.fixedUnscaledTime,
            _ => throw timeMode.GetInvalidOperationException(),
        };

        public static double GetTimeAsDouble(this TimeMode timeMode) => timeMode switch
        {
            TimeMode.Time => Time.timeAsDouble,
            TimeMode.FixedTime => Time.fixedTimeAsDouble,
            TimeMode.UnscaledTime => Time.unscaledTimeAsDouble,
            TimeMode.FixedUnscaledTime => Time.fixedUnscaledTimeAsDouble,
            _ => throw timeMode.GetInvalidOperationException(),
        };

        public static float GetDeltaTime(this TimeMode timeMode) => timeMode switch
        {
            TimeMode.Time => Time.deltaTime,
            TimeMode.FixedTime => Time.fixedDeltaTime,
            TimeMode.UnscaledTime => Time.unscaledDeltaTime,
            TimeMode.FixedUnscaledTime => Time.fixedUnscaledDeltaTime,
            _ => throw timeMode.GetInvalidOperationException(),
        };
    }
}

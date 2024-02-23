#nullable enable
using System;
using UnityEngine;

namespace Cubusky
{
    public enum ApplicationPath
    {
        None,
        DataPath,
        PersistentDataPath,
        StreamingAssetsPath,
        TemporaryCachePath,
        ConsoleLogPath,
    }

    public static class ApplicationPathExtensions
    {
        public static string GetPath(this ApplicationPath applicationPath) => applicationPath switch
        {
            ApplicationPath.None => string.Empty,
            ApplicationPath.DataPath => Application.dataPath,
            ApplicationPath.PersistentDataPath => Application.persistentDataPath,
            ApplicationPath.StreamingAssetsPath => Application.streamingAssetsPath,
            ApplicationPath.TemporaryCachePath => Application.temporaryCachePath,
            ApplicationPath.ConsoleLogPath => Application.consoleLogPath,
            _ => throw new ArgumentOutOfRangeException(nameof(applicationPath))
        };
    }
}

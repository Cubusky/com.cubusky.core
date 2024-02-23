using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Saves data temporarily until the end of the session.
    /// </summary>
    public interface ITempSaver : ISaver, ISaver<object>
    {
        public static Dictionary<string, object> tempSaves = new();

        string key { get; }

        void ISaver<string>.Save(string data) => tempSaves[key] = data;
        void ISaver<byte[]>.Save(byte[] data) => tempSaves[key] = data;
        void ISaver<object>.Save(object data) => tempSaves[key] = data;
    }

    /// <summary>
    ///  Loads data from temporary saves.
    /// </summary>
    public interface ITempLoader : ILoader, ILoader<object>
    {
        string key { get; }

        object ILoader<object>.Load<TData>() => ITempSaver.tempSaves[key];
        string ILoader<string>.Load<TData>() => ITempSaver.tempSaves[key] as string;
        byte[] ILoader<byte[]>.Load<TData>() => ITempSaver.tempSaves[key] as byte[];
    }

    [Serializable]
    public class TempSaverLoader : ITempSaver, ITempLoader
    {
        [field: SerializeField] public string key { get; set; }
    }
}

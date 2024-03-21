using System;
using System.IO;
using UnityEngine;

namespace Cubusky
{
    [Serializable]
    public abstract class UnityPath
    {
        [field: SerializeField] public ApplicationPath applicationPath { get; set; } = ApplicationPath.PersistentDataPath;

        public abstract string relativePath { get; set; }
        public string fullPath => Path.Combine(applicationPath.GetPath(), relativePath);

        public static implicit operator string(UnityPath unityPath) => unityPath.fullPath;

        public override string ToString() => fullPath;
    }
}

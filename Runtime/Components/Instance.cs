#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// A singleton implementation where the last enabled instance is the current instance.
    /// </summary>
    public abstract class Instance<T> : MonoBehaviour where T : Instance<T>
    {
        private static readonly List<T> instances = new();

        /// <summary>
        /// Return the current instance.
        /// </summary>
        public static T? current { get; private set; }

        /// <summary>
        /// Called after the instance is set to the current instance.
        /// </summary>
        public abstract void OnCurrent();

        protected virtual void OnEnable()
        {
            instances.Add(current = (T)this);
            OnCurrent();
        }

        protected virtual void OnDisable()
        {
            if (instances.Remove((T)this) 
                && current == this)
            {
                (current = instances.Count > 0 ? instances[^1] : null)?.OnCurrent();
            }
        }
    }
}

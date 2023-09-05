#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Cubusky
{
    public abstract class Instance<T> : Instance where T : Instance<T>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistration()
        {
            instances.Clear();
        }

        private static readonly List<T> instances = new();

        /// <summary>
        /// Return the current instance.
        /// </summary>
        public static T? current => instances.Count > 0 ? instances[^1] : null;

        protected virtual void OnEnable()
        {
            instances.Add((T)this);
            SetState();
        }

        protected virtual void OnDisable()
        {
            var wasCurrent = current == this;
            instances.Remove((T)this);
            if (wasCurrent && current != null)
            {
                current.SetState();
            }
        }
    }
}

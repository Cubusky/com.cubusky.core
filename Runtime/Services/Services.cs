using UnityEngine;

namespace Cubusky
{
    internal static class Services
    {
        public static GameObject gameObject { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistration()
        {
            gameObject = new GameObject(nameof(Services))
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            Object.DontDestroyOnLoad(gameObject);
        }
    }
}

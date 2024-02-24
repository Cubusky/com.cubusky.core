using System;
using UnityEngine;

namespace Cubusky
{
    public static class UpdaterService
    {
        public static event Action onUpdate
        {
            add => value.AddTo(ref _onUpdate);
            remove => value.RemoveFrom(ref _onUpdate);
        }

        public static event Action onLateUpdate
        {
            add => value.AddTo(ref _onLateUpdate);
            remove => value.RemoveFrom(ref _onLateUpdate);
        }

        public static event Action onFixedUpdate
        {
            add => value.AddTo(ref _onFixedUpdate);
            remove => value.RemoveFrom(ref _onFixedUpdate);
        }

        private static Updater updater;

        private static event Action _onUpdate;
        private static event Action _onLateUpdate;
        private static event Action _onFixedUpdate;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistration()
        {
            updater = Services.gameObject.AddComponent<Updater>();
            updater.enabled = HasInvocations();
        }

        private static void AddTo(this Action value, ref Action action)
        {
            if (updater)
            {
                updater.enabled = true;
            }
            action += value;
        }

        private static void RemoveFrom(this Action value, ref Action action) 
        {
            action -= value;
            updater.enabled = HasInvocations();
        }

        private static bool HasInvocations() => (_onUpdate?.GetInvocationList().Length + _onLateUpdate?.GetInvocationList().Length + _onFixedUpdate?.GetInvocationList().Length).GetValueOrDefault() != 0;

        [AddComponentMenu("")]
        private class Updater : MonoBehaviour
        {
            private void Update()
            {
                _onUpdate?.Invoke();
            }

            private void LateUpdate()
            {
                _onLateUpdate?.Invoke();
            }

            private void FixedUpdate()
            {
                _onFixedUpdate?.Invoke();
            }
        }
    }
}

using UnityEngine;

namespace Cubusky
{
    [CreateAssetMenu(fileName = nameof(Timer), menuName = nameof(Cubusky) + "/" + nameof(Timer))]
    public class SharedTimer : ScriptableObject
    {
        [field: SerializeField] public Timer timer { get; set; }

        public static implicit operator Timer(SharedTimer sharedTimer) => sharedTimer.timer;
    }
}

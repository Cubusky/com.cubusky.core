using UnityEngine;

namespace Cubusky
{
    public abstract class Instance : MonoBehaviour
    {
        /// <summary>
        /// Sets the instance state.
        /// </summary>
        public abstract void SetState();
    }
}

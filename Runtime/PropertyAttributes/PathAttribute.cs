using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Serializes a <see cref="string"/> as a valid path.
    /// </summary>
    public class PathAttribute : PropertyAttribute 
    {
        public bool withoutExtension;

        public PathAttribute() { }

        public PathAttribute(bool withoutExtension)
        {
            this.withoutExtension = withoutExtension;
        }
    }
}

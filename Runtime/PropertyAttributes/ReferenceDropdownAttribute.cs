using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Draws a selectable dropdown for <see cref="SerializeReference"/> fields. Can be used to instantiate references inside components.
    /// </summary>
    public class ReferenceDropdownAttribute : PropertyAttribute 
    {
        public bool nullable;

        public ReferenceDropdownAttribute() { } 
        public ReferenceDropdownAttribute(bool nullable) 
        {
            this.nullable = nullable;
        }
    }
}

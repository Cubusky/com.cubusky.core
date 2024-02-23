using System;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Draws a selectable dropdown for <see cref="SerializeReference"/> fields.
    /// </summary>
    public class ReferenceDropdownAttribute : PropertyAttribute 
    {
        public bool nullable;
        public Type[] types;

        public ReferenceDropdownAttribute(params Type[] types) : this(default, types) { } 
        public ReferenceDropdownAttribute(bool nullable, params Type[] types)
        {
            this.nullable = nullable;
            this.types = types;
        }
    }
}

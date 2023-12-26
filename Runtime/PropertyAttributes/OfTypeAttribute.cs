using System;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Specifies a type that an Object needs to be of. Can be used to create an Object selector that allows interfaces.
    /// </summary>
    public class OfTypeAttribute : PropertyAttribute
    {
        public Type type;

        public OfTypeAttribute(Type type)
        {
            this.type = type;
        }
    }
}

using System;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Specifies types that an Object needs to be of. Can be used to create an Object selector that allows interfaces.
    /// </summary>
    public class OfTypeAttribute : PropertyAttribute
    {
        public Type[] types;

        public OfTypeAttribute(Type type) : this(new Type[] { type }) { }
        public OfTypeAttribute(params Type[] types)
        {
            this.types = types;
        }
    }
}

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cubusky
{
    /// <summary>
    /// Specifies <see cref="Type"/> in an <see cref="Object"/> selector.
    /// </summary>
    /// <remarks>
    /// Can be used to create <see cref="Object"/> selectors that allow interfaces:
    /// <code>
    /// [<see cref="OfTypeAttribute">OfType</see>(<see langword="typeof"/>(<see cref="UnityEngine.Animations.IConstraint"/>))] <see langword="public"/> <see cref="Object"/> constraint;
    /// </code>
    /// </remarks>
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

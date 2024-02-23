using System;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Serializes a <see cref="string"/> as a valid <see cref="Uri"/>.
    /// </summary>
    public class UriAttribute : PropertyAttribute 
    {
        public static readonly Uri EmptyUri = new("about:blank");

        public UriKind uriKind;

        public UriAttribute() : this(default) { }
        public UriAttribute(UriKind uriKind)
        {
            this.uriKind = uriKind;
        }
    }
}

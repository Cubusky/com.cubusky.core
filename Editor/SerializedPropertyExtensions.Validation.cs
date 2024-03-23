using System;
using UnityEditor;

namespace Cubusky.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        public static void ThrowIfPropertyType(this SerializedProperty property, params SerializedPropertyType[] invalidPropertyTypes)
        {
            if (Array.IndexOf(invalidPropertyTypes, property.propertyType) != -1)
            {
                throw new ArgumentException($"This property has invalid property type {property.propertyType}.", nameof(property.propertyType));
            }
        }

        public static void ThrowIfNotPropertyType(this SerializedProperty property, params SerializedPropertyType[] validPropertyTypes) 
        {
            if (Array.IndexOf(validPropertyTypes, property.propertyType) == -1)
            {
                throw new ArgumentException($"This property has invalid property type {property.propertyType}.", nameof(property.propertyType));
            }
        }

#if UNITY_2022_3_OR_NEWER
        public static void ThrowIfNumericType(this SerializedProperty property, params SerializedPropertyNumericType[] invalidNumericTypes)
        {
            if (Array.IndexOf(invalidNumericTypes, property.numericType) != -1)
            {
                throw new ArgumentException($"This property has invalid property type {property.numericType}.", nameof(property.numericType));
            }
        }

        public static void ThrowIfNotNumericType(this SerializedProperty property, params SerializedPropertyNumericType[] validNumericTypes)
        {
            if (Array.IndexOf(validNumericTypes, property.numericType) == -1)
            {
                throw new ArgumentException($"This property has invalid property type {property.numericType}.", nameof(property.numericType));
            }
        }
#endif
    }
}

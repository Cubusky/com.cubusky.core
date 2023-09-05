#nullable enable
using System;
using System.Reflection;
using UnityEditor;

namespace Cubusky.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        /// <summary>Indicates whether both properties refer to the same underlying field.</summary>
        public static bool PropertyEquals(this SerializedProperty? property, SerializedProperty? obj)
        {
            if (property == obj
                || property == null && obj == null)
            {
                return true;
            }
            else if (property == null || obj == null)
            {
                return false;
            }
            else if (property.propertyPath != obj.propertyPath)
            {
                return false;
            }

            var aTargets = property.serializedObject.targetObjects;
            var bTargets = obj.serializedObject.targetObjects;
            if (aTargets.Length != bTargets.Length)
            {
                return false;
            }

            for (int i = 0; i < aTargets.Length; i++)
            {
                if (aTargets[i] != bTargets[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Executes the `action` once with a new <see cref="SerializedProperty"/> for each of the
        /// <see cref="SerializedObject.targetObjects"/>. Or if there is only one target, it uses the `property`.
        /// </summary>
        public static void ForEachTarget(this SerializedProperty property, Action<SerializedProperty> function)
        {
            var targets = property.serializedObject.targetObjects;

            Undo.IncrementCurrentGroup(); // May be unnecessary, but added for safety.
            Undo.RecordObjects(targets, Undo.GetCurrentGroupName());

            if (targets.Length == 1)
            {
                function(property);
                property.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var path = property.propertyPath;
                for (int i = 0; i < targets.Length; i++)
                {
                    using (var serializedObject = new SerializedObject(targets[i]))
                    {
                        property = serializedObject.FindProperty(path);
                        function(property);
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the specified `property` and marks its target objects as dirty so any changes to a prefab will be saved.
        /// </summary>
        public static void SetTargetsDirty(this SerializedProperty property)
        {
            var targets = property.serializedObject.targetObjects;

            // If this change is made to a prefab, this makes sure that any instances in the scene will be updated.
            for (int i = 0; i < targets.Length; i++)
            {
                EditorUtility.SetDirty(targets[i]);
            }

            property.serializedObject.Update();
        }

        /// <summary>Delete the element at the specified index in the array and reorder the array accordingly.</summary>
        /// <remarks>
        /// If the element is not at its default value, the first call to
        /// <see cref="SerializedProperty.DeleteArrayElementAtIndex"/> will only reset it, so this method will
        /// call it again if necessary to ensure that it actually gets removed.
        /// </remarks>
        public static void DeleteArrayElementAtIndexSafe(this SerializedProperty property, int index)
        {
            var count = property.arraySize;
            property.DeleteArrayElementAtIndex(index);
            if (property.arraySize == count)
            {
                property.DeleteArrayElementAtIndex(index);
            }
        }
    }
}

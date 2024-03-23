using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(ReferenceDropdownAttribute))]
    public class ReferenceDropdownDrawer : PropertyDrawer
    {
        private const string nullString = "null";
        private static readonly Type unityObjectType = typeof(UnityEngine.Object);

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.ManagedReference);

            foreach (var targetObject in property.serializedObject.targetObjects)
            {
                SerializationUtility.ClearAllManagedReferencesWithMissingTypes(targetObject);
            }

            if (property.hasMultipleDifferentValues)
            {
                return new MultivalueEditingNotSupported(property.displayName);
            }

            var referenceDropdownAttribute = attribute as ReferenceDropdownAttribute;

            // Set up the property field and collect all derived types where Activators can instantiate them.
            var propertyField = new PropertyField(property);
            InternalEditorBridge.GetTypeFromManagedReferenceFullTypeName(property.managedReferenceFieldTypename, out var fieldType);
            var derivedTypes = (from derivedType in TypeCache.GetTypesDerivedFrom(fieldType)
                                where !derivedType.IsAbstract
                                where derivedType.GetConstructor(Type.EmptyTypes) != null
                                where !unityObjectType.IsAssignableFrom(derivedType)  // Cannot assign an object deriving from UnityEngine.Object to a managed reference. This is not supported.
                                where referenceDropdownAttribute.types.All(type => type.IsAssignableFrom(derivedType))
                                orderby derivedType.Name
                                select derivedType).ToList();

            var isNullable = referenceDropdownAttribute.nullable // TODO: Use "or NullabilityInfoContext" in .NET 6 to determine nullability of a type.
                || derivedTypes.Count == 0;

            // Create choices based on nullability.
            List<string> choices = new();
            if (!isNullable)
            {
                // Ensure the property is not null if it shouldn't be.
                if (property.managedReferenceValue == null)
                {
                    property.managedReferenceValue = Activator.CreateInstance(derivedTypes[0]);
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            else
            {
                choices.Add(nullString);
            }

            var choiceToType = derivedTypes.ToDictionary(derivedType => derivedType.AssemblyQualifiedName);
            choices.AddRange(choiceToType.Keys);

            // Set up the dropdown field.
            var dropdown = new DropdownField(property.displayName, choices, 0, FormatSelectedValue, FormatListItem)
            {
                showMixedValue = property.hasMultipleDifferentValues,
            };
#if UNITY_2022_3_OR_NEWER
            dropdown.AddToClassList(DropdownField.alignedFieldUssClassName);
#endif
            dropdown.TrackPropertyValue(property, PropertyChanged);
            dropdown.RegisterValueChangedCallback(changed =>
            {
                if (property.hasMultipleDifferentValues)
                {
                    return;
                }

                // Unbind our property as to not trigger a property changed event.
                propertyField.Unbind();

                // Change the property.
                property.managedReferenceValue = choiceToType.TryGetValue(changed.newValue, out var type)
                    ? Activator.CreateInstance(type)
                    : null;
                property.serializedObject.ApplyModifiedProperties();

                // Trigger the property changed event, which will rebind the property.
                PropertyChanged(property);
            });

            // Trigger the property changed event in the beginning.
            PropertyChanged(property);

            // Set up the root and create the dropdown as an overlay.
            VisualElement root = new();
            root.Add(propertyField);
            root.Add(dropdown);
            return root;

            // Dropdown drawers.
            string FormatSelectedValue(string value) => choiceToType.TryGetValue(value, out var type)
                ? ObjectNames.NicifyVariableName(type.Name)
                : nullString;
            string FormatListItem(string item) => FormatSelectedValue(item) 
                + (choiceToType.TryGetValue(item, out var type)
                    ? $" ({type.FullName})"
                    : string.Empty);

            // Redraw the dropdown when the property changes.
            void PropertyChanged(SerializedProperty changedProperty)
            {
                propertyField.BindProperty(changedProperty);

                {
                    var isOverlay = changedProperty.hasVisibleChildren;

                    dropdown.labelElement.visible = !isOverlay;

                    dropdown.pickingMode = isOverlay ? PickingMode.Ignore : PickingMode.Position;
                    dropdown.ElementAt(1).pickingMode = isOverlay ? PickingMode.Position : PickingMode.Ignore;

                    dropdown.style.position = isOverlay ? Position.Absolute : Position.Relative;
                    dropdown.style.width = isOverlay ? Length.Percent(100f) : new Length();
                }

                InternalEditorBridge.GetTypeFromManagedReferenceFullTypeName(changedProperty.managedReferenceFullTypename, out var valueType);
                var changedIndex = Math.Max(derivedTypes.IndexOf(valueType) + (isNullable ? 1 : 0), 0); // Increment to account for "Null" option.
                dropdown.SetValueWithoutNotify(choices[changedIndex]);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.ManagedReference);

            using var propertyScope = new EditorGUI.PropertyScope(position, label, property);

            foreach (var targetObject in property.serializedObject.targetObjects)
            {
                SerializationUtility.ClearAllManagedReferencesWithMissingTypes(targetObject);
            }

            if (property.hasMultipleDifferentValues)
            {
                var label2 = new GUIContent(label);
                EditorGUI.LabelField(position, label2, EditorGUIUtility.TrTempContent("Multi-value editing not supported."));
                return;
            }

            var referenceDropdownAttribute = attribute as ReferenceDropdownAttribute;

            // Set up the property field and collect all derived types where Activators can instantiate them.
            InternalEditorBridge.GetTypeFromManagedReferenceFullTypeName(property.managedReferenceFieldTypename, out var fieldType);
            var derivedTypes = (from derivedType in TypeCache.GetTypesDerivedFrom(fieldType)
                                where !derivedType.IsAbstract
                                where derivedType.GetConstructor(Type.EmptyTypes) != null
                                where !unityObjectType.IsAssignableFrom(derivedType)  // Cannot assign an object deriving from UnityEngine.Object to a managed reference. This is not supported.
                                where referenceDropdownAttribute.types.All(type => type.IsAssignableFrom(derivedType))
                                orderby derivedType.Name
                                select derivedType).ToList();

            var isNullable = referenceDropdownAttribute.nullable // TODO: Use "or NullabilityInfoContext" in .NET 6 to determine nullability of a type.
                || derivedTypes.Count == 0;

            // Create choices based on nullability.
            List<string> choices = new();
            if (!isNullable)
            {
                // Ensure the property is not null if it shouldn't be.
                if (property.managedReferenceValue == null)
                {
                    property.managedReferenceValue = Activator.CreateInstance(derivedTypes[0]);
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            else
            {
                choices.Add(nullString);
            }

            var choiceToType = derivedTypes.ToDictionary(derivedType => $"{ObjectNames.NicifyVariableName(derivedType.Name)} ({derivedType.FullName})");
            choices.AddRange(choiceToType.Keys);

            // Draw the popup and affect the property if it changes.
            using var changeCheckScope = new EditorGUI.ChangeCheckScope();
            InternalEditorBridge.GetTypeFromManagedReferenceFullTypeName(property.managedReferenceFullTypename, out var valueType);
            var index = Math.Max(derivedTypes.IndexOf(valueType) + (isNullable ? 1 : 0), 0); // Increment to account for "Null" option.
            var popupPosition = EditorGUI.PrefixLabel(position, label);
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            index = EditorGUI.Popup(popupPosition, index, choices.ToArray());
            if (changeCheckScope.changed)
            {
                if (property.hasMultipleDifferentValues)
                {
                    return;
                }

                property.managedReferenceValue = choiceToType.TryGetValue(choices[index], out var type)
                    ? Activator.CreateInstance(type) 
                    : null;
                property.serializedObject.ApplyModifiedProperties();
            }

            // Draw the property and the dropdown.
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}

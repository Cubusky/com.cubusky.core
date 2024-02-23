using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
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
            dropdown.AddToClassList(DropdownField.alignedFieldUssClassName);
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
                    dropdown.style.width = isOverlay ? Length.Percent(100f) : Length.Auto();
                }

                InternalEditorBridge.GetTypeFromManagedReferenceFullTypeName(changedProperty.managedReferenceFullTypename, out var valueType);
                var changedIndex = Math.Max(derivedTypes.IndexOf(valueType) + (isNullable ? 1 : 0), 0); // Increment to account for "Null" option.
                dropdown.SetValueWithoutNotify(choices[changedIndex]);
            }
        }
    }
}

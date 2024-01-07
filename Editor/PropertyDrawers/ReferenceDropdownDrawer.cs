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

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Check if the property type is of a managed reference.
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                throw new ArgumentException("This attribute is not supported on properties of this property type.", nameof(property.propertyType));
            }

            // Set up the property field and collect all derived types where Activators can instantiate them.
            var propertyField = new PropertyField(property);
            InternalEditorBridge.GetTypeFromManagedReferenceFullTypeName(property.managedReferenceFieldTypename, out var fieldType);
            var derivedTypes = (from derivedType in TypeCache.GetTypesDerivedFrom(fieldType)
                                where !derivedType.IsAbstract
                                where derivedType.GetConstructor(Type.EmptyTypes) != null
                                where !typeof(UnityEngine.Object).IsAssignableFrom(derivedType)  // Cannot assign an object deriving from UnityEngine.Object to a managed reference. This is not supported.
                                orderby derivedType.Name
                                select derivedType).ToList();

            var isNullable = (attribute as ReferenceDropdownAttribute).nullable // TODO: Use "or NullabilityInfoContext" in .NET 6 to determine nullability of a type.
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
            var dropdown = new DropdownField(property.displayName, choices, 0, FormatSelectedValue, FormatListItem);
            dropdown.AddToClassList(DropdownField.alignedFieldUssClassName);
            dropdown.TrackPropertyValue(property, PropertyChanged);
            dropdown.RegisterValueChangedCallback(changed =>
            {
                // Unbind our property as to not trigger a property changed event.
                propertyField.Unbind();

                // Change the property.
                if (isNullable)
                {
                    property.managedReferenceValue = dropdown.index == 0 ? null : Activator.CreateInstance(derivedTypes[dropdown.index - 1]); // Decrement to account for "Null" option.
                }
                else
                {
                    property.managedReferenceValue = Activator.CreateInstance(derivedTypes[dropdown.index]);
                }
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
                    dropdown.style.width = isOverlay ? Length.Percent(100f) : Length.Auto();
                }

                InternalEditorBridge.GetTypeFromManagedReferenceFullTypeName(changedProperty.managedReferenceFullTypename, out var valueType);
                var changedIndex = derivedTypes.IndexOf(valueType);
                changedIndex += isNullable ? 1 : 0; // Increment to account for "Null" option.
                dropdown.SetValueWithoutNotify(choices[changedIndex]);
            }
        }
    }
}

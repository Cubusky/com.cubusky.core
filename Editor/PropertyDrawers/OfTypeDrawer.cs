using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(OfTypeAttribute))]
    public class OfTypeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference
                && property.propertyType != SerializedPropertyType.ExposedReference)
            {
                throw new System.ArgumentException("This attribute is not supported on properties of this property type.", nameof(property.propertyType));
            }

            var ofType = attribute as OfTypeAttribute;

            var objectField = new ObjectField(property.displayName);
            objectField.AddToClassList(ObjectField.alignedFieldUssClassName);
            objectField.BindProperty(property);
            objectField.objectType = ofType.type;

            objectField.RegisterValueChangedCallback(changed =>
            {
                Component component;
                if (IsValid(changed.newValue))
                {
                    return;
                }
                else if (changed.newValue is GameObject gameObject
                    && (component = gameObject.GetComponents<Component>().FirstOrDefault(component => IsValid(component))))
                {
                    objectField.SetValueWithoutNotify(component);
                    return;
                }
                else if (changed.newValue)
                {
                    objectField.SetValueWithoutNotify(null);
                }

                switch (property.propertyType)
                {
                    case SerializedPropertyType.ObjectReference:
                        property.objectReferenceValue = objectField.value;
                        break;
                    case SerializedPropertyType.ExposedReference:
                        property.exposedReferenceValue = objectField.value;
                        break;
                }
                property.serializedObject.ApplyModifiedProperties();

                bool IsValid(Object obj) => obj && ofType.type.IsAssignableFrom(obj.GetType());
            });

            return objectField;
        }
    }
}

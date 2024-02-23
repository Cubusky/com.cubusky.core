using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(UriAttribute))]
    public class UriDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.String);

            var uriAttribute = attribute as UriAttribute;

            var textField = new TextField(property.displayName)
            {
                isDelayed = true,
            };
            textField.AddToClassList(TextField.alignedFieldUssClassName);
            textField.BindProperty(property);

            textField.RegisterValueChangedCallback(changed =>
            {
                if (property.hasMultipleDifferentValues)
                {
                    return;
                }

                if ((!Uri.TryCreate(changed.newValue, uriAttribute.uriKind, out var uri)
                        && !Uri.TryCreate(changed.previousValue, uriAttribute.uriKind, out uri))
                        || string.IsNullOrEmpty(changed.newValue))
                {
                    uri = UriAttribute.EmptyUri;
                }

                textField.SetValueWithoutNotify(property.stringValue = uri.ToString());
                property.serializedObject.ApplyModifiedProperties();
            });

            return textField;
        }
    }
}

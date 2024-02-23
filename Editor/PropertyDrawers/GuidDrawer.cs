using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(GuidAttribute))]
    public class GuidDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.String);

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

                if (string.IsNullOrEmpty(changed.newValue)
                    || (!Guid.TryParse(changed.newValue, out var guid)
                        && !Guid.TryParse(changed.previousValue, out guid)))
                {
                    guid = Guid.NewGuid();
                }

                textField.SetValueWithoutNotify(property.stringValue = guid.ToString());
                property.serializedObject.ApplyModifiedProperties();
            });

            return textField;
        }
    }
}

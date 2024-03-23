using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
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
#if UNITY_2022_3_OR_NEWER
            textField.AddToClassList(TextField.alignedFieldUssClassName);
#endif
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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.String);

            using var propertyScope = new EditorGUI.PropertyScope(position, label, property);
            using var changeCheckScope = new EditorGUI.ChangeCheckScope();
            var previousValue = property.stringValue;
            EditorGUI.DelayedTextField(position, property, label);
            var newValue = property.stringValue;

            if (changeCheckScope.changed || string.IsNullOrEmpty(newValue))
            {
                if (property.hasMultipleDifferentValues)
                {
                    return;
                }

                if (string.IsNullOrEmpty(newValue)
                    || (!Guid.TryParse(newValue, out var guid)
                        && !Guid.TryParse(previousValue, out guid)))
                {
                    guid = Guid.NewGuid();
                }

                property.stringValue = guid.ToString();
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

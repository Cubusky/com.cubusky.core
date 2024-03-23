using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.String);

            var uriAttribute = attribute as UriAttribute;

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

                if ((!Uri.TryCreate(newValue, uriAttribute.uriKind, out var uri)
                        && !Uri.TryCreate(previousValue, uriAttribute.uriKind, out uri))
                        || string.IsNullOrEmpty(newValue))
                {
                    uri = UriAttribute.EmptyUri;
                }

                property.stringValue = uri.ToString();
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

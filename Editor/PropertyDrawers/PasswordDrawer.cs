using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(PasswordAttribute))]
    public class PasswordDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.String);

            var textField = new TextField(property.displayName, int.MaxValue, false, true, '*');
#if UNITY_2022_3_OR_NEWER
            textField.AddToClassList(TextField.alignedFieldUssClassName);
#endif

            textField.BindProperty(property);

            return textField;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.String);

            using var propertyScope = new EditorGUI.PropertyScope(position, label, property);
            property.stringValue = EditorGUI.PasswordField(position, label, property.stringValue);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}

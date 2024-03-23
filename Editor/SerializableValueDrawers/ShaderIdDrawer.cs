#nullable enable
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(ShaderId))]
    public class ShaderIdDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var textField = new TextField(property.displayName);
            textField.BindProperty(property.FindPropertyRelative($"_{nameof(ShaderId.name)}"));
#if UNITY_2022_3_OR_NEWER
            textField.AddToClassList(TextField.alignedFieldUssClassName);
#endif
            return textField;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var propertyScope = new EditorGUI.PropertyScope(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative($"_{nameof(ShaderId.name)}"), label);
        }
    }
}

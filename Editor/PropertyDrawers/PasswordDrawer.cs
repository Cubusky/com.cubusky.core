using System.Collections;
using System.Collections.Generic;
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
            var textField = new TextField(property.displayName, int.MaxValue, false, true, '*');
            textField.AddToClassList(TextField.alignedFieldUssClassName);

            textField.BindProperty(property);

            return textField;
        }
    }
}

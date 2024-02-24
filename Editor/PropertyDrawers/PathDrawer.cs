using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(PathAttribute))]
    public class PathDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.String);

            var pathAttribute = attribute as PathAttribute;

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

                var invalidPathChars = Path.GetInvalidPathChars();
                var directoryName = new ReadOnlySpan<char>(changed.newValue.Where(valueChar => !invalidPathChars.Contains(valueChar)).ToArray());

                ReadOnlySpan<char> fileName;
                ReadOnlySpan<char> value = string.Empty;
                var invalidFileNameChars = Path.GetInvalidFileNameChars();
                while (!directoryName.IsEmpty)
                {
                    fileName = Path.GetFileName(directoryName);
                    fileName = fileName.ToArray().Where(fileNameChar => !invalidFileNameChars.Contains(fileNameChar)).ToArray();
                    value = Path.Join(fileName, value);
                    directoryName = Path.GetDirectoryName(directoryName);
                }

                if (pathAttribute.withoutExtension && !value.IsEmpty)
                {
                    directoryName = Path.GetDirectoryName(value);

                    do
                    {
                        value = Path.GetFileNameWithoutExtension(value);
                    }
                    while (Path.HasExtension(value));

                    value = Path.Join(directoryName, value);
                }

                textField.SetValueWithoutNotify(property.stringValue = value.ToString());
                property.serializedObject.ApplyModifiedProperties();
            });

            return textField;
        }
    }
}

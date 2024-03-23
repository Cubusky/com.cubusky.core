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
                string value = string.Empty;
                var invalidFileNameChars = Path.GetInvalidFileNameChars();
                ReadOnlySpan<char> pathRoot = Path.IsPathRooted(directoryName) ? Path.GetPathRoot(directoryName) : string.Empty;
                while (directoryName.Length > pathRoot.Length)
                {
                    fileName = Path.GetFileName(directoryName);
                    directoryName = directoryName[..^Math.Min(fileName.Length + 1, directoryName.Length)];
                    fileName = fileName.ToArray().Where(fileNameChar => !invalidFileNameChars.Contains(fileNameChar)).ToArray();
                    value = Path.Join(fileName, value);
                }
                value = pathRoot.ToString() + value;

                if (pathAttribute.withoutExtension && value.Length > 0)
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

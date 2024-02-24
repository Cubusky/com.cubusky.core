#nullable enable
using Cubusky.UIElements;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(TimeSpanAttribute), true)]
    public class TimeSpanDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            property.ThrowIfNotNumericType(SerializedPropertyNumericType.Int64);

            // Create the TimeSpanField.
            var timeSpanField = new TimeSpanField(preferredLabel)
            {
                isDelayed = fieldInfo.GetCustomAttribute<DelayedAttribute>(true) != null,
                showMixedValue = property.hasMultipleDifferentValues,
            };

            // Bind the property.
            timeSpanField.RegisterCallback<BlurEvent>(OnBlur);
            timeSpanField.TrackPropertyValue(property, PropertyChanged);
            PropertyChanged(property);

            // Return the TimeSpanField.
            timeSpanField.AddToClassList(TimeSpanField.alignedFieldUssClassName);
            return timeSpanField;

            // Bind property methods.
            void OnBlur(BlurEvent blurEvent)
            {
                property.longValue = timeSpanField.value.Ticks;
                property.serializedObject.ApplyModifiedProperties();
            }

            void PropertyChanged(SerializedProperty property)
            {
                timeSpanField.SetValueWithoutNotify(new TimeSpan(property.longValue));
            }
        }
    }
}
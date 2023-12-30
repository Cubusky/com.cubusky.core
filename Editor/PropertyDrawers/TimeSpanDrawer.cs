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
            // Check if property is of type long.
            if (property.numericType != SerializedPropertyNumericType.Int64)
            {
                return base.CreatePropertyGUI(property);
            }

            // Create the TimeSpanField.
            var timeSpanField = new TimeSpanField(preferredLabel)
            {
                isDelayed = fieldInfo.GetCustomAttribute<DelayedAttribute>(true) != null,
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
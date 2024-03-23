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
#if UNITY_2022_3_OR_NEWER
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
#else
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.ThrowIfNotPropertyType(SerializedPropertyType.Integer);

            using var propertyScope = new EditorGUI.PropertyScope(position, label, property);
            using var changeCheckScope = new EditorGUI.ChangeCheckScope();

            const float minLongWidth = 160f;
            const float timeSpanWidth = 100f;
            const int ticksPerMillisecond = (int)TimeSpan.TicksPerMillisecond;

            var width = position.width;
            position.width = MathF.Max(position.width - timeSpanWidth - 2f, EditorGUIUtility.labelWidth + minLongWidth);
            if (fieldInfo.GetCustomAttribute<DelayedAttribute>(true) != null)
            {
                property.intValue = EditorGUI.DelayedIntField(position, label, property.intValue / ticksPerMillisecond) * ticksPerMillisecond;
            }
            else
            {
                property.intValue = EditorGUI.IntField(position, label, property.intValue / ticksPerMillisecond) * ticksPerMillisecond;
            }

            position.x += position.width + 2f;
            position.width = timeSpanWidth;
            EditorGUI.LabelField(position, new TimeSpan(property.intValue).ToString("g"));

            if (changeCheckScope.changed)
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}
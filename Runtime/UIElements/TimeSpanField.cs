using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.UIElements
{
    //
    // Summary:
    //     Makes a text field for entering TimeSpans.
    public class TimeSpanField : TextValueField<TimeSpan>
    {
        private class TimeSpanInput : TextValueInput
        {
            private TimeSpanField parentTimeSpanField => (TimeSpanField)base.parent;

            protected override string allowedCharacters => UITimeFieldsUtils.k_AllowedCharactersForTime;

            internal TimeSpanInput()
            {
                base.formatString = UITimeFieldsUtils.k_TimeFieldFormatString;
            }

            public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, TimeSpan startValue)
            {
                var ticksPerSpeed = speed switch
                {
                    DeltaSpeed.Fast => TimeSpan.TicksPerMinute,
                    DeltaSpeed.Slow => TimeSpan.TicksPerMillisecond,
                    _ => TimeSpan.TicksPerSecond
                };

                double num = InternalEngineBridge.CalculateIntDragSensitivity(startValue.Ticks / TimeSpan.TicksPerSecond);
                double niceDelta = (double)InternalEngineBridge.NiceDelta(delta, 1f);   // Acceleration = 1. Normally `speed` would define `acceleration`, but for TimeSpan we use `speed` to define `ticksPerSpeed` instead.
                long roundedDelta = (long)Math.Round(niceDelta * num) * ticksPerSpeed;

                long value = ClampMinMaxLongValue(roundedDelta, StringToValue(base.text).Ticks);
                if (parentTimeSpanField.isDelayed)
                {
                    base.text = ValueToString(new TimeSpan(value));
                }
                else
                {
                    parentTimeSpanField.value = new TimeSpan(value);
                }
            }

            private long ClampMinMaxLongValue(long niceDelta, long value)
            {
                long num = Math.Abs(niceDelta);
                if (niceDelta > 0)
                {
                    if (value > 0 && num > long.MaxValue - value)
                    {
                        return long.MaxValue;
                    }

                    return value + niceDelta;
                }

                if (value < 0 && value < long.MinValue + num)
                {
                    return long.MinValue;
                }

                return value - num;
            }

            protected override string ValueToString(TimeSpan v)
            {
                return v.ToString(base.formatString);
            }

            protected override TimeSpan StringToValue(string str)
            {
                UITimeFieldsUtils.TryConvertStringToTimeSpan(str, this.GetOriginalText(), out var value);
                return value;
            }
        }

        //
        // Summary:
        //     USS class name of elements of this type.
        public new static readonly string ussClassName = "unity-timespan-field";

        //
        // Summary:
        //     USS class name of labels in elements of this type.
        public new static readonly string labelUssClassName = ussClassName + "__label";

        //
        // Summary:
        //     USS class name of input elements in elements of this type.
        public new static readonly string inputUssClassName = ussClassName + "__input";

        private TimeSpanInput timeSpanInput => (TimeSpanInput)base.textInputBase;

        //
        // Summary:
        //     Converts the given TimeSpan to a string.
        //
        // Parameters:
        //   v:
        //     The TimeSpan to be converted to string.
        //
        // Returns:
        //     The TimeSpan as string.
        protected override string ValueToString(TimeSpan v)
        {
            return v.ToString(base.formatString);
        }

        //
        // Summary:
        //     Converts a string to a TimeSpan.
        //
        // Parameters:
        //   str:
        //     The string to convert.
        //
        // Returns:
        //     The TimeSpan parsed from the string.
        protected override TimeSpan StringToValue(string str)
        {
            return UITimeFieldsUtils.TryConvertStringToTimeSpan(str, base.textInputBase.GetOriginalText(), out var value) ? value : base.rawValue;
        }

        //
        // Summary:
        //     Constructor.
        public TimeSpanField() : this(null) { }

        //
        // Summary:
        //     Constructor.
        //
        // Parameters:
        //   maxLength:
        //     Maximum number of characters the field can take.
        //
        //   label:
        public TimeSpanField(string label) : base(label, -1, (TextValueInput)new TimeSpanInput())
        {
            AddToClassList(ussClassName);
            base.labelElement.AddToClassList(labelUssClassName);
            InternalEngineBridge.GetVisualInput(this).AddToClassList(inputUssClassName);
            AddLabelDragger<TimeSpan>();
        }

        //
        // Summary:
        //     Applies the values of a 3D delta and a speed from an input device.
        //
        // Parameters:
        //   delta:
        //     A vector used to compute the value change.
        //
        //   speed:
        //     A multiplier for the value change.
        //
        //   startValue:
        //     The start value.
        public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, TimeSpan startValue)
        {
            timeSpanInput.ApplyInputDeviceDelta(delta, speed, startValue);
        }
    }
}
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.UIElements
{
    internal static class InternalEngineBridge
    {
        public static float Acceleration(bool shiftPressed, bool altPressed) => NumericFieldDraggerUtility.Acceleration(shiftPressed, altPressed);
        public static double CalculateFloatDragSensitivity(double value) => NumericFieldDraggerUtility.CalculateFloatDragSensitivity(value);
        public static double CalculateFloatDragSensitivity(double value, double minValue, double maxValue) => NumericFieldDraggerUtility.CalculateFloatDragSensitivity(value, minValue, maxValue);
        public static long CalculateIntDragSensitivity(long value) => NumericFieldDraggerUtility.CalculateIntDragSensitivity(value);
        public static ulong CalculateIntDragSensitivity(ulong value) => NumericFieldDraggerUtility.CalculateIntDragSensitivity(value);
        public static long CalculateIntDragSensitivity(long value, long minValue, long maxValue) => NumericFieldDraggerUtility.CalculateIntDragSensitivity(value, minValue, maxValue);
        public static float NiceDelta(Vector2 deviceDelta, float acceleration) => NumericFieldDraggerUtility.NiceDelta(deviceDelta, acceleration);

        public static string GetOriginalText<TValueType>(this TextInputBaseField<TValueType>.TextInputBase textInputBase) => textInputBase.originalText;
        public static VisualElement GetVisualInput<TValueType>(this BaseField<TValueType> baseField) => baseField.visualInput;

        public static readonly string k_AllowedCharactersForInt = UINumericFieldsUtils.k_AllowedCharactersForInt;
        public static bool TryConvertStringToLong(string str, out long value) => UINumericFieldsUtils.TryConvertStringToLong(str, out value);
        public static bool TryConvertStringToLong(string str, out long value, out ExpressionEvaluator.Expression expr) => UINumericFieldsUtils.TryConvertStringToLong(str, out value, out expr);
        public static bool TryConvertStringToLong(string str, string initialValueAsString, out long value) => UINumericFieldsUtils.TryConvertStringToLong(str, initialValueAsString, out value);
    }
}
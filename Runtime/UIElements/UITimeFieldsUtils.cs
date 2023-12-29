using System;

namespace Cubusky.UIElements
{
    internal static class UITimeFieldsUtils
    {
        public static readonly string k_AllowedCharactersForTime = InternalEngineBridge.k_AllowedCharactersForInt + ".:";

        public static readonly string k_TimeFieldFormatString = "g";

        private static bool TryConvertTimeStringToLongString(string str, out string longString, Func<string, string> timeConversion)
        {
            longString = default;
            var timeSpanChars = new char[12] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ':' };

            int start;
            for (int i = start = 0; i < str.Length; i++)
            {
                if (Array.IndexOf(timeSpanChars, str[i]) == -1)
                {
                    longString += Parse(start, i);
                    longString += str[i];

                    start = i + 1;
                }
            }
            longString += Parse(start, str.Length);

            return true;

            string Parse(int start, int end)
            {
                if (start == end)
                {
                    return string.Empty;
                }

                var timeSpanString = str[start..end];
                return timeConversion(timeSpanString);
            }
        }

        public static bool TryConvertStringToTimeSpan(string str, out TimeSpan value)
        {
            if (TryConvertTimeStringToLongString(str, out var longString, TimeConversion)
                && InternalEngineBridge.TryConvertStringToLong(longString, out var longValue))
            {
                value = new(longValue);
                return true;
            }
            else
            {
                value = default;
                return false;
            }

            static string TimeConversion(string timeString) => TimeSpan.TryParse(timeString, out var timeSpan) ? timeSpan.Ticks.ToString() : timeString;
        }

        public static bool TryConvertStringToTimeSpan(string str, string initialValueAsString, out TimeSpan value) => TryConvertStringToTimeSpan(str, out value) || TryConvertStringToTimeSpan(initialValueAsString, out value);

        //public static bool TryConvertStringToTimeOnly(string str, string initialValueAsString, out TimeOnly value) => TryConvertStringToTimeOnly(str, out value) || TryConvertStringToTimeOnly(initialValueAsString, out value);
    }
}
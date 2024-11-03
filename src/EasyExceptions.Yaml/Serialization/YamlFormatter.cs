using System;
using System.Globalization;

namespace EasyExceptions.Yaml.Serialization
{
    public class YamlFormatter
    {
        public static YamlFormatter Default { get; } = new YamlFormatter();

        private NumberFormatInfo NumberFormat { get; } = new NumberFormatInfo
        {
            CurrencyDecimalSeparator = ".",
            CurrencyGroupSeparator = "_",
            CurrencyGroupSizes = new[] { 3 },
            CurrencySymbol = string.Empty,
            CurrencyDecimalDigits = 99,
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = "_",
            NumberGroupSizes = new[] { 3 },
            NumberDecimalDigits = 99,
            NaNSymbol = ".nan",
            PositiveInfinitySymbol = ".inf",
            NegativeInfinitySymbol = "-.inf"
        };

        public string FormatNumber(object number)
        {
            return Convert.ToString(number, NumberFormat)!;
        }

        public string FormatNumber(double number)
        {
            return number.ToString("G", NumberFormat);
        }

        public string FormatNumber(float number)
        {
            return number.ToString("G", NumberFormat);
        }

        public string FormatBoolean(object boolean)
        {
            return boolean.Equals(true) ? "true" : "false";
        }

        public string FormatDateTime(object dateTime)
        {
            return ((DateTime)dateTime).ToString("o", CultureInfo.InvariantCulture);
        }

        public string FormatTimeSpan(object timeSpan)
        {
            return ((TimeSpan)timeSpan).ToString();
        }

        /// <summary>
        /// Converts an enum to it's string representation.
        /// By default it will be the string representation of the enum passed through the naming convention.
        /// </summary>
        /// <returns>A string representation of the enum</returns>
        public Func<object, string> FormatEnum { get; } = value =>
        {
            var result = value == null ? string.Empty : value.ToString();

            return result;
        };

        /// <summary>
        /// If this function returns true, the serializer will put quotes around the formatted enum value if necessary. Defaults to true.
        /// </summary>
        public Func<object, bool> PotentiallyQuoteEnums { get; } = _ => true;
    }
}

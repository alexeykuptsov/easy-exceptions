#if NET6_0_OR_GREATER
using System;
using System.Globalization;
using System.Linq;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
    /// <summary>
    /// This represents the YAML converter entity for <see cref="DateOnly"/>.
    /// </summary>
    public class DateOnlyConverter : IYamlTypeConverter
    {
        private readonly IFormatProvider provider;
        private readonly bool doubleQuotes;
        private readonly string[] formats;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOnlyConverter"/> class.
        /// </summary>
        /// <param name="provider"><see cref="IFormatProvider"/> instance. Default value is <see cref="CultureInfo.InvariantCulture"/>.</param>
        /// <param name="doubleQuotes">If true, will use double quotes when writing the value to the stream.</param>
        /// <param name="formats">List of date/time formats for parsing. Default value is "<c>d</c>".</param>
        /// <remarks>On deserializing, all formats in the list are used for conversion, while on serializing, the first format in the list is used.</remarks>
        public DateOnlyConverter(IFormatProvider? provider = null, bool doubleQuotes = false, params string[] formats)
        {
            this.provider = provider ?? CultureInfo.InvariantCulture;
            this.doubleQuotes = doubleQuotes;
            this.formats = formats.DefaultIfEmpty("d").ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the current converter supports converting the specified type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>Returns <c>True</c>, if the current converter supports; otherwise returns <c>False</c>.</returns>
        public bool Accepts(Type type)
        {
            return type == typeof(DateOnly);
        }

        /// <summary>
        /// Reads an object's state from a YAML parser.
        /// </summary>
        /// <param name="parser"><see cref="IParser"/> instance.</param>
        /// <param name="type"><see cref="Type"/> to convert.</param>
        /// <returns>Returns the <see cref="DateOnly"/> instance converted.</returns>
        /// <remarks>On deserializing, all formats in the list are used for conversion.</remarks>
        public object ReadYaml(IParser parser, Type type)
        {
            var value = parser.Consume<Scalar>().Value;

            var dateOnly = DateOnly.ParseExact(value, this.formats, this.provider);
            return dateOnly;
        }

        /// <summary>
        /// Writes the specified object's state to a YAML emitter.
        /// </summary>
        /// <param name="emitter"><see cref="IEmitter"/> instance.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="type"><see cref="Type"/> to convert.</param>
        /// <remarks>On serializing, the first format in the list is used.</remarks>
        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var dateOnly = (DateOnly)value!;
            var formatted = dateOnly.ToString(this.formats.First(), this.provider); // Always take the first format of the list.

            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, formatted, doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
        }
    }
}
#endif

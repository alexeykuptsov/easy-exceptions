namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a scalar event.
    /// </summary>
    public sealed class Scalar : NodeEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.Scalar;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; }

        /// <summary>
        /// Gets the style of the scalar.
        /// </summary>
        /// <value>The style.</value>
        public ScalarStyle Style { get; }

        /// <summary>
        /// Gets a value indicating whether the tag is optional for the plain style.
        /// </summary>
        public bool IsPlainImplicit { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is canonical.
        /// </summary>
        /// <value></value>
        public override bool IsCanonical => !IsPlainImplicit;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scalar"/> class.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="isPlainImplicit">.</param>
        public Scalar(AnchorName anchor, TagName tag, string value, ScalarStyle style, bool isPlainImplicit)
            : base(anchor, tag)
        {
            Value = value;
            Style = style;
            IsPlainImplicit = isPlainImplicit;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return $"Scalar [anchor = {Anchor}, tag = {Tag}, value = {Value}, style = {Style}, isPlainImplicit = {IsPlainImplicit}]";
        }
    }
}

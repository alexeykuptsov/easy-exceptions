namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a mapping start event.
    /// </summary>
    public sealed class MappingStart : NodeEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.MappingStart;

        /// <summary>
        /// Gets a value indicating whether this instance is canonical.
        /// </summary>
        /// <value></value>
        public override bool IsCanonical => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStart"/> class.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        /// <param name="tag">The tag.</param>
        public MappingStart(AnchorName anchor, TagName tag)
            : base(anchor, tag)
        {;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return $"Mapping start [anchor = {Anchor}, tag = {Tag}]";
        }
    }
}

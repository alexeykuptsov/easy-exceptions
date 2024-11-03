namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a sequence start event.
    /// </summary>
    public sealed class SequenceStart : NodeEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.SequenceStart;

        /// <summary>
        /// Gets a value indicating whether this instance is implicit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is implicit; otherwise, <c>false</c>.
        /// </value>
        public bool IsImplicit { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is canonical.
        /// </summary>
        /// <value></value>
        public override bool IsCanonical => !IsImplicit;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceStart"/> class.
        /// </summary>
        public SequenceStart(AnchorName anchor, TagName tag, bool isImplicit)
            : base(anchor, tag)
        {
            IsImplicit = isImplicit;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return $"Sequence start [anchor = {Anchor}, tag = {Tag}, isImplicit = {IsImplicit}]";
        }
    }
}

namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a document start event.
    /// </summary>
    public sealed class DocumentStart : ParsingEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.DocumentStart;

        /// <summary>
        /// Gets a value indicating whether this instance is implicit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is implicit; otherwise, <c>false</c>.
        /// </value>
        public bool IsImplicit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentStart"/> class.
        /// </summary>
        public DocumentStart()
        {
            IsImplicit = true;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return $"Document start [isImplicit = {IsImplicit}]";
        }
    }
}

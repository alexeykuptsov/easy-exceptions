namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a document end event.
    /// </summary>
    public sealed class DocumentEnd : ParsingEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.DocumentEnd;

        /// <summary>
        /// Gets a value indicating whether this instance is implicit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is implicit; otherwise, <c>false</c>.
        /// </value>
        public bool IsImplicit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentEnd"/> class.
        /// </summary>
        /// <param name="isImplicit">Indicates whether the event is implicit.</param>
        public DocumentEnd(bool isImplicit)
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
            return $"Document end [isImplicit = {IsImplicit}]";
        }
    }
}

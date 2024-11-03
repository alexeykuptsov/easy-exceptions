namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a sequence end event.
    /// </summary>
    public sealed class SequenceEnd : ParsingEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.SequenceEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEnd"/> class.
        /// </summary>
        public SequenceEnd()
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "Sequence end";
        }
    }
}

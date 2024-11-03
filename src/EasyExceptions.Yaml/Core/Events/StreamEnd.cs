namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a stream end event.
    /// </summary>
    public sealed class StreamEnd : ParsingEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.StreamEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEnd"/> class.
        /// </summary>
        public StreamEnd()
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
            return "Stream end";
        }
    }
}

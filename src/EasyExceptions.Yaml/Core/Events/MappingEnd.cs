namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents a mapping end event.
    /// </summary>
    public class MappingEnd : ParsingEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.MappingEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEnd"/> class.
        /// </summary>
        public MappingEnd()
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
            return "Mapping end";
        }
    }
}

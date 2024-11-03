namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Represents an alias event.
    /// </summary>
    public sealed class AnchorAlias : ParsingEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type => EventType.Alias;

        /// <summary>
        /// Gets the value of the alias.
        /// </summary>
        public AnchorName Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnchorAlias"/> class.
        /// </summary>
        /// <param name="value">The value of the alias.</param>
        public AnchorAlias(AnchorName value)
        {
            if (value.IsEmpty)
            {
                throw new YamlException("Anchor value must not be empty.");
            }

            Value = value;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return $"Alias [value = {Value}]";
        }
    }
}

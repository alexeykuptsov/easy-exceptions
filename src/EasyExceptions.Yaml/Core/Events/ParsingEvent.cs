namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Base class for parsing events.
    /// </summary>
    public abstract class ParsingEvent
    {
        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal abstract EventType Type { get; }
    }
}

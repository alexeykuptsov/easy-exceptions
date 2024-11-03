namespace EasyExceptions.Yaml.Core.Events
{
    /// <summary>
    /// Contains the behavior that is common between node events.
    /// </summary>
    public abstract class NodeEvent : ParsingEvent
    {
        /// <summary>
        /// Gets the anchor.
        /// </summary>
        /// <value></value>
        public AnchorName Anchor { get; }

        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <value></value>
        public TagName Tag { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is canonical.
        /// </summary>
        /// <value></value>
        public abstract bool IsCanonical
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEvent"/> class.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        /// <param name="tag">The tag.</param>
        protected NodeEvent(AnchorName anchor, TagName tag)
        {
            Anchor = anchor;
            Tag = tag;
        }
    }
}

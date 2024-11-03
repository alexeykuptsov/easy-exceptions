using System.Collections.Generic;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.RepresentationModel
{
    /// <summary>
    /// Holds state that is used when emitting a stream.
    /// </summary>
    internal class EmitterState
    {
        /// <summary>
        /// Gets the already emitted anchors.
        /// </summary>
        /// <value>The emitted anchors.</value>
        public HashSet<AnchorName> EmittedAnchors { get; } = new HashSet<AnchorName>();
    }
}

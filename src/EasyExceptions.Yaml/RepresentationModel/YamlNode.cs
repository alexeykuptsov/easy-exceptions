using System;
using System.Collections.Generic;
using System.Linq;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Core.Events;

namespace EasyExceptions.Yaml.RepresentationModel
{
    /// <summary>
    /// Represents a single node in the YAML document.
    /// </summary>
    public abstract class YamlNode
    {
        private const int MaximumRecursionLevel = 1000;
        internal const string MaximumRecursionLevelReachedToStringValue = "WARNING! INFINITE RECURSION!";

        /// <summary>
        /// Gets or sets the anchor of the node.
        /// </summary>
        /// <value>The anchor.</value>
        public AnchorName Anchor { get; set; }

        /// <summary>
        /// Gets or sets the tag of the node.
        /// </summary>
        /// <value>The tag.</value>
        public TagName Tag { get; set; }

        /// <summary>
        /// Saves the current node to the specified emitter.
        /// </summary>
        /// <param name="emitter">The emitter where the node is to be saved.</param>
        /// <param name="state">The state.</param>
        internal void Save(IEmitter emitter, EmitterState state)
        {
            if (!Anchor.IsEmpty && !state.EmittedAnchors.Add(Anchor))
            {
                emitter.Emit(new AnchorAlias(Anchor));
            }
            else
            {
                Emit(emitter, state);
            }
        }

        /// <summary>
        /// Saves the current node to the specified emitter.
        /// </summary>
        /// <param name="emitter">The emitter where the node is to be saved.</param>
        /// <param name="state">The state.</param>
        internal abstract void Emit(IEmitter emitter, EmitterState state);

        public override string ToString()
        {
            var level = new RecursionLevel(MaximumRecursionLevel);
            return ToString(level);
        }

        internal abstract string ToString(RecursionLevel level);

        /// <summary>
        /// When implemented, recursively enumerates all the nodes from the document, starting on the current node.
        /// If <see cref="RecursionLevel.Maximum"/> is reached, a <see cref="MaximumRecursionLevelReachedException"/> is thrown
        /// instead of continuing and crashing with a <see cref="StackOverflowException"/>.
        /// </summary>
        internal abstract IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level);

        /// <summary>
        /// Gets the type of node.
        /// </summary>
        protected abstract YamlNodeType NodeType
        {
            get;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="YamlNode"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator YamlNode(string value)
        {
            return new YamlScalarNode(value);
        }

        /// <summary>
        /// Performs an implicit conversion from string[] to <see cref="YamlNode"/>.
        /// </summary>
        /// <param name="sequence">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator YamlNode(string[] sequence)
        {
            return new YamlSequenceNode(sequence.Select(i => (YamlNode)i));
        }

        /// <summary>
        /// Converts a <see cref="YamlScalarNode" /> to a string by returning its value.
        /// </summary>
        public static explicit operator string?(YamlNode node)
        {
            return node is YamlScalarNode scalar
                ? scalar.Value
                : throw new ArgumentException($"Attempted to convert a '{node.NodeType}' to string. This conversion is valid only for Scalars.");
        }
    }
}

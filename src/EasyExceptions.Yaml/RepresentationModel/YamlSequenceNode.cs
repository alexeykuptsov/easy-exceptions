using System.Collections.Generic;
using System.Diagnostics;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Core.Events;
using EasyExceptions.Yaml.Helpers;
using EasyExceptions.Yaml.Serialization;
using static EasyExceptions.Yaml.Core.HashCode;

namespace EasyExceptions.Yaml.RepresentationModel
{
    /// <summary>
    /// Represents a sequence node in the YAML document.
    /// </summary>
    [DebuggerDisplay("Count = {Children.Count}")]
    public sealed class YamlSequenceNode : YamlNode, IEnumerable<YamlNode>, IYamlConvertible
    {
        /// <summary>
        /// Gets the collection of child nodes.
        /// </summary>
        /// <value>The children.</value>
        public IList<YamlNode> Children { get; } = new List<YamlNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlSequenceNode"/> class.
        /// </summary>
        public YamlSequenceNode(IEnumerable<YamlNode> children)
        {
            foreach (var child in children)
            {
                Children.Add(child);
            }
        }

        /// <summary>
        /// Saves the current node to the specified emitter.
        /// </summary>
        /// <param name="emitter">The emitter where the node is to be saved.</param>
        /// <param name="state">The state.</param>
        internal override void Emit(IEmitter emitter, EmitterState state)
        {
            emitter.Emit(new SequenceStart(Anchor, Tag, Tag.IsEmpty));
            foreach (var node in Children)
            {
                node.Save(emitter, state);
            }
            emitter.Emit(new SequenceEnd());
        }

        /// <summary />
        public override bool Equals(object? obj)
        {
            var other = obj as YamlSequenceNode;
            var areEqual = other != null
                && Equals(Tag, other.Tag)
                && Children.Count == other.Children.Count;

            if (!areEqual)
            {
                return false;
            }

            for (var i = 0; i < Children.Count; ++i)
            {
                if (!Equals(Children[i], other!.Children[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach (var item in Children)
            {
                hashCode = CombineHashCodes(hashCode, item);
            }
            hashCode = CombineHashCodes(hashCode, Tag);
            return hashCode;
        }

        /// <summary>
        /// Recursively enumerates all the nodes from the document, starting on the current node,
        /// and throwing <see cref="MaximumRecursionLevelReachedException"/>
        /// if <see cref="RecursionLevel.Maximum"/> is reached.
        /// </summary>
        internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
        {
            level.Increment();
            yield return this;
            foreach (var child in Children)
            {
                foreach (var node in child.SafeAllNodes(level))
                {
                    yield return node;
                }
            }
            level.Decrement();
        }

        /// <summary>
        /// Gets the type of node.
        /// </summary>
        protected override YamlNodeType NodeType => YamlNodeType.Sequence;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        internal override string ToString(RecursionLevel level)
        {
            if (!level.TryIncrement())
            {
                return MaximumRecursionLevelReachedToStringValue;
            }

            using var textBuilder = StringBuilderPool.Rent();
            var text = textBuilder.Builder;
            text.Append("[ ");

            foreach (var child in Children)
            {
                if (text.Length > 2)
                {
                    text.Append(", ");
                }
                text.Append(child.ToString(level));
            }

            text.Append(" ]");

            level.Decrement();

            return text.ToString();
        }

        #region IEnumerable<YamlNode> Members

        /// <summary />
        public IEnumerator<YamlNode> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            Emit(emitter, new EmitterState());
        }
    }
}

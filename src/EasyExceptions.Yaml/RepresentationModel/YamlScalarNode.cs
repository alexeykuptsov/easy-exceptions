using System.Collections.Generic;
using System.Diagnostics;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Core.Events;
using EasyExceptions.Yaml.Serialization;
using EasyExceptions.Yaml.Serialization.Schemas;
using static EasyExceptions.Yaml.Core.HashCode;

namespace EasyExceptions.Yaml.RepresentationModel
{
    /// <summary>
    /// Represents a scalar node in the YAML document.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public sealed class YamlScalarNode : YamlNode, IYamlConvertible
    {
        private bool _forceImplicitPlain;
        private string? _value;

        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
        /// <value>The value.</value>
        public string? Value
        {
            get => _value;
            set
            {
                if (value == null)
                {
                    _forceImplicitPlain = true;
                }
                else
                {
                    _forceImplicitPlain = false;
                }

                _value = value;
            }
        }

        /// <summary>
        /// Gets or sets the style of the node.
        /// </summary>
        /// <value>The style.</value>
        public ScalarStyle Style { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlScalarNode"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public YamlScalarNode(string? value)
        {
            Value = value;
        }

        /// <summary>
        /// Saves the current node to the specified emitter.
        /// </summary>
        /// <param name="emitter">The emitter where the node is to be saved.</param>
        /// <param name="state">The state.</param>
        internal override void Emit(IEmitter emitter, EmitterState state)
        {
            var tag = Tag;
            var implicitPlain = tag.IsEmpty;

            if (_forceImplicitPlain &&
                Style == ScalarStyle.Plain &&
                string.IsNullOrEmpty(Value))
            {
                tag = JsonSchema.Tags.Null;
                implicitPlain = true;
            }
            else if (tag.IsEmpty && Value == null &&
                (Style == ScalarStyle.Plain || Style == ScalarStyle.Any))
            {
                tag = JsonSchema.Tags.Null;
                implicitPlain = true;
            }

            emitter.Emit(new Scalar(Anchor, tag, Value ?? string.Empty, Style, implicitPlain));
        }

        /// <summary />
        public override bool Equals(object? obj)
        {
            return obj is YamlScalarNode other
                && Equals(Tag, other.Tag)
                && Equals(Value, other.Value);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return CombineHashCodes(Tag.GetHashCode(), Value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="YamlScalarNode"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string?(YamlScalarNode value)
        {
            return value.Value;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        internal override string ToString(RecursionLevel level)
        {
            return Value ?? string.Empty;
        }

        /// <summary>
        /// Recursively enumerates all the nodes from the document, starting on the current node,
        /// and throwing <see cref="MaximumRecursionLevelReachedException"/>
        /// if <see cref="RecursionLevel.Maximum"/> is reached.
        /// </summary>
        internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
        {
            yield return this;
        }

        /// <summary>
        /// Gets the type of node.
        /// </summary>
        protected override YamlNodeType NodeType => YamlNodeType.Scalar;

        void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            Emit(emitter, new EmitterState());
        }

    }
}

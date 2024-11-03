using System;
using System.Text.RegularExpressions;

namespace EasyExceptions.Yaml.Core
{
    public readonly struct AnchorName : IEquatable<AnchorName>
    {
        public static readonly AnchorName Empty = default;

        // https://yaml.org/spec/1.2/spec.html#id2785586
        private static readonly Regex AnchorPattern = new Regex(@"^[^\[\]\{\},]+$", RegexOptions.Compiled);

        private readonly string? value;

        public string Value => value ?? throw new InvalidOperationException("Cannot read the Value of an empty anchor");

        public bool IsEmpty => value is null;

        public AnchorName(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));

            if (!AnchorPattern.IsMatch(value))
            {
                throw new ArgumentException($"Anchor cannot be empty or contain disallowed characters: []{{}},\nThe value was '{value}'.", nameof(value));
            }
        }

        public override string ToString() => value ?? "[empty]";

        public bool Equals(AnchorName other) => Equals(value, other.value);

        public override bool Equals(object? obj)
        {
            return obj is AnchorName other && Equals(other);
        }

        public override int GetHashCode()
        {
            return value?.GetHashCode() ?? 0;
        }

        public static bool operator ==(AnchorName left, AnchorName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AnchorName left, AnchorName right)
        {
            return !(left == right);
        }

        public static implicit operator AnchorName(string? value) => value == null ? Empty : new AnchorName(value);
    }
}

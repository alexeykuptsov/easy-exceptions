using System;

namespace EasyExceptions.Yaml.Core
{
    public readonly struct TagName : IEquatable<TagName>
    {
        public static readonly TagName Empty = default;

        private readonly string? value;

        public string Value => value ?? throw new InvalidOperationException("Cannot read the Value of a non-specific tag");

        public bool IsEmpty => value is null;

        private bool IsLocal => !IsEmpty && Value[0] == '!';
        private bool IsGlobal => !IsEmpty && !IsLocal;

        public TagName(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
            {
                throw new ArgumentException("Tag value must not be empty.", nameof(value));
            }

            if (IsGlobal && !Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException("Global tags must be valid URIs.", nameof(value));
            }
        }

        public override string ToString() => value ?? "?";

        public bool Equals(TagName other) => Equals(value, other.value);

        public override bool Equals(object? obj)
        {
            return obj is TagName other && Equals(other);
        }

        public override int GetHashCode()
        {
            return value?.GetHashCode() ?? 0;
        }

        public static bool operator ==(TagName left, TagName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TagName left, TagName right)
        {
            return !(left == right);
        }

        public static bool operator ==(TagName left, string right)
        {
            return Equals(left.value, right);
        }

        public static bool operator !=(TagName left, string right)
        {
            return !(left == right);
        }

        public static implicit operator TagName(string? value) => value == null ? Empty : new TagName(value);
    }
}

using System;

namespace EasyExceptions.Yaml.Core
{
    internal sealed class StringLookAheadBuffer : ILookAheadBuffer
    {
        private readonly string value;

        public int Position { get; private set; }

        public StringLookAheadBuffer(string value)
        {
            this.value = value;
        }

        public int Length => value.Length;

        public bool EndOfInput => IsOutside(Position);

        public char Peek(int offset)
        {
            var index = Position + offset;
            return IsOutside(index) ? '\0' : value[index];
        }

        private bool IsOutside(int index)
        {
            return index >= value.Length;
        }

        public void Skip(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length must be positive.");
            }
            Position += length;
        }
    }
}

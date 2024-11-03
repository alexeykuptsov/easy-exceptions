using System;
using System.Diagnostics;

namespace EasyExceptions.Yaml.Core
{
    [DebuggerStepThrough]
    internal sealed class CharacterAnalyzer<TBuffer> where TBuffer : class, ILookAheadBuffer
    {
        public CharacterAnalyzer(TBuffer buffer)
        {
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        public TBuffer Buffer { get; }

        public bool EndOfInput => Buffer.EndOfInput;

        public void Skip(int length)
        {
            Buffer.Skip(length);
        }

        public bool IsPrintable(int offset = 0)
        {
            var character = Buffer.Peek(offset);
            return
                character == '\x9' ||
                character == '\xA' ||
                character == '\xD' ||
                (character >= '\x20' && character <= '\x7E') ||
                character == '\x85' ||
                (character >= '\xA0' && character <= '\xD7FF') ||
                (character >= '\xE000' && character <= '\xFFFD');
        }

        public bool IsSpace(int offset = 0)
        {
            return Check(' ', offset);
        }

        public bool IsZero(int offset = 0)
        {
            return Check('\0', offset);
        }

        public bool IsTab(int offset = 0)
        {
            return Check('\t', offset);
        }

        public bool IsWhite(int offset = 0)
        {
            return IsSpace(offset) || IsTab(offset);
        }

        public bool IsBreak(int offset = 0)
        {
            return Check("\r\n\x85\x2028\x2029", offset);
        }

        public bool IsBreakOrZero(int offset = 0)
        {
            return IsBreak(offset) || IsZero(offset);
        }

        public bool IsWhiteBreakOrZero(int offset = 0)
        {
            return IsWhite(offset) || IsBreakOrZero(offset);
        }

        public bool Check(char expected, int offset = 0)
        {
            return Buffer.Peek(offset) == expected;
        }

        public bool Check(string expectedCharacters, int offset = 0)
        {
            // Todo: using it this way doesn't break anything, it's not really wrong...
            Debug.Assert(expectedCharacters.Length > 1, "Use Check(char, int) instead.");

            var character = Buffer.Peek(offset);
            return expectedCharacters.IndexOf(character) != -1;
        }
    }
}

using System;

namespace EasyExceptions.Yaml.Core
{
    public sealed class EmitterSettings
    {
        /// <summary>
        /// The preferred indentation.
        /// </summary>
        public int BestIndent => 2;

        /// <summary>
        /// The preferred text width.
        /// </summary>
        public int BestWidth => int.MaxValue;

        /// <summary>
        /// New line characters.
        /// </summary>
        public string NewLine { get; } = Environment.NewLine;

        /// <summary>
        /// If true, write the output in canonical form.
        /// </summary>
        public bool IsCanonical => false;

        /// <summary>
        /// The maximum allowed length for simple keys.
        /// </summary>
        /// <remarks>
        /// The specifiction mandates 1024 characters, but any desired value may be used.
        /// </remarks>
        public int MaxSimpleKeyLength { get; } = 1024;

        /// <summary>
        /// Indent sequences. The default is to not indent.
        /// </summary>
        public bool IndentSequences { get; } = false;

        public static readonly EmitterSettings Default = new EmitterSettings();
    }
}

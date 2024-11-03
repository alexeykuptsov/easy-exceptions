namespace EasyExceptions.Yaml.Core
{
    /// <summary>
    /// Exception that is thrown when a syntax error is detected on a YAML stream.
    /// </summary>
    public sealed class SyntaxErrorException : YamlException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxErrorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SyntaxErrorException(string message)
            : base(message)
        {
        }
    }
}
